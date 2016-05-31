using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.WindowsAzure.Storage;
using MindSageWeb.Engines;

namespace MindSageWeb.Controllers
{
    public class PublicController : Controller
    {
        private IStorageContentReader _storageContent;

        public PublicController(IStorageContentReader storageContent)
        {
            _storageContent = storageContent;
        }

        public IActionResult Content()
        {
            const string ErrorMessage = "Page not found";
            var path = HttpContext.Request.Path;
            if (!path.HasValue)
            {
                ViewBag.ErrorMessage = ErrorMessage;
                return View("Error");
            }

            var requestFileName = path.Value.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            if(string.IsNullOrEmpty(requestFileName))
            {
                ViewBag.ErrorMessage = ErrorMessage;
                return View("Error");
            }

            var text = _storageContent.GetContent(requestFileName);
            if (string.IsNullOrEmpty(text))
            {
                ViewBag.ErrorMessage = ErrorMessage;
                return View("Error");
            }

            return Content(text, "text/html");
        }
    }
}