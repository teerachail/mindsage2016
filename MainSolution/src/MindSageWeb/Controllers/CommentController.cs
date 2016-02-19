using Microsoft.AspNet.Mvc;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Linq;

namespace MindSageWeb.Controllers
{
    /// <summary>
    /// Comment API
    /// </summary>
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        #region Fields

        private IClassCalendarRepository _classCalendarRepo;
        private IUserProfileRepository _userprofileRepo;
        private ICommentRepository _commentRepo;
        private IUserActivityRepository _userActivityRepo;
        private ILikeCommentRepository _likeCommentRepo;
        private IDateTime _dateTime;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize comment controller
        /// </summary>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        /// <param name="userprofileRepo">UserProfile repository</param>
        /// <param name="commentRepo">Comment repository</param>
        /// <param name="userActivityRepo">User activity repository</param>
        /// <param name="likeCommentRepo">Like comment repository</param>
        public CommentController(IClassCalendarRepository classCalendarRepo,
            IUserProfileRepository userprofileRepo,
            ICommentRepository commentRepo,
            IUserActivityRepository userActivityRepo,
            ILikeCommentRepository likeCommentRepo,
            IDateTime dateTime)
        {
            _classCalendarRepo = classCalendarRepo;
            _userprofileRepo = userprofileRepo;
            _commentRepo = commentRepo;
            _userActivityRepo = userActivityRepo;
            _likeCommentRepo = likeCommentRepo;
            _dateTime = dateTime;
        }

        #endregion Constructors

        #region Methods

