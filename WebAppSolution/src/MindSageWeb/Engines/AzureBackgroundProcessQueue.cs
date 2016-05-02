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

        private const string QueueName = "updateUserProfile";

        #endregion Fields

        #region IBackgroundProcessQueue members

        /// <summary>
        /// จัดคิวการอัพเดทข้อมูลผู้ใช้
        /// </summary>
        /// <param name="profileInfo">ข้อมูลผู้ใช้ที่ทำการอัพเดท</param>
        public async Task EnqueueUpdateUserProfile(UpdateUserProfileMessage profileInfo)
        {
            // TODO: Not implement EnqueueUpdateUserProfile
            throw new NotImplementedException();
        }

        #endregion IBackgroundProcessQueue members
    }
}
