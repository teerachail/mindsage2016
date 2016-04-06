using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using MindSageWeb.Repositories;

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

        public MyController(MyCourseController myCourseCtrl,
            ProfileController profileCtrl,
            IUserProfileRepository userprofileRepo,
            IClassCalendarRepository classCalendarRepo,
            IDateTime dateTime)
        {
            _myCourseCtrl = myCourseCtrl;
            _profileCtrl = profileCtrl;
            _userprofileRepo = userprofileRepo;
            _classCalendarRepo = classCalendarRepo;
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
            if (userprofile == null) return View("Error");

            var subscription = userprofile.Subscriptions?
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.CourseCatalogId == id)
                .OrderBy(it => it.LastActiveDate)
                .LastOrDefault();
            if (subscription == null) return View("Error");

            var courseInfo = _myCourseCtrl?.ChangeCourse(new Repositories.Models.ChangeCourseRequest
            {
                UserProfileId = User.Identity.Name,
                ClassRoomId = subscription.ClassRoomId
            });

            return RedirectToAction("Preparing", "My");
        }

        public IActionResult Preparing()
        {
            var userprofile = _userprofileRepo.GetUserProfileById(User.Identity.Name);
            if (userprofile == null) return View("Error");

            var lastActiveSubscription = userprofile.Subscriptions
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.LastActiveDate.HasValue)
                .OrderByDescending(it => it.LastActiveDate)
                .FirstOrDefault();
            if (lastActiveSubscription == null) return View("NoCourseAccess");
            var currentClassRoomId = lastActiveSubscription.ClassRoomId;

            var classCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(lastActiveSubscription.ClassRoomId);
            var isClassCalendarValid = classCalendar != null && classCalendar.LessonCalendars != null && classCalendar.LessonCalendars.Any();
            if (!isClassCalendarValid) return View("NoCourseAccess");

            var now = _dateTime.GetCurrentTime();
            var lessonCalendarQry = classCalendar.LessonCalendars.Where(it => !it.DeletedDate.HasValue);

            var currentLesson = lessonCalendarQry.Where(it => now.Date >= it.BeginDate).OrderByDescending(it => it.BeginDate).FirstOrDefault();
            var currentLessonId = currentLesson?.LessonId;
            if (currentLessonId == null) return View("NoCourseAccess");

            var redirectURL = $"/my#!/app/main/lesson/{ currentLessonId }/{ currentClassRoomId }";
            return Redirect(redirectURL);
        }
    }
}
