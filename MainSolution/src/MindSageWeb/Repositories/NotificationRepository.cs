using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ notification
    /// </summary>
    public class NotificationRepository : INotificationRepository
    {
        #region INotificationRepository members
        /// <summary>
        /// ขอข้อมูล notification จากรหัสผู้ใช้และ class room
        /// </summary>
        /// <param name="userprofileId">ชื่อผู้ใช้ที่ต้องการค้นหาข้อมูล</param>
        /// <param name="classRoomId">Class room id</param>
        public IEnumerable<Notification> GetNotificationByUserIdAndClassRoomId(string userprofileId, string classRoomId)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล notification
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        public void Upsert(Notification data)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        #endregion INotificationRepository members
    }
}
