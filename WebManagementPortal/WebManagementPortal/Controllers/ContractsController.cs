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
    public class ContractsController : Controller
    {
        private MindSageDataModelsContainer db = new MindSageDataModelsContainer();

        // GET: Contracts
        public async Task<ActionResult> Index()
        {
            return View(await db.Contracts.Where(it => !it.RecLog.DeletedDate.HasValue).ToListAsync());
        }

        // GET: Contracts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // GET: Contracts/Create
        public ActionResult Create()
        {
            ViewBag.TimeZone = TimeZoneInfo.GetSystemTimeZones().Select(it => new SelectListItem
            {
                Text = it.DisplayName,
                Value = it.DisplayName
            });
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,StartDate,ExpiredDate,TimeZone,RecLog")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                contract.RecLog.CreatedDate = DateTime.Now;
                db.Contracts.Add(contract);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TimeZone = TimeZoneInfo.GetSystemTimeZones().Select(it => new SelectListItem
            {
                Text = it.DisplayName,
                Value = it.DisplayName
            });
            return View(contract);
        }

        // GET: Contracts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return HttpNotFound();
            }

            ViewBag.TimeZone = TimeZoneInfo.GetSystemTimeZones().Select(it => new SelectListItem
            {
                Text = it.DisplayName,
                Value = it.DisplayName
            });
            return View(contract);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,StartDate,ExpiredDate,TimeZone,RecLog")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                var selectedContract = await db.Contracts.FirstOrDefaultAsync(it => it.Id == contract.Id);
                if (selectedContract == null) return View("Error");

                selectedContract.Name = contract.Name;
                selectedContract.StartDate = contract.StartDate;
                selectedContract.ExpiredDate = contract.ExpiredDate;
                selectedContract.TimeZone = contract.TimeZone;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TimeZone = TimeZoneInfo.GetSystemTimeZones().Select(it => new SelectListItem
            {
                Text = it.DisplayName,
                Value = it.DisplayName
            });
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Contract contract = await db.Contracts.FindAsync(id);
            contract.RecLog.DeletedDate = DateTime.Now;
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
