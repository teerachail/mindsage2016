using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebManagementPortal.EF;
using WebManagementPortal.ViewModels;

namespace WebManagementPortal.Controllers
{
    public class ImportContentController : Controller
    {
        // GET: ImportContent
        public ActionResult Index()
        {
            EF.ImportContentConfiguration importContent;
            using (var dctx = new EF.MindSageDataModelsContainer())
            {
                importContent = dctx.ImportContentConfigurations.Where(it => !it.RecLog.DeletedDate.HasValue).ToList().OrderBy(it => it.RecLog.CreatedDate).LastOrDefault();
            }

            var isImportSettingAvailable = importContent != null;
            var result = new ImportContentViewModel
            {
                Id = isImportSettingAvailable ? importContent.Id : -1,
                BaseURL = isImportSettingAvailable ? importContent.BaseURL : string.Empty,
                HomePageURL = isImportSettingAvailable ? importContent.HomePageURL : string.Empty,
                PagesURLs = isImportSettingAvailable ? JsonConvert.DeserializeObject<IEnumerable<string>>(importContent.PagesURLs) : Enumerable.Empty<string>(),
                ReferenceFileURLs = isImportSettingAvailable ? JsonConvert.DeserializeObject<IEnumerable<string>>(importContent.ReferenceFileURLs) : Enumerable.Empty<string>(),
                ReplaceSections = isImportSettingAvailable ? JsonConvert.DeserializeObject<List<ReplaceSectionInformation>>(importContent.ReplaceSections) : new List<ReplaceSectionInformation>(),
                StorageInfo = isImportSettingAvailable ? JsonConvert.DeserializeObject<StorageInformation>(importContent.StorageInfo) : new StorageInformation(),
            };
            return View(result);
        }

        [HttpPost]
        public ActionResult Index(ImportContentViewModel model)
        {
            cleanModel(model);
            var configuration = updateDatabase(model);

            var credential = $"DefaultEndpointsProtocol=https;AccountName={ model.StorageInfo.AccountName };AccountKey={ model.StorageInfo.StorageKey }";
            var storageAccount = CloudStorageAccount.Parse(credential);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Configuration");
            table.CreateIfNotExists();

            //var data = new ImportContentTableEntity
            //{
            //    PartitionKey = model.BaseURL,
            //    RowKey = model.HomePageURL,
            //    BaseURL = model.BaseURL,
            //    HomePageURL = model.HomePageURL,
            //    PagesURLs = JsonConvert.SerializeObject(model.PagesURLs),
            //    ReferenceFileURLs = JsonConvert.SerializeObject(model.ReferenceFileURLs),
            //    ReplaceSections = JsonConvert.SerializeObject(model.ReplaceSections),
            //    StorageInfo = JsonConvert.SerializeObject(model.StorageInfo),
            //    Timestamp = DateTime.Now
            //};
            //var insertOperation = TableOperation.InsertOrReplace(data);
            //table.Execute(insertOperation);
            return RedirectToAction("Index");
        }

        private void cleanModel(ImportContentViewModel model)
        {
            model.PagesURLs = model.PagesURLs.Where(it => !string.IsNullOrEmpty(it) && !string.IsNullOrWhiteSpace(it)).ToList();
            model.ReferenceFileURLs = model.ReferenceFileURLs.Where(it => !string.IsNullOrEmpty(it) && !string.IsNullOrWhiteSpace(it)).ToList();
            model.ReplaceSections = model.ReplaceSections.Where(it => it != null
                && !string.IsNullOrEmpty(it.Original) && !string.IsNullOrWhiteSpace(it.Original)
                && !string.IsNullOrEmpty(it.ReplacedBy) && !string.IsNullOrWhiteSpace(it.ReplacedBy))
                .ToList();
        }
        private ImportContentConfiguration updateDatabase(ImportContentViewModel model)
        {
            using (var dctx = new EF.MindSageDataModelsContainer())
            {
                var selectedObj = dctx.ImportContentConfigurations.FirstOrDefault(it => it.Id == model.Id);
                if (selectedObj == null)
                {
                    selectedObj = new EF.ImportContentConfiguration
                    {
                        BaseURL = model.BaseURL,
                        HomePageURL = model.HomePageURL,
                        PagesURLs = JsonConvert.SerializeObject(model.PagesURLs),
                        ReferenceFileURLs = JsonConvert.SerializeObject(model.ReferenceFileURLs),
                        ReplaceSections = JsonConvert.SerializeObject(model.ReplaceSections),
                        StorageInfo = JsonConvert.SerializeObject(model.StorageInfo),
                        RecLog = new EF.RecordLog { CreatedDate = DateTime.Now }
                    };
                    dctx.ImportContentConfigurations.Add(selectedObj);
                }
                else
                {
                    selectedObj.BaseURL = model.BaseURL;
                    selectedObj.HomePageURL = model.HomePageURL;
                    selectedObj.PagesURLs = JsonConvert.SerializeObject(model.PagesURLs);
                    selectedObj.ReferenceFileURLs = JsonConvert.SerializeObject(model.ReferenceFileURLs);
                    selectedObj.ReplaceSections = JsonConvert.SerializeObject(model.ReplaceSections);
                    selectedObj.StorageInfo = JsonConvert.SerializeObject(model.StorageInfo);
                    selectedObj.RecLog = new EF.RecordLog { CreatedDate = DateTime.Now };
                }
                dctx.SaveChanges();
                return selectedObj;
            }
        }
    }
}