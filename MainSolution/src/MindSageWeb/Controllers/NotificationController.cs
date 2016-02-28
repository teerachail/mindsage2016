using Microsoft.AspNet.Mvc;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
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
        public int Get(string id, string classRoomId)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        // GET: api/notification/{user-id}/{class-room-id}/content
        /// <summary>
        /// Get notification content
        /// </summary>
        /// <param name="id">User profile id</param>
        /// <param name="classRoomId">Class room id</param>
        [HttpGet]
        [Route("{id}/{classRoomId}/content")]
        public int GetContent(string id, string classRoomId)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
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
