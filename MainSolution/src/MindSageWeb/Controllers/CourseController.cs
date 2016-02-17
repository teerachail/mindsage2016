using Microsoft.AspNet.Mvc;
using MindsageWeb.Repositories;
using MindsageWeb.Repositories.Models;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MindSageWeb.Controllers
{
    public class CourseController : Controller
    {
        #region Fields

        private ICourseCatalogRepository _repo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize course controller
        /// </summary>
        /// <param name="courseCatalogRepo">Course catalog repository</param>
        public CourseController(ICourseCatalogRepository courseCatalogRepo)
        {
            _repo = courseCatalogRepo;
        }

        #endregion Constructors

        #region Methods

        // GET: api/course
        /// <summary>
        /// Get all available courses
        /// </summary>
        public IEnumerable<CourseCatalogRespond> Get()
        {
            var result = _repo.GetAvailableCourses()
                .Where(it => !it.DeletedDate.HasValue)
                .Select(it => new CourseCatalogRespond
                {
                    id = it.id,
                    Description = it.ShortDescription,
                    ImageUrl = it.ThumbnailImageUrl,
                    Name = it.Name
                })
                .ToList();
            return result;
        }

        // GET: api/course/{course-catalog-id}
        /// <summary>
        /// Get course's detail
        /// </summary>
        /// <param name="id">Course id</param>
        public CourseCatalog Get(string id)
        {
            var isArgumentValid = !string.IsNullOrEmpty(id);
            if (!isArgumentValid) return null;

            var selectedCourse = _repo.GetCourseCatalogById(id);
            return selectedCourse;
        }

        #endregion Methods

        //// GET: api/course
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/course/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/course
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/course/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/course/5
        //public void Delete(int id)
        //{
        //}
    }
}
