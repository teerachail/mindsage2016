using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ notification
    /// </summary>
    public interface INotificationRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล notification จากรหัสผู้ใช้และ class room
        /// </summary>
        /// <param name="userprofileId">ชื่อผู้ใช้ที่ต้องการค้นหาข้อมูล</param>
        /// <param name="classRoomId">Class room id</param>
        IEnumerable<Notification> GetNotificationByUserIdAndClassRoomId(string userprofileId, string classRoomId);

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล notification
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        void Upsert(Notification data);

        /// <summary>
        /// เพิ่มข้อมูล notification
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        void Insert(IEnumerable<Notification> data);

        #endregion Methods
    }
}
