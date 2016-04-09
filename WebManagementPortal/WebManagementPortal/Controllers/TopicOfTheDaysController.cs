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
    public class TopicOfTheDaysController : Controller
    {
        private MindSageDataModelsContainer db = new MindSageDataModelsContainer();

        //// GET: TopicOfTheDays
        //public async Task<ActionResult> Index()
        //{
        //    var topicOfTheDays = db.TopicOfTheDays.Include(t => t.Lesson);
        //    return View(await topicOfTheDays.ToListAsync());
        //}

        //// GET: TopicOfTheDays/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TopicOfTheDay topicOfTheDay = await db.TopicOfTheDays.FindAsync(id);
        //    if (topicOfTheDay == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(topicOfTheDay);
        //}

        // GET: TopicOfTheDays/Create
        public async Task<ActionResult> Create(int id)
        {
            var lesson = await db.Lessons.FirstOrDefaultAsync(it => it.Id == id);
            if (lesson == null || lesson.RecLog.DeletedDate.HasValue) return View("Error");

            return View(new TopicOfTheDay
            {
                Lesson = lesson,
                LessonId = lesson.Id
            });
        }

        // POST: TopicOfTheDays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Message,SendOnDay,RecLog,LessonId")] TopicOfTheDay topicOfTheDay)
        {
            if (ModelState.IsValid)
            {
                topicOfTheDay.RecLog.CreatedDate = DateTime.Now;
                db.TopicOfTheDays.Add(topicOfTheDay);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Lessons", new { @id = topicOfTheDay.LessonId });
            }

            return View(topicOfTheDay);
        }

        // GET: TopicOfTheDays/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicOfTheDay topicOfTheDay = await db.TopicOfTheDays.FindAsync(id);
            if (topicOfTheDay == null)
            {
                return HttpNotFound();
            }
            return View(topicOfTheDay);
        }

        // POST: TopicOfTheDays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Message,SendOnDay,RecLog,LessonId")] TopicOfTheDay topicOfTheDay)
        {
            if (ModelState.IsValid)
            {
                var totd = await db.TopicOfTheDays.FirstOrDefaultAsync(it => it.Id == topicOfTheDay.Id);
                if (totd == null) return View("Error");

                totd.Message = topicOfTheDay.Message;
                totd.SendOnDay = topicOfTheDay.SendOnDay;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Lessons", new { @id = topicOfTheDay.LessonId });
            }
            return View(topicOfTheDay);
        }

        // GET: TopicOfTheDays/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicOfTheDay topicOfTheDay = await db.TopicOfTheDays.FindAsync(id);
            if (topicOfTheDay == null)
            {
                return HttpNotFound();
            }
            return View(topicOfTheDay);
        }

        // POST: TopicOfTheDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TopicOfTheDay topicOfTheDay = await db.TopicOfTheDays.FindAsync(id);
            topicOfTheDay.RecLog.DeletedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Lessons", new { @id = topicOfTheDay.LessonId });
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
