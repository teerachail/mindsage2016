using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebManagementPortal.ViewModels
{
    public class ImportContentViewModel
    {
        [Required]
        public string BaseURL { get; set; }
        [Required]
        public string HomePageURL { get; set; }
        public List<string> PagesURLs { get; set; }
        public List<string> ReferenceFileURLs { get; set; }
        public List<ReplaceSectionInformation> ReplaceSections { get; set; }
        public StorageInformation StorageInfo { get; set; }
    }
    public class ReplaceSectionInformation
    {
        public string Original { get; set; }
        public string ReplacedBy { get; set; }
    }
    public class StorageInformation
    {
        [Required]
        public string StorageBaseURL { get; set; }
        [Required]
        public string StorageKey { get; set; }
    }
}
