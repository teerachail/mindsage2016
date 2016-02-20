using Microsoft.AspNet.Mvc;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MindSageWeb.Controllers
{
    /// <summary>
    /// Friend API
    /// </summary>
    [Route("api/[controller]")]
    public class FriendController : Controller
    {
        #region Fields

        private IClassCalendarRepository _classCalendarRepo;
        private IUserProfileRepository _userprofileRepo;
        private IFriendRequestRepository _friendRequestRepo;
        private IUserActivityRepository _userActivityRepo;
        private IDateTime _dateTime;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize friend controller
        /// </summary>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="userprofileRepo">UserProfile repository</param>
        /// <param name="friendRequestRepo">Friend request repository</param>
        /// <param name="userActivityRepo">User activity repository</param>
        public FriendController(IClassCalendarRepository classCalendarRepo,
            IUserProfileRepository userprofileRepo,
            IFriendRequestRepository friendRequestRepo,
            IUserActivityRepository userActivityRepo,
            IDateTime dateTime)
        {
            _classCalendarRepo = classCalendarRepo;
            _userprofileRepo = userprofileRepo;
            _friendRequestRepo = friendRequestRepo;
            _userActivityRepo = userActivityRepo;
            _dateTime = dateTime;
        }

        #endregion Constructors

        #region Methods

        // GET: api/friend/{user-id}/{class-room-id}
        /// <summary>
        /// Get friends from class room id
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}")]
        public IEnumerable<GetFriendListRespond> Get(string id, string classRoomId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return Enumerable.Empty<GetFriendListRespond>();

            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(id, classRoomId);
            if (!canAccessToTheClassRoom) return Enumerable.Empty<GetFriendListRespond>();

            var friendRequests = _friendRequestRepo.GetFriendRequestByUserProfileId(id).ToList();
            if (!friendRequests.Any()) return Enumerable.Empty<GetFriendListRespond>();

            var allStudentsInTheClassRoom = _userprofileRepo.GetUserProfilesByClassRoomId(classRoomId).ToList();
            if (!allStudentsInTheClassRoom.Any()) return Enumerable.Empty<GetFriendListRespond>();

            var students = allStudentsInTheClassRoom
                .Where(it => it.id != id)
                .OrderBy(it => it.id)
                .Select(it =>
                {
                    var selectedRequest = friendRequests.FirstOrDefault(req => req.ToUserProfileId.Equals(it.id));
                    var status = selectedRequest == null ? FriendRequest.RelationStatus.Unfriend : selectedRequest.Status;
                    var result = new GetFriendListRespond
                    {
                        id = it.id,
                        Name = it.Name,
                        ImageUrl = it.ImageProfileUrl,
                        Status = status
                    };
                    return result;
                })
                .ToList();
            return students;
        }

        // GET: api/friend/{user-id}/{class-room-id}/students
        /// <summary>
        /// Get studens from class room id
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/students")]
        public IEnumerable<GetStudentListRespond> Students(string id, string classRoomId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return Enumerable.Empty<GetStudentListRespond>();

            UserProfile userprofile;
            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(id, classRoomId, out userprofile);
            if (!canAccessToTheClassRoom) return Enumerable.Empty<GetStudentListRespond>();

            var isTeacherAccount = userprofile.Subscriptions.First(it => it.ClassRoomId == classRoomId).Role == UserProfile.AccountRole.Teacher;
            if (!isTeacherAccount) return Enumerable.Empty<GetStudentListRespond>();

            var allStudentsInTheClassRoom = _userprofileRepo.GetUserProfilesByClassRoomId(classRoomId).ToList();
            if (!allStudentsInTheClassRoom.Any()) return Enumerable.Empty<GetStudentListRespond>();

            var userActivities = allStudentsInTheClassRoom
                .Select(it => _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(it.id, classRoomId))
                .Where(it => it != null)
                .ToList();

            const int NoneScore = 0;
            var result = allStudentsInTheClassRoom
                .Where(it => it.id != id)
                .OrderBy(it => it.id)
                .Select(it =>
                {
                    var selectedUserActivity = userActivities.FirstOrDefault(ua => ua.UserProfileId == it.id);
                    var isActivityFound = selectedUserActivity != null;

                    return new GetStudentListRespond
                    {
                        id = it.id,
                        Name = it.Name,
                        ImageUrl = it.ImageProfileUrl,
                        CommentPercentage = isActivityFound ? selectedUserActivity.CommentPercentage : NoneScore,
                        OnlineExtrasPercentage = isActivityFound ? selectedUserActivity.OnlineExtrasPercentage : NoneScore,
                        SocialParticipationPercentage = isActivityFound ? selectedUserActivity.SocialParticipationPercentage : NoneScore,
                    };
                })
                .ToList();
            return result;
        }

        #endregion Methods

        //// GET: api/friend
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/friend/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/friend
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/friend/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/friend/5
        //public void Delete(int id)
        //{
        //}
    }
}
