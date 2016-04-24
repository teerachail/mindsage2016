using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Engines
{
    /// <summary>
    /// มาตรฐานในการอัพโหลดรูป
    /// </summary>
    public interface IImageUploader
    {
        #region Methods

        /// <summary>
        /// อัพโหลดรูปผู้ใช้
        /// </summary>
        /// <param name="userprofileName">ชื่อผู้ใช้ที่ทำการอัพโหลด</param>
        /// <param name="filestream">ไฟล์</param>
        /// <param name="contentType">ชนิดของไฟล์</param>
        Task<string> UploadUserProfile(string userprofileName, Stream filestream, string contentType);

        #endregion Methods
    }
}
