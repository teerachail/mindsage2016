using Microsoft.AspNet.Mvc;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using MindSageWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MindSageWeb.Controllers
{
    /// <summary>
    /// Course API
    /// </summary>
    [Route("api/[controller]")]
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

        internal object GetCourseDetail(object courseId)
        {
            throw new NotImplementedException();
        }

        #endregion Constructors

        #region Methods

        // GET: api/course
        /// <summary>
        /// Get all available courses
        /// </summary>
        [HttpGet]
        public IEnumerable<CourseCatalogRespond> GetAvailableCourse()
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
        [HttpGet("{id}")]
        public GetCourseDetailRespond GetCourseDetail(string id)
        {
            var isArgumentValid = !string.IsNullOrEmpty(id);
            if (!isArgumentValid) return null;

            var selectedCourse = _repo.GetCourseCatalogById(id);
            if (selectedCourse == null) return null;

            var result = new GetCourseDetailRespond
            {
                id = selectedCourse.id,
                CreatedDate = selectedCourse.CreatedDate,
                Description = selectedCourse.FullDescription,
                DescriptionImageUrl = selectedCourse.DescriptionImageUrl,
                FullImageUrl = selectedCourse.FullImageUrl,
                Name = selectedCourse.Name,
                Price = selectedCourse.Price,
                Title = selectedCourse.Title,
                Semesters = selectedCourse.Semesters
                .Select(semester => new GetCourseDetailRespond.Semester
                {
                    Description = semester.Description,
                    Name = semester.Name,
                    Title = semester.Title,
                    TotalWeeks = semester.Units.SelectMany(unit => unit.Lessons).Count(),
                    Units = semester.Units.Select(unit => new GetCourseDetailRespond.Unit
                    {
                        Description = unit.FullDescription,
                        Title = unit.Title,
                        TotalWeeks = unit.Lessons.Count(),
                        UnitNo = unit.Order,
                        Lessons = unit.Lessons.Select(it => new GetCourseDetailRespond.Lesson
                        {
                            id = it.id,
                            Name = it.Name,
                            Order = it.Order,
                            SemesterGroupName = it.SemesterGroupName
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
            return result;
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
