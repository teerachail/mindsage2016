using Microsoft.AspNet.Mvc;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MindSageWeb.Controllers
{
    /// <summary>
    /// Notification API
    /// </summary>
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        #region Fields

        private IUserProfileRepository _userProfileRepo;
        private INotificationRepository _notificationRepo;
        private IDateTime _dateTime;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize Notification API
        /// </summary>
        /// <param name="userprofileRepo">User profile repository</param>
        /// <param name="notificationRepo">Notification repository</param>
        public NotificationController(IUserProfileRepository userprofileRepo,
            INotificationRepository notificationRepo,
            IDateTime dateTime)
        {
            _userProfileRepo = userprofileRepo;
            _notificationRepo = notificationRepo;
            _dateTime = dateTime;
        }

        #endregion Constructors

        #region Methods

        // GET: api/notification/{user-id}/{class-room-id}
        /// <summary>
        /// Get notification total amount
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}")]
        public GetNotificationRespond Get(string id, string classRoomId)
        {
            const int NoneNotification = 0;
            var areArgumentsValid = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return new GetNotificationRespond { notificationTotal = NoneNotification };

            var notifications = _notificationRepo.GetNotificationByUserIdAndClassRoomId(id, classRoomId).ToList();
            var anyNotification = notifications != null && notifications.Any(it => !it.HideDate.HasValue);
            if (!anyNotification) return new GetNotificationRespond { notificationTotal = NoneNotification };

            return new GetNotificationRespond { notificationTotal = notifications.Where(it => !it.HideDate.HasValue).Count() };
        }

        // GET: api/notification/{user-id}/{class-room-id}/content
        /// <summary>
        /// Get notification content
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/content")]
        public IEnumerable<NotificationRespond> GetContent(string id, string classRoomId)
        {
            var areArgumentsValid = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(classRoomId);
            if (!areArgumentsValid) return Enumerable.Empty<NotificationRespond>();

            var notifications = _notificationRepo.GetNotificationByUserIdAndClassRoomId(id, classRoomId).ToList();
            var anyNotification = notifications != null && notifications.Any(it => !it.HideDate.HasValue);
            if (!anyNotification) return Enumerable.Empty<NotificationRespond>();

            var now = _dateTime.GetCurrentTime();
            var unreadedMsgs = notifications.Where(it => !it.HideDate.HasValue).ToList();

            const int LastRetrieveDays = 7;
            var needToReachQry = unreadedMsgs
                .Where(it => !it.LastReadedDate.HasValue || it.LastReadedDate < it.LastUpdateDate)
                .Where(it => Math.Abs(it.CreatedDate.Date.Subtract(now).Days) <= LastRetrieveDays);

            var relatedUserProfiles = needToReachQry
                .SelectMany(it => it.ByUserProfileId)
                .Where(it => it != null)
                .Distinct()
                .Select(it => _userProfileRepo.GetUserProfileById(it))
                .Where(it => it != null)
                .ToList();

            var result = needToReachQry
                .Select(it =>
                {
                    var fromUserProfile = relatedUserProfiles
                        .Where(uProfile => it.ByUserProfileId.Contains(uProfile.id))
                        .Select(uProfile => new NotificationRespond.ActionInformation
                        {
                            FromUserProfileId = uProfile.id,
                            FromUserProfileName = uProfile.Name
                        }).ToList();

                    NotificationRespond.NotificationTag tag;
                    switch (it.Tag)
                    {
                        case Notification.NotificationTag.Reminder:
                            tag = NotificationRespond.NotificationTag.Reminder;
                            break;
                        case Notification.NotificationTag.TopicOfTheDay:
                            tag = NotificationRespond.NotificationTag.TOTD;
                            break;
                        case Notification.NotificationTag.FriendCreateNewComment:
                            tag = NotificationRespond.NotificationTag.Comment;
                            break;
                        case Notification.NotificationTag.FriendLikesALesson:
                        case Notification.NotificationTag.SomeOneLikesYourComment:
                        case Notification.NotificationTag.SomeOneLikesYourDiscussion:
                            tag = NotificationRespond.NotificationTag.Like;
                            break;
                        case Notification.NotificationTag.SomeOneCreateDiscussionInYourComment:
                            tag = NotificationRespond.NotificationTag.Discussion;
                            break;
                        default: return null;
                    }

                    return new NotificationRespond
                    {
                        id = it.id,
                        Tag = tag,
                        FromUserProfiles = fromUserProfile,
                        Message = it.Message
                    };
                })
                .Where(it => it != null).ToList();

            return result;
        }

        [HttpGet]
        public void SendNotification()
        {

        }

        #endregion Methods

        //// GET: api/notification
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/notification/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/notification
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/notification/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/notification/5
        //public void Delete(int id)
        //{
        //}
    }
}
