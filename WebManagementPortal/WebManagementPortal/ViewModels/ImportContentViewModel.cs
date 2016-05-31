using Microsoft.WindowsAzure.Storage.Table;
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
        public int Id { get; set; }
        [Required]
        public string BaseURL { get; set; }
        [Required]
        public string HomePageURL { get; set; }
        public IEnumerable<string> PagesURLs { get; set; }
        public IEnumerable<string> ReferenceFileURLs { get; set; }
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
        public string AccountName { get; set; }
        [Required]
        public string StorageKey { get; set; }
    }

    public class ImportContentTableEntity : TableEntity
    {
        public string BaseURL { get; set; }
        public string HomePageURL { get; set; }
        public string PagesURLs { get; set; }
        public string ReferenceFileURLs { get; set; }
        public string ReplaceSections { get; set; }
        public string StorageInfo { get; set; }
    }
}
