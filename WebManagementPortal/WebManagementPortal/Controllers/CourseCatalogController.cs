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
            var units = unitQry.Select(it => new Models.CourseCatalog.Unit
            {
                id = it.Id.ToString(),
                Title = it.Title,
                Description = it.Description,
                Order = unitIdRunner++,
                Lessons = lessons
            });

            var semesterNameRunner = (byte)65;
            var semesters = semesterQry.Select(it => new Models.CourseCatalog.Semester
            {
                id = it.Id.ToString(),
                Title = it.Title,
                Description = it.Description,
                Units = units,
                Name = string.Format("{0}", (char)semesterNameRunner++)
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
    }
}
