using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace MindSageWeb.Controllers
{
    public class HomeController : Controller
    {
        private CourseController _courseCtrl;
        private MyCourseController _myCourseCtrl;

        public HomeController(CourseController courseCtrl, MyCourseController myCourseCtrl)
        {
            _courseCtrl = courseCtrl;
            _myCourseCtrl = myCourseCtrl;
        }

        public IActionResult Index()
        {
            //ViewBag.HaveAnyCourse = _myCourseCtrl.GetAllUserCoursCatalogIds(User.Identity.Name).Any();
            //var availableCourses = _courseCtrl.GetAvailableCourseGroups();
            //return View(availableCourses);
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Detail(string id, bool isCouponInvalid = false)
        {
            var selectedCourse = _courseCtrl?.GetCourseDetail(id);
            if (selectedCourse == null) return RedirectToAction("Error");

            var allUserCourses = Enumerable.Empty<string>();
            var isAlreadyLoggedIn = User?.Identity?.IsAuthenticated ?? false;
            var isAlreadyHaveSelectedCourse = isAlreadyLoggedIn ? _myCourseCtrl.CanAddNewCourseCatalog(User.Identity.Name, selectedCourse.id, out allUserCourses) : false;

            ViewBag.IsAlreadyHaveThisCourse = isAlreadyHaveSelectedCourse;
            ViewBag.HaveAnyCourse = allUserCourses.Any();
            ViewBag.IsCouponInvalid = isCouponInvalid;

            return View(selectedCourse);
        }
    }
}
