using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.OptionsModel;

namespace MindSageWeb.Controllers
{
    public class HomeController : Controller
    {
        private CourseController _courseCtrl;
        private MyCourseController _myCourseCtrl;
        private AppConfigOptions _appConfig;

        public HomeController(CourseController courseCtrl, 
            MyCourseController myCourseCtrl,
            IOptions<AppConfigOptions> options)
        {
            _courseCtrl = courseCtrl;
            _myCourseCtrl = myCourseCtrl;
            _appConfig = options.Value;
        }

        public IActionResult Index()
        {
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
            var isAlreadyHaveSelectedCourse = isAlreadyLoggedIn ? !_myCourseCtrl.CanAddNewCourseCatalog(User.Identity.Name, selectedCourse.id, out allUserCourses) : false;

            ViewBag.IsAlreadyHaveThisCourse = isAlreadyHaveSelectedCourse;
            ViewBag.IsCouponInvalid = isCouponInvalid;
            ViewBag.MindSageUrl = _appConfig.MindSageUrl;

            return View(selectedCourse);
        }

        public async Task<IActionResult> Preview(int id)
        {
            using (var http = new HttpClient())
            {
                ViewBag.MindSageUrl = _appConfig.MindSageUrl;
                var result = await http.GetStringAsync($"{ _appConfig.ManagementPortalUrl }/api/CourseCatalog/{ id }/detail");
                var courseCatalog = JsonConvert.DeserializeObject<Repositories.Models.GetCourseDetailRespond>(result);
                return View(courseCatalog);
            }
        }
    }
}
