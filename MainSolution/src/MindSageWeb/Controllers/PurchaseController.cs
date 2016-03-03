using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

namespace MindSageWeb.Controllers
{
    [Authorize]
    public class PurchaseController : Controller
    {
        #region Fields

        private MyCourseController _myCourseCtrl;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize purchase controller
        /// </summary>
        /// <param name="myCourseCtrl">MyCourse API</param>
        public PurchaseController(MyCourseController myCourseCtrl)
        {
            _myCourseCtrl = myCourseCtrl;
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
            if (result.IsSuccess) return RedirectToAction("Index", "My");

            return RedirectToAction("Detail", "Home", new { @id = courseId, isCouponInvalid = true });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Confirm()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Finished()
        {
            return View();
        }

        #endregion Methods


    }

}
