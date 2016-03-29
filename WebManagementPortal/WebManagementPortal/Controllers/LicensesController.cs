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
    public class LicensesController : Controller
    {
        private MindSageDataModelsContainer db = new MindSageDataModelsContainer();

        // GET: Licenses
        public async Task<ActionResult> Index()
        {
            var licenses = db.Licenses.Include(l => l.Contract).Include(l => l.CourseCatalog);
            return View(await licenses.ToListAsync());
        }

        // GET: Licenses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            License license = await db.Licenses.FindAsync(id);
            if (license == null)
            {
                return HttpNotFound();
            }
            return View(license);
        }

        // GET: Licenses/Create
        public async Task<ActionResult> Create(int id)
        {
            var contract = await db.Contracts.FirstOrDefaultAsync(it => it.Id == id);
            if (contract == null || contract.RecLog.DeletedDate.HasValue) return View("Error");

            ViewBag.CourseCatalogId = new SelectList(db.CourseCatalogs, "Id", "SideName");
            return View(new License
            {
                Contract = contract,
                ContractId = contract.Id
            });
        }

        // POST: Licenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CourseName,Grade,StudentsCapacity,RecLog,ContractId,CourseCatalogId")] License license)
        {
            if (ModelState.IsValid)
            {
                var courseCatalog = await db.CourseCatalogs.FirstOrDefaultAsync(it => it.Id == license.CourseCatalogId);
                if (courseCatalog == null || courseCatalog.RecLog.DeletedDate.HasValue) return View("Error");

                var now = DateTime.Now;
                license.CourseName = courseCatalog.SideName;
                license.Grade = courseCatalog.Grade.ToString();
                var newTeacherKey = new TeacherKey
                {
                    Grade = courseCatalog.Grade.ToString(),
                    License = license,
                    Code = generateTeacherCode(license),
                    RecLog = new RecordLog { CreatedDate = now }
                };
                license.TeacherKeys.Add(newTeacherKey);
                license.RecLog.CreatedDate = now;
                db.Licenses.Add(license);
                db.TeacherKeys.Add(newTeacherKey);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Contracts", new { @id = license.ContractId });
            }

            ViewBag.CourseCatalogId = new SelectList(db.CourseCatalogs, "Id", "SideName");
            return View(license);
        }

        // GET: Licenses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            License license = await db.Licenses.FindAsync(id);
            if (license == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractId = new SelectList(db.Contracts, "Id", "Name", license.ContractId);
            ViewBag.CourseCatalogId = new SelectList(db.CourseCatalogs, "Id", "GroupName", license.CourseCatalogId);
            return View(license);
        }

        // POST: Licenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CourseName,Grade,StudentsCapacity,RecLog,ContractId,CourseCatalogId")] License license)
        {
            if (ModelState.IsValid)
            {
                var selectedLicense = await db.Licenses.FirstOrDefaultAsync(it => it.Id == license.Id);
                if (selectedLicense == null || selectedLicense.RecLog.DeletedDate.HasValue) return View("Error");

                selectedLicense.StudentsCapacity = license.StudentsCapacity;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Contracts", new { @id = license.ContractId });
            }
            ViewBag.ContractId = new SelectList(db.Contracts, "Id", "Name", license.ContractId);
            ViewBag.CourseCatalogId = new SelectList(db.CourseCatalogs, "Id", "GroupName", license.CourseCatalogId);
            return View(license);
        }

        // GET: Licenses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            License license = await db.Licenses.FindAsync(id);
            if (license == null)
            {
                return HttpNotFound();
            }
            return View(license);
        }

        // POST: Licenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            License license = await db.Licenses.FindAsync(id);
            license.RecLog.DeletedDate = DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Contracts", new { @id = license.ContractId });
        }

        // GET: Licenses/RegenerateTeacherCode/5
        public async Task<ActionResult> RegenerateTeacherCode(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            License license = await db.Licenses.FindAsync(id);
            if (license == null)
            {
                return HttpNotFound();
            }
            return View(license);
        }

        // POST: Licenses/RegenerateTeacherCode/5
        [HttpPost, ActionName("RegenerateTeacherCode")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegenerateTeacherCodeConfirmed(int id)
        {
            License license = await db.Licenses.FindAsync(id);
            var now = DateTime.Now;
            foreach (var item in license.TeacherKeys) item.RecLog.DeletedDate = now;
            var newTeacherKey = new TeacherKey
            {
                Grade = license.Grade,
                License = license,
                Code = generateTeacherCode(license),
                RecLog = new RecordLog { CreatedDate = now }
            };
            license.TeacherKeys.Add(newTeacherKey);
            db.TeacherKeys.Add(newTeacherKey); 
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Contracts", new { @id = license.ContractId });
        }

        private string generateTeacherCode(License license)
        {
            var teacherKey = string.Empty;
            while (true)
            {

                var courseCatalog = license.CourseCatalog;
                var contract = license.Contract;
                var random = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 4);
                var rgx = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]");
                var safeSchoolName = rgx.Replace(contract.SchoolName, string.Empty);
                const int MaxSchoolNameLength = 10;
                var canUseSchoolMaxLength = safeSchoolName.Length >= MaxSchoolNameLength;
                const int BeginSchoolNameIndex = 0;
                var schoolName = safeSchoolName.Substring(BeginSchoolNameIndex, canUseSchoolMaxLength ? MaxSchoolNameLength : safeSchoolName.Length);
                teacherKey = string.Format("{0:00}{1}{2}{3}{4}", courseCatalog.Grade, contract.State, contract.ZipCode, random, schoolName);

                var canUseTheNewKey = false;
                using (var dctx = new EF.MindSageDataModelsContainer())
                {
                    canUseTheNewKey = !dctx.TeacherKeys
                        .Where(it => !it.RecLog.DeletedDate.HasValue && it.Code == teacherKey)
                        .Any();
                }

                if (canUseTheNewKey) break;
            }

            return teacherKey;
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
