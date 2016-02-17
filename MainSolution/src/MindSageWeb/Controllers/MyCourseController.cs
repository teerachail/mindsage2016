using Microsoft.AspNet.Mvc;
using MindsageWeb.Repositories;
using MindsageWeb.Repositories.Models;
using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MindsageWeb.Controllers
{
    public class MyCourseController : Controller
    {
        #region Fields

        private IClassCalendarRepository _classCalendarRepo;
        private IUserProfileRepository _userprofileRepo;
        private IUserActivityRepository _userActivityRepo;
        private IDateTime _dateTime;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize comment controller
        /// </summary>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="userprofileRepo">UserProfile repository</param>
        /// <param name="userActivityRepo">User activity repository</param>
        public MyCourseController(IClassCalendarRepository classCalendarRepo,
            IUserProfileRepository userprofileRepo,
            IUserActivityRepository userActivityRepo,
            IDateTime dateTime)
        {
            _classCalendarRepo = classCalendarRepo;
            _userprofileRepo = userprofileRepo;
            _userActivityRepo = userActivityRepo;
            _dateTime = dateTime;
        }

        #endregion Constructors

        #region Methods

        // GET: api/mycourse/{user-id}/{class-room-id}
        public IEnumerable<CourseMapContentRespond> Get(string id, string classRoomId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return Enumerable.Empty<CourseMapContentRespond>();

            if (!checkAccessPermissionToSelectedClassRoom(id, classRoomId)) return Enumerable.Empty<CourseMapContentRespond>();

            var now = _dateTime.GetCurrentTime();
            var classCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(classRoomId);
            var canAccessToTheClassRoom = classCalendar != null
                && classCalendar.LessonCalendars != null
                && !classCalendar.DeletedDate.HasValue
                && !classCalendar.CloseDate.HasValue
                && classCalendar.ExpiredDate.HasValue
                && classCalendar.ExpiredDate.Value.Date > now.Date;
            if (!canAccessToTheClassRoom) return Enumerable.Empty<CourseMapContentRespond>();

            var result = classCalendar.LessonCalendars
                .Where(it => !it.DeletedDate.HasValue)
                .Select(it => new CourseMapContentRespond
                {
                    LessonId = it.LessonId,
                    IsLocked = now.Date < it.BeginDate.Date,
                    LessonWeekName = string.Format("Week{0:00}", it.Order),
                    SemesterName = it.SemesterGroupName
                })
                .ToList();
            return result;
        }

        [Route(":id/:classRoomId/status")]
        public IEnumerable<CourseMapStatusRespond> GetStatus(string id, string classRoomId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                 && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return Enumerable.Empty<CourseMapStatusRespond>();

            if (!checkAccessPermissionToSelectedClassRoom(id, classRoomId)) return Enumerable.Empty<CourseMapStatusRespond>();

            var now = _dateTime.GetCurrentTime();
            var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(id, classRoomId);
            var canAccessUserActivit = selectedUserActivity != null
                && selectedUserActivity.LessonActivities != null
                && !selectedUserActivity.DeletedDate.HasValue;
            if (!canAccessUserActivit) Enumerable.Empty<CourseMapStatusRespond>();

            const int NoneComment = 0;
            var result = selectedUserActivity.LessonActivities
                .Select(it => new CourseMapStatusRespond
                {
                    LessonId = it.LessonId,
                    HaveAnyComments = it.CreatedCommentAmount > NoneComment,
                    IsReadedAllContents = it.SawContentIds.Count() >= it.TotalContentsAmount
                })
                .ToList();
            return result;
        }

        private bool checkAccessPermissionToSelectedClassRoom(string userprofileId, string classRoomId)
        {
            UserProfile userprofile;
            return checkAccessPermissionToSelectedClassRoom(userprofileId, classRoomId, out userprofile);
        }
        private bool checkAccessPermissionToSelectedClassRoom(string userprofileId, string classRoomId, out UserProfile userprofile)
        {
            userprofile = null;
            var areArgumentsValid = !string.IsNullOrEmpty(userprofileId) && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return false;

            var selectedUserProfile = _userprofileRepo.GetUserProfileById(userprofileId);
            if (selectedUserProfile == null) return false;
            userprofile = selectedUserProfile;

            var canAccessToTheClass = selectedUserProfile
                .Subscriptions
                .Where(it => it.ClassRoomId.Equals(classRoomId, StringComparison.CurrentCultureIgnoreCase))
                .Any();

            return canAccessToTheClass;
        }

        #endregion Methods

        //// GET: api/mycourse
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/mycourse/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/mycourse
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/mycourse/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/mycourse/5
        //public void Delete(int id)
        //{
        //}
    }
}