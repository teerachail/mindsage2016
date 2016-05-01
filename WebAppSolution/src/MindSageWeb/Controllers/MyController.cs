using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using MindSageWeb.Repositories;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.Logging;

namespace MindSageWeb.Controllers
{
    [Authorize]
    public class MyController : Controller
    {
        private IDateTime _dateTime;
        private IClassCalendarRepository _classCalendarRepo;
        private IUserProfileRepository _userprofileRepo;
        private MyCourseController _myCourseCtrl;
        private ProfileController _profileCtrl;
        private ErrorMessageOptions _errorMsgs;
        private ILogger _logger;

        public MyController(MyCourseController myCourseCtrl,
            ProfileController profileCtrl,
            IUserProfileRepository userprofileRepo,
            IClassCalendarRepository classCalendarRepo,
            IOptions<ErrorMessageOptions> errorMsg,
            ILoggerFactory loggerFactory,
            IDateTime dateTime)
        {
            _myCourseCtrl = myCourseCtrl;
            _profileCtrl = profileCtrl;
            _userprofileRepo = userprofileRepo;
            _classCalendarRepo = classCalendarRepo;
            _errorMsgs = errorMsg.Value;
            _logger = loggerFactory.CreateLogger<MyController>();
            _dateTime = dateTime;
        }

        // GET: /<controller>/
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Enter to the user's course
        /// </summary>
        /// <param name="id">Course catalog id</param>
        public IActionResult EnterCourse(string id)
        {
            var userprofile = _userprofileRepo?.GetUserProfileById(User.Identity.Name);
            if (userprofile == null)
            {
                _logger.LogCritical($"User profile { User.Identity.Name } not found.");
                ViewBag.ErrorMessage = _errorMsgs.AccountNotFound;
                return View("Error");
            }

            var lastActiveSubscription = userprofile.Subscriptions?
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.CourseCatalogId == id)
                .OrderBy(it => it.LastActiveDate)
                .LastOrDefault();
            var isAnyActivatedSubscription = lastActiveSubscription != null;
            if (!isAnyActivatedSubscription)
            {
                ViewBag.ErrorMessage = _errorMsgs.NoLastActivatedCourse;
                return View("Error");
            }

            var courseInfo = _myCourseCtrl?.ChangeCourse(new Repositories.Models.ChangeCourseRequest
            {
                UserProfileId = User.Identity.Name,
                ClassRoomId = lastActiveSubscription.ClassRoomId
            });

            return RedirectToAction("Preparing", "My");
        }

        /// <summary>
        /// Find the user's last activate course and navigate user to that course
        /// </summary>
        public IActionResult Preparing()
        {
            var userprofile = _userprofileRepo.GetUserProfileById(User.Identity.Name);
            if (userprofile == null)
            {
                _logger.LogCritical($"User profile { User.Identity.Name } not found.");
                ViewBag.ErrorMessage = _errorMsgs.AccountNotFound;
                return View("Error");
            }

            var lastActiveSubscription = userprofile.Subscriptions
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.LastActiveDate.HasValue)
                .OrderByDescending(it => it.LastActiveDate)
                .FirstOrDefault();
            var isAnyActivatedSubscription = lastActiveSubscription != null;
            if (!isAnyActivatedSubscription)
            {
                ViewBag.ErrorMessage = _errorMsgs.NoLastActivatedCourse;
                return View("Error");
            }

            var classCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(lastActiveSubscription.ClassRoomId);
            var isClassCalendarValid = classCalendar != null && classCalendar.LessonCalendars != null && classCalendar.LessonCalendars.Any(it => !it.DeletedDate.HasValue);
            if (!isClassCalendarValid)
            {
                ViewBag.ErrorMessage = _errorMsgs.EntireCourseIsIncomplete;
                return View("Error");
            }

            var now = _dateTime.GetCurrentTime();
            var currentLessonCalendar = classCalendar.LessonCalendars
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => now.Date >= it.BeginDate)
                .OrderByDescending(it => it.BeginDate)
                .FirstOrDefault() ?? classCalendar.LessonCalendars.OrderBy(it => it.BeginDate).FirstOrDefault();
            var isCurrentLessonValid = currentLessonCalendar != null;
            if (!isCurrentLessonValid)
            {
                ViewBag.ErrorMessage = _errorMsgs.EntireCourseIsIncomplete;
                return View("Error");
            }

            var redirectURL = $"/my#!/app/main/lesson/{ currentLessonCalendar.LessonId }/{ lastActiveSubscription.ClassRoomId }";
            return Redirect(redirectURL);
        }
    }
}
