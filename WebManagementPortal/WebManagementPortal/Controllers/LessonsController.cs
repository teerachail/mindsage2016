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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,ShortDescription,MoreDescription,ShortTeacherLessonPlan,MoreTeacherLessonPlan,PrimaryContentURL,PrimaryContentDescription,IsPreviewable,RecLog,UnitId")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                var selectedLesson = await db.Lessons.FirstOrDefaultAsync(it => it.Id == lesson.Id);
                if (selectedLesson == null) return View("Error");

                selectedLesson.Title = lesson.Title;
                selectedLesson.ShortDescription = lesson.ShortDescription;
                selectedLesson.MoreDescription = lesson.MoreDescription;
                selectedLesson.ShortTeacherLessonPlan = lesson.ShortTeacherLessonPlan;
                selectedLesson.MoreTeacherLessonPlan = lesson.MoreTeacherLessonPlan;
                selectedLesson.PrimaryContentURL = lesson.PrimaryContentURL;
                selectedLesson.PrimaryContentDescription = lesson.PrimaryContentDescription;
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
