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
            var availableCourses = _courseCtrl.GetAvailableCourseGroups();
            return View(availableCourses);
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
            var selectedCourse = _courseCtrl.GetCourseDetail(id);
            if (selectedCourse == null) return RedirectToAction("Error");

            var userId = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty;
            var isAlreadyHaveThisCourse = _myCourseCtrl.GetAllCourses(userId)
                .Select(it => it.CourseCatalogId)
                .Contains(selectedCourse.id);

            ViewBag.IsAlreadyHaveThisCourse = isAlreadyHaveThisCourse;
            ViewBag.IsCouponInvalid = isCouponInvalid;

            return View(selectedCourse);
        }
    }
}
