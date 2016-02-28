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
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        #region Fields

        private IUserProfileRepository _userProfileRepo;
        private IClassCalendarRepository _classCalendarRepo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize Profile API
        /// </summary>
        /// <param name="userprofileRepo">User profile repository</param>
        /// <param name="classCalendarRepo">Class calendar repository</param>
        public ProfileController(IUserProfileRepository userprofileRepo, IClassCalendarRepository classCalendarRepo)
        {
            _userProfileRepo = userprofileRepo;
            _classCalendarRepo = classCalendarRepo;
        }

        #endregion Constructors

        #region Methods

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

            // TODO: Not implemented
            throw new NotImplementedException();
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

            var isUserProfileDataValid = userprofile.Subscriptions != null && userprofile.Subscriptions.Any();
            if (!isUserProfileDataValid) return new GetUserProfileRespond
            {
                // TODO: Not implemented
            };

            // TODO: Not implemented
            throw new NotImplementedException();
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
