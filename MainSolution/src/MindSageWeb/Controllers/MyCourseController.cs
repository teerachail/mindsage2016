using Microsoft.AspNet.Mvc;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MindSageWeb.Controllers
{
    /// <summary>
    /// MyCourse API
    /// </summary>
    [Route("api/[controller]")]
    public class MyCourseController : Controller
    {
        #region Fields

        private IClassRoomRepository _classRoomRepo;
        private IClassCalendarRepository _classCalendarRepo;
        private IUserProfileRepository _userprofileRepo;
        private IUserActivityRepository _userActivityRepo;
        private IStudentKeyRepository _studentKeyRepo;
        private ILessonCatalogRepository _lessonCatalogRepo;
        private ILikeLessonRepository _likeLessonRepo;
        private ILikeCommentRepository _likeCommentRepo;
        private ILikeDiscussionRepository _likeDiscussionRepo;
        private IDateTime _dateTime;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize comment controller
        /// </summary>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="userprofileRepo">UserProfile repository</param>
        /// <param name="userActivityRepo">User activity repository</param>
        /// <param name="classRoomRepo">Class room repository</param>
        /// <param name="studentKeyRepo">Student key repository</param>
        /// <param name="lessonCatalogRepo">Lesson catalog repository</param>
        public MyCourseController(IClassCalendarRepository classCalendarRepo,
            IUserProfileRepository userprofileRepo,
            IUserActivityRepository userActivityRepo,
            IClassRoomRepository classRoomRepo,
            IStudentKeyRepository studentKeyRepo,
            ILessonCatalogRepository lessonCatalogRepo,
            ILikeLessonRepository likeLessonRepo,
            ILikeCommentRepository likeCommentRepo,
            ILikeDiscussionRepository likeDiscussionRepo,
            IDateTime dateTime)
        {
            _classCalendarRepo = classCalendarRepo;
            _userprofileRepo = userprofileRepo;
            _userActivityRepo = userActivityRepo;
            _classRoomRepo = classRoomRepo;
            _studentKeyRepo = studentKeyRepo;
            _lessonCatalogRepo = lessonCatalogRepo;
            _likeLessonRepo = likeLessonRepo;
            _likeCommentRepo = likeCommentRepo;
            _likeDiscussionRepo = likeDiscussionRepo;
            _dateTime = dateTime;
        }

        #endregion Constructors

        #region Methods

        // GET: api/mycourse/{user-id}/{class-room-id}
        /// <summary>
        /// Get lesson map content
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        [HttpGet("{id}/{classRoomId}")]
        public IEnumerable<CourseMapContentRespond> Get(string id, string classRoomId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return Enumerable.Empty<CourseMapContentRespond>();

            if (!_userprofileRepo.CheckAccessPermissionToSelectedClassRoom(id, classRoomId)) return Enumerable.Empty<CourseMapContentRespond>();

            var now = _dateTime.GetCurrentTime();
            var classCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(classRoomId);
            var canAccessToTheClassRoom = classCalendar != null
                && classCalendar.LessonCalendars != null
                && !classCalendar.DeletedDate.HasValue
                && !classCalendar.CloseDate.HasValue
                && classCalendar.ExpiredDate.HasValue
                && classCalendar.ExpiredDate.Value.Date > now.Date;
            if (!canAccessToTheClassRoom) return Enumerable.Empty<CourseMapContentRespond>();

            var result = classCalendar.LessonCalendars
                .Where(it => !it.DeletedDate.HasValue)
                .GroupBy(it => it.SemesterGroupName)
                .Select(group => new CourseMapContentRespond
                {
                    SemesterName = group.Key,
                    LessonStatus = group.Select(it => new CourseMapLessonStatus
                    {
                        LessonId = it.LessonId,
                        IsLocked = now.Date < it.BeginDate.Date,
                        LessonWeekName = string.Format("Week{0:00}", it.Order),
                    }).ToList()
                }).ToList();
            var lessonQry = result.SelectMany(it => it.LessonStatus);
            var currentLesson = lessonQry.LastOrDefault(it => !it.IsLocked);
            if (currentLesson != null) currentLesson.IsCurrent = true;
            else if (lessonQry.Any()) lessonQry.Last().IsCurrent = true;
            return result;
        }

        // GET: api/mycourse/{user-id}/{class-room-id}/status
        /// <summary>
        /// Get lesson map status
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/status")]
        public IEnumerable<CourseMapStatusRespond> GetStatus(string id, string classRoomId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                 && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return Enumerable.Empty<CourseMapStatusRespond>();

            if (!_userprofileRepo.CheckAccessPermissionToSelectedClassRoom(id, classRoomId)) return Enumerable.Empty<CourseMapStatusRespond>();

            var now = _dateTime.GetCurrentTime();
            var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(id, classRoomId);
            var canAccessUserActivit = selectedUserActivity != null
                && selectedUserActivity.LessonActivities != null
                && !selectedUserActivity.DeletedDate.HasValue;
            if (!canAccessUserActivit) Enumerable.Empty<CourseMapStatusRespond>();

            const int NoneComment = 0;
            var result = selectedUserActivity.LessonActivities
                .Select(it => new CourseMapStatusRespond
                {
                    LessonId = it.LessonId,
                    HaveAnyComments = it.CreatedCommentAmount > NoneComment,
                    IsReadedAllContents = it.SawContentIds.Count() >= it.TotalContentsAmount
                })
                .ToList();
            return result;
        }

        // GET: api/mycourse/{user-id}/{class-room-id}/students
        /// <summary>
        /// Get studens from class room id
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/students")]
        public IEnumerable<GetStudentListRespond> Students(string id, string classRoomId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return Enumerable.Empty<GetStudentListRespond>();

            UserProfile userprofile;
            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(id, classRoomId, out userprofile);
            if (!canAccessToTheClassRoom) return Enumerable.Empty<GetStudentListRespond>();

            var isTeacherAccount = userprofile.Subscriptions.First(it => it.ClassRoomId == classRoomId).Role == UserProfile.AccountRole.Teacher;
            if (!isTeacherAccount) return Enumerable.Empty<GetStudentListRespond>();

            var allStudentsInTheClassRoom = _userprofileRepo.GetUserProfilesByClassRoomId(classRoomId).ToList();
            if (!allStudentsInTheClassRoom.Any()) return Enumerable.Empty<GetStudentListRespond>();

            var userActivities = allStudentsInTheClassRoom
                .Select(it => _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(it.id, classRoomId))
                .Where(it => it != null)
                .ToList();

            const int NoneScore = 0;
            var result = allStudentsInTheClassRoom
                .Where(it => it.id != id)
                .OrderBy(it => it.id)
                .Select(it =>
                {
                    var selectedUserActivity = userActivities.FirstOrDefault(ua => ua.UserProfileId == it.id);
                    var isActivityFound = selectedUserActivity != null;

                    return new GetStudentListRespond
                    {
                        id = it.id,
                        Name = it.Name,
                        ImageUrl = it.ImageProfileUrl,
                        CommentPercentage = isActivityFound ? selectedUserActivity.CommentPercentage : NoneScore,
                        OnlineExtrasPercentage = isActivityFound ? selectedUserActivity.OnlineExtrasPercentage : NoneScore,
                        SocialParticipationPercentage = isActivityFound ? selectedUserActivity.SocialParticipationPercentage : NoneScore,
                    };
                })
                .ToList();
            return result;
        }

        // GET: api/mycourse/{user-id}/status
        /// <summary>
        /// Get all user's courses
        /// </summary>
        /// <param name="id">User profile id</param>
        [HttpGet]
        [Route("{id}/courses")]
        public IEnumerable<CourseCatalogRespond> GetAllCourses(string id)
        {
            var isArguementValid = !string.IsNullOrEmpty(id);
            if (!isArguementValid) return Enumerable.Empty<CourseCatalogRespond>();

            var selectedUserProfile = _userprofileRepo.GetUserProfileById(id);
            var canAccessToSubscription = selectedUserProfile != null
                && selectedUserProfile.Subscriptions != null
                && selectedUserProfile.Subscriptions.Any();
            if (!canAccessToSubscription) return Enumerable.Empty<CourseCatalogRespond>();

            var subscriptionQry = selectedUserProfile.Subscriptions.Where(it => !it.DeletedDate.HasValue);

            var now = _dateTime.GetCurrentTime();
            var availableClassRooms = subscriptionQry
                .Select(it => _classCalendarRepo.GetClassCalendarByClassRoomId(it.ClassRoomId))
                .Where(it => it != null)
                .Where(it => it.BeginDate.Date <= now.Date)
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => !it.CloseDate.HasValue)
                .Where(it => it.ExpiredDate.HasValue)
                .Where(it => it.ExpiredDate.Value.Date > now.Date)
                .ToList();

            var availableSubscriptions = subscriptionQry
                .Where(it => availableClassRooms.Any(cc=>cc.ClassRoomId == it.ClassRoomId))
                .Select(subscription =>
                {
                    var classCalendar = availableClassRooms.FirstOrDefault(cc => cc.ClassRoomId == subscription.ClassRoomId);
                    if (classCalendar == null) return null;
                    var isClassCalendarValid = classCalendar != null && classCalendar.LessonCalendars != null && classCalendar.LessonCalendars.Any();
                    if (!isClassCalendarValid) return null;
                    var lessonCalendarQry = classCalendar.LessonCalendars.Where(it => !it.DeletedDate.HasValue);
                    var currentLesson = lessonCalendarQry.Where(it => now.Date >= it.BeginDate).OrderBy(it => it.BeginDate).FirstOrDefault();
                    if (currentLesson == null) return null;

                    return new CourseCatalogRespond
                    {
                        id = subscription.id,
                        Name = subscription.ClassRoomName,
                        ClassRoomId = subscription.ClassRoomId,
                        LessonId = currentLesson.LessonId
                    };
                }).Where(it => it != null).ToList();

            return availableSubscriptions;
        }

        // POST: api/mycourse/message
        /// <summary>
        /// Post new course's message
        /// </summary>
        /// <param name="body">Request's information</param>
        [HttpPost]
        [Route("message")]
        public void Post(PostNewCourseMessageRequest body)
        {
            var areArgumentsValid = body != null 
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.Message)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!areArgumentsValid) return;

            UserProfile userprofile;
            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId, out userprofile);
            if (!canAccessToTheClassRoom) return;

            var isTeacherAccount = userprofile.Subscriptions.First(it => it.ClassRoomId == body.ClassRoomId).Role == UserProfile.AccountRole.Teacher;
            if (!isTeacherAccount) return;

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(body.ClassRoomId);
            if (selectedClassRoom == null) return;

            selectedClassRoom.Message = body.Message;
            selectedClassRoom.LastUpdatedMessageDate = _dateTime.GetCurrentTime();
            _classRoomRepo.UpsertClassRoom(selectedClassRoom);
        }

        // POST: api/mycourse/removestud
        /// <summary>
        /// Remove a student
        /// </summary>
        /// <param name="body">Request's information</param>
        [HttpPost]
        [Route("removestud")]
        public void RemoveStudent(RemoveStudentRequest body)
        {
            var areArgumentsValid = body != null 
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.RemoveUserProfileId)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!areArgumentsValid) return;

            UserProfile userprofile;
            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId, out userprofile);
            if (!canAccessToTheClassRoom) return;

            var isTeacherAccount = userprofile.Subscriptions.First(it => it.ClassRoomId == body.ClassRoomId).Role == UserProfile.AccountRole.Teacher;
            if (!isTeacherAccount) return;

            var selectedUserProfile = _userprofileRepo.GetUserProfileById(body.RemoveUserProfileId);
            if (selectedUserProfile == null) return;

            var selectedSubscription = selectedUserProfile.Subscriptions
                .Where(it => it.ClassRoomId.Equals(body.ClassRoomId))
                .Where(it => !it.DeletedDate.HasValue)
                .FirstOrDefault();
            if (selectedSubscription == null) return;

            selectedSubscription.DeletedDate = _dateTime.GetCurrentTime();
            _userprofileRepo.UpsertUserProfile(selectedUserProfile);
        }

        // POST: api/mycourse/leave
        /// <summary>
        /// Teacher leave course
        /// </summary>
        /// <param name="body">Request's information</param>
        [HttpPost]
        [Route("leave")]
        public void Leave(LeaveCourseRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!areArgumentsValid) return;

            UserProfile userprofile;
            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId, out userprofile);
            if (!canAccessToTheClassRoom) return;

            var isTeacherAccount = userprofile.Subscriptions.First(it => it.ClassRoomId == body.ClassRoomId).Role == UserProfile.AccountRole.Teacher;
            if (!isTeacherAccount) return;

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(body.ClassRoomId);
            if (selectedClassRoom == null) return;

            var students = _userprofileRepo.GetUserProfilesByClassRoomId(body.ClassRoomId).ToList();
            if (students == null) return;

            var selectedClassCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(body.ClassRoomId);
            if (selectedClassCalendar == null) return;

            var now = _dateTime.GetCurrentTime();
            selectedClassRoom.DeletedDate = now;
            _classRoomRepo.UpsertClassRoom(selectedClassRoom);

            selectedClassCalendar.DeletedDate = now;
            _classCalendarRepo.UpsertClassCalendar(selectedClassCalendar);

            var subscriptions = students.SelectMany(it => it.Subscriptions)
                .Where(it => it.ClassRoomId.Equals(body.ClassRoomId))
                .Where(it => !it.DeletedDate.HasValue)
                .ToList();

            subscriptions.ForEach(it => it.DeletedDate = now);
            students.ForEach(it => _userprofileRepo.UpsertUserProfile(it));

            var activities = students.Select(it => _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(it.id, body.ClassRoomId))
                .Where(it => it != null)
                .Where(it => !it.DeletedDate.HasValue)
                .ToList();

            activities.ForEach(it =>
            {
                it.DeletedDate = now;
                _userActivityRepo.UpsertUserActivity(it);
            });

            var selectedStudentKey = _studentKeyRepo.GetStudentKeyByClassRoomId(body.ClassRoomId);
            if (selectedStudentKey == null) return;

            selectedStudentKey.DeletedDate = now;
            _studentKeyRepo.UpsertStudentKey(selectedStudentKey);
        }

        // POST: api/mycourse/addcourse
        /// <summary>
        /// Add new course by code
        /// </summary>
        /// <param name="body">Request's information</param>
        [HttpPost]
        [Route("addcourse")]
        public void AddCourse(AddCourseRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.UserProfileId)
                && !string.IsNullOrEmpty(body.Code)
                && !string.IsNullOrEmpty(body.Grade);
            if (!areArgumentsValid) return;

            var selectedStudentKey = _studentKeyRepo.GetStudentKeyByCodeAndGrade(body.Code, body.Grade);
            if (selectedStudentKey == null) return;

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(selectedStudentKey.ClassRoomId);
            if (selectedClassRoom == null) return;

            var selectedClassCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(selectedStudentKey.ClassRoomId);
            if (selectedClassCalendar == null) return;

            var selectedUserProfile = _userprofileRepo.GetUserProfileById(body.UserProfileId);
            if (selectedUserProfile == null) return;

            var lessonCatalogs = selectedClassRoom.Lessons
                .Select(it => _lessonCatalogRepo.GetLessonCatalogById(it.LessonCatalogId))
                .ToList();
            if (lessonCatalogs.Any(it => it == null)) return;

            var now = _dateTime.GetCurrentTime();
            var subscriptions = selectedUserProfile.Subscriptions.ToList();
            subscriptions.Add(new UserProfile.Subscription
            {
                id = Guid.NewGuid().ToString(),
                Role = UserProfile.AccountRole.Student,
                ClassRoomId = selectedClassRoom.id,
                ClassCalendarId = selectedClassCalendar.id,
                CreatedDate = now,
                ClassRoomName = selectedClassRoom.Name,
            });
            selectedUserProfile.Subscriptions = subscriptions;
            _userprofileRepo.UpsertUserProfile(selectedUserProfile);

            const int PrimaryContent = 1;
            var lessonActivities = selectedClassRoom.Lessons.Select(lesson =>
            {
                var selectedLessonCalendar = selectedClassCalendar.LessonCalendars
                    .Where(it => !it.DeletedDate.HasValue)
                    .FirstOrDefault(lc => lc.LessonId == lesson.id);

                var selectedLessonCatalog = lessonCatalogs
                    .FirstOrDefault(it => it.id == lesson.LessonCatalogId);

                return new UserActivity.LessonActivity
                {
                    id = Guid.NewGuid().ToString(),
                    BeginDate = selectedLessonCalendar.BeginDate,
                    TotalContentsAmount = selectedLessonCatalog.ExtraContentUrls.Count() + PrimaryContent,
                    LessonId = lesson.id,
                    SawContentIds = Enumerable.Empty<string>()
                };
            }).ToList();

            var userActivity = new UserActivity
            {
                id = Guid.NewGuid().ToString(),
                UserProfileName = selectedUserProfile.Name,
                UserProfileImageUrl = selectedUserProfile.ImageProfileUrl,
                UserProfileId = selectedUserProfile.id,
                ClassRoomId = selectedClassRoom.id,
                CreatedDate = now,
                LessonActivities = lessonActivities
            };
            _userActivityRepo.UpsertUserActivity(userActivity);
        }

        // GET: api/mycourse/{user-id}/{class-room-id}/info
        /// <summary>
        /// Get course information
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/{classRoomId}/info")]
        public GetCourseInfoRespond GetCourseInfo(string id, string classRoomId)
        {
            var isArgumentValid = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(classRoomId);
            if (!isArgumentValid) return null;

            var userProfile = _userprofileRepo.GetUserProfileById(id);
            if (userProfile == null || userProfile.DeletedDate.HasValue) return null;

            var classRoom = _classRoomRepo.GetClassRoomById(classRoomId);
            if (classRoom == null || classRoom.DeletedDate.HasValue) return null;

            var isUserProfileDataValid = userProfile.Subscriptions != null
                && userProfile.Subscriptions.Any();
            if (!isUserProfileDataValid) return null;

            var lastSubscription = userProfile.Subscriptions.OrderByDescending(x => x.LastActiveDate).FirstOrDefault(x => !x.DeletedDate.HasValue);
            if (lastSubscription == null) return null;

            var courseInfoRespond = new GetCourseInfoRespond()
            {
                UserProfileId = id,
                ClassRoomId = classRoomId,
                IsTeacher = lastSubscription.Role == UserProfile.AccountRole.Teacher,
                ClassName = lastSubscription.ClassRoomName,
                NumberOfStudents = 0,
            };

            if (lastSubscription.Role == UserProfile.AccountRole.Teacher)
            {
                var studentKey = _studentKeyRepo.GetStudentKeyByClassRoomId(classRoomId);
                if (studentKey != null && !studentKey.DeletedDate.HasValue) courseInfoRespond.CurrentStudentCode = studentKey.Code;
            }

            var classCarlendar = _classCalendarRepo.GetClassCalendarByClassRoomId(lastSubscription.ClassRoomId);
            if (classCarlendar != null && !classCarlendar.DeletedDate.HasValue) courseInfoRespond.StartDate = classCarlendar.BeginDate;

            return courseInfoRespond;
        }

        // PUT: api/mycourse/{user-id}
        /// <summary>
        /// Update course information
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="body">Request's information</param>
        [HttpPut]
        [Route("{id}")]
        public void Put(string id, UpdateCourseInfoRequest body)
        {
            var isArgumentValid = !string.IsNullOrEmpty(id)
                && body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.ClassName)
                && !string.IsNullOrEmpty(body.ChangedStudentCode);
            if (!isArgumentValid) return;

            var userprofile = _userprofileRepo.GetUserProfileById(id);
            if (userprofile == null) return;

            var isUserprofileValid = userprofile.Subscriptions != null && userprofile.Subscriptions.Any();
            if (!isUserprofileValid) return;

            var isCanUpdateCourse = userprofile.Subscriptions.Any(it => it.ClassRoomId == body.ClassRoomId && it.Role == UserProfile.AccountRole.Teacher);
            if (!isCanUpdateCourse) return;

            var now = _dateTime.GetCurrentTime();
            var isRequestUpdateStudentCode = !string.IsNullOrEmpty(body.ChangedStudentCode);
            if (isRequestUpdateStudentCode)
            {
                var selectedStudentKeyObj = _studentKeyRepo.GetStudentKeyByClassRoomId(body.ClassRoomId);
                if (selectedStudentKeyObj != null)
                {

                    if (!selectedStudentKeyObj.DeletedDate.HasValue)
                    {
                        selectedStudentKeyObj.DeletedDate = now;
                        _studentKeyRepo.UpsertStudentKey(selectedStudentKeyObj);
                    }

                    var newStudentKey = new StudentKey
                    {
                        id = Guid.NewGuid().ToString(),
                        Grade = selectedStudentKeyObj.Grade,
                        Code = body.ChangedStudentCode,
                        CourseCatalogId = selectedStudentKeyObj.CourseCatalogId,
                        ClassRoomId = selectedStudentKeyObj.ClassRoomId,
                        CreatedDate = now
                    };
                    _studentKeyRepo.UpsertStudentKey(newStudentKey);
                }
            }

            var isRequestUpdateClassName = !string.IsNullOrEmpty(body.ClassName);
            if (isRequestUpdateClassName)
            {
                var classRoom = _classRoomRepo.GetClassRoomById(body.ClassRoomId);
                if (classRoom != null)
                {
                    classRoom.Name = body.ClassName;
                    _classRoomRepo.UpsertClassRoom(classRoom);
                }
            }

            if (body.BeginDate.HasValue)
            {
                var classCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(body.ClassRoomId);
                var canUpdateClassCalendar = classCalendar != null;
                if (!canUpdateClassCalendar) return;

                classCalendar.BeginDate = body.BeginDate.Value;
                classCalendar.LastCalculateRequest = now;
                _classCalendarRepo.UpsertClassCalendar(classCalendar);
            }
        }

        // GET: api/mycourse/{user-id}/{class-room-id}/{lesson-id}
        /// <summary>
        /// Get likes 
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        /// <param name="lessonId">Lesson id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/{lessonId}")]
        public GetLikeRespond GetLikes(string id, string classRoomId, string lessonId)
        {
            var invalidDataRespond = new GetLikeRespond
            {
                LessonId = lessonId,
                LikeCommentIds = Enumerable.Empty<string>(),
                LikeDiscussionIds = Enumerable.Empty<string>()
            };
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId)
                && !string.IsNullOrEmpty(lessonId);
            if (!areArgumentsValid) return invalidDataRespond;

            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(id, classRoomId);
            if (!canAccessToTheClassRoom) return invalidDataRespond;

            var now = _dateTime.GetCurrentTime();
            var canAccessToTheClassLesson = _classCalendarRepo.CheckAccessPermissionToSelectedClassLesson(classRoomId, lessonId, now);
            if (!canAccessToTheClassLesson) return invalidDataRespond;

            var likeLessons = _likeLessonRepo.GetLikeLessonsByUserProfileIdAndLesson(id, lessonId);
            if (likeLessons == null) return invalidDataRespond;
            var likeComments = _likeCommentRepo.GetLikeCommentsByUserProfileIdAndLesson(id, lessonId);
            if (likeComments == null) return invalidDataRespond;
            var likeDiscussions = _likeDiscussionRepo.GetLikeDiscussionsByUserProfileIdAndLesson(id, lessonId);
            if (likeDiscussions == null) return invalidDataRespond;

            return new GetLikeRespond
            {
                LessonId = lessonId,
                IsLikedLesson = likeLessons.Any(it => !it.DeletedDate.HasValue),
                LikeCommentIds = likeComments.Where(it => !it.DeletedDate.HasValue).Select(it => it.id).ToList(),
                LikeDiscussionIds = likeDiscussions.Where(it => !it.DeletedDate.HasValue).Select(it => it.id).ToList(),
            };
        }

        #endregion Methods

        //// GET: api/mycourse
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/mycourse/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/mycourse
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/mycourse/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/mycourse/5
        //public void Delete(int id)
        //{
        //}
    }
}