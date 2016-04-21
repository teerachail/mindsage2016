using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Controllers
{
    /// <summary>
    /// MyCourse API
    /// </summary>
    [Route("api/[controller]")]
    public class MyCourseController : Controller
    {
        #region Fields

        private const int LessonDuration = 5;
        private IClassRoomRepository _classRoomRepo;
        private IClassCalendarRepository _classCalendarRepo;
        private IUserProfileRepository _userprofileRepo;
        private IUserActivityRepository _userActivityRepo;
        private IStudentKeyRepository _studentKeyRepo;
        private ILessonCatalogRepository _lessonCatalogRepo;
        private ILikeLessonRepository _likeLessonRepo;
        private ILikeCommentRepository _likeCommentRepo;
        private ILikeDiscussionRepository _likeDiscussionRepo;
        private IContractRepository _contractRepo;
        private ICourseCatalogRepository _courseCatalogRepo;
        private ILogger _logger;
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
        /// <param name="contractRepo">Contract repository</param>
        /// <param name="likeCommentRepo">Like comment repository</param>
        /// <param name="likeDiscussionRepo">Like discussion repository</param>
        /// <param name="likeLessonRepo">Like lesson repository</param>
        /// <param name="courseCatalogRepo">Course catalog repository</param>
        public MyCourseController(IClassCalendarRepository classCalendarRepo,
            IUserProfileRepository userprofileRepo,
            IUserActivityRepository userActivityRepo,
            IClassRoomRepository classRoomRepo,
            IStudentKeyRepository studentKeyRepo,
            ILessonCatalogRepository lessonCatalogRepo,
            ILikeLessonRepository likeLessonRepo,
            ILikeCommentRepository likeCommentRepo,
            ILikeDiscussionRepository likeDiscussionRepo,
            IContractRepository contractRepo,
            ICourseCatalogRepository courseCatalogRepo,
            ILoggerFactory loggerFactory,
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
            _contractRepo = contractRepo;
            _courseCatalogRepo = courseCatalogRepo;
            _logger = loggerFactory.CreateLogger<MyCourseController>();
            _dateTime = dateTime;
        }

        #endregion Constructors

        #region Methods

        // GET: api/mycourse/{user-id}/{class-room-id}/{class-calendar-id}/map
        /// <summary>
        /// Get lesson map content
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        /// <param name="classCalendarId">Class calendar id</param>
        [HttpGet("{id}/{classRoomId}/{classCalendarId}/mapcontent")]
        public async Task<IEnumerable<CourseMapContentRespond>> Get(string id, string classRoomId, string classCalendarId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId)
                && !string.IsNullOrEmpty(classCalendarId);
            if (!areArgumentsValid) return Enumerable.Empty<CourseMapContentRespond>();

            UserProfile userprofile;
            if (!_userprofileRepo.CheckAccessPermissionToSelectedClassRoom(id, classRoomId, out userprofile)) return Enumerable.Empty<CourseMapContentRespond>();

            var isSelfPurchaseOrTeacher = userprofile.Subscriptions
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.ClassRoomId == classRoomId)
                .Any(it => it.Role == UserProfile.AccountRole.SelfPurchaser || it.Role == UserProfile.AccountRole.Teacher);

            var now = _dateTime.GetCurrentTime();
            var classCalendar = await _classCalendarRepo.GetClassCalendarById(classCalendarId);
            var canAccessToTheClassRoom = classCalendar != null
                && classCalendar.LessonCalendars != null
                && !classCalendar.DeletedDate.HasValue
                && !classCalendar.CloseDate.HasValue
                && (classCalendar.ExpiredDate.HasValue && classCalendar.ExpiredDate.Value.Date > now.Date) || isSelfPurchaseOrTeacher;
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

        // GET: api/mycourse/{user-id}/{class-room-id}/{class-calendar-id}/status
        /// <summary>
        /// Get lesson map status
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        /// <param name="classCalendarId">Class calendar id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/{classCalendarId}/mapstatus")]
        public IEnumerable<CourseMapStatusRespond> GetStatus(string id, string classRoomId, string classCalendarId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                 && !string.IsNullOrEmpty(classRoomId)
                 && !string.IsNullOrEmpty(classCalendarId);
            if (!areArgumentsValid) return Enumerable.Empty<CourseMapStatusRespond>();

            if (!_userprofileRepo.CheckAccessPermissionToSelectedClassRoom(id, classRoomId)) return Enumerable.Empty<CourseMapStatusRespond>();

            var now = _dateTime.GetCurrentTime();
            var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(id, classRoomId);
            var canAccessUserActivit = selectedUserActivity != null
                && selectedUserActivity.LessonActivities != null
                && !selectedUserActivity.DeletedDate.HasValue;
            if (!canAccessUserActivit) return Enumerable.Empty<CourseMapStatusRespond>();

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
                        IsPrivateAccount = it.IsPrivateAccount,
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
            var classCalendars = _classCalendarRepo.GetClassCalendarByClassRoomId(subscriptionQry.Select(it => it.ClassRoomId).Distinct())
                .Where(it => it != null)
                .Where(it => it.BeginDate.HasValue)
                .Where(it => it.BeginDate <= now.Date)
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => !it.CloseDate.HasValue);

            var availablePrivateClassCalendars = subscriptionQry
                .Where(it => it.Role != UserProfile.AccountRole.SelfPurchaser)
                .Select(it => classCalendars.FirstOrDefault(cc => cc.ClassRoomId == it.ClassRoomId))
                .Where(it => it != null)
                .Where(it => it.ExpiredDate.HasValue)
                .Where(it => it.ExpiredDate.Value.Date > now.Date);
            var availablePublicClassCalendars = subscriptionQry
                .Where(it => it.Role == UserProfile.AccountRole.SelfPurchaser)
                .Select(it => classCalendars.FirstOrDefault(cc => cc.ClassRoomId == it.ClassRoomId))
                .Where(it => it != null)
                .Where(it => !it.ExpiredDate.HasValue);
            var availableClassRooms = availablePrivateClassCalendars.Union(availablePublicClassCalendars).ToList();

            var availableSubscriptions = subscriptionQry
                .Where(it => availableClassRooms.Any(cc => cc.ClassRoomId == it.ClassRoomId))
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
                        LessonId = currentLesson.LessonId,
                        CourseCatalogId = subscription.CourseCatalogId,
                        ClassCalendarId = subscription.ClassCalendarId
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

            var isTeacherAccount = userprofile.Subscriptions
                .Any(it => !it.DeletedDate.HasValue && it.ClassRoomId == body.ClassRoomId && it.Role == UserProfile.AccountRole.Teacher);
            if (!isTeacherAccount) return;

            var selectedUserProfile = _userprofileRepo.GetUserProfileById(body.RemoveUserProfileId);
            if (selectedUserProfile == null) return;

            var selectedSubscriptions = selectedUserProfile.Subscriptions
                .Where(it => it.ClassRoomId.Equals(body.ClassRoomId))
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.Role == UserProfile.AccountRole.Student);
            if (!selectedSubscriptions.Any()) return;

            foreach (var item in selectedSubscriptions) item.DeletedDate = _dateTime.GetCurrentTime();
            _userprofileRepo.UpsertUserProfile(selectedUserProfile);

            var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(body.RemoveUserProfileId, body.ClassRoomId);
            if (selectedUserActivity == null) return;
            selectedUserActivity.DeletedDate = _dateTime.GetCurrentTime();
            _userActivityRepo.UpsertUserActivity(selectedUserActivity);
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
        public async Task<AddCourseRespond> AddCourse(AddCourseRequest body)
        {
            var addCourseFailRespond = new AddCourseRespond
            {
                Code = body.Code,
                Grade = body.Grade,
            };
            
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.UserProfileId)
                && !string.IsNullOrEmpty(body.Code)
                && !string.IsNullOrEmpty(body.Grade);
            if (!areArgumentsValid) return addCourseFailRespond;

            var selectedUserProfile = _userprofileRepo.GetUserProfileById(body.UserProfileId);
            var isValidUserProfile = selectedUserProfile != null && selectedUserProfile.Subscriptions != null;
            if (!isValidUserProfile) return addCourseFailRespond;

            var now = _dateTime.GetCurrentTime();
            var selectedStudentKey = _studentKeyRepo.GetStudentKeyByCodeAndGrade(body.Code, body.Grade);
            var isStudentKey = selectedStudentKey != null && !selectedStudentKey.DeletedDate.HasValue;
            var isSuccessed = isStudentKey ?
                addNewCourseByStudentCode(selectedUserProfile, selectedStudentKey, now) :
                await addNewCourseByTeacherCode(body.Code, body.Grade, selectedUserProfile, now);
            return new AddCourseRespond
            {
                Code = body.Code,
                Grade = body.Grade,
                IsSuccess = isSuccessed
            };
        }
        private bool addNewCourseByStudentCode(UserProfile selectedUserProfile, StudentKey selectedStudentKey, DateTime currentTime)
        {
            var canUserprofileAccessToTheClassRoom = selectedUserProfile.Subscriptions
                .Where(it => !it.DeletedDate.HasValue)
                .Any(it => it.ClassRoomId == selectedStudentKey.ClassRoomId);
            if (canUserprofileAccessToTheClassRoom) return false;

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(selectedStudentKey.ClassRoomId);
            if (selectedClassRoom == null) return false;

            var selectedClassCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(selectedStudentKey.ClassRoomId);
            if (selectedClassCalendar == null) return false;

            var lessonCatalogs = selectedClassRoom.Lessons
                .Select(it => _lessonCatalogRepo.GetLessonCatalogById(it.LessonCatalogId))
                .ToList();
            if (lessonCatalogs.Any(it => it == null)) return false;

            var teacherSubscription = _userprofileRepo.GetUserProfilesByClassRoomId(selectedClassRoom.id)
                .Where(it => !it.DeletedDate.HasValue)
                .SelectMany(it => it.Subscriptions)
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.Role == UserProfile.AccountRole.Teacher)
                .Where(it => it.ClassRoomId == selectedClassRoom.id)
                .FirstOrDefault();
            if (teacherSubscription == null)
            {
                _logger.LogWarning($"Teacher account in the private ClassRoomId: { selectedStudentKey.ClassRoomId } not found.");
                return false;
            }

            selectedUserProfile.Subscriptions = createNewSubscription(selectedUserProfile.Subscriptions,
                UserProfile.AccountRole.Student,
                selectedClassRoom,
                selectedClassCalendar.id,
                teacherSubscription.LicenseId,
                currentTime);
            _userprofileRepo.UpsertUserProfile(selectedUserProfile);

            var userActivity = selectedUserProfile.CreateNewUserActivity(selectedClassRoom, selectedClassCalendar, lessonCatalogs, currentTime);
            _userActivityRepo.UpsertUserActivity(userActivity);

            return true;
        }
        private async Task<bool> addNewCourseByTeacherCode(string code, string grade, UserProfile selectedUserProfile, DateTime currentTime)
        {
            var selectedContract = await _contractRepo.GetContractsByTeacherCode(code, grade);
            var isTeacherKey = selectedContract != null
                && !selectedContract.DeletedDate.HasValue
                && selectedContract.Licenses.Any();
            if (!isTeacherKey) return false;

            var selectedLicense = selectedContract.Licenses
                .Where(it => !it.DeletedDate.HasValue)
                .FirstOrDefault(it => it.TeacherKeys.Any(t => !t.DeletedDate.HasValue && t.Code == code && t.Grade == grade));
            if (selectedLicense == null) return false;

            var selectedTeacherKey = selectedLicense.TeacherKeys
                .Where(it => !it.DeletedDate.HasValue)
                .OrderBy(it => it.CreatedDate)
                .LastOrDefault();
            if (selectedTeacherKey == null) return false;

            // Create new ClassRoom
            var selectedCourseCatalog = _courseCatalogRepo.GetCourseCatalogById(selectedLicense.CourseCatalogId);
            if (selectedCourseCatalog == null) return false;
            var lessonCatalogs = _lessonCatalogRepo.GetLessonCatalogByCourseCatalogId(selectedCourseCatalog.id).ToList();
            if (lessonCatalogs == null || !lessonCatalogs.Any()) return false;
            var lessons = lessonCatalogs
                .Where(it => !it.DeletedDate.HasValue)
                .Select(it => new ClassRoom.Lesson
                {
                    id = it.id,
                    LessonCatalogId = it.id
                });
            var newClassRoom = new ClassRoom
            {
                id = Guid.NewGuid().ToString(),
                Name = selectedCourseCatalog.SideName,
                CourseCatalogId = selectedCourseCatalog.id,
                CreatedDate = currentTime,
                LastUpdatedMessageDate = currentTime,
                Lessons = lessons
            };
            await _classRoomRepo.CreateNewClassRoom(newClassRoom);

            // Create new ClassCalendar
            var lessonCalendars = lessonCatalogs
                .Where(it => !it.DeletedDate.HasValue)
                .Select(lesson => new ClassCalendar.LessonCalendar
                {
                    id = Guid.NewGuid().ToString(),
                    Order = lesson.Order,
                    SemesterGroupName = lesson.SemesterName,
                    BeginDate = currentTime,
                    CreatedDate = currentTime,
                    LessonId = lesson.id,
                    TopicOfTheDays = lesson.TopicOfTheDays
                        .Where(it => !it.DeletedDate.HasValue)
                        .Select(it => new ClassCalendar.TopicOfTheDay
                        {
                            id = it.id,
                            Message = it.Message,
                            SendOnDay = it.SendOnDay,
                            CreatedDate = currentTime,
                        })
                });
            var newClassCalendar = new ClassCalendar
            {
                id = Guid.NewGuid().ToString(),
                ClassRoomId = newClassRoom.id,
                CreatedDate = currentTime,
                Holidays = Enumerable.Empty<DateTime>(),
                ShiftDays = Enumerable.Empty<DateTime>(),
                LessonCalendars = lessonCalendars
            };
            await _classCalendarRepo.CreateNewClassCalendar(newClassCalendar);

            // Create new UserActivity
            var userActivity = selectedUserProfile.CreateNewUserActivity(newClassRoom, newClassCalendar, lessonCatalogs, currentTime);
            _userActivityRepo.UpsertUserActivity(userActivity);

            // Create new subscription
            var subscriptions = createNewSubscription(selectedUserProfile.Subscriptions, UserProfile.AccountRole.Teacher, newClassRoom, newClassCalendar.id, selectedLicense.id, currentTime);
            selectedUserProfile.Subscriptions = subscriptions;
            _userprofileRepo.UpsertUserProfile(selectedUserProfile);

            // Create new student key
            var newStudentKey = new StudentKey
            {
                id = Guid.NewGuid().ToString(),
                Grade = grade,
                Code = generateStudentCode(grade),
                CourseCatalogId = selectedCourseCatalog.id,
                ClassRoomId = newClassRoom.id,
                CreatedDate = currentTime
            };

            await _studentKeyRepo.CreateNewStudentKey(newStudentKey);
            return true;
        }
        private string generateStudentCode(string grade)
        {
            var newStudentCode = string.Empty;
            while (true)
            {
                newStudentCode = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 7);
                var selectedStudentKey = _studentKeyRepo.GetStudentKeyByCodeAndGrade(newStudentCode, grade);
                if (selectedStudentKey == null) break;
            }
            return newStudentCode;
        }
        private IEnumerable<UserProfile.Subscription> createNewSubscription(IEnumerable<UserProfile.Subscription> subscriptions, UserProfile.AccountRole role, ClassRoom classRoom, string classCalendarId, string licenseId, DateTime currentTime)
        {
            var result = subscriptions.ToList();
            result.Add(new UserProfile.Subscription
            {
                id = Guid.NewGuid().ToString(),
                Role = role,
                LastActiveDate = currentTime,
                ClassRoomId = classRoom.id,
                ClassCalendarId = classCalendarId,
                CreatedDate = currentTime,
                LicenseId = licenseId,
                ClassRoomName = classRoom.Name,
                CourseCatalogId = classRoom.CourseCatalogId
            });
            return result;
        }

        // POST: api/mycourse/changecourse
        /// <summary>
        /// Change course
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("changecourse")]
        public GetCourseInfoRespond ChangeCourse(ChangeCourseRequest body)
        {
            var isArgumentValid = body != null && !string.IsNullOrEmpty(body.UserProfileId) && !string.IsNullOrEmpty(body.ClassRoomId);
            if (!isArgumentValid) return null;

            var userprofileId = body.UserProfileId;
            var classRoomId = body.ClassRoomId;

            var userProfile = _userprofileRepo.GetUserProfileById(userprofileId);
            if (userProfile == null || userProfile.DeletedDate.HasValue) return null;

            var classRoom = _classRoomRepo.GetClassRoomById(classRoomId);
            if (classRoom == null || classRoom.DeletedDate.HasValue) return null;

            var isUserProfileDataValid = userProfile.Subscriptions != null
                && userProfile.Subscriptions.Any()
                && userProfile.Subscriptions.Where(it => !it.DeletedDate.HasValue).Any(it => it.ClassRoomId == classRoomId);
            if (!isUserProfileDataValid) return null;

            var selectedSubscription = userProfile.Subscriptions
                    .Where(it => !it.DeletedDate.HasValue)
                    .FirstOrDefault(it => it.ClassRoomId == classRoomId);
            if (selectedSubscription != null)
            {
                var now = _dateTime.GetCurrentTime();
                selectedSubscription.LastActiveDate = now;
                _userprofileRepo.UpsertUserProfile(userProfile);
            }

            var lastSubscription = userProfile.Subscriptions.OrderByDescending(it => it.LastActiveDate).FirstOrDefault(it => !it.DeletedDate.HasValue);
            if (lastSubscription == null) return null;

            var courseInfoRespond = new GetCourseInfoRespond()
            {
                UserProfileId = userprofileId,
                ClassRoomId = classRoom.id,
                IsTeacher = lastSubscription.Role == UserProfile.AccountRole.Teacher,
                ClassName = classRoom.Name,
                IsSelfPurchase = selectedSubscription.Role == UserProfile.AccountRole.SelfPurchaser,
            };

            if (lastSubscription.Role == UserProfile.AccountRole.Teacher)
            {
                var studentKey = _studentKeyRepo.GetStudentKeyByClassRoomId(classRoomId);
                if (studentKey != null && !studentKey.DeletedDate.HasValue) courseInfoRespond.CurrentStudentCode = studentKey.Code;
            }

            var classCarlendar = _classCalendarRepo.GetClassCalendarByClassRoomId(lastSubscription.ClassRoomId);
            if (classCarlendar != null && !classCarlendar.DeletedDate.HasValue) courseInfoRespond.StartDate = classCarlendar.BeginDate;

            courseInfoRespond.NumberOfStudents = _userprofileRepo.GetUserProfilesByClassRoomId(classRoomId).Count();
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
                && !string.IsNullOrEmpty(body.ClassRoomId);
            if (!isArgumentValid) return;

            var userprofile = _userprofileRepo.GetUserProfileById(id);
            if (userprofile == null) return;

            var isUserprofileValid = userprofile.Subscriptions != null && userprofile.Subscriptions.Any();
            if (!isUserprofileValid) return;

            var isCanUpdateCourse = userprofile.Subscriptions.Any(it => !it.DeletedDate.HasValue && it.ClassRoomId == body.ClassRoomId && it.Role == UserProfile.AccountRole.Teacher);
            if (!isCanUpdateCourse) return;

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(body.ClassRoomId);
            var canAccessCalssRoom = selectedClassRoom != null && !selectedClassRoom.DeletedDate.HasValue;
            if (!canAccessCalssRoom) return;

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
                    _studentKeyRepo.CreateNewStudentKey(newStudentKey);
                }
            }

            var isRequestUpdateClassName = !string.IsNullOrEmpty(body.ClassName);
            if (isRequestUpdateClassName)
            {
                selectedClassRoom.Name = body.ClassName;
                _classRoomRepo.UpsertClassRoom(selectedClassRoom);
            }

            //if (body.BeginDate.HasValue)
            //{
            //    var classCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(body.ClassRoomId);
            //    var canUpdateClassCalendar = classCalendar != null;
            //    if (!canUpdateClassCalendar) return;

            //    classCalendar.BeginDate = body.BeginDate.Value;
            //    _classCalendarRepo.UpsertClassCalendar(classCalendar);
            //}
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

            var likeCommentIds = likeComments.Where(it => !it.DeletedDate.HasValue).Select(it => it.CommentId).ToList();
            var likeDiscussionIds = likeDiscussions.Where(it => !it.DeletedDate.HasValue).Select(it => it.DiscussionId).ToList();
            return new GetLikeRespond
            {
                LessonId = lessonId,
                IsLikedLesson = likeLessons.Any(it => !it.DeletedDate.HasValue),
                LikeCommentIds = likeCommentIds,
                LikeDiscussionIds = likeDiscussionIds
            };
        }

        // GET: api/mycourse/{user-id}/{class-room-id}/alllikes
        /// <summary>
        /// Get likes 
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        /// <param name="lessonId">Lesson id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/alllikes")]
        public GetLikeRespond GetAllLikes(string id, string classRoomId)
        {
            var invalidDataRespond = new GetLikeRespond
            {
                LikeCommentIds = Enumerable.Empty<string>(),
                LikeDiscussionIds = Enumerable.Empty<string>()
            };
            var areArgumentsValid = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return invalidDataRespond;

            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(id, classRoomId);
            if (!canAccessToTheClassRoom) return invalidDataRespond;

            var now = _dateTime.GetCurrentTime();

            var likeComments = _likeCommentRepo.GetLikeCommentsByUserProfileIdAndClassRoomId(id, classRoomId);
            if (likeComments == null) return invalidDataRespond;
            var likeDiscussions = _likeDiscussionRepo.GetLikeDiscussionsByUserProfileIdAndClassRoomId(id, classRoomId);
            if (likeDiscussions == null) return invalidDataRespond;

            var likeCommentIds = likeComments.Where(it => !it.DeletedDate.HasValue).Select(it => it.CommentId).ToList();
            var likeDiscussionIds = likeDiscussions.Where(it => !it.DeletedDate.HasValue).Select(it => it.DiscussionId).ToList();
            return new GetLikeRespond
            {
                LikeCommentIds = likeCommentIds,
                LikeDiscussionIds = likeDiscussionIds
            };
        }

        #endregion Methods

        // GET: api/mycourse/{user-id}/{class-room-id}/schedule
        /// <summary>
        /// Get course schedule
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/{classRoomId}/schedule")]
        public GetCourseScheduleRespond GetCourseSchedule(string id, string classRoomId)
        {
            var areArgumentValid = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentValid) return null;

            var canAccessToCourse = validateAccessToCourseScheduleManagement(id, classRoomId);
            if (!canAccessToCourse) return null;

            var classCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(classRoomId);
            var canAccessClassCalendar = classCalendar != null
                && !classCalendar.DeletedDate.HasValue
                && !classCalendar.CloseDate.HasValue;
            if (!canAccessClassCalendar) return null;

            var result = getCourseSchedule(classCalendar, true);
            return result;
        }

        // POST: api/mycourse/startdate
        /// <summary>
        /// Add or change startdate
        /// </summary>
        /// <param name="body">Start date's information</param>
        [HttpPost]
        [Route("startdate")]
        public GetCourseScheduleRespond SetStartDate(SetStartDateRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.UserProfileId)
                && !string.IsNullOrEmpty(body.ClassRoomId);
            if (!areArgumentsValid) return null;

            var canAccessToCourse = validateAccessToCourseScheduleManagement(body.UserProfileId, body.ClassRoomId);
            if (!canAccessToCourse) return null;

            var classCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(body.ClassRoomId);
            if (classCalendar == null) return null;

            var canSetStartDate = !classCalendar.BeginDate.HasValue
                || _dateTime.GetCurrentTime().ToUniversalTime().Date <= body.BeginDate.ToUniversalTime().Date;
            if (canSetStartDate)
            {
                classCalendar.BeginDate = body.BeginDate.Date.ToUniversalTime();
                classCalendar.CalculateCourseSchedule();
                _classCalendarRepo.UpsertClassCalendar(classCalendar);
            }

            var result = getCourseSchedule(classCalendar, true);
            return result;
        }

        // POST: api/mycourse/schedulerange
        /// <summary>
        /// Edit Schedule with range
        /// </summary>
        /// <param name="body">Start date's information</param>
        [HttpPost]
        [Route("schedulerange")]
        public GetCourseScheduleRespond SetScheduleWithRange(SetScheduleWithRangeRequest body)
        {
            var areArgumentValid = body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!areArgumentValid) return null;

            var canAccessToCourse = validateAccessToCourseScheduleManagement(body.UserProfileId, body.ClassRoomId);
            if (!canAccessToCourse) return null;

            var classCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(body.ClassRoomId);
            if (classCalendar == null) return null;

            var requestValid = classCalendar.BeginDate.HasValue;
            if (requestValid)
            {
                var dateRange = Enumerable.Empty<DateTime>();
                var isMoreThanOneDay = body.ToDate.HasValue;
                if (isMoreThanOneDay)
                {
                    var isTodayIsCorrect = body.ToDate.Value > body.FromDate;
                    var fromDate = (isTodayIsCorrect ? body.FromDate : body.ToDate.Value).Date;
                    var toDate = (isTodayIsCorrect ? body.ToDate.Value : body.FromDate).Date;
                    const int ShiftOneDay = 1;
                    var diffDays = (toDate - fromDate).Days + ShiftOneDay;
                    dateRange = Enumerable.Range(0, diffDays).Select(it => fromDate.AddDays(it).ToUniversalTime());
                }
                else dateRange = new List<DateTime> { body.FromDate.Date.ToUniversalTime() };

                var holidays = classCalendar.Holidays ?? Enumerable.Empty<DateTime>();
                holidays = body.IsHoliday ? holidays.Union(dateRange) : holidays.Except(dateRange);
                classCalendar.Holidays = holidays.Distinct();

                var shiftDays = classCalendar.ShiftDays ?? Enumerable.Empty<DateTime>();
                shiftDays = body.IsShift ? shiftDays.Union(dateRange) : shiftDays.Except(dateRange);
                classCalendar.ShiftDays = shiftDays.Distinct();

                classCalendar.CalculateCourseSchedule();
                _classCalendarRepo.UpsertClassCalendar(classCalendar);
            }

            var result = getCourseSchedule(classCalendar, requestValid);
            return result;
        }

        // POST: api/mycourse/scheduleweek
        /// <summary>
        /// Edit Schedule with week days
        /// </summary>
        /// <param name="body">Start date's information</param>
        [HttpPost]
        [Route("scheduleweek")]
        public GetCourseScheduleRespond SetScheduleWithWeek(SetScheduleWithWeekRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.UserProfileId)
                && !string.IsNullOrEmpty(body.ClassRoomId);
            if (!areArgumentsValid) return null;

            var canAccessToCourse = validateAccessToCourseScheduleManagement(body.UserProfileId, body.ClassRoomId);
            if (!canAccessToCourse) return null;

            var classCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(body.ClassRoomId);
            if (classCalendar == null) return null;

            var isRequestValid = classCalendar.BeginDate.HasValue && classCalendar.ExpiredDate.HasValue;
            if (isRequestValid)
            {
                var beginDate = classCalendar.BeginDate.Value.ToUniversalTime();
                var endedDate = classCalendar.ExpiredDate.Value.ToUniversalTime();

                const int ShiftTwoDays = 2;
                var diffDays = (int)(Math.Abs((beginDate - endedDate).TotalDays) + ShiftTwoDays);
                var dateRange = Enumerable.Range(0, diffDays).Select(it => beginDate.AddDays(it));

                classCalendar.Holidays = classCalendar.Holidays ?? Enumerable.Empty<DateTime>();
                classCalendar.ShiftDays = classCalendar.ShiftDays ?? Enumerable.Empty<DateTime>();

                if (body.IsSunday) setHolidayAndShiftDate(classCalendar, body, DayOfWeek.Sunday, dateRange);
                if (body.IsMonday) setHolidayAndShiftDate(classCalendar, body, DayOfWeek.Monday, dateRange);
                if (body.IsTuesday) setHolidayAndShiftDate(classCalendar, body, DayOfWeek.Tuesday, dateRange);
                if (body.IsWednesday) setHolidayAndShiftDate(classCalendar, body, DayOfWeek.Wednesday, dateRange);
                if (body.IsThursday) setHolidayAndShiftDate(classCalendar, body, DayOfWeek.Thursday, dateRange);
                if (body.IsFriday) setHolidayAndShiftDate(classCalendar, body, DayOfWeek.Friday, dateRange);
                if (body.IsSaturday) setHolidayAndShiftDate(classCalendar, body, DayOfWeek.Saturday, dateRange);

                classCalendar.Holidays = classCalendar.Holidays.Distinct();
                classCalendar.ShiftDays = classCalendar.ShiftDays.Distinct();

                classCalendar.CalculateCourseSchedule();
                _classCalendarRepo.UpsertClassCalendar(classCalendar);
            }

            var result = getCourseSchedule(classCalendar, isRequestValid);
            return result;
        }
        private void setHolidayAndShiftDate(ClassCalendar classCalendar, SetScheduleWithWeekRequest body, DayOfWeek day, IEnumerable<DateTime> dateRange)
        {
            var qry = dateRange.Where(it => it.DayOfWeek == day);
            classCalendar.Holidays = body.IsHoliday ? classCalendar.Holidays.Union(qry) : classCalendar.Holidays.Except(qry);
            classCalendar.ShiftDays = body.IsShift ? classCalendar.ShiftDays.Union(qry) : classCalendar.ShiftDays.Except(qry);
        }

        // POST: api/mycourse/applytoall
        /// <summary>
        /// Set schedule apply to all course.
        /// </summary>
        /// <param name="body">Start date's information</param>
        [HttpPost]
        [Route("applytoall")]
        public void ApplyToAllCourse(ApplyToAllCourseRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.UserProfileId)
                && !string.IsNullOrEmpty(body.ClassRoomId);
            if (!areArgumentsValid) return;

            UserProfile userprofile;
            var canAccessToCourse = validateAccessToCourseScheduleManagement(body.UserProfileId, body.ClassRoomId, out userprofile);
            if (!canAccessToCourse) return;

            var classCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(body.ClassRoomId);
            if (classCalendar == null) return;

            var reqChangeCourseScheduleClassRoomIds = userprofile.Subscriptions
                .Where(it => it.ClassRoomId != body.ClassRoomId)
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.Role == UserProfile.AccountRole.Teacher)
                .Select(it => it.ClassRoomId);
            if (!reqChangeCourseScheduleClassRoomIds.Any()) return;

            var reqApplyScheduleClassCalendars = _classCalendarRepo.GetClassCalendarByClassRoomId(reqChangeCourseScheduleClassRoomIds)
                .Where(it => it != null)
                .Where(it => !it.CloseDate.HasValue)
                .Where(it => !it.DeletedDate.HasValue)
                .ToList();

            reqApplyScheduleClassCalendars.ForEach(it =>
            {
                it.BeginDate = classCalendar.BeginDate;
                it.ExpiredDate = classCalendar.ExpiredDate;
                it.ShiftDays = classCalendar.ShiftDays;
                it.Holidays = classCalendar.Holidays;
                it.CalculateCourseSchedule();
            });

            reqApplyScheduleClassCalendars.ForEach(it => _classCalendarRepo.UpsertClassCalendar(it));
        }

        private GetCourseScheduleRespond getCourseSchedule(ClassCalendar classCalendar, bool isComplete)
        {
            var runningLessonId = 1;
            var result = new GetCourseScheduleRespond
            {
                IsComplete = isComplete,
                BeginDate = classCalendar.BeginDate?.ToUniversalTime(),
                EndDate = classCalendar.ExpiredDate?.ToUniversalTime(),
                Lessons = classCalendar.LessonCalendars?.Select(it => new LessonSchedule
                {
                    BeginDate = it.BeginDate.ToUniversalTime(),
                    Name = $"Lesson {runningLessonId++}"
                }).ToList(),
                Holidays = classCalendar.Holidays ?? Enumerable.Empty<DateTime>(),
                ShiftDays = classCalendar.ShiftDays ?? Enumerable.Empty<DateTime>(),
            };
            return result;
        }
        private bool validateAccessToCourseScheduleManagement(string userprofileId, string classRoomId)
        {
            UserProfile userprofile;
            return validateAccessToCourseScheduleManagement(userprofileId, classRoomId, out userprofile);
        }
        private bool validateAccessToCourseScheduleManagement(string userprofileId, string classRoomId, out UserProfile userprofile)
        {
            var canAccessToTheClassRoom = _userprofileRepo
                .CheckAccessPermissionToSelectedClassRoom(userprofileId, classRoomId, out userprofile);
            if (!canAccessToTheClassRoom) return false;
            var isTeacher = userprofile.Subscriptions
                .Where(it => !it.DeletedDate.HasValue && it.ClassRoomId == classRoomId)
                .Any(it => it.Role == UserProfile.AccountRole.Teacher);
            return isTeacher;
        }

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