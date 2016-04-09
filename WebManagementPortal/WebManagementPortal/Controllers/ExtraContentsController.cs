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
    [Authorize(Users = "admin@mindsage.com")]
    public class ExtraContentsController : Controller
    {
        private MindSageDataModelsContainer db = new MindSageDataModelsContainer();

        //// GET: ExtraContents
        //public async Task<ActionResult> Index()
        //{
        //    var extraContents = db.ExtraContents.Include(e => e.Lesson);
        //    return View(await extraContents.ToListAsync());
        //}

        //// GET: ExtraContents/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ExtraContent extraContent = await db.ExtraContents.FindAsync(id);
        //    if (extraContent == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(extraContent);
        //}

        // GET: ExtraContents/Create
        public async Task<ActionResult> Create(int id)
        {
            var lesson = await db.Lessons.FirstAsync(it => it.Id == id);
            if (lesson == null) return View("Error");

            return View(new ExtraContent
            {
                Lesson = lesson,
                LessonId = lesson.Id
            });
        }

        // POST: ExtraContents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,IsPreviewable,ContentURL,Description,IconURL,LessonId,RecLog")] ExtraContent extraContent)
        {
            if (ModelState.IsValid)
            {
                extraContent.RecLog.CreatedDate = DateTime.Now;
                db.ExtraContents.Add(extraContent);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Lessons", new { @id = extraContent.LessonId });
            }

            return View(extraContent);
        }

        // GET: ExtraContents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExtraContent extraContent = await db.ExtraContents.FindAsync(id);
            if (extraContent == null)
            {
                return HttpNotFound();
            }
            return View(extraContent);
        }

        // POST: ExtraContents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,IsPreviewable,ContentURL,Description,IconURL,LessonId,RecLog")] ExtraContent extraContent)
        {
            if (ModelState.IsValid)
            {
                var selectedExtraContent = await db.ExtraContents.FirstOrDefaultAsync(it => it.Id == extraContent.Id);
                if (selectedExtraContent == null) return View("Error");

                selectedExtraContent.IsPreviewable = extraContent.IsPreviewable;
                selectedExtraContent.ContentURL = extraContent.ContentURL;
                selectedExtraContent.Description = extraContent.Description;
                selectedExtraContent.IconURL = extraContent.IconURL;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Lessons", new { @id = extraContent.LessonId });
            }
            return View(extraContent);
        }

        // GET: ExtraContents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExtraContent extraContent = await db.ExtraContents.FindAsync(id);
            if (extraContent == null)
            {
                return HttpNotFound();
            }
            return View(extraContent);
        }

        // POST: ExtraContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ExtraContent extraContent = await db.ExtraContents.FindAsync(id);
            extraContent.RecLog.DeletedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Lessons", new { @id = extraContent.LessonId });
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
