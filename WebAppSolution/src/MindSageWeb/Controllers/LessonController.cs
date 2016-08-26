using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.OptionsModel;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MindSageWeb.Controllers
{
    /// <summary>
    /// Lesson API
    /// </summary>
    [Route("api/[controller]")]
    public class LessonController : Controller
    {
        #region Fields

        private AppConfigOptions _appConfig;
        private NotificationController _notificationCtrl;
        private IClassCalendarRepository _classCalendarRepo;
        private IUserProfileRepository _userprofileRepo;
        private IClassRoomRepository _classRoomRepo;
        private ILikeLessonRepository _likeLessonRepo;
        private ILessonCatalogRepository _lessonCatalogRepo;
        private ICommentRepository _commentRepo;
        private IFriendRequestRepository _friendRequestRepo;
        private IUserActivityRepository _userActivityRepo;
        private ILessonTestResultRepository _lessonTestResultRepo;
        private IDateTime _dateTime;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize lesson controller
        /// </summary>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="userprofileRepo">UserProfile repository</param>
        /// <param name="classRoomRepo">Class room repository</param>
        /// <param name="likeLessonRepo">Like lesson repository</param>
        /// <param name="lessonCatalogRepo">Lesson catalog repository</param>
        /// <param name="commentRepo">Comment repository</param>
        /// <param name="friendRequestRepo">Friend request repository</param>
        /// <param name="userActivityRepo">User activity repository</param>
        /// <param name="notificationCtrl">Notificaotion API</param>
        /// <param name="config">App configuration option</param>
        public LessonController(IClassCalendarRepository classCalendarRepo,
            IUserProfileRepository userprofileRepo,
            IClassRoomRepository classRoomRepo,
            ILikeLessonRepository likeLessonRepo,
            ILessonCatalogRepository lessonCatalogRepo,
            ICommentRepository commentRepo,
            IFriendRequestRepository friendRequestRepo,
            IUserActivityRepository userActivityRepo,
            NotificationController notificationCtrl,
            IOptions<AppConfigOptions> options,
            ILessonTestResultRepository lessonTestResultRepo,
            IDateTime dateTime)
        {
            _classCalendarRepo = classCalendarRepo;
            _userprofileRepo = userprofileRepo;
            _classRoomRepo = classRoomRepo;
            _likeLessonRepo = likeLessonRepo;
            _lessonCatalogRepo = lessonCatalogRepo;
            _commentRepo = commentRepo;
            _friendRequestRepo = friendRequestRepo;
            _userActivityRepo = userActivityRepo;
            _notificationCtrl = notificationCtrl;
            _appConfig = options.Value;
            _dateTime = dateTime;
            _lessonTestResultRepo = lessonTestResultRepo;
        }

        #endregion Constructors

        #region Methods

        // GET: api/lesson/{lesson-id}/{class-room-id}/{user-id}
        /// <summary>
        /// Get lesson's content
        /// </summary>
        /// <param name="id">Lesson's id</param>
        /// <param name="classRoomId">Class room id</param>
        /// <param name="userId">Request by user id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/{userId}")]
        public LessonContentRespond Get(string id, string classRoomId, string userId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId)
                && !string.IsNullOrEmpty(userId);
            if (!areArgumentsValid) return null;
            
            UserProfile userprofile;
            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(userId, classRoomId, out userprofile);
            if (!canAccessToTheClassRoom) return null;

            var subscriptions = userprofile.Subscriptions
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.ClassRoomId.Equals(classRoomId, StringComparison.CurrentCultureIgnoreCase));
            if (!subscriptions.Any()) return null;
            var now = _dateTime.GetCurrentTime();
            var isTeacher = subscriptions.Any(it => it.Role == UserProfile.AccountRole.Teacher);
            var canAccessToTheClassLesson = _classCalendarRepo.CheckAccessPermissionToSelectedClassLesson(classRoomId, id, now, isTeacher);
            if (!canAccessToTheClassLesson) return null;

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(classRoomId);
            var isClassRoomValid = selectedClassRoom != null && !selectedClassRoom.DeletedDate.HasValue;
            if (!isClassRoomValid) return null;

            var selectedLesson = selectedClassRoom.Lessons.FirstOrDefault(it => it.id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
            if (selectedLesson == null) return null;

            var selectedLessonCatalog = _lessonCatalogRepo.GetLessonCatalogById(selectedLesson.LessonCatalogId);
            if (selectedLessonCatalog == null) return null;

            var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(userId, classRoomId);
            var isUserActivityValid = selectedUserActivity != null && !selectedUserActivity.DeletedDate.HasValue;
            if (!isUserActivityValid) return null;
            var selectedLessonActivity = selectedUserActivity.LessonActivities.FirstOrDefault(it => it.LessonId.Equals(id, StringComparison.CurrentCultureIgnoreCase));
            if (selectedLessonActivity == null) return null;

            var selectedSubscription = userprofile.Subscriptions.FirstOrDefault(it => it.ClassRoomId == classRoomId);
            if (selectedSubscription == null) return null;
            selectedSubscription.LastActiveDate = now;
            _userprofileRepo.UpsertUserProfile(userprofile);

            var shouldUpdateSawPrimaryContent = !selectedLessonActivity.SawContentIds.Contains(selectedLessonCatalog.id);
            if (shouldUpdateSawPrimaryContent)
            {
                var sawList = selectedLessonActivity.SawContentIds.ToList();
                sawList.Add(selectedLessonCatalog.id);
                selectedLessonActivity.SawContentIds = sawList;
                _userActivityRepo.UpsertUserActivity(selectedUserActivity);
            }

            var isDisplayTeacherMsg = selectedUserActivity.HideClassRoomMessageDate.HasValue ?
                selectedClassRoom.LastUpdatedMessageDate > selectedUserActivity.HideClassRoomMessageDate.Value :
                true;

            var result = new LessonContentRespond
            {
                id = id,
                Order = selectedLessonCatalog.Order,
                SemesterName = selectedLessonCatalog.SemesterName,
                UnitNo = selectedLessonCatalog.UnitNo,
                CourseCatalogId = selectedLessonCatalog.CourseCatalogId,
                Title = selectedLessonCatalog.Title,
                CreatedDate = selectedLessonCatalog.CreatedDate,
                Advertisments = selectedLessonCatalog.Advertisments,
                CourseMessage = isDisplayTeacherMsg ? selectedClassRoom.Message : null,
                IsTeacher = isTeacher,
                TotalLikes = selectedLesson.TotalLikes,
                StudentItems = selectedLessonCatalog.StudentItems ?? Enumerable.Empty<LessonCatalog.LessonItem>(),
                TeacherItems = selectedLessonCatalog.TeacherItems ?? Enumerable.Empty<LessonCatalog.LessonItem>(),
                PostAssessments = selectedLessonCatalog.PostAssessments ?? Enumerable.Empty<LessonCatalog.AssessmentItem>(),
                PreAssessments = selectedLessonCatalog.PreAssessments ?? Enumerable.Empty<LessonCatalog.AssessmentItem>(),
            };
            return result;
        }

        [HttpGet]
        [Route("{id}/lessonpreview")]
        public async System.Threading.Tasks.Task<LessonContentRespond> GetLessonPreview(string id)
        {
            using (var http = new System.Net.Http.HttpClient())
            {
                var result = await http.GetStringAsync($"{ _appConfig.ManagementPortalUrl }/api/lessonpreview/{ id }/lesson");
                var lessonCatalog = Newtonsoft.Json.JsonConvert.DeserializeObject<LessonContentRespond>(result);
                return lessonCatalog;
            }
        }

        [HttpGet]
        [Route("{id}/lessonpreviewads")]
        public async System.Threading.Tasks.Task<OwnCarousel> GetLessonPreviewAds(int id)
        {
            using (var http = new System.Net.Http.HttpClient())
            {
                var result = await http.GetStringAsync($"{ _appConfig.ManagementPortalUrl }/api/lessonpreview/{ id }/ads");
                var ads = Newtonsoft.Json.JsonConvert.DeserializeObject<OwnCarousel>(result);
                return ads;
            }
        }

        // GET: api/lesson/{lesson-id}/{class-room-id}/answers/{user-id}
        /// <summary>
        /// Get lesson's comments
        /// </summary>
        /// <param name="id">Lesson's id</param>
        /// <param name="classRoomId">Class room id</param>
        /// <param name="userId">Request by user id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/answers/{userId}")]
        public LessonTestResult Answers(string id, string classRoomId, string userId)
        {
            var invalidRequestRespond = new LessonTestResult
            {
                ClassRoomId = classRoomId,
                LessonId = id,
                UserProfileId = userId,
                Answers = Enumerable.Empty<LessonTestResult.AnswerInformation>()
            };
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId)
                && !string.IsNullOrEmpty(userId);
            if (!areArgumentsValid) return invalidRequestRespond;

            var selectedTestedResult = _lessonTestResultRepo.GetTestedResult(classRoomId, id, userId);
            if (selectedTestedResult == null) return invalidRequestRespond;
            return selectedTestedResult;
        }

        // HttpPost: api/lesson/answer
        /// <summary>
        /// Create a answer
        /// </summary>
        /// <param name="body">Request information</param>
        [HttpPost]
        [Route("answer")]
        public void Post([FromBody]LessonTestedResultRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.UserProfileId)
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.LessonId)
                && !string.IsNullOrEmpty(body.Answer)
                && !string.IsNullOrEmpty(body.AssessmentId);
            if (!areArgumentsValid) return;

            var currentAnswer = Answers(body.LessonId, body.ClassRoomId, body.UserProfileId);
            if (currentAnswer.Answers.Any(it => it.AssessmentId == body.AssessmentId)) return;

            var newAnswer = new List<LessonTestResult.AnswerInformation>
            {
                new LessonTestResult.AnswerInformation
                {
                    AssessmentId = body.AssessmentId,
                    Answer = body.Answer
                }
            };
            currentAnswer.Answers = currentAnswer.Answers.Union(newAnswer);
            _lessonTestResultRepo.UpsertTestedResult(currentAnswer);
        }

        // GET: api/lesson/{lesson-id}/{class-room-id}/comments/{user-id}
        /// <summary>
        /// Get lesson's comments
        /// </summary>
        /// <param name="id">Lesson's id</param>
        /// <param name="classRoomId">Class room id</param>
        /// <param name="userId">Request by user id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/comments/{userId}")]
        public GetCommentRespond Comments(string id, string classRoomId, string userId)
        {
            var invalidRequestRespond = new GetCommentRespond { Comments = Enumerable.Empty<CommentInformation>() };
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId)
                && !string.IsNullOrEmpty(userId);
            if (!areArgumentsValid) return invalidRequestRespond;

            UserProfile userprofile;
            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(userId, classRoomId, out userprofile);
            if (!canAccessToTheClassRoom) return invalidRequestRespond;

            var userSubscription = userprofile.Subscriptions
                .Where(it => !it.DeletedDate.HasValue)
                .FirstOrDefault(it => it.ClassRoomId == classRoomId);
            if (userSubscription == null) return invalidRequestRespond;
            var isTeacher = userSubscription.Role == UserProfile.AccountRole.Teacher;

            var now = _dateTime.GetCurrentTime();
            var canAccessToTheClassLesson = isTeacher || _classCalendarRepo.CheckAccessPermissionToSelectedClassLesson(classRoomId, id, now);
            if (!canAccessToTheClassLesson) return invalidRequestRespond;

            var allUsersInCourse = _userprofileRepo.GetUserProfilesByClassRoomId(classRoomId).ToList();
            var filterByCreatorNames = Enumerable.Empty<string>();
            if (isTeacher) filterByCreatorNames = allUsersInCourse.Select(it => it.id).ToList();
            else
            {
                var friendRequests = _friendRequestRepo.GetFriendRequestByUserProfileId(userId);
                if (friendRequests == null) return invalidRequestRespond;

                var teacherIds = allUsersInCourse
                    .Where(it => it.Subscriptions.Any(s => s.ClassRoomId == classRoomId && s.Role == UserProfile.AccountRole.Teacher))
                    .Select(it => it.id);
                var friendIds = friendRequests
                    .Where(it => !it.DeletedDate.HasValue)
                    .Where(it => it.Status == FriendRequest.RelationStatus.Friend)
                    .Select(it => it.ToUserProfileId);
                filterByCreatorNames = friendIds.Union(new string[] { userId }).Union(teacherIds).Distinct();
            }

            var runningCommentOrder = 1;
            var comments = _commentRepo.GetCommentsByClassRoomAndLessonId(classRoomId, id, filterByCreatorNames, isTeacher)
                .Where(it => !it.DeletedDate.HasValue)
                .OrderByDescending(it => it.CreatedDate)
                .Select(it => new CommentInformation
                {
                    id = it.id,
                    Order = runningCommentOrder++,
                    Description = it.Description,
                    TotalLikes = it.TotalLikes,
                    CreatorImageUrl = it.CreatorImageUrl,
                    CreatorDisplayName = it.CreatorDisplayName,
                    ClassRoomId = it.ClassRoomId,
                    LessonId = it.LessonId,
                    CreatedByUserProfileId = it.CreatedByUserProfileId,
                    TotalDiscussions = (it.Discussions ?? Enumerable.Empty<Comment.Discussion>())
                        .Where(dit => !dit.DeletedDate.HasValue).Count(),
                    CreatedDate = it.CreatedDate
                }).ToList();

            var result = new GetCommentRespond
            {
                Comments = comments,
                IsDiscussionAvailable = true,
            };
            return result;
        }

        // GET: api/lesson/{lesson-id}/{class-room-id}/discussions/{user-id}
        /// <summary>
        /// Get comment's discussions
        /// </summary>
        /// <param name="id">Lesson's id</param>
        /// <param name="classRoomId">Class room id</param>
        /// <param name="commentId">Comment id</param>
        /// <param name="userId">Request by user id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/{commentId}/discussions/{userId}")]
        public IEnumerable<GetDiscussionRespond> Discussions(string id, string classRoomId, string commentId, string userId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId)
                && !string.IsNullOrEmpty(commentId)
                && !string.IsNullOrEmpty(userId);
            if (!areArgumentsValid) return null;

            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(userId, classRoomId);
            if (!canAccessToTheClassRoom) return null;

            var now = _dateTime.GetCurrentTime();
            var canAccessToTheClassLesson = _classCalendarRepo.CheckAccessPermissionToSelectedClassLesson(classRoomId, id, now);
            if (!canAccessToTheClassLesson) return null;

            var friendRequests = _friendRequestRepo.GetFriendRequestByUserProfileId(userId);
            if (friendRequests == null) return null;

            var friendIds = friendRequests
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.Status == FriendRequest.RelationStatus.Friend)
                .Select(it => it.ToUserProfileId);

            var filterByCreatorNames = friendIds.Union(new string[] { userId });
            var selectedComment = _commentRepo.GetCommentById(commentId);
            var canAccessToTheComment = selectedComment != null && !selectedComment.DeletedDate.HasValue;
            if (!canAccessToTheComment) return null;

            var order = 1;
            var discussions = (selectedComment.Discussions ?? Enumerable.Empty<Comment.Discussion>())
                .Where(it => !it.DeletedDate.HasValue)
                .OrderByDescending(it => it.CreatedDate)
                .Select(it => new GetDiscussionRespond
                {
                    id = it.id,
                    Order = order++,
                    CommentId = commentId,
                    Description = it.Description,
                    TotalLikes = it.TotalLikes,
                    CreatorImageUrl = it.CreatorImageUrl,
                    CreatorDisplayName = it.CreatorDisplayName,
                    CreatedByUserProfileId = it.CreatedByUserProfileId,
                    CreatedDate = it.CreatedDate
                }).ToList();
            return discussions;
        }

        // POST: api/lesson/like
        /// <summary>
        /// Like a lesson
        /// </summary>
        /// <param name="body">Request's information</param>
        [HttpPost]
        [Route("like")]
        public void Post(LikeLessonRequest body)
        {
            var isArgumentValid = body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.LessonId)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!isArgumentValid) return;

            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId);
            if (!canAccessToTheClassRoom) return;

            var now = _dateTime.GetCurrentTime();
            var canAccessToTheClassLesson = _classCalendarRepo.CheckAccessPermissionToSelectedClassLesson(body.ClassRoomId, body.LessonId, now);
            if (!canAccessToTheClassLesson) return;

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(body.ClassRoomId);
            var isLikeConditionValid = selectedClassRoom != null && selectedClassRoom.Lessons.Any(it => it.id == body.LessonId);
            if (!isLikeConditionValid) return;

            var likeLessons = _likeLessonRepo.GetLikeLessonsByLessonId(body.LessonId)
                .Where(it => !it.DeletedDate.HasValue)
                .ToList();
            if (likeLessons == null) return;

            var likedLessonsByThisUser = likeLessons
                .Where(it => it.LikedByUserProfileId.Equals(body.UserProfileId, StringComparison.CurrentCultureIgnoreCase));

            var isUnlike = likedLessonsByThisUser.Any();
            if (isUnlike)
            {
                foreach (var item in likedLessonsByThisUser)
                {
                    item.DeletedDate = now;
                    _likeLessonRepo.UpsertLikeLesson(item);
                }
            }
            else
            {
                var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(body.UserProfileId, body.ClassRoomId);
                if (selectedUserActivity == null) return;
                var selectedLessonActivity = selectedUserActivity.LessonActivities.FirstOrDefault(it => it.LessonId.Equals(body.LessonId));
                if (selectedLessonActivity == null) return;
                selectedLessonActivity.ParticipationAmount++;
                _userActivityRepo.UpsertUserActivity(selectedUserActivity);

                var newLike = new LikeLesson
                {
                    id = Guid.NewGuid().ToString(),
                    ClassRoomId = body.ClassRoomId,
                    LessonId = body.LessonId,
                    LikedByUserProfileId = body.UserProfileId,
                    CreatedDate = now
                };
                likeLessons.Add(newLike);
                _likeLessonRepo.UpsertLikeLesson(newLike);
            }

            var selectedLesson = selectedClassRoom.Lessons.First(it => it.id == body.LessonId);
            selectedLesson.TotalLikes = likeLessons.Where(it => !it.DeletedDate.HasValue).Count();
            _classRoomRepo.UpsertClassRoom(selectedClassRoom);
            _notificationCtrl.SendNotification();
        }

        // POST: api/lesson/readnote
        /// <summary>
        /// Hide the current teacher's message
        /// </summary>
        /// <param name="body">Request's information</param>
        [HttpPost]
        [Route("readnote")]
        public void ReadNote(ReadNoteRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!areArgumentsValid) return;

            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId);
            if (!canAccessToTheClassRoom) return;

            var userActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(body.UserProfileId, body.ClassRoomId);
            if (userActivity == null) return;

            userActivity.HideClassRoomMessageDate = _dateTime.GetCurrentTime();
            _userActivityRepo.UpsertUserActivity(userActivity);
        }

        // GET: api/lesson{lesson-id}/{class-room-id}/{user-id}/ads
        /// <summary>
        /// Get lesson's ads
        /// </summary>
        /// <param name="id">Lesson id</param>
        /// <param name="classRoomId">Class room id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/ads")]
        public object GetAds(string id, string classRoomId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return null;

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(classRoomId);
            var canAccessClassRoom = selectedClassRoom != null
                && selectedClassRoom.Lessons != null;
            if (!canAccessClassRoom) return null;

            var selectedLesson = selectedClassRoom.Lessons.FirstOrDefault(it => it.id == id);
            if (selectedLesson == null) return null;

            var selectedLessonCatalog = _lessonCatalogRepo.GetLessonCatalogById(selectedLesson.LessonCatalogId);
            var canAccessLessonCatalog = selectedLessonCatalog != null
                && selectedLessonCatalog.Advertisments != null
                && selectedLessonCatalog.Advertisments.Any();
            if (!canAccessLessonCatalog) return null;

            var adsUrls = selectedLessonCatalog.Advertisments ?? Enumerable.Empty<LessonCatalog.Ads>();
            var result = new
            {
                owl = adsUrls.Select(it => new
                {
                    item = $"<div class='item'><img src='{ it.ImageUrl }' /></div>"
                })
            };
            return result;
        }

        #endregion Methods

        //// GET: api/Lesson
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Lesson/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Lesson
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Lesson/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Lesson/5
        //public void Delete(int id)
        //{
        //}
    }
}
