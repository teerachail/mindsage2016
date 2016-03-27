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
using repoModel = WebManagementPortal.Repositories.Models;

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
        public async Task<ActionResult> Create([Bind(Include = "Id,GroupName,Grade,SideName,PriceUSD,Series,Title,FullDescription,TotalWeeks,DescriptionImageUrl,RecLog")] CourseCatalog courseCatalog, IEnumerable<string> Advertisements)
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,GroupName,Grade,SideName,PriceUSD,Series,Title,FullDescription,TotalWeeks,DescriptionImageUrl,RecLog")] CourseCatalog courseCatalog, IEnumerable<string> Advertisements)
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
                selectedCourseCatalog.TotalWeeks = courseCatalog.TotalWeeks;
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
            var now = DateTime.Now;
            courseCatalog.RecLog.DeletedDate = now;
            var semesterQry = courseCatalog.Semesters;
            foreach (var item in semesterQry) item.RecLog.DeletedDate = now;
            var unitQry = semesterQry.SelectMany(it => it.Units);
            foreach (var item in unitQry) item.RecLog.DeletedDate = now;
            var lessonQry = unitQry.SelectMany(it => it.Lessons);
            foreach (var item in lessonQry) item.RecLog.DeletedDate = now;
            var adsQry = lessonQry.SelectMany(it => it.Advertisements);
            foreach (var item in adsQry) item.RecLog.DeletedDate = now;
            var totdQry = lessonQry.SelectMany(it => it.TopicOfTheDays);
            foreach (var item in totdQry) item.RecLog.DeletedDate = now;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: CourseCatalogs/UpdateAllCourses
        public ActionResult UpdateAllCourses()
        {
            return View();
        }

        // POST: CourseCatalogs/UpdateAllCourses/5
        [HttpPost, ActionName("UpdateAllCourses")]
        public async Task<ActionResult> UpdateAllCoursesConfirmed()
        {
            IEnumerable<CourseCatalog> allCourseCatalog;
            using (var dctx = new EF.MindSageDataModelsContainer())
            {
                allCourseCatalog = dctx.CourseCatalogs
                    .Include("Semesters.Units.Lessons.Advertisements")
                    .Include("Semesters.Units.Lessons.TopicOfTheDays")
                    .ToList();
            }
            var canUpdateCourses = allCourseCatalog != null && allCourseCatalog.Any();
            if (!canUpdateCourses) return RedirectToAction("Index");

            await updateCourseCatalog(allCourseCatalog);

            return RedirectToAction("Index");
        }

        private async Task updateCourseCatalog(IEnumerable<CourseCatalog> allCourseCatalog)
        {
            var courseCatalogRepo = new WebManagementPortal.Repositories.CourseCatalogRepository();
            foreach (var courseCatalog in allCourseCatalog)
            {
                var semesterQry = courseCatalog.Semesters.Where(it => !it.RecLog.DeletedDate.HasValue);
                var unitQry = semesterQry.SelectMany(it => it.Units).Where(it => !it.RecLog.DeletedDate.HasValue);
                var lessonQry = unitQry.SelectMany(it => it.Lessons).Where(it => !it.RecLog.DeletedDate.HasValue);

                var lessonIdRunner = 1;
                var lessons = lessonQry.Select(it => new repoModel.CourseCatalog.Lesson
                {
                    id = it.Id.ToString(),
                    Order = lessonIdRunner++,
                    Contents = Enumerable.Empty<repoModel.CourseCatalog.LessonContent>() // HACK: Create lesson's contents
                });

                var unitIdRunner = 1;
                var semesterNameRunner = (byte)65;
                var semesters = semesterQry.Select(it => new repoModel.CourseCatalog.Semester
                {
                    id = it.Id.ToString(),
                    Title = it.Title,
                    Description = it.Description,
                    Name = string.Format("{0}", (char)semesterNameRunner++),
                    TotalWeeks = it.TotalWeeks,
                    Units = unitQry.Where(unit => unit.SemesterId == it.Id).Select(unit => new repoModel.CourseCatalog.Unit
                    {
                        id = unit.Id.ToString(),
                        Title = unit.Title,
                        Description = unit.Description,
                        Order = unitIdRunner++,
                        TotalWeeks = unit.TotalWeeks,
                        Lessons = lessons
                    }),
                });

                var result = new repoModel.CourseCatalog
                {
                    id = courseCatalog.Id.ToString(),
                    GroupName = courseCatalog.GroupName,
                    Grade = courseCatalog.Grade,
                    Advertisements = courseCatalog.Advertisements.Split(new string[] { "#;" }, StringSplitOptions.RemoveEmptyEntries),
                    SideName = courseCatalog.SideName,
                    PriceUSD = courseCatalog.PriceUSD,
                    Series = courseCatalog.Series,
                    Title = courseCatalog.Title,
                    FullDescription = courseCatalog.FullDescription,
                    DescriptionImageUrl = courseCatalog.DescriptionImageUrl,
                    CreatedDate = courseCatalog.RecLog.CreatedDate,
                    DeletedDate = courseCatalog.RecLog.DeletedDate,
                    Semesters = semesters,
                    TotalWeeks = courseCatalog.TotalWeeks,
                };

                await courseCatalogRepo.UpsertCourseCatalog(result);
            }
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
