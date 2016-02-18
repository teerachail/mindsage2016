using Microsoft.AspNet.Mvc;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Linq;

namespace MindSageWeb.Controllers
{
    /// <summary>
    /// Discussion API
    /// </summary>
    [Route("api/[controller]")]
    public class DiscussionController : Controller
    {
        #region Fields

        private IClassCalendarRepository _classCalendarRepo;
        private IUserProfileRepository _userprofileRepo;
        private ICommentRepository _commentRepo;
        private IUserActivityRepository _userActivityRepo;
        private ILikeDiscussionRepository _likeDiscussionRepo;
        private IDateTime _dateTime;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize discussion controller
        /// </summary>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="userprofileRepo">UserProfile repository</param>
        /// <param name="commentRepo">Comment repository</param>
        /// <param name="userActivityRepo">User activity repository</param>
        /// <param name="likeDiscussionRepo">Like discussion repository</param>
        public DiscussionController(IClassCalendarRepository classCalendarRepo,
            IUserProfileRepository userprofileRepo,
            ICommentRepository commentRepo,
            IUserActivityRepository userActivityRepo,
            ILikeDiscussionRepository likeDiscussionRepo,
            IDateTime dateTime)
        {
            _classCalendarRepo = classCalendarRepo;
            _userprofileRepo = userprofileRepo;
            _commentRepo = commentRepo;
            _userActivityRepo = userActivityRepo;
            _likeDiscussionRepo = likeDiscussionRepo;
            _dateTime = dateTime;
        }

        #endregion Constructors

        #region Methods

        // POST: api/discussion
        /// <summary>
        /// Create a discussion
        /// </summary>
        /// <param name="body">Request information</param>
        [HttpPost]
        public void Post(PostNewDiscussionRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.CommentId)
                && !string.IsNullOrEmpty(body.Description)
                && !string.IsNullOrEmpty(body.LessonId)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!areArgumentsValid) return;

