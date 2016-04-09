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
    public class AdvertisementsController : Controller
    {
        private MindSageDataModelsContainer db = new MindSageDataModelsContainer();

        //// GET: Advertisements
        //public async Task<ActionResult> Index()
        //{
        //    var advertisements = db.Advertisements.Include(a => a.Lesson);
        //    return View(await advertisements.ToListAsync());
        //}

        // GET: Advertisements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertisement advertisement = await db.Advertisements.FindAsync(id);
            if (advertisement == null)
            {
                return HttpNotFound();
            }
            return View(advertisement);
        }

        // GET: Advertisements/Create
        public async Task<ActionResult> Create(int id)
        {
            var lesson = await db.Lessons.FirstOrDefaultAsync(it => it.Id == id);
            if (lesson == null || lesson.RecLog.DeletedDate.HasValue) return View("Error");

            return View(new Advertisement
            {
                Lesson = lesson,
                LessonId = lesson.Id
            });
        }

        // POST: Advertisements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ImageUrl,LinkUrl,RecLog,LessonId")] Advertisement advertisement)
        {
            if (ModelState.IsValid)
            {
                advertisement.RecLog.CreatedDate = DateTime.Now;
                advertisement.LinkUrl = "Unknow";
                db.Advertisements.Add(advertisement);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Lessons", new { @id = advertisement.LessonId });
            }

            return View(advertisement);
        }

        // GET: Advertisements/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertisement advertisement = await db.Advertisements.FindAsync(id);
            if (advertisement == null)
            {
                return HttpNotFound();
            }

            return View(advertisement);
        }

        // POST: Advertisements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ImageUrl,LinkUrl,RecLog,LessonId")] Advertisement advertisement)
        {
            if (ModelState.IsValid)
            {
                var ads = await db.Advertisements.FirstOrDefaultAsync(it => it.Id == advertisement.Id);
                if (ads == null) return View("Error");

                ads.ImageUrl = advertisement.ImageUrl;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Lessons", new { @id = ads.LessonId });
            }

            return View(advertisement);
        }

        // GET: Advertisements/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertisement advertisement = await db.Advertisements.FindAsync(id);
            if (advertisement == null)
            {
                return HttpNotFound();
            }
            return View(advertisement);
        }

        // POST: Advertisements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Advertisement advertisement = await db.Advertisements.FindAsync(id);
            advertisement.RecLog.DeletedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Lessons", new { @id = advertisement.LessonId });
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
