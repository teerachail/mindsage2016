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
    public class CourseCatalogsController : Controller
    {
        private MindSageDataModelsContainer db = new MindSageDataModelsContainer();

        // GET: CourseCatalogs
        public async Task<ActionResult> Index()
        {
            var courseCatalogs = await db.CourseCatalogs
                .Include("Semesters.Units.Lessons")
                .Where(it => !it.RecLog.DeletedDate.HasValue)
                .OrderBy(it => it.GroupName)
                .ThenBy(it => it.Grade)
                .ThenBy(it => it.Series)
                .ToListAsync();
            return View(courseCatalogs);
        }

        // GET: CourseCatalogs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCatalog courseCatalog = await db.CourseCatalogs.FindAsync(id);
            if (courseCatalog == null)
            {
                return HttpNotFound();
            }
            var semesters = db.Semesters.Include(s => s.CourseCatalog)
                .Where(it => !it.RecLog.DeletedDate.HasValue)
                .OrderBy(it => it.RecLog.CreatedDate);
            ViewBag.Semesters = await semesters.ToListAsync();
            return View(courseCatalog);
        }

        // GET: CourseCatalogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseCatalogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,GroupName,Grade,SideName,PriceUSD,Series,Title,FullDescription,DescriptionImageUrl,RecLog")] CourseCatalog courseCatalog, IEnumerable<string> Advertisements)
        {
            if (ModelState.IsValid)
            {
                var advetisements = Advertisements ?? Enumerable.Empty<string>();
                courseCatalog.Advertisements = string.Join("#;", advetisements);
                courseCatalog.RecLog.CreatedDate = DateTime.Now;
                db.CourseCatalogs.Add(courseCatalog);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(courseCatalog);
        }

        // GET: CourseCatalogs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCatalog courseCatalog = await db.CourseCatalogs.FindAsync(id);
            if (courseCatalog == null)
            {
                return HttpNotFound();
            }
            return View(courseCatalog);
        }

        // POST: CourseCatalogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,GroupName,Grade,SideName,PriceUSD,Series,Title,FullDescription,DescriptionImageUrl,RecLog")] CourseCatalog courseCatalog, IEnumerable<string> Advertisements)
        {
            if (ModelState.IsValid)
            {
                var selectedCourseCatalog = await db.CourseCatalogs.FirstOrDefaultAsync(it => it.Id == courseCatalog.Id);
                if (selectedCourseCatalog == null) return View("Error");

                var advetisements = Advertisements ?? Enumerable.Empty<string>();
                selectedCourseCatalog.Advertisements = string.Join("#;", advetisements);
                selectedCourseCatalog.GroupName = courseCatalog.GroupName;
                selectedCourseCatalog.Grade = courseCatalog.Grade;
                selectedCourseCatalog.SideName = courseCatalog.SideName;
                selectedCourseCatalog.PriceUSD = courseCatalog.PriceUSD;
                selectedCourseCatalog.Series = courseCatalog.Series;
                selectedCourseCatalog.Title = courseCatalog.Title;
                selectedCourseCatalog.FullDescription = courseCatalog.FullDescription;
                selectedCourseCatalog.DescriptionImageUrl = courseCatalog.DescriptionImageUrl;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { @id = selectedCourseCatalog.Id });
            }
            return View(courseCatalog);
        }

        // GET: CourseCatalogs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCatalog courseCatalog = await db.CourseCatalogs.FindAsync(id);
            if (courseCatalog == null)
            {
                return HttpNotFound();
            }
            return View(courseCatalog);
        }

        // POST: CourseCatalogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CourseCatalog courseCatalog = await db.CourseCatalogs.FindAsync(id);
            courseCatalog.RecLog.DeletedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
