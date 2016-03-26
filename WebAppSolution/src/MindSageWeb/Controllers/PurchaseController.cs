using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using MindSageWeb.ViewModels.PurchaseCourse;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;

namespace MindSageWeb.Controllers
{
    [Authorize]
    public class PurchaseController : Controller
    {
        #region Fields

        private IUserActivityRepository _userActivityRepo;
        private ILessonCatalogRepository _lessonCatalogRepo;
        private IClassCalendarRepository _classCalendarRepo;
        private IClassRoomRepository _classRoomRepo;
        private IUserProfileRepository _userprofileRepo;
        private IDateTime _dateTime;
        private CourseController _courseCtrl;
        private MyCourseController _myCourseCtrl;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize purchase controller
        /// </summary>
        /// <param name="courseCtrl">Course API</param>
        /// <param name="myCourseCtrl">MyCourse API</param>
        /// <param name="userProfileRepo">User profile repository</param>
        /// <param name="classRoomRepo">Class room repository</param>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="lessonCatalogRepo">Lesson catalog repository</param>
        /// <param name="userActivityRepo">User activity repository</param>
        public PurchaseController(CourseController courseCtrl, 
            MyCourseController myCourseCtrl,
            IUserProfileRepository userProfileRepo,
            IClassRoomRepository classRoomRepo,
            IClassCalendarRepository classCalendarRepo,
            ILessonCatalogRepository lessonCatalogRepo,
            IUserActivityRepository userActivityRepo,
            IDateTime dateTime)
        {
            _courseCtrl = courseCtrl;
            _myCourseCtrl = myCourseCtrl;
            _userprofileRepo = userProfileRepo;
            _classRoomRepo = classRoomRepo;
            _classCalendarRepo = classCalendarRepo;
            _lessonCatalogRepo = lessonCatalogRepo;
            _userActivityRepo = userActivityRepo;
            _dateTime = dateTime;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Add new course using a code
        /// </summary>
        /// <param name="id">Code</param>
        /// <param name="grade">Course's grade name</param>
        /// <param name="courseId">Select course id</param>
        [HttpPost]
        public IActionResult UserCode(string id, string grade, string courseId)
        {
            var body = new Repositories.Models.AddCourseRequest
            {
                Code = id,
                Grade = grade,
                UserProfileId = User.Identity.Name
            };

            var result = _myCourseCtrl.AddCourse(body);
            if (result.IsSuccess)
            {
                return RedirectToAction("Preparing", "My");
            }

            return RedirectToAction("Detail", "Home", new { @id = courseId, isCouponInvalid = true });
        }

        /// <summary>
        /// Credit card form
        /// </summary>
        /// <param name="id">Course id</param>
        [HttpGet]
        public IActionResult Index(string id)
        {
            var selectedCourse = _courseCtrl.GetCourseDetail(id);
            if (selectedCourse == null) return View("Error");

            var canAddNewCourse = _myCourseCtrl.CanAddNewCourseCatalog(User.Identity.Name, id);
            if (!canAddNewCourse) return RedirectToAction("entercourse", "my", new { @id = id });

            var model = new PurchaseCourseViewModel
            {
                CourseId = id,
                TotalChargeAmount = selectedCourse.PriceUSD
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(PurchaseCourseViewModel model)
        {
            var canAddNewCourse = _myCourseCtrl.CanAddNewCourseCatalog(User.Identity.Name, model.CourseId);
            if (!canAddNewCourse) return RedirectToAction("entercourse", "my", new { @id = model.CourseId });

            if (ModelState.IsValid)
            {
                var selectedCourse = _courseCtrl.GetCourseDetail(model.CourseId);
                if (selectedCourse == null) return View("Error");

                var selectedClassRoom = _classRoomRepo.GetPublicClassRoomByCourseCatalogId(model.CourseId);
                if (selectedClassRoom == null) return View("Error");

                var selectedUserProfile = _userprofileRepo.GetUserProfileById(User.Identity.Name);
                if (selectedUserProfile == null) return View("Error");

                // TODO: Pay with Paypal

                var now = _dateTime.GetCurrentTime();
                var lessonCatalogs = _lessonCatalogRepo.GetLessonCatalogById(selectedClassRoom.Lessons.Select(it => it.LessonCatalogId)).ToList();
                var newClassCalendar = createClassCalendar(selectedClassRoom, lessonCatalogs, now);
                newClassCalendar.CalculateCourseSchedule();

                var newSubscriptionId = string.Empty;
                selectedUserProfile.Subscriptions = addNewSelfPurchaseSubscription(
                    selectedUserProfile.Subscriptions, selectedClassRoom,
                    newClassCalendar.id, model.CourseId, 
                    now, out newSubscriptionId);

                var userActivity = selectedUserProfile.CreateNewUserActivity(selectedClassRoom, newClassCalendar, lessonCatalogs, now);

                _classCalendarRepo.UpsertClassCalendar(newClassCalendar);
                _userprofileRepo.UpsertUserProfile(selectedUserProfile);
                _userActivityRepo.UpsertUserActivity(userActivity);

                return RedirectToAction("Finished", new { @id = newSubscriptionId });
            }
            return View(model);
        }

        private IEnumerable<UserProfile.Subscription> addNewSelfPurchaseSubscription(IEnumerable<UserProfile.Subscription> subscriptions, ClassRoom selectedClassRoom, string classCalendarId, string courseCatalogId, DateTime currentTime, out string newSubscriptionId)
        {
            newSubscriptionId = Guid.NewGuid().ToString();
            var newSubscriptions = subscriptions.ToList();
            var newSubscription = new UserProfile.Subscription
            {
                id = newSubscriptionId,
                Role = UserProfile.AccountRole.SelfPurchaser,
                LastActiveDate = currentTime,
                ClassRoomId = selectedClassRoom.id,
                ClassCalendarId = classCalendarId,
                CreatedDate = currentTime,
                ClassRoomName = selectedClassRoom.Name,
                CourseCatalogId = courseCatalogId
            };
            newSubscriptions.Add(newSubscription);
            return newSubscriptions;
        }

        private ClassCalendar createClassCalendar(ClassRoom classRoom, IEnumerable<LessonCatalog> lessonCatalogs, DateTime currentTime)
        {
            var lessonOrderRunner = 1;
            var lessonCalendars = classRoom.Lessons.Select(it =>
            {
                var lessonCatalog = lessonCatalogs.FirstOrDefault(lc => lc.id == it.LessonCatalogId);
                if (lessonCatalog == null) return null;

                var topicOfTheDays = lessonCatalog.TopicOfTheDays.Select(totd => new ClassCalendar.TopicOfTheDay
                {
                    id = Guid.NewGuid().ToString(),
                    CreatedDate = currentTime,
                    Message = totd.Message,
                    SendOnDay = totd.SendOnDay,
                    RequiredSendTopicOfTheDayDate = currentTime
                }).ToList();

                var result = new ClassCalendar.LessonCalendar
                {
                    id = Guid.NewGuid().ToString(),
                    Order = lessonOrderRunner++,
                    BeginDate = currentTime.Date,
                    CreatedDate = currentTime,
                    LessonId = it.id,
                    SemesterGroupName = lessonCatalog.SemesterName,
                    TopicOfTheDays = topicOfTheDays
                };
                return result;
            }).Where(it => it != null).ToList();
            var classCalendar = new ClassCalendar
            {
                id = Guid.NewGuid().ToString(),
                BeginDate = currentTime.Date,
                ClassRoomId = classRoom.id,
                CreatedDate = currentTime,
                Holidays = Enumerable.Empty<DateTime>(),
                ShiftDays = Enumerable.Empty<DateTime>(),
                LessonCalendars = lessonCalendars,
            };
            return classCalendar;
        }

        /// <summary>
        /// Get purchased information
        /// </summary>
        /// <param name="id">Tracking id</param>
        [HttpGet]
        public IActionResult Finished(string id)
        {
            // HACK: Show billing address
            var model = new CoursePurchasedViewModel
            {
                AddressSummary = "Pimankhondopark Building 2 room number 989/148 Khonkean Naimung USA 40000",
                CardType = "VISA",
                CourseId = "CourseId01",
                LastFourDigits = "1234",
                TotalChargeAmount = 53.567
            };
            return View();
        }

        #endregion Methods
    }
}
