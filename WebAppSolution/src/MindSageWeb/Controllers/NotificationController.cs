using Microsoft.AspNet.Mvc;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MindSageWeb.Controllers
{
    /// <summary>
    /// Notification API
    /// </summary>
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        #region Fields

        private IUserProfileRepository _userProfileRepo;
        private INotificationRepository _notificationRepo;
        private ILikeLessonRepository _likeLessonRepo;
        private ILikeCommentRepository _likeCommentRepo;
        private ILikeDiscussionRepository _likeDiscussionRepo;
        private ICommentRepository _commentRepo;
        private IClassCalendarRepository _classCalendarRepo;
        private IFriendRequestRepository _friendRequestRepo;
        private IDateTime _dateTime;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize Notification API
        /// </summary>
        /// <param name="userprofileRepo">User profile repository</param>
        /// <param name="notificationRepo">Notification repository</param>
        /// <param name="commentRepo">Comment repository</param>
        /// <param name="likeCommentRepo">Like comment repository</param>
        /// <param name="likeDiscussionRepo">Like discussion repository</param>
        /// <param name="likeLessonRepo">Like lesson repository</param>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="friendRequestRepo">Friend request repository</param>
        public NotificationController(IUserProfileRepository userprofileRepo,
            INotificationRepository notificationRepo,
            ILikeLessonRepository likeLessonRepo,
            ILikeCommentRepository likeCommentRepo,
            ILikeDiscussionRepository likeDiscussionRepo,
            ICommentRepository commentRepo,
            IClassCalendarRepository classCalendarRepo,
            IFriendRequestRepository friendRequestRepo,
            IDateTime dateTime)
        {
            _userProfileRepo = userprofileRepo;
            _notificationRepo = notificationRepo;
            _likeCommentRepo = likeCommentRepo;
            _likeDiscussionRepo = likeDiscussionRepo;
            _likeLessonRepo = likeLessonRepo;
            _commentRepo = commentRepo;
            _classCalendarRepo = classCalendarRepo;
            _friendRequestRepo = friendRequestRepo;
            _dateTime = dateTime;
        }

        #endregion Constructors

        #region Methods

        // GET: api/notification/{user-id}/{class-room-id}
        /// <summary>
        /// Get notification total amount
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}")]
        public GetNotificationRespond Get(string id, string classRoomId)
        {
            const int NoneNotification = 0;
            var areArgumentsValid = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return new GetNotificationRespond { notificationTotal = NoneNotification };

            var notifications = _notificationRepo.GetNotificationByUserIdAndClassRoomId(id, classRoomId).ToList();
            var anyNotification = notifications != null && notifications.Any(it => !it.HideDate.HasValue);
            if (!anyNotification) return new GetNotificationRespond { notificationTotal = NoneNotification };

            return new GetNotificationRespond { notificationTotal = notifications.Where(it => !it.HideDate.HasValue).Count() };
        }

        // GET: api/notification/{user-id}/{class-room-id}/content
        /// <summary>
        /// Get notification content
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/content")]
        public IEnumerable<NotificationRespond> GetContent(string id, string classRoomId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return Enumerable.Empty<NotificationRespond>();

            var notifications = _notificationRepo.GetNotificationByUserIdAndClassRoomId(id, classRoomId).ToList();
            var anyNotification = notifications != null && notifications.Any(it => !it.HideDate.HasValue);
            if (!anyNotification) return Enumerable.Empty<NotificationRespond>();

            var now = _dateTime.GetCurrentTime();
            var unreadedMsgs = notifications.Where(it => !it.HideDate.HasValue).ToList();

            const int LastRetrieveDays = 7;
            var needToReachQry = unreadedMsgs
                .Where(it => !it.LastReadedDate.HasValue || it.LastReadedDate < it.LastUpdateDate)
                .Where(it => Math.Abs(it.CreatedDate.Date.Subtract(now).Days) <= LastRetrieveDays);

            var relatedUserProfiles = needToReachQry
                .SelectMany(it => it.ByUserProfileId)
                .Where(it => it != null)
                .Distinct()
                .Select(it => _userProfileRepo.GetUserProfileById(it))
                .Where(it => it != null)
                .ToList();

            var result = needToReachQry
                .Select(it =>
                {
                    var fromUserProfile = relatedUserProfiles
                        .Where(uProfile => it.ByUserProfileId.Contains(uProfile.id))
                        .Select(uProfile => new NotificationRespond.ActionInformation
                        {
                            FromUserProfileId = uProfile.id,
                            FromUserProfileName = uProfile.Name
                        }).ToList();

                    NotificationRespond.NotificationTag tag;
                    switch (it.Tag)
                    {
                        case Notification.NotificationTag.Reminder:
                            tag = NotificationRespond.NotificationTag.Reminder;
                            break;
                        case Notification.NotificationTag.TopicOfTheDay:
                            tag = NotificationRespond.NotificationTag.TOTD;
                            break;
                        case Notification.NotificationTag.FriendCreateNewComment:
                            tag = NotificationRespond.NotificationTag.Comment;
                            break;
                        case Notification.NotificationTag.FriendLikesALesson:
                        case Notification.NotificationTag.SomeOneLikesYourComment:
                        case Notification.NotificationTag.SomeOneLikesYourDiscussion:
                            tag = NotificationRespond.NotificationTag.Like;
                            break;
                        case Notification.NotificationTag.SomeOneCreateDiscussionInYourComment:
                            tag = NotificationRespond.NotificationTag.Discussion;
                            break;
                        default: return null;
                    }

                    return new NotificationRespond
                    {
                        id = it.id,
                        Tag = tag,
                        FromUserProfiles = fromUserProfile,
                        Message = it.Message
                    };
                })
                .Where(it => it != null).ToList();

            return result;
        }

        [HttpGet]
        public void SendNotification()
        {
            var now = _dateTime.GetCurrentTime();
            sendNotifyLikeLessons(now);
            sendNotifyLikeComments(now);
            sendNotifyLikeDiscussions(now);
            sendNotifyNewComments(now);
            sendNotifyNewDiscussions(now);
            sendNotifyTopicOfTheDay(now);
            sendNotifyReminder(now);
        }

        private void sendNotifyLikeLessons(DateTime now)
        {
            var requiredNotifyLikeLesson = _likeLessonRepo.GetRequireNotifyLikeLessons().ToList();
            if (!requiredNotifyLikeLesson.Any()) return;

            var lessonLikerUserProfileIds = requiredNotifyLikeLesson.Select(it => it.LikedByUserProfileId).Distinct();
            var friendsRelated = _friendRequestRepo.GetFriendRequestByUserProfileId(lessonLikerUserProfileIds).ToList(); // HACK: จะต้องส่งให้เฉพาะเพื่อนที่กำลังอยู่ใน course นี้เท่านั้น
            const string Message = "Like a lesson";
            var notifyLikeLessons = requiredNotifyLikeLesson.Select(it =>
            {
                var friends = friendsRelated
                    .Where(f => !f.DeletedDate.HasValue)
                    .Where(f => f.FromUserProfileId == it.LikedByUserProfileId && f.Status == FriendRequest.RelationStatus.Friend)
                    .Select(f => new Notification
                    {
                        id = Guid.NewGuid().ToString(),
                        ByUserProfileId = new List<string> { it.LikedByUserProfileId },
                        ClassRoomId = it.ClassRoomId,
                        CreatedDate = now,
                        LastUpdateDate = now,
                        LessonId = it.LessonId,
                        Message = Message,
                        Tag = Notification.NotificationTag.FriendLikesALesson,
                        TotalLikes = 1,
                        ToUserProfileId = f.ToUserProfileId
                    });
                return friends;
            }).Where(it => it.Any()).SelectMany(it => it).ToList();
            _notificationRepo.Insert(notifyLikeLessons);
            requiredNotifyLikeLesson.ForEach(it =>
            {
                it.LastNotifyComplete = now;
                _likeLessonRepo.UpsertLikeLesson(it);
            });
        }
        private void sendNotifyLikeComments(DateTime now)
        {
            var requiredNotifyLikeComments = _likeCommentRepo.GetRequireNotifyLikeComments().ToList();
            if (!requiredNotifyLikeComments.Any()) return;

            var reqSendNotifyCommentIds = requiredNotifyLikeComments.Select(it => it.CommentId).Distinct();
            var reqSendNotifyComments = _commentRepo.GetCommentById(reqSendNotifyCommentIds).ToList();

            var commentCreatorUserProfileIds = reqSendNotifyComments.Select(it => it.CreatedByUserProfileId).Distinct();
            var commentCreatorUserProfiles = _userProfileRepo.GetUserProfileById(commentCreatorUserProfileIds).ToList();

            const string Message = "Like your comment.";
            var notifyComments = requiredNotifyLikeComments.Select(it =>
            {
                var comment = reqSendNotifyComments.FirstOrDefault(c => c.id == it.CommentId);
                if (comment == null) return null;
                var userprofile = commentCreatorUserProfiles.FirstOrDefault(u => u.id == comment.CreatedByUserProfileId);
                if (userprofile == null) return null;
                if (it.LikedByUserProfileId == userprofile.id) return null;
                var isLikeSelfComment = it.LikedByUserProfileId == comment.CreatedByUserProfileId;
                if (isLikeSelfComment) return null;

                return new CommentNotification
                {
                    id = Guid.NewGuid().ToString(),
                    LastUpdateDate = now,
                    ToUserProfileId = userprofile.id,
                    ClassRoomId = it.ClassRoomId,
                    LessonId = it.LessonId,
                    CreatedDate = now,
                    Message = Message,
                    ByUserProfileId = new List<string> { it.LikedByUserProfileId },
                    CommentId = it.CommentId,
                    TotalLikes = 1,
                    Tag = Notification.NotificationTag.SomeOneLikesYourComment
                };
            }).Where(it => it != null).ToList();
            _notificationRepo.Insert(notifyComments);
            requiredNotifyLikeComments.ForEach(it =>
            {
                it.LastNotifyComplete = now;
                _likeCommentRepo.UpsertLikeComment(it);
            });
        }
        private void sendNotifyLikeDiscussions(DateTime now)
        {
            var requiredNotifyLikeDiscussions = _likeDiscussionRepo.GetRequireNotifyLikeDiscussions().ToList();
            if (!requiredNotifyLikeDiscussions.Any()) return;

            var relatedCommentIds = requiredNotifyLikeDiscussions.Select(it => it.CommentId).Distinct();
            var relatedDiscussions = _commentRepo.GetCommentById(relatedCommentIds)
                .Where(it=>!it.DeletedDate.HasValue)
                .SelectMany(it => it.Discussions)
                .Where(it=>!it.DeletedDate.HasValue)
                .ToList();

            const string Message = "Like your discussion.";
            var notifyDiscussions = requiredNotifyLikeDiscussions.Select(it =>
            {
                var discussion = relatedDiscussions.FirstOrDefault(d => d.id == it.DiscussionId);
                if (discussion == null) return null;
                var isLikeSelfDiscussion = it.LikedByUserProfileId == discussion.CreatedByUserProfileId;
                if (isLikeSelfDiscussion) return null;

                return new Notification
                {
                    id = Guid.NewGuid().ToString(),
                    ByUserProfileId = new List<string> { it.LikedByUserProfileId },
                    ClassRoomId = it.ClassRoomId,
                    CreatedDate = now,
                    LastUpdateDate = now,
                    LessonId = it.LessonId,
                    Message = Message,
                    Tag = Notification.NotificationTag.SomeOneLikesYourDiscussion,
                    TotalLikes = 1,
                    ToUserProfileId = discussion.CreatedByUserProfileId
                };
            }).Where(it => it != null).ToList();
            _notificationRepo.Insert(notifyDiscussions);
            requiredNotifyLikeDiscussions.ForEach(it =>
            {
                it.LastNotifyComplete = now;
                _likeDiscussionRepo.UpsertLikeDiscussion(it);
            });
        }
        private void sendNotifyNewComments(DateTime now)
        {
            var requiredNotifyComments = _commentRepo.GetRequireNotifyComments().ToList();
            if (!requiredNotifyComments.Any()) return;

            var commentCreaterUserprofildIds = requiredNotifyComments.Select(it => it.CreatedByUserProfileId).Distinct();
            var friendUserProfileds = _friendRequestRepo.GetFriendRequestByUserProfileId(commentCreaterUserprofildIds) // HACK: จะต้องส่งให้เฉพาะเพื่อนที่กำลังอยู่ใน course นี้เท่านั้น
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.Status == FriendRequest.RelationStatus.Friend)
                .ToList();

            const string Message = "Create new comment.";
            var notifyCreateComments = requiredNotifyComments.Select(it =>
            {
                var friends = friendUserProfileds
                    .Where(fReq => fReq.FromUserProfileId == it.CreatedByUserProfileId)
                    .Select(fReq => new CommentNotification
                    {
                        id = Guid.NewGuid().ToString(),
                        ByUserProfileId = new List<string> { it.CreatedByUserProfileId },
                        ClassRoomId = it.ClassRoomId,
                        CommentId  = it.id,
                        CreatedDate = now,
                        LastUpdateDate = now,
                        LessonId = it.LessonId,
                        Message = Message,
                        Tag = Notification.NotificationTag.FriendCreateNewComment,
                        ToUserProfileId = fReq.ToUserProfileId
                    });
                return friends;
            }).Where(it => it.Any()).SelectMany(it => it).ToList();
            _notificationRepo.Insert(notifyCreateComments);
            requiredNotifyComments.ForEach(it =>
            {
                it.LastNotifyComplete = now;
                _commentRepo.UpsertComment(it);
            });
        }
        private void sendNotifyNewDiscussions(DateTime now)
        {
            var relatedComments = _commentRepo.GetRequireNotifyDiscussions().ToList();
            if (!relatedComments.Any()) return;

            var requiredNotifyDiscussions = relatedComments.SelectMany(it => it.Discussions)
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => !it.LastNotifyComplete.HasValue)
                .ToList();

            const string Message = "Create a discussion in you comment.";
            var discussions = requiredNotifyDiscussions
                .Select(discussion =>
                {
                    var comment = relatedComments.FirstOrDefault(it => it.Discussions.Any(d => d.id == discussion.id));
                    if (comment == null) return null;
                    var isSelfDiscussion = comment.CreatedByUserProfileId == discussion.CreatedByUserProfileId;
                    if (isSelfDiscussion) return null;

                    return new DiscussionNotification
                    {
                        id = Guid.NewGuid().ToString(),
                        ByUserProfileId = new List<string> { discussion.CreatedByUserProfileId },
                        ClassRoomId = comment.ClassRoomId,
                        CommentId = comment.id,
                        CreatedDate = now,
                        DiscussionId = discussion.id,
                        LastUpdateDate = now,
                        LessonId = comment.LessonId,
                        Message = Message,
                        Tag = Notification.NotificationTag.SomeOneCreateDiscussionInYourComment,
                        ToUserProfileId = comment.CreatedByUserProfileId
                    };
                }).Where(it => it != null).ToList();
            _notificationRepo.Insert(discussions);
            requiredNotifyDiscussions.ForEach(it => it.LastNotifyComplete = now);
            relatedComments.ForEach(it => _commentRepo.UpsertComment(it));
        }
        private void sendNotifyTopicOfTheDay(DateTime now)
        {
            // TODO: Send topic of the day
            //var requiredNotifyTOTD = _classCalendarRepo.GetRequireNotifyTopicOfTheDay(now).ToList();
            //if (!requiredNotifyTOTD.Any()) return;

            //var classIds = requiredNotifyTOTD.Select(it => it.ClassRoomId).Distinct();
            //var students = _userProfileRepo.GetUserProfilesByClassRoomId(classIds).ToList();

            //var qry = from classCalendar in requiredNotifyTOTD
            //          where !classCalendar.CloseDate.HasValue
            //          from lessonCalendar in classCalendar.LessonCalendars
            //          where !lessonCalendar.DeletedDate.HasValue
            //          where !lessonCalendar.SendTopicOfTheDayDate.HasValue
            //          where lessonCalendar.RequiredSendTopicOfTheDayDate.Date <= now.Date
            //          from student in students
            //          where !student.DeletedDate.HasValue
            //          from subscription in student.Subscriptions
            //          where !subscription.DeletedDate.HasValue
            //          where subscription.ClassRoomId == classCalendar.ClassRoomId
            //          where subscription.Role != UserProfile.AccountRole.Teacher
            //          select new Notification
            //          {
            //              id = Guid.NewGuid().ToString(),
            //              ByUserProfileId = Enumerable.Empty<string>(),
            //              ClassRoomId = classCalendar.ClassRoomId,
            //              CreatedDate = now,
            //              LastUpdateDate = now,
            //              LessonId = lessonCalendar.LessonId,
            //              Message = lessonCalendar.TopicOfTheDayMessage,
            //              Tag = Notification.NotificationTag.TopicOfTheDay,
            //              ToUserProfileId = student.id
            //          };
            //_notificationRepo.Insert(qry);

            //var reqUpdateLessons = from classCalendar in requiredNotifyTOTD
            //                       where !classCalendar.CloseDate.HasValue
            //                       from lessonCalendar in classCalendar.LessonCalendars
            //                       where !lessonCalendar.DeletedDate.HasValue
            //                       select lessonCalendar;
            //foreach (var item in reqUpdateLessons) item.SendTopicOfTheDayDate = now;
            //requiredNotifyTOTD.ForEach(it => _classCalendarRepo.UpsertClassCalendar(it));
        }
        private void sendNotifyReminder(DateTime now)
        {
            // TODO: Not implemented Reminder
        }

        #endregion Methods

        //// GET: api/notification
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/notification/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/notification
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/notification/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/notification/5
        //public void Delete(int id)
        //{
        //}
    }
}
