using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Linq;


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

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize Profile API
        /// </summary>
        /// <param name="userprofileRepo">User profile repository</param>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        public ProfileController(IUserProfileRepository userprofileRepo, 
            IClassCalendarRepository classCalendarRepo,
            IDateTime dateTime)
        {
            _userProfileRepo = userprofileRepo;
            _classCalendarRepo = classCalendarRepo;
            _dateTime = dateTime;
        }

        #endregion Constructors

        #region Methods

        // GET: api/profile
        [HttpGet]
        public GetProfileRespond Get()
        {
            var userProfileName = User.Identity.Name;
            return new GetProfileRespond
            {
                UserProfileId = userProfileName
            };
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
               && !string.IsNullOrEmpty(body.Name)
               && !string.IsNullOrEmpty(body.SchoolName);
            if (!areArgumentsValid) return;

            // HACK: Uncomment it when we done the authentication
            //var currentUserId = User.Identity.Name;
            //if (currentUserId != id) return;

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

            var isUserProfileSubscriptionValid = userprofile.Subscriptions != null && userprofile.Subscriptions.Any(it => it.LastActiveDate.HasValue);
            var userProfileInfo = new GetUserProfileRespond
            {
                UserProfileId = userprofile.id,
                FullName = userprofile.Name,
                ImageUrl = userprofile.ImageProfileUrl,
                SchoolName = userprofile.SchoolName,
                IsPrivateAccout = userprofile.IsPrivateAccount,
                IsReminderOnceTime = userprofile.CourseReminder == UserProfile.ReminderFrequency.Once
            };
            if (!isUserProfileSubscriptionValid) return userProfileInfo;

            var lastActiveSubscription = userprofile.Subscriptions
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
            else userProfileInfo.CurrentLessonId = currentLesson.LessonId;

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
