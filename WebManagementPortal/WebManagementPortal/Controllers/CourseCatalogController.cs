using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebManagementPortal.EF;
using System.Data.Entity;

namespace WebManagementPortal.Controllers
{
    [RoutePrefix("api/coursecatalog")]
    public class CourseCatalogController : ApiController
    {
        // GET: api/CourseCatalog/{course-catalog-id}
        [Route("{id}")]
        public async Task<Models.CourseCatalog> Get(int id)
        {
            CourseCatalog courseCatalog;
            using (var dctx = new EF.MindSageDataModelsContainer())
            {
                courseCatalog = await dctx.CourseCatalogs
                    .Include("Semesters.Units.Lessons.Advertisements")
                    .Include("Semesters.Units.Lessons.TopicOfTheDays")
                    .FirstOrDefaultAsync(it => it.Id == id);
            }
            if (courseCatalog == null) return null;

            var semesterQry = courseCatalog.Semesters.Where(it => !it.RecLog.DeletedDate.HasValue);
            var unitQry = semesterQry.SelectMany(it => it.Units).Where(it => !it.RecLog.DeletedDate.HasValue);
            var lessonQry = unitQry.SelectMany(it => it.Lessons).Where(it => !it.RecLog.DeletedDate.HasValue);

            var lessonIdRunner = 1;
            var lessons = lessonQry.Select(it => new Models.CourseCatalog.Lesson
            {
                id = it.Id.ToString(),
                Order = lessonIdRunner++
                //Contents
            });

            var unitIdRunner = 1;
            var semesterNameRunner = (byte)65;
            var semesters = semesterQry.Select(it => new Models.CourseCatalog.Semester
            {
                id = it.Id.ToString(),
                Title = it.Title,
                Description = it.Description,
                Name = string.Format("{0}", (char)semesterNameRunner++),
                Units = unitQry.Where(unit => unit.SemesterId == it.Id).Select(unit => new Models.CourseCatalog.Unit
                {
                    id = unit.Id.ToString(),
                    Title = unit.Title,
                    Description = unit.Description,
                    Order = unitIdRunner++,
                    Lessons = lessons
                }),
            });

            var result = new Models.CourseCatalog
            {
                id = courseCatalog.Id.ToString(),
                Advertisements = courseCatalog.Advertisements.Split(new string[] { "#;" }, StringSplitOptions.RemoveEmptyEntries),
                CreatedDate = courseCatalog.RecLog.CreatedDate,
                DeletedDate = courseCatalog.RecLog.DeletedDate,
                DescriptionImageUrl = courseCatalog.DescriptionImageUrl,
                FullDescription = courseCatalog.FullDescription,
                Grade = courseCatalog.Grade,
                GroupName = courseCatalog.GroupName,
                PriceUSD = courseCatalog.PriceUSD,
                Semesters = semesters,
                Series = courseCatalog.Series,
                SideName = courseCatalog.SideName,
                Title = courseCatalog.Title,
            };
            return result;
        }

        // GET: api/CourseCatalog/{course-catalog-id}/detail
        [Route("{id}/detail")]
        public async Task<Models.GetCourseDetailRespond> GetDetail(int id)
        {
            CourseCatalog courseCatalog;
            var relatedCourseList = Enumerable.Empty<CourseCatalog>();
            using (var dctx = new EF.MindSageDataModelsContainer())
            {
                courseCatalog = await dctx.CourseCatalogs
                    .Include("Semesters.Units.Lessons.Advertisements")
                    .Include("Semesters.Units.Lessons.TopicOfTheDays")
                    .FirstOrDefaultAsync(it => it.Id == id);

                if (courseCatalog != null)
                {
                    relatedCourseList = dctx.CourseCatalogs
                        .Where(it => it.GroupName == courseCatalog.GroupName)
                        .Where(it => it.Id != courseCatalog.Id)
                        .Where(it => !it.RecLog.DeletedDate.HasValue)
                        .ToList();
                }
            }
            if (courseCatalog == null) return null;

            var semesterQry = courseCatalog.Semesters.Where(it => !it.RecLog.DeletedDate.HasValue);
            var unitQry = semesterQry.SelectMany(it => it.Units).Where(it => !it.RecLog.DeletedDate.HasValue);
            var lessonQry = unitQry.SelectMany(it => it.Lessons).Where(it => !it.RecLog.DeletedDate.HasValue);

            var lessonIdRunner = 1;
            var lessons = lessonQry.Select(it => new Models.GetCourseDetailRespond.Lesson
            {
                id = it.Id.ToString(),
                Order = lessonIdRunner++,
                Contents = Enumerable.Empty<Models.GetCourseDetailRespond.LessonContent>() // HACK: Create lesson's contents
            });

            var unitIdRunner = 1;
            var semesterNameRunner = (byte)65;
            var semesters = semesterQry.Select(it => new Models.GetCourseDetailRespond.Semester
            {
                Title = it.Title,
                Description = it.Description,
                Name = string.Format("{0}", (char)semesterNameRunner++),
                TotalWeeks = it.TotalWeeks,
                Units = unitQry.Where(unit => unit.SemesterId == it.Id).Select(unit => new Models.GetCourseDetailRespond.Unit
                {
                    Title = unit.Title,
                    Description = unit.Description,
                    UnitNo = unitIdRunner++,
                    Lessons = lessons,
                    TotalWeeks = unit.TotalWeeks
                }),
            });

            var relatedCourses = (relatedCourseList ?? Enumerable.Empty<CourseCatalog>())
                .Select(it => new Models.GetCourseDetailRespond.RelatedCourse
                {
                    CourseId = it.Id.ToString(),
                    Name = it.SideName
                });
            var result = new Models.GetCourseDetailRespond
            {
                id = courseCatalog.Id.ToString(),
                CreatedDate = courseCatalog.RecLog.CreatedDate,
                DescriptionImageUrl = courseCatalog.DescriptionImageUrl,
                FullDescription = courseCatalog.FullDescription,
                Grade = courseCatalog.Grade,
                GroupName = courseCatalog.GroupName,
                PriceUSD = courseCatalog.PriceUSD,
                Semesters = semesters,
                Series = courseCatalog.Series,
                SideName = courseCatalog.SideName,
                Title = courseCatalog.Title,
                RelatedCourses = relatedCourses,
                TotalWeeks = courseCatalog.TotalWeeks
            };
            return result;
        }

        [HttpGet]
        [Route("{id}/ads")]
        public async Task<Models.OwnCarousel> GetAds(int id)
        {
            CourseCatalog courseCatalog;
            using (var dctx = new EF.MindSageDataModelsContainer())
            {
                courseCatalog = await dctx.CourseCatalogs
                    .Include("Semesters.Units.Lessons.Advertisements")
                    .Include("Semesters.Units.Lessons.TopicOfTheDays")
                    .FirstOrDefaultAsync(it => it.Id == id);
            }
            if (courseCatalog == null) return null;
            var adsUrls = courseCatalog?.Advertisements?.Split(new string[] { "#;" }, StringSplitOptions.RemoveEmptyEntries) ?? Enumerable.Empty<string>();
            var result = new Models.OwnCarousel
            {
                owl = adsUrls.Select(it => new Models.OwnCarousel.OwnItem
                {
                    item = $"<div class='item'><img src='{ it }' /></div>"
                })
            };
            return result;
        }
    }
}
