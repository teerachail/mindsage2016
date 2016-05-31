using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Engines
{
    public class AzureBlobStorageContentReader : IStorageContentReader
    {
        private string _credential;

        public AzureBlobStorageContentReader(string credential)
        {
            _credential = credential;
        }

        public string GetContent(string fileName)
        {
            var storageAccount = CloudStorageAccount.Parse(_credential);
            var tableClient = storageAccount.CreateCloudBlobClient();
            var blobRef = tableClient.GetContainerReference("htmls");
            blobRef.CreateIfNotExists();

            var blockBlob = blobRef.GetBlockBlobReference(fileName);
            var text = blockBlob.Exists() ? blockBlob.DownloadText() : string.Empty;
            return text;
        }
    }
}
