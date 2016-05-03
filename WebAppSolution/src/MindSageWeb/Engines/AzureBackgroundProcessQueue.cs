using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Engines
{
    /// <summary>
    /// ตัวทำงานกับการทำงานที่เป็นคิวด้านหลัง
    /// </summary>
    public class AzureBackgroundProcessQueue : IBackgroundProcessQueue
    {
        #region Fields

        private const string QueueName = "update-user-profile";
        private readonly string _storageConnectionString;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="storageConnectionString">ที่อยู่ในการเชื่อมต่อกับที่เก็บข้อมูล</param>
        public AzureBackgroundProcessQueue(string storageConnectionString)
        {
            _storageConnectionString = storageConnectionString;
        }

        #endregion Constructors

        #region IBackgroundProcessQueue members

        /// <summary>
        /// จัดคิวการอัพเดทข้อมูลผู้ใช้
        /// </summary>
        /// <param name="profileInfo">ข้อมูลผู้ใช้ที่ทำการอัพเดท</param>
        public async Task EnqueueUpdateUserProfile(UpdateUserProfileMessage profileInfo)
        {
            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference(QueueName);
            await queue.CreateIfNotExistsAsync();

            var profileInfoString = JsonConvert.SerializeObject(profileInfo);
            var message = new CloudQueueMessage(profileInfoString);
            await queue.AddMessageAsync(message);
        }

        #endregion IBackgroundProcessQueue members
    }
}
