using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Engines
{
    /// <summary>
    /// ตัวอัพโหลดรูป
    /// </summary>
    public class AzureBlobStorageImageUploader : IImageUploader
    {
        #region Fields

        private const string ContainerName = "userprofileimage";
        private readonly string _storageConnectionString;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="storageConnectionString">ที่อยู่ในการเชื่อมต่อกับที่เก็บข้อมูล</param>
        public AzureBlobStorageImageUploader(string storageConnectionString)
        {
            _storageConnectionString = storageConnectionString;
        }

        #endregion Constructors

        #region IImageUploader members

        /// <summary>
        /// อัพโหลดรูปผู้ใช้
        /// </summary>
        /// <param name="userprofileName">ชื่อผู้ใช้ที่ทำการอัพโหลด</param>
        /// <param name="filestream">ไฟล์</param>
        /// <param name="contentType">ชนิดของไฟล์</param>
        public async Task<string> UploadUserProfile(string userprofileName, Stream filestream, string contentType)
        {
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(_storageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(ContainerName);
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference(userprofileName);
            blob.Properties.ContentType = contentType;
            await blob.UploadFromStreamAsync(filestream);
            filestream.Close();
            return blob.Uri.AbsoluteUri;
        }

        #endregion IImageUploader members
    }
}
