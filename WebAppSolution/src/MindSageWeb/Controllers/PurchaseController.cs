using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using MindSageWeb.ViewModels.PurchaseCourse;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;

namespace MindSageWeb.Controllers
{
    [Authorize]
    public class PurchaseController : Controller
    {
        #region Fields

        private IUserProfileRepository _userprofileRepo;
        private IDateTime _dateTime;
        private CourseController _courseCtrl;
        private MyCourseController _myCourseCtrl;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize purchase controller
        /// </summary>
        /// <param name="courseCtrl">Course API</param>
        /// <param name="myCourseCtrl">MyCourse API</param>
        /// <param name="userProfileRepo">User profile repository</param>
        public PurchaseController(CourseController courseCtrl, 
            MyCourseController myCourseCtrl,
            IUserProfileRepository userProfileRepo,
            IDateTime dateTime)
        {
            _courseCtrl = courseCtrl;
            _myCourseCtrl = myCourseCtrl;
            _userprofileRepo = userProfileRepo;
            _dateTime = dateTime;
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
            if (result.IsSuccess)
            {
                var redirectURL = $"/my#!/preparing";
                return Redirect(redirectURL);
            }

            return RedirectToAction("Detail", "Home", new { @id = courseId, isCouponInvalid = true });
        }

        /// <summary>
        /// Credit card form
        /// </summary>
        /// <param name="id">Course id</param>
        [HttpGet]
        public IActionResult Index(string id)
        {
            var canAddNewCourse = _myCourseCtrl.CanAddNewCourseCatalog(User.Identity.Name, id);
            if(!canAddNewCourse) return View("Error");

            // HACK: Create tracking form
            var model = new PurchaseCourseViewModel { CourseId = id };
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(PurchaseCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                // HACK: Validate tracking form
                var selectedCourse = _courseCtrl.GetCourseDetail(model.CourseId);
                if (selectedCourse == null) return View("Error");

                var data = new PurchaseCourseConfirmViewModel(model)
                {
                    TotalChargeAmount = selectedCourse.Price
                };
                return View("Confirm", data);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Confirm(PurchaseCourseConfirmViewModel model)
        {
            var canAddNewCourse = _myCourseCtrl.CanAddNewCourseCatalog(User.Identity.Name, model.CourseId);
            if (!canAddNewCourse) return View("Error");

            // HACK: Validate tracking form
            // TODO: Pay with Paypal
            //addNewSubscriptionToUser(model.CourseId);
            return RedirectToAction("Finished", new { @id = "TrackingId01" });
        }

        private void addNewSubscriptionToUser(string courseCatalogId)
        {
            // TODO: Check self purchase class room id (if it doesn't existing then create it)
            // TODO: Create new class calendar
            var userProfileId = User.Identity.Name;
            var selectedUserProfile = _userprofileRepo.GetUserProfileById(userProfileId);
            var subscriptions = selectedUserProfile.Subscriptions.ToList();
            var now = _dateTime.GetCurrentTime();
            subscriptions.Add(new UserProfile.Subscription
            {
                id = Guid.NewGuid().ToString(),
                Role = UserProfile.AccountRole.SelfPurchaser,
                LastActiveDate = now,
                ClassRoomId = "SELFPURCHASE_CLASS_ROOM_ID", // HACK: Set selfpurchase class room id
                ClassCalendarId = "CLASS_CALENDAR_ID", // HACK: Set class's calendar id
                CreatedDate = now,
                ClassRoomName = "CLASS_ROOM_NAME", // HACK: Class room name
                CourseCatalogId = courseCatalogId
            });
        }

        /// <summary>
        /// Get purchased information
        /// </summary>
        /// <param name="id">Tracking id</param>
        [HttpGet]
        public IActionResult Finished(string id)
        {
            // HACK: Get tracking information
            var model = new CoursePurchasedViewModel
            {
                AddressSummary = "Pimankhondopark Building 2 room number 989/148 Khonkean Naimung USA 40000",
                CardType = "VISA",
                CourseId = "CourseId01",
                LastFourDigits = "1234",
                TotalChargeAmount = 53.567
            };
            return View();
        }

        #endregion Methods
    }
}
