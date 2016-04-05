using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using WebManagementPortal.EF;
using WebManagementPortal.Repositories.Models;

namespace WebManagementPortal.Controllers
{
    [RoutePrefix("api/lessonpreview")]
    public class LessonPreviewController : ApiController
    {
        // GET: api/lessonpreview/{lesson-id}
        [Route("{id}/lesson")]
        public async Task<LessonContentRespond> GetLesson(int id)
        {
            EF.CourseCatalog courseCatalog;
            Lesson lessonCatalog;
            using (var dctx = new EF.MindSageDataModelsContainer())
            {
                lessonCatalog = await dctx.Lessons
                    .Include("Unit.Semester")
                    .Include("ExtraContents")
                    .FirstOrDefaultAsync(it => it.Id == id);
                if (lessonCatalog == null) return null;

                courseCatalog = await dctx.CourseCatalogs
                    .Include("Semesters.Units")
                    .FirstOrDefaultAsync(it => it.Id == lessonCatalog.Unit.Semester.CourseCatalogId);
                if (courseCatalog == null) return null;
            }

            var semesters = courseCatalog.Semesters.Where(it => !it.RecLog.DeletedDate.HasValue).OrderBy(it => it.RecLog.CreatedDate);
            var semesterRunner = 65;
            foreach (var item in semesters)
            {
                if (item.Id == lessonCatalog.Unit.SemesterId) break;
                semesterRunner++;
            }

            var units = semesters.SelectMany(it => it.Units).Where(it => !it.RecLog.DeletedDate.HasValue).OrderBy(it => it.RecLog.CreatedDate);
            var unitRunner = 1;
            foreach (var item in units)
            {
                if (item.Id == lessonCatalog.UnitId) break;
                unitRunner++;
            }
            var extraContents = lessonCatalog.ExtraContents
                                .Where(it => !it.RecLog.DeletedDate.HasValue)
                                .Select(it => new LessonContentRespond.ExtraContent
                                {
                                    id = it.Id.ToString(),
                                    ContentURL = it.ContentURL,
                                    Description = it.Description,
                                    IconURL = it.IconURL
                                });
            var result = new LessonContentRespond
            {
                ExtraContents = extraContents,
                CreatedDate = lessonCatalog.RecLog.CreatedDate,
                PrimaryContentURL = lessonCatalog.PrimaryContentURL,
                PrimaryContentDescription = lessonCatalog.PrimaryContentDescription,
                IsPreviewable = lessonCatalog.IsPreviewable,
                SemesterName = string.Format("{0}", (char)semesterRunner),
                ShortDescription = lessonCatalog.ShortDescription,
                ShortTeacherLessonPlan = lessonCatalog.ShortTeacherLessonPlan,
                MoreDescription = lessonCatalog.MoreDescription,
                MoreTeacherLessonPlan = lessonCatalog.MoreTeacherLessonPlan,
                Title = lessonCatalog.Title,
                Order = unitRunner
            };
            return result;
        }

        [HttpGet]
        [Route("{id}/ads")]
        public async Task<OwnCarousel> GetAds(int id)
        {
            Lesson lessonCatalog;
            using (var dctx = new EF.MindSageDataModelsContainer())
            {
                lessonCatalog = await dctx.Lessons
                    .Include("Advertisements")
                    .FirstOrDefaultAsync(it => it.Id == id);
            }
            if (lessonCatalog == null) return null;
            var adsUrls = (lessonCatalog.Advertisements ?? Enumerable.Empty<Advertisement>())
                .Where(it => !it.RecLog.DeletedDate.HasValue)
                .Select(it => it.ImageUrl);
            var result = new OwnCarousel
            {
                owl = adsUrls.Select(it => new OwnCarousel.OwnItem
                {
                    item = $"<div class='item'><img src='{ it }' /></div>"
                })
            };
            return result;
        }
    }
}
