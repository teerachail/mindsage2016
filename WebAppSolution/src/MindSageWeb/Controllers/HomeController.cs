using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Extensions.Logging;
using MindSageWeb.ViewModels.Manage;

namespace MindSageWeb.Controllers
{
    public class HomeController : Controller
    {
        private CourseController _courseCtrl;
        private MyCourseController _myCourseCtrl;
        private AppConfigOptions _appConfig;
        private ErrorMessageOptions _errorMsgs;
        private ILogger _logger;
        private readonly Engines.IEmailSender _mindsageEmailSender;

        public HomeController(CourseController courseCtrl,
            MyCourseController myCourseCtrl,
            IOptions<AppConfigOptions> options,
            IOptions<ErrorMessageOptions> errorMgs,
            ILoggerFactory loggerFactory,
            Engines.IEmailSender mindsageEmailSender)
        {
            _courseCtrl = courseCtrl;
            _myCourseCtrl = myCourseCtrl;
            _appConfig = options.Value;
            _errorMsgs = errorMgs.Value;
            _logger = loggerFactory.CreateLogger<CourseController>();
            _mindsageEmailSender = mindsageEmailSender;
        }

        public IActionResult Index()
        {
            return Redirect("/public/content/home");
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

        //
        // GET: /Home/ContactUs
        public IActionResult ContactUs()
        {
            return View();
        }

        //
        // POST: /Home/ContactUs
        [HttpPost]
        public async Task ContactUs(ContactUsViewModel model)
        {
            if (!(string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Message)))
            {
				var main = "Contact US";
				var receiver = "captain.omega@hotmail.com";
                //var receiver = "toddm@etamedia.com";
                var body = "<label><b>Name </b>" + model.Name + "</label></br></br><label><b>Message</b></br>" + model.Message + "</label>";
                await _mindsageEmailSender.Send(model.Email, receiver, main, body);
            }
        }
            
    }
}
