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
        private IClassRoomRepository _classRoomRepo;
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
        /// <param name="classRoomRepo">Class room repository</param>
        public FriendController(IClassCalendarRepository classCalendarRepo,
            IUserProfileRepository userprofileRepo,
            IFriendRequestRepository friendRequestRepo,
            IUserActivityRepository userActivityRepo,
            IClassRoomRepository classRoomRepo,
            IDateTime dateTime)
        {
            _classCalendarRepo = classCalendarRepo;
            _userprofileRepo = userprofileRepo;
            _friendRequestRepo = friendRequestRepo;
            _userActivityRepo = userActivityRepo;
            _classRoomRepo = classRoomRepo;
            _dateTime = dateTime;
        }

        #endregion Constructors

        #region Methods

        // GET: api/friend/{user-id}/{class-room-id}
        /// <summary>
        /// Get friends from class room id (Student)
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

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(classRoomId);
            var isClassRoomValid = selectedClassRoom != null
                && !selectedClassRoom.DeletedDate.HasValue
                && !selectedClassRoom.IsPublic;
            if (!isClassRoomValid) return Enumerable.Empty<GetFriendListRespond>();

            var allStudentsInTheClassRoom = _userprofileRepo.GetUserProfilesByClassRoomId(classRoomId)
                .Where(it => it.Subscriptions.Any(s => !s.DeletedDate.HasValue && s.ClassRoomId == classRoomId))
                .ToList();
            if (!allStudentsInTheClassRoom.Any()) return Enumerable.Empty<GetFriendListRespond>();

            var friendRequests = _friendRequestRepo.GetFriendRequestByUserProfileId(id).ToList();
            var students = allStudentsInTheClassRoom
                .Where(it => it.id != id)
                .OrderBy(it => it.id)
                .Select(it =>
                {
                    var isTeacher = it.Subscriptions.Where(s => !s.DeletedDate.HasValue && s.ClassRoomId == classRoomId)
                    .Any(s => s.Role == UserProfile.AccountRole.Teacher);
                    var selectedRequest = friendRequests.FirstOrDefault(req => req.ToUserProfileId.Equals(it.id));
                    var status = selectedRequest == null ? FriendRequest.RelationStatus.Unfriend : selectedRequest.Status;
                    var result = new GetFriendListRespond
                    {
                        UserProfileId = it.id,
                        Name = it.Name,
                        ImageUrl = it.ImageProfileUrl,
                        Status = status,
                        RequestId = selectedRequest == null ? null : selectedRequest.id,
                        IsTeacher = isTeacher
                    };
                    return result;
                })
                .OrderBy(it => it.Name)
                .ToList();
            return students;
        }

        // GET: api/friend/{user-id}/{class-room-id}/selfpurchase
        /// <summary>
        /// Get friends from class room id (Self purchase)
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/selfpurchase")]
        public IEnumerable<GetFriendListRespond> GetFriendForSelfPurchase(string id, string classRoomId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return Enumerable.Empty<GetFriendListRespond>();

            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(id, classRoomId);
            if (!canAccessToTheClassRoom) return Enumerable.Empty<GetFriendListRespond>();

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(classRoomId);
            var isClassRoomValid = selectedClassRoom != null
                && !selectedClassRoom.DeletedDate.HasValue
                && selectedClassRoom.IsPublic;
            if (!isClassRoomValid) return Enumerable.Empty<GetFriendListRespond>();

            var friendRequests = _friendRequestRepo.GetFriendRequestByUserProfileId(id)
                .Where(it => it.Status != FriendRequest.RelationStatus.Unfriend)
                .ToList();
            var relatedUserProfiles = _userprofileRepo.GetUserProfileById(friendRequests.Select(it => it.ToUserProfileId).Distinct());

            var students = relatedUserProfiles
                .Where(it => it.id != id)
                .OrderBy(it => it.id)
                .Select(it =>
                {
                    var selectedRequest = friendRequests.FirstOrDefault(req => req.ToUserProfileId.Equals(it.id));
                    if (selectedRequest == null) return null;
                    var result = new GetFriendListRespond
                    {
                        UserProfileId = it.id,
                        Name = it.Name,
                        ImageUrl = it.ImageProfileUrl,
                        Status = selectedRequest.Status,
                        RequestId = selectedRequest.id,
                    };
                    return result;
                }).Where(it => it != null)
                .OrderBy(it => it.Name)
                .ToList();
            return students;
        }

        [HttpGet]
        [Route("{id}/{classRoomId}/{key}")]
        public IEnumerable<GetFriendListRespond> GetSearchResult(string id, string classRoomId, string key)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && !string.IsNullOrEmpty(classRoomId)
                && !string.IsNullOrEmpty(key);
            if (!areArgumentsValid) return Enumerable.Empty<GetFriendListRespond>();

            var canAccessToTheClassRoom = _userprofileRepo.CheckAccessPermissionToSelectedClassRoom(id, classRoomId);
            if (!canAccessToTheClassRoom) return Enumerable.Empty<GetFriendListRespond>();

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(classRoomId);
            var isClassRoomValid = selectedClassRoom != null
                && !selectedClassRoom.DeletedDate.HasValue
                && selectedClassRoom.IsPublic;
            if (!isClassRoomValid) return Enumerable.Empty<GetFriendListRespond>();

            key = key.ToLower();
            var allStudentsInTheClassRoom = _userprofileRepo.GetUserProfilesByClassRoomId(classRoomId)
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.Subscriptions.Any(s => !s.DeletedDate.HasValue && s.ClassRoomId == classRoomId))
                .Where(it => it.id != id)
                .Where(it => it.id.ToLower().Contains(key) || it.Name.ToLower().Contains(key) || (it.SchoolName?.Contains(key) ?? false))
                .ToList();
            if (!allStudentsInTheClassRoom.Any()) return Enumerable.Empty<GetFriendListRespond>();

            var friendRequests = _friendRequestRepo.GetFriendRequestByUserProfileId(id).ToList();
            var containKeyUserProfiles = allStudentsInTheClassRoom
                .Select(it =>
                {
                    var selectedRequest = friendRequests.FirstOrDefault(req => req.ToUserProfileId.Equals(it.id));
                    var status = selectedRequest == null ? FriendRequest.RelationStatus.Unfriend : selectedRequest.Status;
                    return new GetFriendListRespond
                    {
                        UserProfileId = it.id,
                        Name = it.Name,
                        ImageUrl = it.ImageProfileUrl,
                        Status = status,
                        RequestId = selectedRequest?.id
                    };
                }).ToList();
            return containKeyUserProfiles;
        }

        // POST: api/friend
        /// <summary>
        /// Send or respond a friend request
        /// </summary>
        /// <param name="body">Request information</param>
        [HttpPost]
        public void Post(SendFriendRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.FromUserProfileId)
                && !string.IsNullOrEmpty(body.ToUserProfileId);
            if (!areArgumentsValid) return;

            var requestUserProfileIds = new List<string> { body.FromUserProfileId, body.ToUserProfileId };
            var relatedUserProfiles = _userprofileRepo.GetUserProfileById(requestUserProfileIds);
            var usersExisting = relatedUserProfiles.Count() == requestUserProfileIds.Count();
            if (!usersExisting) return;

            var requests = _friendRequestRepo.GetFriendRequestByUserProfileId(body.FromUserProfileId)
                .Where(it => it.ToUserProfileId.Equals(body.ToUserProfileId))
                .ToList();
            var currentStatus = requests.OrderByDescending(it => it.CreatedDate).FirstOrDefault();
            var isRequestValid = currentStatus == null ? string.IsNullOrEmpty(body.RequestId) : currentStatus.id.Equals(body.RequestId);
            if (!isRequestValid) return;

            var now = _dateTime.GetCurrentTime();
            var isNewFriendRequest = currentStatus == null;
            if (isNewFriendRequest)
            {
                requests.ForEach(it => it.DeletedDate = now);
                var newRequestFrom = new FriendRequest
                {
                    id = Guid.NewGuid().ToString(),
                    CreatedDate = now,
                    FromUserProfileId = body.FromUserProfileId,
                    ToUserProfileId = body.ToUserProfileId,
                    Status = FriendRequest.RelationStatus.SendRequest
                };
                requests.Add(newRequestFrom);

                var newRequestTo = new FriendRequest
                {
                    id = Guid.NewGuid().ToString(),
                    CreatedDate = now,
                    FromUserProfileId = body.ToUserProfileId,
                    ToUserProfileId = body.FromUserProfileId,
                    Status = FriendRequest.RelationStatus.ReceiveRequest
                };
                requests.Add(newRequestTo);
                requests.ForEach(it => _friendRequestRepo.UpsertFriendRequest(it));
            }
            else
            {
                var isRequestInvalid = currentStatus.Status == FriendRequest.RelationStatus.SendRequest && body.IsAccept;
                if (isRequestInvalid) return;

                var friendSideRequest = _friendRequestRepo.GetFriendRequestByUserProfileId(body.ToUserProfileId)
                    .Where(it => it.ToUserProfileId.Equals(body.FromUserProfileId))
                    .ToList();

                var currentFriendSideStatus = friendSideRequest.OrderByDescending(it => it.CreatedDate).FirstOrDefault();
                var fRequests = friendSideRequest.Except(new List<FriendRequest> { currentFriendSideStatus });
                foreach (var item in fRequests) item.DeletedDate = now;

                if (body.IsAccept)
                {
                    currentStatus.AcceptedDate = now;
                    currentStatus.Status = FriendRequest.RelationStatus.Friend;
                    currentFriendSideStatus.AcceptedDate = now;
                    currentFriendSideStatus.Status = FriendRequest.RelationStatus.Friend;
                }
                else
                {
                    currentStatus.DeletedDate = now;
                    currentStatus.Status = FriendRequest.RelationStatus.Unfriend;
                    currentFriendSideStatus.DeletedDate = now;
                    currentFriendSideStatus.Status = FriendRequest.RelationStatus.Unfriend;
                }
                friendSideRequest.Add(currentStatus);
                friendSideRequest.ForEach(it => _friendRequestRepo.UpsertFriendRequest(it));
            }
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
