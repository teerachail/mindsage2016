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
        private IUserProfileRepository _userprofileRepo;
        private MyCourseController _myCourseCtrl;

        public MyController(MyCourseController myCourseCtrl, 
            IUserProfileRepository userprofileRepo)
        {
            _myCourseCtrl = myCourseCtrl;
            _userprofileRepo = userprofileRepo;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EnterCourse(string id)
        {
            var userprofile = _userprofileRepo?.GetUserProfileById(User.Identity.Name);
            if (userprofile == null) return RedirectToAction("Error");

            var subscription = userprofile.Subscriptions?
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.CourseCatalogId == id)
                .OrderBy(it => it.LastActiveDate)
                .LastOrDefault();
            if (subscription == null) return RedirectToAction("Error");

            var courseInfo =_myCourseCtrl?.ChangeCourse(new Repositories.Models.ChangeCourseRequest
            {
                UserProfileId = User.Identity.Name,
                ClassRoomId = subscription.ClassRoomId
            });

            var redirectURL = $"/my#!/preparing";
            return Redirect(redirectURL);
        }
    }
}
