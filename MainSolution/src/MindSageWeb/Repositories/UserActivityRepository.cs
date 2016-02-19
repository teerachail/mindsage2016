using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;

namespace MindSageWeb.Repositories
{
    public class UserActivityRepository : IUserActivityRepository
    {
        #region Fields

        // HACK: Table name
        private const string ClassCalendarsTableName = "test.au.mindsage.UserActivities";

        #endregion Fields

        #region IUserActivityRepository members

        /// <summary>
        /// ขอข้อมูลกิจกรรมต่างๆที่ผู้ใช้ทำกับ class room
        /// </summary>
        /// <param name="userprofile">รหัสบัญชีผู้ใช้ที่ต้องการข้อมูล</param>
        /// <param name="classRoomId">รหัส class room</param>
        public UserActivity GetUserActivityByUserProfileIdAndClassRoomId(string userprofile, string classRoomId)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูลกิจกรรม
        /// </summary>
        /// <param name="data">ข้อมูลกิจกรรมที่ต้องการดำเนินการ</param>
        public void UpsertUserActivity(UserActivity data)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        #endregion IUserActivityRepository members
    }
}
