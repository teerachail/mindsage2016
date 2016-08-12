using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebManagementPortal.EF;

namespace WebManagementPortal.Controllers
{
    public class AssessmentsController : Controller
    {
        private MindSageDataModelsContainer db = new MindSageDataModelsContainer();

        // GET: Assessments
        //public ActionResult Index()
        //{
        //    var assessments = db.Assessments.Include(a => a.AssessmentItem);
        //    return View(assessments.ToList());
        //}

        // GET: Assessments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assessment assessment = db.Assessments.Find(id);
            if (assessment == null)
            {
                return HttpNotFound();
            }
            return View(assessment);
        }

        // GET: Assessments/Create
        public async Task<ActionResult> Create(int id, string contentType = "QA")
        {
            var assessmentItem = await db.AssessmentItems.FirstOrDefaultAsync(it => it.Id == id);
            if (assessmentItem == null || assessmentItem.RecLog.DeletedDate.HasValue) return View("Error");

            var lesson = assessmentItem.PreLesson ?? assessmentItem.PostLesson;
            ViewBag.CourseName = lesson.Unit.Semester.CourseCatalog.SideName;
            ViewBag.SemesterName = lesson.Unit.Semester.Title;
            ViewBag.UnitName = lesson.Unit.Title;
            ViewBag.LessonName = lesson.Title;

            return View(new Assessment
            {
                AssessmentItem = assessmentItem,
                AssessmentItemId = assessmentItem.Id,
                ContentType = contentType
            });
        }

        // POST: Assessments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Order,ContentType,Question,StatementBefore,StatementAfter,AssessmentItemId,RecLog")] Assessment assessment, IEnumerable<string> Choices, IEnumerable<string> IsCorrects)
        {
            if (ModelState.IsValid)
            {
                var assessmentItem = await db.AssessmentItems.FirstOrDefaultAsync(it => it.Id == assessment.AssessmentItemId);
                if (assessmentItem == null || assessmentItem.RecLog.DeletedDate.HasValue) return View("Error");

                var now = DateTime.Now;
                int choiceIndex = 0;
                var choices = (Choices ?? Enumerable.Empty<string>()).Select(it =>
                {
                    return new Choice
                    {
                        Assessment = assessment,
                        Name = it,
                        RecLog = new RecordLog { CreatedDate = now },
                        IsCorrect = IsCorrects.Contains((choiceIndex++).ToString())
                    };
                }).ToList();
                if (choices.Any()) db.Choices.AddRange(choices);

                assessment.RecLog.CreatedDate = now;
                assessment.Order = assessmentItem.Assessments.Count + 1;
                db.Assessments.Add(assessment);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "PreAssessmentItems", new { @id = assessment.AssessmentItemId });
            }

            return View(assessment);
        }

        // GET: Assessments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assessment assessment = db.Assessments.Find(id);
            if (assessment == null)
            {
                return HttpNotFound();
            }

            var lesson = assessment.AssessmentItem.PreLesson ?? assessment.AssessmentItem.PostLesson;
            ViewBag.CourseName = lesson.Unit.Semester.CourseCatalog.SideName;
            ViewBag.SemesterName = lesson.Unit.Semester.Title;
            ViewBag.UnitName = lesson.Unit.Title;
            ViewBag.LessonName = lesson.Title;

            return View(assessment);
        }

        // POST: Assessments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Order,ContentType,Question,StatementBefore,StatementAfter,AssessmentItemId,RecLog")] Assessment assessment, IEnumerable<string> Choices, IEnumerable<string> IsCorrects)
        {
            if (ModelState.IsValid)
            {
                var selectedAssessment = await db.Assessments.FirstOrDefaultAsync(it => it.Id == assessment.Id);
                if (selectedAssessment == null) return View("Error");

                selectedAssessment.Question = assessment.Question;
                db.Choices.RemoveRange(selectedAssessment.Choices);
                
                var now = DateTime.Now;
                int choiceIndex = 0;
                var choices = (Choices ?? Enumerable.Empty<string>()).Select(it =>
                {
                    return new Choice
                    {
                        Assessment = selectedAssessment,
                        Name = it,
                        RecLog = new RecordLog { CreatedDate = now },
                        IsCorrect = IsCorrects.Contains((choiceIndex++).ToString())
                    };
                }).ToList();
                db.Choices.AddRange(choices);

                await db.SaveChangesAsync();
                return RedirectToAction("Details", "PreAssessmentItems", new { @id = selectedAssessment.AssessmentItemId });
            }

            return View(assessment);
        }

        // GET: Assessments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assessment assessment = db.Assessments.Find(id);
            if (assessment == null)
            {
                return HttpNotFound();
            }
            var lesson = assessment.AssessmentItem.PreLesson ?? assessment.AssessmentItem.PostLesson;
            ViewBag.CourseName = lesson.Unit.Semester.CourseCatalog.SideName;
            ViewBag.SemesterName = lesson.Unit.Semester.Title;
            ViewBag.UnitName = lesson.Unit.Title;
            ViewBag.LessonName = lesson.Title;
            return View(assessment);
        }

        // POST: Assessments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Assessment assessment = db.Assessments.Find(id);
            assessment.RecLog.DeletedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "PreAssessmentItems", new { @id = assessment.AssessmentItemId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
