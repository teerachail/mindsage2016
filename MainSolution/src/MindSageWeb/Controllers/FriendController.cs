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
        private IDateTime _dateTime;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize friend controller
        /// </summary>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="userprofileRepo">UserProfile repository</param>
        /// <param name="friendRequestRepo">Friend request repository</param>
        public FriendController(IClassCalendarRepository classCalendarRepo,
            IUserProfileRepository userprofileRepo,
            IFriendRequestRepository friendRequestRepo,
            IDateTime dateTime)
        {
            _classCalendarRepo = classCalendarRepo;
            _userprofileRepo = userprofileRepo;
            _friendRequestRepo = friendRequestRepo;
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
        /// <returns></returns>
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
