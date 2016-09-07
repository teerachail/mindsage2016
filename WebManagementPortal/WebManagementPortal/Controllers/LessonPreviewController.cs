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
                    .Include("TeacherLessonItems")
                    .Include("TeacherLessonItems")
                    .Include("StudentLessonItems")
                    .Include("PreAssessments.Assessments.Choices")
                    .Include("PostAssessments.Assessments.Choices")
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
            var teacherItemQry = createLessonItems(lessonCatalog.TeacherLessonItems);
            var studentItemQry = createLessonItems(lessonCatalog.StudentLessonItems);
            var preAssessmentQry = createAssessmentItems(lessonCatalog.PreAssessments);
            var postAssessmentQry = createAssessmentItems(lessonCatalog.PostAssessments);
            var result = new LessonContentRespond
            {
                //ExtraContents = extraContents,
                CreatedDate = lessonCatalog.RecLog.CreatedDate,
                IsPreviewable = lessonCatalog.IsPreviewable,
                SemesterName = string.Format("{0}", (char)semesterRunner),
                Title = lessonCatalog.Title,
                Order = unitRunner,
                TeacherItems = teacherItemQry,
                StudentItems = studentItemQry,
                PreAssessments = preAssessmentQry,
                PostAssessments = postAssessmentQry,
            };
            return result;
        }
        private IEnumerable<LessonContentRespond.LessonItem> createLessonItems(IEnumerable<LessonItem> collection)
        {
            var isParameterValid = collection != null && collection.Any();
            if (!isParameterValid) return Enumerable.Empty<LessonContentRespond.LessonItem>();

            return collection
                .Where(it => !it.RecLog.DeletedDate.HasValue)
                .Select(it => new LessonContentRespond.LessonItem
                {
                    id = it.Id.ToString(),
                    Order = it.Order,
                    Name = it.Name,
                    IsPreviewable = it.IsPreviewable,
                    Description = it.Description,
                    IconURL = it.IconURL,
                    ContentType = it.ContentType,
                    ContentURL = it.ContentURL,
                });
        }
        private IEnumerable<LessonContentRespond.AssessmentItem> createAssessmentItems(IEnumerable<AssessmentItem> collection)
        {
            var isParameterValid = collection != null && collection.Any();
            if (!isParameterValid) return Enumerable.Empty<LessonContentRespond.AssessmentItem>();

            return collection
                .Where(assessmentItem => !assessmentItem.RecLog.DeletedDate.HasValue)
                .Select(assessmentItem =>
                {
                    var assessmentQry = assessmentItem.Assessments
                         .Where(it => !it.RecLog.DeletedDate.HasValue)
                         .Select(it => new LessonContentRespond.Assessment
                         {
                             id = it.Id.ToString(),
                             Order = it.Order,
                             ContentType = it.ContentType,
                             Question = it.Question,
                             StatementAfter = it.StatementAfter,
                             StatementBefore = it.StatementBefore,
                             Choices = it.Choices
                                 .Where(choice => !choice.RecLog.DeletedDate.HasValue)
                                 .Select(choice => new LessonContentRespond.Choice
                                 {
                                     id = choice.Id.ToString(),
                                     Name = choice.Name,
                                     IsCorrect = choice.IsCorrect,
                                 })
                         });
                    return new LessonContentRespond.AssessmentItem
                    {
                        id = assessmentItem.Id.ToString(),
                        Order = assessmentItem.Order,
                        Name = assessmentItem.Name,
                        IsPreviewable = assessmentItem.IsPreviewable,
                        IconURL = assessmentItem.IconURL,
                        Assessments = assessmentQry
                    };
                });
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