        // POST: api/comment
        /// <summary>
        /// Create a comment
        /// </summary>
        /// <param name="body">Request information</param>
        [HttpPost]
        public void Post(PostNewCommentRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.Description)
                && !string.IsNullOrEmpty(body.LessonId)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!areArgumentsValid) return;

            UserProfile userprofile;
            var canAccessToTheClassRoom = checkAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId, out userprofile);
            if (!canAccessToTheClassRoom) return;

            var now = _dateTime.GetCurrentTime();
            var canAccessToTheClassLesson = checkAccessPermissionToSelectedClassLesson(body.ClassRoomId, body.LessonId, now);
            if (!canAccessToTheClassLesson) return;

            var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(body.UserProfileId, body.ClassRoomId);
            if (selectedUserActivity == null) return;

            var selectedLesson = selectedUserActivity.LessonActivities.FirstOrDefault(it => it.LessonId == body.LessonId);
            if (selectedLesson == null) return;

            var newComment = new Comment
            {
                id = Guid.NewGuid().ToString(),
                ClassRoomId = body.ClassRoomId,
                CreatedByUserProfileId = body.UserProfileId,
                Description = body.Description,
                LessonId = body.LessonId,
                CreatedDate = now,
                CreatorDisplayName = userprofile.Name,
                CreatorImageUrl = userprofile.ImageProfileUrl,
                Discussions = Enumerable.Empty<Comment.Discussion>(),
            };
            _commentRepo.UpsertComment(newComment);

            selectedLesson.CreatedCommentAmount++;
            _userActivityRepo.UpsertUserActivity(selectedUserActivity);
        }

        // PUT: api/comment/{comment-id}
        /// <summary>
        /// Update a comment
        /// </summary>
        /// <param name="id">Comment's id</param>
        /// <param name="body">Request information</param>
        [HttpPut("{id}")]
        public void Put(string id, RemoveCommentRequest body)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id)
                && body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.LessonId)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!areArgumentsValid) return;

            var canAccessToTheClassRoom = checkAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId);
            if (!canAccessToTheClassRoom) return;

            var now = _dateTime.GetCurrentTime();
            var canAccessToTheClassLesson = checkAccessPermissionToSelectedClassLesson(body.ClassRoomId, body.LessonId, now);
            if (!canAccessToTheClassLesson) return;

            var selectedComment = _commentRepo.GetCommentById(id);
            if (selectedComment == null) return;

            var isCommentOwner = selectedComment.CreatedByUserProfileId.Equals(body.UserProfileId, StringComparison.CurrentCultureIgnoreCase);
            if (!isCommentOwner) return;

            selectedComment.DeletedDate = now;
            _commentRepo.UpsertComment(selectedComment);
        }

        // POST: api/comment/like
        /// <summary>
        /// Like a comment
        /// </summary>
        /// <param name="body">Request information</param>
        [HttpPost]
        [Route("like")]
        public void Like(LikeCommentRequest body)
        {
            var areArgumentsValid = body != null
                && !string.IsNullOrEmpty(body.ClassRoomId)
                && !string.IsNullOrEmpty(body.CommentId)
                && !string.IsNullOrEmpty(body.LessonId)
                && !string.IsNullOrEmpty(body.UserProfileId);
            if (!areArgumentsValid) return;

            var canAccessToTheClassRoom = checkAccessPermissionToSelectedClassRoom(body.UserProfileId, body.ClassRoomId);
            if (!canAccessToTheClassRoom) return;

            var now = _dateTime.GetCurrentTime();
            var canAccessToTheClassLesson = checkAccessPermissionToSelectedClassLesson(body.ClassRoomId, body.LessonId, now);
            if (!canAccessToTheClassLesson) return;

            var selectedComment = _commentRepo.GetCommentById(body.CommentId);
            if (selectedComment == null) return;

            var likeComments = _likeCommentRepo.GetLikeCommentByCommentId(body.CommentId)
                .Where(it => !it.DeletedDate.HasValue)
                .ToList();

            var likedCommentsByThisUser = likeComments
                .Where(it => it.LikedByUserProfileId.Equals(body.UserProfileId, StringComparison.CurrentCultureIgnoreCase));

            var isUnlike = likedCommentsByThisUser.Any();
            if (isUnlike)
            {
                foreach (var item in likedCommentsByThisUser)
                {
                    item.DeletedDate = now;
                    _likeCommentRepo.UpsertLikeComment(item);
                }
            }
            else
            {
                var isCommentOwner = selectedComment.CreatedByUserProfileId.Equals(body.UserProfileId, StringComparison.CurrentCultureIgnoreCase);
                if (!isCommentOwner)
                {
                    var canLikeThisComment = checkAccessPermissionToUserProfile(selectedComment.CreatedByUserProfileId);
                    if (!canLikeThisComment) return;
                }

                var selectedUserActivity = _userActivityRepo.GetUserActivityByUserProfileIdAndClassRoomId(body.UserProfileId, body.ClassRoomId);
                if (selectedUserActivity == null) return;
                var selectedLessonActivity = selectedUserActivity.LessonActivities.FirstOrDefault(it => it.LessonId.Equals(body.LessonId));
                if (selectedLessonActivity == null) return;
                selectedLessonActivity.ParticipationAmount++;
                _userActivityRepo.UpsertUserActivity(selectedUserActivity);

                var newLike = new LikeComment
                {
                    id = Guid.NewGuid().ToString(),
                    LessonId = body.LessonId,
                    CommentId = body.CommentId,
                    LikedByUserProfileId = body.UserProfileId,
                    CreatedDate = now
                };
                likeComments.Add(newLike);
                _likeCommentRepo.UpsertLikeComment(newLike);
            }

            selectedComment.TotalLikes = likeComments.Where(it => !it.DeletedDate.HasValue).Count();
            _commentRepo.UpsertComment(selectedComment);
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
        private bool checkAccessPermissionToSelectedClassLesson(string classRoomId, string lessonId, DateTime currentTime)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(classRoomId) && !string.IsNullOrEmpty(lessonId);
            if (!areArgumentsValid) return false;

            var selectedClassCalendar = _classCalendarRepo.GetClassCalendarByClassRoomId(classRoomId);
            if (selectedClassCalendar == null) return false;

            var canAccessToTheLesson = selectedClassCalendar.LessonCalendars
                .Where(it => !it.DeletedDate.HasValue)
                .Where(it => it.LessonId.Equals(lessonId, StringComparison.CurrentCultureIgnoreCase))
                .Where(it => it.BeginDate <= currentTime)
                .Any();
            return canAccessToTheLesson;
        }
        private bool checkAccessPermissionToUserProfile(string userprofileId)
        {
            var selectedCommentOwnerProfile = _userprofileRepo.GetUserProfileById(userprofileId);
            var canPostNewDiscussion = selectedCommentOwnerProfile != null
                && !selectedCommentOwnerProfile.IsPrivateAccount;
            return canPostNewDiscussion;
        }

        #endregion Methods

        //// GET: api/Comment
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Comment/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Comment
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Comment/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Comment/5
        //public void Delete(int id)
        //{
        //}
    }
}
