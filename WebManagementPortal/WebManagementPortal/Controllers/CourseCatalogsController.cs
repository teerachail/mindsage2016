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
                    .Include("Semesters.Units.Lessons.ExtraContents")
                    .ToList();
            }
            var canUpdateCourses = allCourseCatalog != null && allCourseCatalog.Any();
            if (!canUpdateCourses) return RedirectToAction("Index");

            // TODO: Handle update to MongoDB error
            await updateLessonCatalog(allCourseCatalog);
            await updateCourseCatalog(allCourseCatalog);
            await createPublicClassRooms(allCourseCatalog);
            return RedirectToAction("Index");
        }

        private async Task updateLessonCatalog(IEnumerable<CourseCatalog> allCourseCatalog)
        {
            var lessonCatalogRepo = new WebManagementPortal.Repositories.LessonCatalogRepository();
            foreach (var courseCatalog in allCourseCatalog)
            {
                var unitOrderRunner = 1;
                var lessonOrderRunner = 1;
                var semesterNameRunner = (byte)65;
                foreach (var semester in courseCatalog.Semesters.Where(it => !it.RecLog.DeletedDate.HasValue))
                {
                    foreach (var unit in semester.Units.Where(it => !it.RecLog.DeletedDate.HasValue))
                    {
                        foreach (var lesson in unit.Lessons.Where(it => !it.RecLog.DeletedDate.HasValue))
                        {
                            var adsQry = lesson.Advertisements.Where(it => !it.RecLog.DeletedDate.HasValue).Select(it => new repoModel.LessonCatalog.Ads
                            {
                                id = it.Id.ToString(),
                                ImageUrl = it.ImageUrl,
                                LinkUrl = it.LinkUrl,
                                CreatedDate = it.RecLog.CreatedDate,
                                DeletedDate = it.RecLog.DeletedDate
                            });
                            var totdQry = lesson.TopicOfTheDays.Where(it => !it.RecLog.DeletedDate.HasValue).Select(it => new repoModel.LessonCatalog.TopicOfTheDay
                            {
                                id = it.Id.ToString(),
                                Message = it.Message,
                                SendOnDay = it.SendOnDay,
                                CreatedDate = it.RecLog.CreatedDate,
                                DeletedDate = it.RecLog.DeletedDate
                            });
                            var extraContents = lesson.ExtraContents
                                .Where(it => !it.RecLog.DeletedDate.HasValue)
                                .Select(it => new repoModel.LessonCatalog.ExtraContent
                                {
                                    id = it.Id.ToString(),
                                    ContentURL = it.ContentURL,
                                    Description = it.Description,
                                    IconURL = ControllerHelper.ConvertToIconUrl(it.IconURL)
                                });
                            var lessonCatalog = new repoModel.LessonCatalog
                            {
                                id = lesson.Id.ToString(),
                                Order = lessonOrderRunner++,
                                Title = lesson.Title,
                                UnitNo = unitOrderRunner,
                                SemesterName = string.Format("{0}", (char)semesterNameRunner),
                                ShortDescription = lesson.ShortDescription,
                                MoreDescription = lesson.MoreDescription,
                                ShortTeacherLessonPlan = lesson.ShortTeacherLessonPlan,
                                MoreTeacherLessonPlan = lesson.MoreTeacherLessonPlan,
                                PrimaryContentURL = lesson.PrimaryContentURL,
                                PrimaryContentDescription = lesson.PrimaryContentDescription,
                                ExtraContents = extraContents,
                                CourseCatalogId = courseCatalog.Id.ToString(),
                                CreatedDate = lesson.RecLog.CreatedDate,
                                DeletedDate = lesson.RecLog.DeletedDate,
                                Advertisments = adsQry,
                                TopicOfTheDays = totdQry
                            };
                            await lessonCatalogRepo.UpsertLessonCatalog(lessonCatalog);
                        }
                        unitOrderRunner++;
                    }
                    semesterNameRunner++;
                }
            }
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
                var unitIdRunner = 1;
                var semesterNameRunner = (byte)65;
                var semesters = semesterQry.Select(semester => new repoModel.CourseCatalog.Semester
                {
                    id = semester.Id.ToString(),
                    Title = semester.Title,
                    Description = semester.Description,
                    Name = string.Format("{0}", (char)semesterNameRunner++),
                    TotalWeeks = semester.TotalWeeks,
                    Units = unitQry.Where(unit => unit.SemesterId == semester.Id).Select(unit => new repoModel.CourseCatalog.Unit
                    {
                        id = unit.Id.ToString(),
                        Title = unit.Title,
                        Description = unit.Description,
                        Order = unitIdRunner++,
                        TotalWeeks = unit.TotalWeeks,
                        Lessons = lessonQry.Where(it => it.UnitId == unit.Id).Select(it =>
                        {
                            var extraContents = it.ExtraContents
                                    .Where(eit => !eit.RecLog.DeletedDate.HasValue)
                                    .Where(eit => eit.LessonId == it.Id)
                                    .Select(eit => new repoModel.CourseCatalog.LessonContent
                                    {
                                        ImageUrl = ControllerHelper.ConvertToIconUrl(eit.IconURL),
                                        ContentURL = eit.ContentURL,
                                        Description = eit.Description,
                                    });
                            var contents = new List<repoModel.CourseCatalog.LessonContent>
                            {
                                new repoModel.CourseCatalog.LessonContent {
                                    ContentURL = it.PrimaryContentURL,
                                    Description = it.PrimaryContentDescription,
                                    ImageUrl = ExtraContentType.Video.ConvertToIconUrl(),
                                    IsPreviewable = it.IsPreviewable
                                }
                            };
                            return new repoModel.CourseCatalog.Lesson
                            {
                                id = it.Id.ToString(),
                                Order = lessonIdRunner++,
                                Contents = contents.Union(extraContents)
                            };
                        }),
                    }),
                });

                var result = new repoModel.CourseCatalog
                {
                    id = courseCatalog.Id.ToString(),
                    GroupName = courseCatalog.GroupName,
                    Grade = courseCatalog.Grade.ToString(),
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
        private async Task createPublicClassRooms(IEnumerable<CourseCatalog> allCourseCatalog)
        {
            var courseCatalogIds = allCourseCatalog.Select(it => it.Id.ToString()).Distinct();
            var classRoomRepo = new WebManagementPortal.Repositories.ClassRoomRepository();
            var publicClassRooms = (await classRoomRepo.GetPublicClassRoomByCourseCatalogId(courseCatalogIds)).ToList();
            foreach (var publicClassRoom in publicClassRooms)
            {
                var coursCatalog = allCourseCatalog.FirstOrDefault(it => it.Id.ToString() == publicClassRoom.id);
                if (coursCatalog == null) continue;

                publicClassRoom.Name = coursCatalog.SideName;
                publicClassRoom.DeletedDate = coursCatalog.RecLog.DeletedDate;
                var lessonQry = coursCatalog.Semesters
                    .Where(it => !it.RecLog.DeletedDate.HasValue)
                    .SelectMany(it => it.Units)
                    .Where(it => !it.RecLog.DeletedDate.HasValue)
                    .SelectMany(it => it.Lessons)
                    .Where(it => !it.RecLog.DeletedDate.HasValue)
                    .Select(it =>
                    {
                        var selectedLesson = publicClassRoom.Lessons.FirstOrDefault(l => l.id == it.Id.ToString());
                        var totalLikes = selectedLesson?.TotalLikes ?? 0;

                        return new repoModel.ClassRoom.Lesson
                        {
                            id = it.Id.ToString(),
                            LessonCatalogId = it.Id.ToString(),
                            TotalLikes = totalLikes
                        };
                    });
                publicClassRoom.Lessons = lessonQry.ToList();
                await classRoomRepo.UpsertClassRoom(publicClassRoom);
            }

            var needToCreateClassRooms = allCourseCatalog.Where(it => publicClassRooms.All(p => p.id != it.Id.ToString()));
            foreach (var courseCatalog in needToCreateClassRooms)
            {
                var lessonQry = courseCatalog.Semesters
                    .Where(it => !it.RecLog.DeletedDate.HasValue)
                    .SelectMany(it => it.Units)
                    .Where(it => !it.RecLog.DeletedDate.HasValue)
                    .SelectMany(it => it.Lessons)
                    .Where(it => !it.RecLog.DeletedDate.HasValue)
                    .Select(it => new repoModel.ClassRoom.Lesson
                    {
                        id = it.Id.ToString(),
                        LessonCatalogId = it.Id.ToString(),
                    });
                var classRoom = new repoModel.ClassRoom
                {
                    id = courseCatalog.Id.ToString(),
                    Name = courseCatalog.SideName,
                    CourseCatalogId = courseCatalog.Id.ToString(),
                    CreatedDate = courseCatalog.RecLog.CreatedDate,
                    DeletedDate = courseCatalog.RecLog.DeletedDate,
                    IsPublic = true,
                    Lessons = lessonQry
                };
                await classRoomRepo.UpsertClassRoom(classRoom);
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