            UserProfile userprofile;
            var canAccessToTheClassRoom = checkAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId, out userprofile);
            if (!canAccessToTheClassRoom) return;

            var now = _dateTime.GetCurrentTime();
            var canAccessToTheClassLesson = checkAccessPermissionToSelectedClassLesson(body.ClassRoomId, body.LessonId, now);
            if (!canAccessToTheClassLesson) return;

            var selectedComment = _commentRepo.GetCommentById(body.CommentId);
            if (selectedComment == null) return;

            var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(body.UserProfileId, body.ClassRoomId);
            if (selectedUserActivity == null) return;

            var selectedLesson = selectedUserActivity.LessonActivities.FirstOrDefault(it => it.LessonId == body.LessonId);
            if (selectedLesson == null) return;

            var isCommentOwner = selectedComment.CreatedByUserProfileId.Equals(body.UserProfileId, StringComparison.CurrentCultureIgnoreCase);
            if (!isCommentOwner)
            {
                var canPostNewDiscussion = checkAccessPermissionToUserProfile(selectedComment.CreatedByUserProfileId);
                if (!canPostNewDiscussion) return;
            }

            var discussions = selectedComment.Discussions.ToList();
            var newDiscussion = new Comment.Discussion
            {
                id = Guid.NewGuid().ToString(),
                CreatedByUserProfileId = body.UserProfileId,
                CreatorDisplayName = userprofile.Name,
                CreatorImageUrl = userprofile.ImageProfileUrl,
                Description = body.Description,
                CreatedDate = now,
            };
            discussions.Add(newDiscussion);
            selectedComment.Discussions = discussions;
            _commentRepo.UpsertComment(selectedComment);

            selectedLesson.ParticipationAmount++;
            _userActivityRepo.UpsertUserActivity(selectedUserActivity);
        }

        // PUT: api/discussion/{discusion-id}
        /// <summary>
        /// Update a discussion
        /// </summary>
        /// <param name="id">Discussion's id</param>
        /// <param name="body">Request information</param>
        [HttpPut]
        public void Put(string id, RemoveDiscussionRequest body)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.CommentId)
                && !string.IsNullOrEmpty(body.LessonId)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!areArgumentsValid) return;

            UserProfile userprofile;
            var canAccessToTheClassRoom = checkAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId, out userprofile);
            if (!canAccessToTheClassRoom) return;

            var now = _dateTime.GetCurrentTime();
            var canAccessToTheClassLesson = checkAccessPermissionToSelectedClassLesson(body.ClassRoomId, body.LessonId, now);
            if (!canAccessToTheClassLesson) return;

            var selectedComment = _commentRepo.GetCommentById(body.CommentId);
            if (selectedComment == null) return;
            var selectedDiscussion = selectedComment.Discussions.FirstOrDefault(it => it.id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
            if (selectedDiscussion == null) return;

            var canDeleteTheDiscussion = selectedComment.CreatedByUserProfileId.Equals(body.UserProfileId, StringComparison.CurrentCultureIgnoreCase)
                || selectedDiscussion.CreatedByUserProfileId.Equals(body.UserProfileId, StringComparison.CurrentCultureIgnoreCase);
            if (!canDeleteTheDiscussion) return;

            selectedDiscussion.DeletedDate = now;
            _commentRepo.UpsertComment(selectedComment);
        }

        // POST: api/discussion/like
        /// <summary>
        /// Like a discussion
        /// </summary>
        /// <param name="body">Request information</param>
        [HttpPost]
        [Route("like")]
        public void Like(LikeDiscussionRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.CommentId)
                && !string.IsNullOrEmpty(body.DiscussionId)
                && !string.IsNullOrEmpty(body.UserProfileId)
                && !string.IsNullOrEmpty(body.LessonId);
            if (!areArgumentsValid) return;

            var canAccessToTheClassRoom = checkAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId);
            if (!canAccessToTheClassRoom) return;

            var now = _dateTime.GetCurrentTime();
            var canAccessToTheClassLesson = checkAccessPermissionToSelectedClassLesson(body.ClassRoomId, body.LessonId, now);
            if (!canAccessToTheClassLesson) return;

            var selectedComment = _commentRepo.GetCommentById(body.CommentId);
            if (selectedComment == null) return;
            var selectedDiscussion = selectedComment.Discussions.FirstOrDefault(it => it.id.Equals(body.DiscussionId, StringComparison.CurrentCultureIgnoreCase));
            if (selectedDiscussion == null) return;

            var likeDiscussions = _likeDiscussionRepo.GetLikeDiscussionByDiscusionId(body.DiscussionId)
                .Where(it => !it.DeletedDate.HasValue)
                .ToList();

            var likedDiscussionsByThisUser = likeDiscussions
                .Where(it => it.LikedByUserProfileId.Equals(body.UserProfileId, StringComparison.CurrentCultureIgnoreCase));

            var isUnlike = likedDiscussionsByThisUser.Any();
            if (isUnlike)
            {
                foreach (var item in likedDiscussionsByThisUser)
                {
                    item.DeletedDate = now;
                    _likeDiscussionRepo.UpsertLikeDiscussion(item);
                }
            }
            else
            {
                var isCommentOwner = selectedComment.CreatedByUserProfileId.Equals(body.UserProfileId, StringComparison.CurrentCultureIgnoreCase);
                if (!isCommentOwner)
                {
                    var canLikeThisComment = checkAccessPermissionToUserProfile(selectedComment.CreatedByUserProfileId);
                    if (!canLikeThisComment) return;
                }

                var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(body.UserProfileId, body.ClassRoomId);
                if (selectedUserActivity == null) return;
                var selectedLessonActivity = selectedUserActivity.LessonActivities.FirstOrDefault(it => it.LessonId.Equals(body.LessonId));
                if (selectedLessonActivity == null) return;
                selectedLessonActivity.ParticipationAmount++;
                _userActivityRepo.UpsertUserActivity(selectedUserActivity);

                var newLike = new LikeDiscussion
                {
                    id = Guid.NewGuid().ToString(),
                    LessonId = body.LessonId,
                    CommentId = body.CommentId,
                    DiscussionId = body.DiscussionId,
                    LikedByUserProfileId = body.UserProfileId,
                    CreatedDate = now
                };
                likeDiscussions.Add(newLike);
                _likeDiscussionRepo.UpsertLikeDiscussion(newLike);
            }

            selectedDiscussion.TotalLikes = likeDiscussions.Where(it => !it.DeletedDate.HasValue).Count();
            _commentRepo.UpsertComment(selectedComment);
        }

        private bool checkAccessPermissionToSelectedClassRoom(string userprofileId, string classRoomId)
        {
            UserProfile userprofile;
            return checkAccessPermissionToSelectedClassRoom(userprofileId, classRoomId, out userprofile);
        }
        private bool checkAccessPermissionToSelectedClassRoom(string userprofileId, string classRoomId, out UserProfile userprofile)
        {
            userprofile = null;
            var areArgumentsValid = !string.IsNullOrEmpty(userprofileId) && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return false;

            var selectedUserProfile = _userprofileRepo.GetUserProfileById(userprofileId);
            if (selectedUserProfile == null) return false;
            userprofile = selectedUserProfile;

            var canAccessToTheClass = selectedUserProfile
                .Subscriptions
                .Where(it => it.ClassRoomId.Equals(classRoomId, StringComparison.CurrentCultureIgnoreCase))
                .Any();

            return canAccessToTheClass;
        }
        private bool checkAccessPermissionToSelectedClassLesson(string classRoomId, string lessonId, DateTime currentTime)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(classRoomId) && !string.IsNullOrEmpty(lessonId);
            if (!areArgumentsValid) return false;

            var selectedClassCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(classRoomId);
            if (selectedClassCalendar == null) return false;

            var canAccessToTheLesson = selectedClassCalendar.LessonCalendars
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.LessonId.Equals(lessonId, StringComparison.CurrentCultureIgnoreCase))
                .Where(it => it.BeginDate <= currentTime)
                .Any();
            return canAccessToTheLesson;
        }
        private bool checkAccessPermissionToUserProfile(string userprofileId)
        {
            var selectedCommentOwnerProfile = _userprofileRepo.GetUserProfileById(userprofileId);
            var canPostNewDiscussion = selectedCommentOwnerProfile != null
                && !selectedCommentOwnerProfile.IsPrivateAccount;
            return canPostNewDiscussion;
        }

        #endregion Methods

        //// GET: api/Discussion
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Discussion/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Discussion
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Discussion/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Discussion/5
        //public void Delete(int id)
        //{
        //}
    }
}
