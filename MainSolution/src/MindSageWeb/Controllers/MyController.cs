using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MindSageWeb.Controllers
{
    [Authorize]
    public class MyController : Controller
    {
        private MyCourseController _myCourseCtrl;

        public MyController(MyCourseController myCourseCtrl)
        {
            _myCourseCtrl = myCourseCtrl;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.HaveAnyCourse = getAllUserCourses().Any();
            return View();
        }

        private IEnumerable<string> getAllUserCourses()
        {
            var userId = User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty;
            var qry = _myCourseCtrl.GetAllCourses(userId)
                .Select(it => it.CourseCatalogId);
            return qry;
        }
    }
}
