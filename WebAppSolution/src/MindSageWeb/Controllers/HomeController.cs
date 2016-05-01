using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.Logging;

namespace MindSageWeb.Controllers
{
    public class HomeController : Controller
    {
        private CourseController _courseCtrl;
        private MyCourseController _myCourseCtrl;
        private AppConfigOptions _appConfig;
        private ErrorMessageOptions _errorMsgs;
        private ILogger _logger;

        public HomeController(CourseController courseCtrl, 
            MyCourseController myCourseCtrl,
            IOptions<AppConfigOptions> options,
            IOptions<ErrorMessageOptions> errorMgs,
            ILoggerFactory loggerFactory)
        {
            _courseCtrl = courseCtrl;
            _myCourseCtrl = myCourseCtrl;
            _appConfig = options.Value;
            _errorMsgs = errorMgs.Value;
            _logger = loggerFactory.CreateLogger<CourseController>();
        }

        public IActionResult Index()
        {
            return Redirect("/public/home.html");
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Detail(string id, bool isCouponInvalid = false)
        {
            try
            {
                var selectedCourse = _courseCtrl?.GetCourseDetail(id);
                if (selectedCourse == null)
                {
                    ViewBag.ErrorMessage = _errorMsgs.CourseNotFound;
                    return View("Error");
                }

                var allUserCourses = Enumerable.Empty<string>();
                var isAlreadyLoggedIn = User?.Identity?.IsAuthenticated ?? false;
                var isAlreadyHaveSelectedCourse = isAlreadyLoggedIn ? !_myCourseCtrl.CanAddNewCourseCatalog(User.Identity.Name, selectedCourse.id, out allUserCourses) : false;

                ViewBag.IsAlreadyHaveThisCourse = isAlreadyHaveSelectedCourse;
                ViewBag.IsCouponInvalid = isCouponInvalid;
                ViewBag.MindSageUrl = _appConfig.MindSageUrl;

                return View(selectedCourse);
            }
            catch (Exception e)
            {
                _logger.LogError($"MongoDB: { e.ToString() }");
                ViewBag.ErrorMessage = _errorMsgs.CanNotConnectToTheDatabase;
                return View("Error");
            }
        }

        public async Task<IActionResult> Preview(int id)
        {
            try
            {
                using (var http = new HttpClient())
                {
                    ViewBag.MindSageUrl = _appConfig.MindSageUrl;
                    var result = await http.GetStringAsync($"{ _appConfig.ManagementPortalUrl }/api/CourseCatalog/{ id }/detail");
                    var courseCatalog = JsonConvert.DeserializeObject<Repositories.Models.GetCourseDetailRespond>(result);
                    return View(courseCatalog);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"ManagementPortal: { e.ToString() }");
                ViewBag.ErrorMessage = _errorMsgs.CanNotConnectToTheDatabase;
                return View("Error");
            }
        }
    }
}
