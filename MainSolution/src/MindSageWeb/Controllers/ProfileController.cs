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
        // PUT: api/profile/{user-id}
        [HttpPut]
        [Route("{id}")]
        public void Put(string id, UpdateProfileRequest body)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        // GET: api/profile/{user-id}
        [HttpGet]
        [Route("{id}")]
        public GetUserProfileRespond Get(string id)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

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
