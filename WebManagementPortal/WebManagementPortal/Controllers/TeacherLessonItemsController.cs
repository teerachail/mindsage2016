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
    public class TeacherLessonItemsController : Controller
    {
        private MindSageDataModelsContainer db = new MindSageDataModelsContainer();

        // GET: TeacherLessonItems
        //public ActionResult Index()
        //{
        //    var lessonItems = db.LessonItems.Include(l => l.TeacherLesson).Include(l => l.StudentLesson);
        //    return View(lessonItems.ToList());
        //}

        // GET: TeacherLessonItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonItem lessonItem = db.LessonItems.Find(id);
            if (lessonItem == null)
            {
                return HttpNotFound();
            }
            return View(lessonItem);
        }

        // GET: TeacherLessonItems/Create
        public async Task<ActionResult> Create(int id)
        {
            var lesson = await db.Lessons.FirstOrDefaultAsync(it => it.Id == id);
            if (lesson == null || lesson.RecLog.DeletedDate.HasValue) return View("Error");

            return View(new LessonItem
            {
                TeacherLesson = lesson,
                TeacherLessonId = lesson.Id,
            });
        }

        // POST: TeacherLessonItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,IconURL,Description,IsPreviewable,ContentType,ContentURL,TeacherLessonId,StudentLessonId,RecLog")] LessonItem lessonItem)
        {
            if (ModelState.IsValid)
            {
                var lesson = await db.Lessons.FirstOrDefaultAsync(it => it.Id == lessonItem.TeacherLessonId);
                if (lesson == null || lesson.RecLog.DeletedDate.HasValue) return View("Error");

                lessonItem.RecLog.CreatedDate = DateTime.Now;
                lessonItem.IconURL = ControllerHelper.ConvertToIconUrl(lessonItem.ContentType);
                db.LessonItems.Add(lessonItem);
                db.SaveChanges();
                return RedirectToAction("Details", "Lessons", new { @id = lessonItem.TeacherLessonId });
            }

            return View(lessonItem);
        }

        // GET: TeacherLessonItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonItem lessonItem = db.LessonItems.Find(id);
            if (lessonItem == null)
            {
                return HttpNotFound();
            }

            return View(lessonItem);
        }

        // POST: TeacherLessonItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,IconURL,Description,IsPreviewable,ContentType,ContentURL,TeacherLessonId,StudentLessonId,RecLog")] LessonItem lessonItem)
        {
            if (ModelState.IsValid)
            {
                var selectedTeacherItem = await db.LessonItems.FirstOrDefaultAsync(it => it.Id == lessonItem.Id);
                if (selectedTeacherItem == null) return View("Error");

                selectedTeacherItem.Name = lessonItem.Name;
                selectedTeacherItem.Description = lessonItem.Description;
                selectedTeacherItem.IsPreviewable = lessonItem.IsPreviewable;
                selectedTeacherItem.ContentType = lessonItem.ContentType;
                selectedTeacherItem.ContentURL = lessonItem.ContentURL;
                selectedTeacherItem.IconURL = ControllerHelper.ConvertToIconUrl(lessonItem.ContentType);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Lessons", new { @id = lessonItem.TeacherLessonId });
            }

            return View(lessonItem);
        }

        // GET: TeacherLessonItems/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonItem lessonItem = await db.LessonItems.FindAsync(id);
            if (lessonItem == null)
            {
                return HttpNotFound();
            }
            return View(lessonItem);
        }

        // POST: TeacherLessonItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LessonItem lessonItem = await db.LessonItems.FindAsync(id);
            lessonItem.RecLog.DeletedDate = DateTime.Now;

            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Lessons", new { @id = lessonItem.TeacherLessonId });
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
