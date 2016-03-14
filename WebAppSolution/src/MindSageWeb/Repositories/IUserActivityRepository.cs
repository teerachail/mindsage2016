using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ User activity
    /// </summary>
    public interface IUserActivityRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูลกิจกรรมต่างๆที่ผู้ใช้ทำกับ class room
        /// </summary>
        /// <param name="userprofile">รหัสบัญชีผู้ใช้ที่ต้องการข้อมูล</param>
        /// <param name="classRoomId">รหัส class room</param>
        UserActivity GetUserActivityByUserProfileIdAndClassRoomId(string userprofile, string classRoomId);

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูลกิจกรรม
        /// </summary>
        /// <param name="data">ข้อมูลกิจกรรมที่ต้องการดำเนินการ</param>
        void UpsertUserActivity(UserActivity data);

        #endregion Methods
    }
}
