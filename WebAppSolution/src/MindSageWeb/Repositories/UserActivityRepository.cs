using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;

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
            var result = MongoAccess.MongoUtil.Instance.GetCollection<UserActivity>(ClassCalendarsTableName)
                 .Find(it => !it.DeletedDate.HasValue && it.UserProfileId == userprofile && it.ClassRoomId == classRoomId)
                 .ToEnumerable()
                 .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูลกิจกรรม
        /// </summary>
        /// <param name="data">ข้อมูลกิจกรรมที่ต้องการดำเนินการ</param>
        public void UpsertUserActivity(UserActivity data)
        {
            var update = Builders<UserActivity>.Update
              .Set(it => it.IsTeacher, data.IsTeacher)
              .Set(it => it.IsPrivateAccount, data.IsPrivateAccount)
              .Set(it => it.UserProfileName, data.UserProfileName)
              .Set(it => it.UserProfileImageUrl, data.UserProfileImageUrl)
              .Set(it => it.HideClassRoomMessageDate, data.HideClassRoomMessageDate)
              .Set(it => it.UserProfileId, data.UserProfileId)
              .Set(it => it.ClassRoomId, data.ClassRoomId)
              .Set(it => it.CreatedDate, data.CreatedDate)
              .Set(it => it.DeletedDate, data.DeletedDate)
              .Set(it => it.LessonActivities, data.LessonActivities);

            var updateOption = new UpdateOptions { IsUpsert = true };
            MongoAccess.MongoUtil.Instance.GetCollection<UserActivity>(ClassCalendarsTableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        #endregion IUserActivityRepository members
    }
}
