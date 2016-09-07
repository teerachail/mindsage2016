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
    public class StudentLessonItemsController : Controller
    {
        private MindSageDataModelsContainer db = new MindSageDataModelsContainer();

        // GET: StudentLessonItems
        //public ActionResult Index()
        //{
        //    var lessonItems = db.LessonItems.Include(l => l.TeacherLesson).Include(l => l.StudentLesson);
        //    return View(lessonItems.ToList());
        //}

        // GET: StudentLessonItems/Details/5
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

        // GET: StudentLessonItems/Create
        public async Task<ActionResult> Create(int id)
        {
            var lesson = await db.Lessons.FirstOrDefaultAsync(it => it.Id == id);
            if (lesson == null || lesson.RecLog.DeletedDate.HasValue) return View("Error");

            return View(new LessonItem
            {
                StudentLesson = lesson,
                StudentLessonId = lesson.Id,
            });
        }

        // POST: StudentLessonItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,IconURL,Description,IsPreviewable,ContentType,ContentURL,TeacherLessonId,StudentLessonId,RecLog")] LessonItem lessonItem)
        {
            if (ModelState.IsValid)
            {
                var lesson = await db.Lessons.FirstOrDefaultAsync(it => it.Id == lessonItem.StudentLessonId);
                if (lesson == null || lesson.RecLog.DeletedDate.HasValue) return View("Error");

                lessonItem.Order = lesson.StudentLessonItems.Where(it => !it.RecLog.DeletedDate.HasValue).Count() + 1;
                lessonItem.RecLog.CreatedDate = DateTime.Now;
                lessonItem.IconURL = ControllerHelper.ConvertToIconUrl(lessonItem.ContentType);
                db.LessonItems.Add(lessonItem);
                db.SaveChanges();
                return RedirectToAction("Details", "Lessons", new { @id = lessonItem.StudentLessonId });
            }

            return View(lessonItem);
        }

        // GET: StudentLessonItems/Edit/5
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

        // POST: StudentLessonItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,IconURL,Description,IsPreviewable,ContentType,ContentURL,TeacherLessonId,StudentLessonId,RecLog")] LessonItem lessonItem)
        {
            if (ModelState.IsValid)
            {
                var selectedLessonItem = await db.LessonItems.FirstOrDefaultAsync(it => it.Id == lessonItem.Id);
                if (selectedLessonItem == null) return View("Error");

                selectedLessonItem.Name = lessonItem.Name;
                selectedLessonItem.Description = lessonItem.Description;
                selectedLessonItem.IsPreviewable = lessonItem.IsPreviewable;
                selectedLessonItem.ContentType = lessonItem.ContentType;
                selectedLessonItem.ContentURL = lessonItem.ContentURL;
                selectedLessonItem.IconURL = ControllerHelper.ConvertToIconUrl(lessonItem.ContentType);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Lessons", new { @id = lessonItem.StudentLessonId });
            }

            return View(lessonItem);
        }

        // GET: StudentLessonItems/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: StudentLessonItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LessonItem lessonItem = db.LessonItems.Find(id);
            lessonItem.RecLog.DeletedDate = DateTime.Now;
            var lessonId = lessonItem.StudentLessonId;

            var deleteOrder = lessonItem.Order;
            var lessonList = db.LessonItems.Where(it => !it.RecLog.DeletedDate.HasValue && it.StudentLessonId == lessonId && it.Order > deleteOrder);
            foreach (var item in lessonList) item.Order--;

            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Lessons", new { @id = lessonId });
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
