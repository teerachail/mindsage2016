using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Engines
{
    /// <summary>
    /// มาตรฐานในการทำงานกับการทำงานที่เป็นคิวด้านหลัง
    /// </summary>
    public interface IBackgroundProcessQueue
    {
        #region Methods

        /// <summary>
        /// จัดคิวการอัพเดทข้อมูลผู้ใช้
        /// </summary>
        /// <param name="profileInfo">ข้อมูลผู้ใช้ที่ทำการอัพเดท</param>
        Task EnqueueUpdateUserProfile(UpdateUserProfileMessage profileInfo);

        #endregion Methods
    }
}
