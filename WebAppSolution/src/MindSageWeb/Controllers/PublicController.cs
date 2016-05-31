using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.WindowsAzure.Storage;

namespace MindSageWeb.Controllers
{
    public class PublicController : Controller
    {
        public IActionResult Content(string id)
        {
            //var credential = $"DefaultEndpointsProtocol=https;AccountName={ model.StorageInfo.AccountName };AccountKey={ model.StorageInfo.StorageKey }";
            var credential = $"DefaultEndpointsProtocol=https;AccountName=mindsageimportcontent;AccountKey=ycIX4+Bl12XM2q7ZbE2SEqPu6ZIP78JMdakIvD7sSJgYRiCNbg6hYARe1oByOe3a3f0Uytt7Qt0SPeSXo9JVkA==";
            var storageAccount = CloudStorageAccount.Parse(credential);
            var tableClient = storageAccount.CreateCloudBlobClient();
            var blobRef = tableClient.GetContainerReference("htmls");
            blobRef.CreateIfNotExists();
            
            var blockBlob = blobRef.GetBlockBlobReference(id);
            var text = blockBlob.DownloadText();
            return Content(text, "text/html");
        }
    }
}