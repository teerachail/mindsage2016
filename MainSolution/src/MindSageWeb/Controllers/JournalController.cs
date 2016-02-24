using Microsoft.AspNet.Mvc;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MindSageWeb.Controllers
{
    /// <summary>
    /// Journal API
    /// </summary>
    [Route("api/[controller]")]
    public class JournalController : Controller
    {
        #region Fields

        private IUserProfileRepository _userprofileRepo;
        private IFriendRequestRepository _friendRequestRepo;
        private ILessonCatalogRepository _lessonCatalogRepo;
        private IClassRoomRepository _classRoomRepo;
        private ICommentRepository _commentRepo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize Journal API
        /// </summary>
        /// <param name="userprofileRepo">Userprofile repository</param>
        /// <param name="friendReqeustRepo">Friend request repository</param>
        /// <param name="lessonCatalogRepo">Lesson catalog repository</param>
        /// <param name="classRoomRepo">Class room repository</param>
        /// <param name="commentRepo">Comment repository</param>
        public JournalController(IUserProfileRepository userprofileRepo,
            IFriendRequestRepository friendReqeustRepo,
            ILessonCatalogRepository lessonCatalogRepo,
            IClassRoomRepository classRoomRepo,
            ICommentRepository commentRepo)
        {
            _userprofileRepo = userprofileRepo;
            _friendRequestRepo = friendReqeustRepo;
            _lessonCatalogRepo = lessonCatalogRepo;
            _classRoomRepo = classRoomRepo;
            _commentRepo = commentRepo;
        }

        #endregion Constructors

        #region Methods

        // GET: api/journal/{target-user-id}/{request-by-user-id}
        /// <summary>
        /// Get all user's comments
        /// </summary>
        /// <param name="targetUserId">Target user profile id</param>
        /// <param name="requestByUserId">Request by user profile id</param>
        /// <param name="classRoomId">Class room id</param>
        [HttpGet]
        [Route("{targetUserId}/{requestByUserId}/{classRoomId}")]
        public GetJournalRespond Get(string targetUserId, string requestByUserId, string classRoomId)
        {
            var noDataRespond = new GetJournalRespond { Comments = Enumerable.Empty<GetCommentRespond>() };
            var areArgumentsValid = !string.IsNullOrEmpty(targetUserId) && !string.IsNullOrEmpty(requestByUserId);
            if (!areArgumentsValid) return noDataRespond;

            var friends = _friendRequestRepo.GetFriendRequestByUserProfileId(requestByUserId);
            if (friends == null || !friends.Any()) return noDataRespond;

            var isFriend = friends
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.Status == FriendRequest.RelationStatus.Friend)
                .Any(it => it.ToUserProfileId.Equals(targetUserId));

            if (!isFriend)
            {
                var targetUserProfile = _userprofileRepo.GetUserProfileById(targetUserId);
                if (requestByUserId == null) return noDataRespond;

                if (targetUserProfile.IsPrivateAccount) return new GetJournalRespond { IsPrivateAccount = true };
            }

            var userComments = _commentRepo.GetCommentsByUserProfileId(targetUserId, classRoomId).ToList();
            if (userComments == null) return noDataRespond;

            var selectedClassRoom = _classRoomRepo.GetClassRoomById(classRoomId);
            if (selectedClassRoom == null) return noDataRespond;

            var lessonIds = userComments.Select(it => it.LessonId).Distinct();

            var lessonQry = from it in selectedClassRoom.Lessons
                          where it != null
                          where lessonIds.Contains(it.id)
                          select new
                          {
                              LessonId = it.LessonCatalogId,
                              LessonCatalogId = it.LessonCatalogId,
                              LessonCatalog = _lessonCatalogRepo.GetLessonCatalogById(it.LessonCatalogId)
                          };
            var lessons = lessonQry.Where(it => it.LessonCatalog != null).ToList();

            var comments = userComments
                .Where(it => lessons.Any(lesson => lesson.LessonId == it.LessonId))
                .OrderByDescending(it => it.CreatedDate)
                .Select(it =>
                {
                    var selectedLessonCatalog = lessons.FirstOrDefault(l => l.LessonId == it.LessonId);
                    if (selectedLessonCatalog == null) return null;

                    return new GetCommentRespond
                    {
                        ClassRoomId = it.ClassRoomId,
                        CreatedByUserProfileId = it.CreatedByUserProfileId,
                        CreatedDate = it.CreatedDate,
                        CreatorDisplayName = it.CreatorDisplayName,
                        CreatorImageUrl = it.CreatorImageUrl,
                        Description = it.Description,
                        id = it.id,
                        LessonId = it.LessonId,
                        LessonWeek = selectedLessonCatalog.LessonCatalog.Order,
                        TotalDiscussions = it.Discussions.Count(),
                        TotalLikes = it.TotalLikes
                    };
                })
                .Where(it => it != null)
                .ToList();

            return new GetJournalRespond
            {
                IsDiscussionAvailable = isFriend,
                Comments = comments
            };
        }

        #endregion Methods

        //// GET: api/journal
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/journal/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/journal
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/journal/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/journal/5
        //public void Delete(int id)
        //{
        //}
    }
}
