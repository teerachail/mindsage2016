using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebManagementPortal.ViewModels;

namespace WebManagementPortal.Controllers
{
    public class ImportContentController : Controller
    {
        // GET: ImportContent
        public ActionResult Index()
        {
            var configInfo = new ImportContentViewModel
            {
                BaseURL = "base url",
                HomePageURL = "home.html",
                StorageInfo = new StorageInformation
                {
                    StorageBaseURL = "storage url",
                    StorageKey = "storage key"
                },
                PagesURLs = new List<string>
                {
                    @"/what-is-mind-sage.html",
                    @"/why-mind-sage.html",
                    @"/how-to-mind-sage.html",
                },
                ReferenceFileURLs = new List<string>
                {
                    @"www.google.com/javascript.js",
                    @"www.google.com/twitter/script.js",
                },
                ReplaceSections = new List<ReplaceSectionInformation>
                {
                    new ReplaceSectionInformation { Original = "A", ReplacedBy = "A1" },
                    new ReplaceSectionInformation { Original = "B", ReplacedBy = "B1" },
                    new ReplaceSectionInformation { Original = "C", ReplacedBy = "C1" },
                    new ReplaceSectionInformation { Original = "D", ReplacedBy = "D1" },
                }
            };
            return View(configInfo);
        }

        [HttpPost]
        public ActionResult Index(ImportContentViewModel model)
        {
            model.PagesURLs = model.PagesURLs.Where(it => !string.IsNullOrEmpty(it) && !string.IsNullOrWhiteSpace(it)).ToList();
            model.ReferenceFileURLs = model.ReferenceFileURLs.Where(it => !string.IsNullOrEmpty(it) && !string.IsNullOrWhiteSpace(it)).ToList();
            model.ReplaceSections = model.ReplaceSections.Where(it => it != null 
                && !string.IsNullOrEmpty(it.Original) && !string.IsNullOrWhiteSpace(it.Original)
                && !string.IsNullOrEmpty(it.ReplacedBy) && !string.IsNullOrWhiteSpace(it.ReplacedBy))
                .ToList();

            return View(model);
        }
    }
}