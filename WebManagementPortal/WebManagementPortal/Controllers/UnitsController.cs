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
    public class UnitsController : Controller
    {
        private MindSageDataModelsContainer db = new MindSageDataModelsContainer();

        //// GET: Units
        //public async Task<ActionResult> Index()
        //{
        //    var units = db.Units.Include(u => u.Semester);
        //    return View(await units.ToListAsync());
        //}

        //// GET: Units/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Unit unit = await db.Units.FindAsync(id);
        //    if (unit == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(unit);
        //}

        // GET: Units/Create
        public async Task<ActionResult> Create(int id)
        {
            var semester = await db.Semesters.Include("CourseCatalog").FirstOrDefaultAsync(it => it.Id == id);
            if (semester == null || semester.RecLog.DeletedDate.HasValue) return View("Error");

            return View(new Unit
            {
                Semester = semester,
                SemesterId = semester.Id
            });
        }

        // POST: Units/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,TotalWeeks,RecLog,SemesterId")] Unit unit)
        {
            if (ModelState.IsValid)
            {
                var semester = await db.Semesters.Include("CourseCatalog").FirstOrDefaultAsync(it => it.Id == unit.SemesterId);
                if (semester == null) return View("Error");

                unit.RecLog.CreatedDate = DateTime.Now;
                db.Units.Add(unit);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "CourseCatalogs", new { @id = semester.CourseCatalogId });
            }

            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Title", unit.SemesterId);
            return View(unit);
        }

        // GET: Units/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = await db.Units.FindAsync(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Title", unit.SemesterId);
            return View(unit);
        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,TotalWeeks,RecLog,SemesterId")] Unit unit)
        {
            if (ModelState.IsValid)
            {
                var selectedUnit = await db.Units.FirstOrDefaultAsync(it => it.Id == unit.Id);
                if (selectedUnit == null) return View("Error");

                selectedUnit.Title = unit.Title;
                selectedUnit.Description = unit.Description;
                selectedUnit.TotalWeeks = unit.TotalWeeks;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "CourseCatalogs", new { @id = selectedUnit.Semester.CourseCatalogId });
            }
            ViewBag.SemesterId = new SelectList(db.Semesters, "Id", "Title", unit.SemesterId);
            return View(unit);
        }

        // GET: Units/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = await db.Units.FindAsync(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Unit unit = await db.Units.FindAsync(id);
            var now = DateTime.Now;
            unit.RecLog.DeletedDate = now;
            foreach (var item in unit.Lessons) item.RecLog.DeletedDate = now;
            var adsQry = unit.Lessons.SelectMany(it => it.Advertisements);
            foreach (var item in adsQry) item.RecLog.DeletedDate = now;
            var totdQry = unit.Lessons.SelectMany(it => it.TopicOfTheDays);
            foreach (var item in totdQry) item.RecLog.DeletedDate = now;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "CourseCatalogs", new { @id = unit.Semester.CourseCatalogId });
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
