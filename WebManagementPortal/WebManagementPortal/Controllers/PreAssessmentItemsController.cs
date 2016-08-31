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
    public class PreAssessmentItemsController : Controller
    {
        private MindSageDataModelsContainer db = new MindSageDataModelsContainer();

        // GET: PreAssessmentItems
        //public ActionResult Index()
        //{
        //    var assessmentItems = db.AssessmentItems.Include(a => a.PreLesson).Include(a => a.PostLesson);
        //    return View(assessmentItems.ToList());
        //}

        // GET: PreAssessmentItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssessmentItem assessmentItem = db.AssessmentItems.Find(id);
            if (assessmentItem == null)
            {
                return HttpNotFound();
            }
            return View(assessmentItem);
        }

        // GET: PreAssessmentItems/Create
        public async Task<ActionResult> Create(int id)
        {
            var lesson = await db.Lessons.FirstOrDefaultAsync(it => it.Id == id);
            if (lesson == null || lesson.RecLog.DeletedDate.HasValue) return View("Error");

            return View(new AssessmentItem
            {
                PreLesson = lesson,
                PreLessonId = lesson.Id
            });
        }

        // POST: PreAssessmentItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,IconURL,IsPreviewable,PreLessonId,PostLessonId,RecLog")] AssessmentItem assessmentItem)
        {
            if (ModelState.IsValid)
            {
                var lesson = await db.Lessons.FirstOrDefaultAsync(it => it.Id == assessmentItem.PreLessonId);
                if (lesson == null || lesson.RecLog.DeletedDate.HasValue) return View("Error");

                assessmentItem.RecLog.CreatedDate = DateTime.Now;
                var IconUrl = ExtraContentType.PreAssessment.ToString();
                assessmentItem.IconURL = ControllerHelper.ConvertToIconUrl(IconUrl);
                db.AssessmentItems.Add(assessmentItem);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Lessons", new { @id = assessmentItem.PreLessonId });
            }

            return View(assessmentItem);
        }

        // GET: PreAssessmentItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssessmentItem assessmentItem = db.AssessmentItems.Find(id);
            if (assessmentItem == null)
            {
                return HttpNotFound();
            }

            return View(assessmentItem);
        }

        // POST: PreAssessmentItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,IconURL,IsPreviewable,PreLessonId,PostLessonId,RecLog")] AssessmentItem assessmentItem)
        {
            if (ModelState.IsValid)
            {
                var selectedAssessmentItem = await db.AssessmentItems.FirstOrDefaultAsync(it => it.Id == assessmentItem.Id);
                if (selectedAssessmentItem == null) return View("Error");

                selectedAssessmentItem.Name = assessmentItem.Name;
                selectedAssessmentItem.IsPreviewable = assessmentItem.IsPreviewable;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Lessons", new { @id = assessmentItem.PreLessonId });
            }

            return View(assessmentItem);
        }

        // GET: PreAssessmentItems/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssessmentItem assessmentItem = await db.AssessmentItems.FindAsync(id);
            if (assessmentItem == null)
            {
                return HttpNotFound();
            }
            return View(assessmentItem);
        }

        // POST: PreAssessmentItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AssessmentItem assessmentItem = await db.AssessmentItems.FindAsync(id);
            var now = DateTime.Now;
            assessmentItem.RecLog.DeletedDate = now;
            foreach (var item in assessmentItem.Assessments) item.RecLog.DeletedDate = now;

            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Lessons", new { @id = assessmentItem.PreLessonId });
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
