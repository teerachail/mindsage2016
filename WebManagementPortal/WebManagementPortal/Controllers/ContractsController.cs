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
using WebManagementPortal.Repositories;
using repoModel = WebManagementPortal.Repositories.Models;

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

        // GET: Contracts/UpdateAllContracts
        public async Task<ActionResult> UpdateAllContracts()
        {
            return View();
        }

        // POST: Contracts/UpdateAllContracts
        [HttpPost, ActionName("UpdateAllContracts")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateAllContractsConfirmed()
        {
            var allContracts = Enumerable.Empty<Contract>();
            using (var dctx = new MindSageDataModelsContainer())
            {
                allContracts = dctx.Contracts
                    .Include("Licenses.CourseCatalog")
                    .Include("Licenses.TeacherKeys")
                    .ToList();
            }
            var canUpdateContracts = allContracts != null && allContracts.Any();
            if (!canUpdateContracts) return RedirectToAction("Index");

            // TODO: Handle update to MongoDB error
            var contractIds = allContracts.Select(it => it.Id.ToString()).Distinct();
            var contractRepo = new ContractRepository();
            var mongoDBcontracts = (await contractRepo.GetContractsById(contractIds)).ToList();
            await updateContracts(contractRepo, allContracts, mongoDBcontracts);
            await createNewContracts(contractRepo, allContracts, mongoDBcontracts);

            return RedirectToAction("Index");
        }

        private async Task updateContracts(IContractRepository repo, IEnumerable<Contract> allContracts, IEnumerable<repoModel.Contract> mongoDBcontracts)
        {
            foreach (var mongoDBContract in mongoDBcontracts)
            {
                var contract = allContracts.FirstOrDefault(it => it.Id.ToString() == mongoDBContract.id);
                if (contract == null) continue;

                var licenseQry = contract.Licenses
                    .Select(it => new repoModel.Contract.License
                    {
                        id = it.Id.ToString(),
                        CourseCatalogId = it.CourseCatalogId.ToString(),
                        CourseName = it.CourseName,
                        CreatedDate = it.RecLog.CreatedDate,
                        DeletedDate = it.RecLog.DeletedDate,
                        Grade = it.Grade,
                        StudentsCapacity = it.StudentsCapacity,
                        TeacherKeys = it.TeacherKeys.Select(tk => new repoModel.Contract.TeacherKey
                        {
                            id = tk.Id.ToString(),
                            Code = tk.Code,
                            CreatedDate = tk.RecLog.CreatedDate,
                            DeletedDate = tk.RecLog.DeletedDate,
                            Grade = tk.Grade
                        })
                    });

                mongoDBContract.Name = contract.Name;
                mongoDBContract.StartDate = contract.StartDate;
                mongoDBContract.ExpiredDate = contract.ExpiredDate;
                mongoDBContract.CreatedDate = contract.RecLog.CreatedDate;
                mongoDBContract.DeletedDate = contract.RecLog.DeletedDate;
                mongoDBContract.TimeZone = contract.TimeZone;
                mongoDBContract.Licenses = licenseQry.ToList();

                await repo.UpsertContract(mongoDBContract);
            }
        }
        private async Task createNewContracts(IContractRepository repo, IEnumerable<Contract> allContracts, IEnumerable<repoModel.Contract> mongoDBcontracts)
        {
            var needToCreateContracts = allContracts.Where(it => mongoDBcontracts.All(c => c.id != it.Id.ToString()));
            foreach (var efContract in needToCreateContracts)
            {
                var licenseQry = efContract.Licenses
                    .Select(it => new repoModel.Contract.License
                    {
                        id = it.Id.ToString(),
                        CourseCatalogId = it.CourseCatalogId.ToString(),
                        CourseName = it.CourseName,
                        CreatedDate = it.RecLog.CreatedDate,
                        DeletedDate = it.RecLog.DeletedDate,
                        Grade = it.Grade,
                        StudentsCapacity = it.StudentsCapacity,
                        TeacherKeys = it.TeacherKeys.Select(tk => new repoModel.Contract.TeacherKey
                        {
                            id = tk.Id.ToString(),
                            Code = tk.Code,
                            CreatedDate = tk.RecLog.CreatedDate,
                            DeletedDate = tk.RecLog.DeletedDate,
                            Grade = tk.Grade
                        })
                    });

                var contract = new repoModel.Contract
                {
                    id = efContract.Id.ToString(),
                    CreatedDate = efContract.RecLog.CreatedDate,
                    DeletedDate = efContract.RecLog.DeletedDate,
                    ExpiredDate = efContract.ExpiredDate,
                    Name = efContract.Name,
                    StartDate = efContract.StartDate,
                    TimeZone = efContract.TimeZone,
                    Licenses = licenseQry.ToList()
                };

                await repo.UpsertContract(contract);
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
