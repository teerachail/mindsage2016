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
    public class SemestersController : Controller
    {
        private MindSageDataModelsContainer db = new MindSageDataModelsContainer();

        // GET: Semesters
        public async Task<ActionResult> Index()
        {
            var semesters = db.Semesters.Include(s => s.CourseCatalog)
                .Where(it => !it.RecLog.DeletedDate.HasValue)
                .OrderBy(it => it.RecLog.CreatedDate);
            return View(await semesters.ToListAsync());
        }

        // GET: Semesters/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Semester semester = await db.Semesters.FindAsync(id);
        //    if (semester == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(semester);
        //}

        // GET: Semesters/Create
        /// <summary>
        /// Create new semester
        /// </summary>
        /// <param name="id">Selected course catalog id</param>
        /// <returns></returns>
        public async Task<ActionResult> Create(int id)
        {
            var courseCatalog = await db.CourseCatalogs.FirstOrDefaultAsync(it => it.Id == id);
            if (courseCatalog == null || courseCatalog.RecLog.DeletedDate.HasValue) return View("Error");

            return View(new Semester
            {
                CourseCatalog = courseCatalog,
                CourseCatalogId = courseCatalog.Id
            });
        }

        // POST: Semesters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,RecLog,CourseCatalogId")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                semester.RecLog.CreatedDate = DateTime.Now;
                db.Semesters.Add(semester);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "CourseCatalogs", new { @id = semester.CourseCatalogId });
            }

            ViewBag.CourseCatalogId = new SelectList(db.CourseCatalogs, "Id", "GroupName", semester.CourseCatalogId);
            return View(semester);
        }

        // GET: Semesters/Edit/{semester-id}/{course-catalog-id}
        public async Task<ActionResult> Edit(int id, int courseCatalogId)
        {
            Semester semester = await db.Semesters.FindAsync(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            var courseCatalog = await db.CourseCatalogs.FirstOrDefaultAsync(it => it.Id == courseCatalogId);
            if (courseCatalog == null || courseCatalog.RecLog.DeletedDate.HasValue) return View("Error");

            ViewBag.CourseCatalog = courseCatalog;
            return View(semester);
        }

        // POST: Semesters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,CourseCatalogId")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                var selectedSemester = await db.Semesters.FirstOrDefaultAsync(it => it.Id == semester.Id);
                if (selectedSemester == null) return View("Error");

                selectedSemester.Title = semester.Title;
                selectedSemester.Description = semester.Description;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "CourseCatalogs", new { @id = semester.CourseCatalogId });
            }
            ViewBag.CourseCatalogId = new SelectList(db.CourseCatalogs, "Id", "GroupName", semester.CourseCatalogId);
            return View(semester);
        }

        // GET: Semesters/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = await db.Semesters.FindAsync(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // POST: Semesters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Semester semester = await db.Semesters.FindAsync(id);
            var now = DateTime.Now;
            semester.RecLog.DeletedDate = now;
            foreach (var item in semester.Units) item.RecLog.DeletedDate = now;
            foreach (var item in semester.Units.SelectMany(it => it.Lessons)) item.RecLog.DeletedDate = now;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "CourseCatalogs", new { @id = semester.CourseCatalogId });
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
