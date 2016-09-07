using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebManagementPortal.EF;

namespace WebManagementPortal.Controllers
{
    [Authorize]
    public class LessonsController : Controller
    {
        private MindSageDataModelsContainer db = new MindSageDataModelsContainer();

        //// GET: Lessons
        //public async Task<ActionResult> Index()
        //{
        //    var lessons = db.Lessons.Include(l => l.Unit);
        //    return View(await lessons.ToListAsync());
        //}

        // GET: Lessons/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = await db.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        [HttpPost]
        public async Task<ActionResult> ItemReorder(int? id, bool isUp)
        {
            LessonItem selected = await db.LessonItems.FirstOrDefaultAsync(it => it.Id == id);
            if (selected == null) return View("Error");

            var selectedOrder = isUp? --selected.Order : ++selected.Order;
            var lessonId = selected.TeacherLessonId == null ? selected.StudentLessonId : selected.TeacherLessonId;

            LessonItem swapItem;
            if (selected.TeacherLessonId == null)
                swapItem = await db.LessonItems.FirstOrDefaultAsync(it => !it.RecLog.DeletedDate.HasValue && it.StudentLessonId == lessonId && it.Order == selectedOrder);
            else
                swapItem = await db.LessonItems.FirstOrDefaultAsync(it => !it.RecLog.DeletedDate.HasValue && it.TeacherLessonId == lessonId && it.Order == selectedOrder);

            if(swapItem == null) return View("Error");

            swapItem.Order = isUp ? ++swapItem.Order : --swapItem.Order;

            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Lessons", new { @id = lessonId });
        }

        [HttpPost]
        public async Task<ActionResult> AssessmentReorder(int? id, bool isUp)
        {
            AssessmentItem selected = await db.AssessmentItems.FirstOrDefaultAsync(it => it.Id == id);
            if (selected == null) return View("Error");

            var selectedOrder = isUp ? --selected.Order : ++selected.Order;
            var lessonId = selected.PreLessonId == null ? selected.PostLessonId : selected.PreLessonId;

            AssessmentItem swapItem;
            if (selected.PreLessonId == null)
                swapItem = await db.AssessmentItems.FirstOrDefaultAsync(it => !it.RecLog.DeletedDate.HasValue && it.PostLessonId == lessonId && it.Order == selectedOrder);
            else
                swapItem = await db.AssessmentItems.FirstOrDefaultAsync(it => !it.RecLog.DeletedDate.HasValue && it.PreLessonId == lessonId && it.Order == selectedOrder);

            if (swapItem == null) return View("Error");

            swapItem.Order = isUp ? ++swapItem.Order : --swapItem.Order;

            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Lessons", new { @id = lessonId });
        }

        // GET: Lessons/Create
        public async Task<ActionResult> Create(int id)
        {
            var unit = await db.Units.FirstOrDefaultAsync(it => it.Id == id);
            if (unit == null || unit.RecLog.DeletedDate.HasValue) return View("Error");

            return View(new Lesson
            {
                Unit = unit,
                UnitId = unit.Id
            });
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,ShortDescription,MoreDescription,ShortTeacherLessonPlan,MoreTeacherLessonPlan,PrimaryContentURL,PrimaryContentDescription,IsPreviewable,RecLog,UnitId")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                var unit = await db.Units.Include("Semester.CourseCatalog").FirstOrDefaultAsync(it => it.Id == lesson.UnitId);
                if (unit == null || unit.RecLog.DeletedDate.HasValue) return View("Error");

                lesson.RecLog.CreatedDate = DateTime.Now;
                db.Lessons.Add(lesson);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "CourseCatalogs", new { @id = unit.Semester.CourseCatalogId });
            }

            return View(lesson);
        }

        // GET: Lessons/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = await db.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }

            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,ShortDescription,MoreDescription,ShortTeacherLessonPlan,MoreTeacherLessonPlan,PrimaryContentURL,PrimaryContentDescription,IsPreviewable,RecLog,UnitId")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                var selectedLesson = await db.Lessons.FirstOrDefaultAsync(it => it.Id == lesson.Id);
                if (selectedLesson == null) return View("Error");

                selectedLesson.Title = lesson.Title;
                selectedLesson.IsPreviewable = lesson.IsPreviewable;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Lessons", new { @id = lesson.Id });
            }
            
            return View(lesson);
        }

        // GET: Lessons/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = await db.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Lesson lesson = await db.Lessons.FindAsync(id);
            var now = DateTime.Now;
            lesson.RecLog.DeletedDate = now;
            foreach (var item in lesson.Advertisements) item.RecLog.DeletedDate = now;
            foreach (var item in lesson.TopicOfTheDays) item.RecLog.DeletedDate = now;
            foreach (var item in lesson.TeacherLessonItems) item.RecLog.DeletedDate = now;
            foreach (var item in lesson.StudentLessonItems) item.RecLog.DeletedDate = now;
            foreach (var item in lesson.PreAssessments) item.RecLog.DeletedDate = now;
            foreach (var item in lesson.PostAssessments) item.RecLog.DeletedDate = now;

            await db.SaveChangesAsync();
            return RedirectToAction("Details", "CourseCatalogs", new { @id = lesson.Unit.Semester.CourseCatalogId });
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
