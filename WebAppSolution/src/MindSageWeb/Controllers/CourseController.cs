using Microsoft.AspNet.Mvc;
using MindSageWeb.Repositories;
using MindSageWeb.Repositories.Models;
using MindSageWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.OptionsModel;

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
        private AppConfigOptions _appConfig;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize course controller
        /// </summary>
        /// <param name="courseCatalogRepo">Course catalog repository</param>
        /// <param name="config">App configuration option</param>
        public CourseController(ICourseCatalogRepository courseCatalogRepo,
            IOptions<AppConfigOptions> options)
        {
            _repo = courseCatalogRepo;
            _appConfig = options.Value;
        }

        #endregion Constructors

        #region Methods

        // GET: api/course
        /// <summary>
        /// Get all available course groups
        /// </summary>
        [HttpGet]
        public IEnumerable<CourseCatalogRespond> GetAvailableCourseGroups()
        {
            var allCourses = _repo.GetAvailableCourses().ToList();
            var anyCourses = allCourses != null && allCourses.Any();
            if (!anyCourses) return Enumerable.Empty<CourseCatalogRespond>();

            var courseGroup = allCourses.Select(it => it.GroupName).Distinct();

            var result = courseGroup.Select(groupName => allCourses.FirstOrDefault(it => it.GroupName == groupName))
                .Where(it => it != null)
                .Where(it => !it.DeletedDate.HasValue)
                .Select(it => new CourseCatalogRespond
                {
                    id = it.id,
                    Name = it.GroupName,
                    Description = it.FullDescription,
                }).ToList();
            return result;
        }

        // GET: api/course/{course-catalog-id}/related
        /// <summary>
        /// Get related courses
        /// </summary>
        /// <param name="id">Group name</param>
        [HttpGet("{id}/related")]
        public IEnumerable<CourseCatalogRespond> GetRelatedCourses(string id)
        {
            var isArgumentValid = !string.IsNullOrEmpty(id);
            if (!isArgumentValid) return Enumerable.Empty<CourseCatalogRespond>();

            var courses = _repo.GetRelatedCoursesByGroupName(id).ToList();
            var anyCourses = courses != null && courses.Any();
            if (!anyCourses) return Enumerable.Empty<CourseCatalogRespond>();

            var result = courses
                .Where(it => it != null)
                .Where(it => !it.DeletedDate.HasValue)
                .Select(it => new CourseCatalogRespond
                {
                    id = it.id,
                    Name = it.SideName,
                    GroupName = it.GroupName,
                    Description = it.FullDescription,
                }).ToList();
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

            var relatedCourses = GetRelatedCourses(selectedCourse.GroupName)
                .Where(it => it.id != id)
                .Select(it => new GetCourseDetailRespond.RelatedCourse
                {
                    CourseId = it.id,
                    Name = it.Name
                }).ToList();

            var result = new GetCourseDetailRespond
            {
                id = selectedCourse.id,
                GroupName = selectedCourse.GroupName,
                Grade = selectedCourse.Grade,
                SideName = selectedCourse.SideName,
                PriceUSD = selectedCourse.PriceUSD,
                Series = selectedCourse.Series,
                Title = selectedCourse.Title,
                FullDescription = selectedCourse.FullDescription,
                DescriptionImageUrl = selectedCourse.DescriptionImageUrl,
                TotalWeeks = selectedCourse.TotalWeeks,
                CreatedDate = selectedCourse.CreatedDate,
                RelatedCourses = relatedCourses,
                Semesters = selectedCourse.Semesters.Select(semester => new GetCourseDetailRespond.Semester
                {
                    Name = semester.Name,
                    Title = semester.Title,
                    Description = semester.Description,
                    TotalWeeks = semester.TotalWeeks,
                    Units = semester.Units.Select(unit => new GetCourseDetailRespond.Unit
                    {
                        UnitNo = unit.Order,
                        Title = unit.Title,
                        Description = unit.Description,
                        TotalWeeks = unit.TotalWeeks,
                        Lessons = unit.Lessons.Select(lesson => new GetCourseDetailRespond.Lesson
                        {
                            id = lesson.id,
                            Order = lesson.Order,
                            Contents = lesson.Contents.Select(it => new GetCourseDetailRespond.LessonContent
                            {
                                ContentUrl = it.ContentUrl,
                                Description = it.Description,
                                ImageUrl = it.ImageUrl,
                                IsPreviewable = it.IsPreviewable,
                            })
                        })
                    })
                }).ToList()
            };
            return result;
        }

        // GET: api/course/{course-catalog-id}/ads
        /// <summary>
        /// Get course ads
        /// </summary>
        /// <param name="id">Course catalog id</param>
        [HttpGet]
        [Route("{id}/ads")]
        public object GetAds(string id)
        {
            var selectedCourse = _repo.GetCourseCatalogById(id);
            if (selectedCourse == null) return null;
            var adsUrls = selectedCourse.Advertisements ?? Enumerable.Empty<string>();
            var result = new
            {
                owl = adsUrls.Select(it => new
                {
                    item = $"<div class='item'><img src='{ it }' /></div>"
                })
            };
            return result;
        }

        // GET: api/course/{course-catalog-id}/ads
        /// <summary>
        /// Get course ads
        /// </summary>
        /// <param name="id">Course catalog id</param>
        [HttpGet]
        [Route("{id}/previewads")]
        public async System.Threading.Tasks.Task<OwnCarousel> GetPreviewads(string id)
        {
            using (var http = new System.Net.Http.HttpClient())
            {
                var result = await http.GetStringAsync($"{ _appConfig.ManagementPortalUrl }/api/CourseCatalog/{ id }/ads");
                var courseCatalog = JsonConvert.DeserializeObject<OwnCarousel>(result);
                return courseCatalog;
            }
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
