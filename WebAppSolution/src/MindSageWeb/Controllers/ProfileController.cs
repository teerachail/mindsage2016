﻿using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using MindSageWeb.Models;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Controllers
{
    /// <summary>
    /// Profile API
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        #region Fields

        private IUserProfileRepository _userProfileRepo;
        private IClassCalendarRepository _classCalendarRepo;
        private IDateTime _dateTime;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize Profile API
        /// </summary>
        /// <param name="userprofileRepo">User profile repository</param>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        public ProfileController(IUserProfileRepository userprofileRepo, 
            IClassCalendarRepository classCalendarRepo,
            IDateTime dateTime,
            UserManager<ApplicationUser> userManager)
        {
            _userProfileRepo = userprofileRepo;
            _classCalendarRepo = classCalendarRepo;
            _dateTime = dateTime;
            _userManager = userManager;
        }

        #endregion Constructors

        #region Methods

        // GET: api/profile
        [HttpGet]
        public GetUserProfileRespond Get()
        {
            var userProfileName = User?.Identity?.Name;
            var result = Get(userProfileName);
            return result;
        }

        // PUT: api/profile/{user-id}
        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="body">Request's information</param>
        [HttpPut]
        [Route("{id}")]
        public void Put(string id, UpdateProfileRequest body)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
               && body != null
               && !string.IsNullOrEmpty(body.Name);
            if (!areArgumentsValid) return;

            var currentUserId = User.Identity.Name;
            if (currentUserId != id) return;

            var userProfile = _userProfileRepo.GetUserProfileById(id);
            var isUserProfileValid = userProfile != null && !userProfile.DeletedDate.HasValue;
            if (!isUserProfileValid) return;

            userProfile.Name = body.Name;
            userProfile.SchoolName = body.SchoolName;
            userProfile.IsPrivateAccount = body.IsPrivate;
            userProfile.CourseReminder = body.IsReminderOnceTime ? 
                UserProfile.ReminderFrequency.Once: 
                UserProfile.ReminderFrequency.Twice;
            _userProfileRepo.UpsertUserProfile(userProfile);
        }

        // GET: api/profile/{user-id}
        /// <summary>
        /// Get user profile information
        /// </summary>
        /// <param name="id">User profile id</param>
        [HttpGet]
        [Route("{id}")]
        public GetUserProfileRespond Get(string id)
        {
            var isArgumentValid = !string.IsNullOrEmpty(id);
            if (!isArgumentValid) return null;

            var userprofile = _userProfileRepo.GetUserProfileById(id);
            if (userprofile == null) return null;
            var currentUser = System.Security.Claims.PrincipalExtensions.GetUserId(HttpContext.User);
            var user = _userManager.FindByIdAsync(currentUser).Result;
            var isUserProfileSubscriptionValid = userprofile.Subscriptions != null && userprofile.Subscriptions.Any(it => it.LastActiveDate.HasValue);
            var userProfileInfo = new GetUserProfileRespond
            {
                UserProfileId = userprofile.id,
                HasPassword = _userManager.HasPasswordAsync(user).Result,
                FullName = userprofile.Name,
                ImageUrl = userprofile.ImageProfileUrl,
                SchoolName = userprofile.SchoolName,
                IsPrivateAccout = userprofile.IsPrivateAccount,
                IsReminderOnceTime = userprofile.CourseReminder == UserProfile.ReminderFrequency.Once
            };
            if (!isUserProfileSubscriptionValid) return userProfileInfo;

            var lastActiveSubscription = userprofile.Subscriptions
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.LastActiveDate.HasValue)
                .OrderByDescending(it => it.LastActiveDate)
                .FirstOrDefault();

            userProfileInfo.CurrentClassRoomId = lastActiveSubscription.ClassRoomId;
            var classCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(lastActiveSubscription.ClassRoomId);
            var isClassCalendarValid = classCalendar != null && classCalendar.LessonCalendars != null && classCalendar.LessonCalendars.Any();
            if (!isClassCalendarValid) return userProfileInfo;

            var now = _dateTime.GetCurrentTime();
            var lessonCalendarQry = classCalendar.LessonCalendars.Where(it => !it.DeletedDate.HasValue);

            var currentLesson = lessonCalendarQry.Where(it => now.Date >= it.BeginDate).OrderByDescending(it => it.BeginDate).FirstOrDefault();
            if (currentLesson == null) return userProfileInfo;
            else
            {
                userProfileInfo.CurrentLessonId = currentLesson.LessonId;
                userProfileInfo.CurrentLessonNo = currentLesson.Order;
            }

            return userProfileInfo;
        }

        #endregion Methods

        //// GET: api/profile
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/profile/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/profile
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/profile/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/profile/5
        //public void Delete(int id)
        //{
        //}
    }
}
