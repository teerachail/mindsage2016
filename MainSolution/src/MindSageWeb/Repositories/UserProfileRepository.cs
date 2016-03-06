using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;

namespace MindSageWeb.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        #region Fields

        // HACK: Table name
        private const string UserProfileTableName = "test.au.mindsage.UserProfiles";

        #endregion Fields

        #region IUserProfileRepository members

        /// <summary>
        /// ขอข้อมูล User profile จากรหัส
        /// </summary>
        /// <param name="userprofileId">รหัส User profile ที่จะทำการขอข้อมูล</param>
        public UserProfile GetUserProfileById(string userprofileId)
        {
            var result = MongoAccess.MongoUtil.Instance.GetCollection<UserProfile>(UserProfileTableName)
                .Find(it => !it.DeletedDate.HasValue && it.id == userprofileId)
                .ToEnumerable()
                .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// ขอข้อมูล User profile จากรหัส
        /// </summary>
        /// <param name="userprofileId">รหัส User profile ที่จะทำการขอข้อมูล</param>
        public IEnumerable<UserProfile> GetUserProfileById(IEnumerable<string> userprofileIds)
        {
            var qry = MongoAccess.MongoUtil.Instance.GetCollection<UserProfile>(UserProfileTableName)
                .Find(it => !it.DeletedDate.HasValue && userprofileIds.Contains(it.id))
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ขอข้อมูล User profile จากรหัส class room
        /// </summary>
        /// <param name="classRoomId">รหัส class room ที่จะทำการขอข้อมูล</param>
        public IEnumerable<UserProfile> GetUserProfilesByClassRoomId(string classRoomId)
        {
            var result = MongoAccess.MongoUtil.Instance.GetCollection<UserProfile>(UserProfileTableName)
                .Find(it => !it.DeletedDate.HasValue && it.Subscriptions.Any())
                .ToEnumerable()
                .Where(it => it.Subscriptions.Where(subscription => !subscription.DeletedDate.HasValue).Select(s => s.ClassRoomId).Contains(classRoomId));
            return result;
        }

        /// <summary>
        /// ขอข้อมูล User profile จากรหัส class room
        /// </summary>
        /// <param name="classRoomIds">รหัส class room ที่จะทำการขอข้อมูล</param>
        public IEnumerable<UserProfile> GetUserProfilesByClassRoomId(IEnumerable<string> classRoomIds)
        {
            var result = MongoAccess.MongoUtil.Instance.GetCollection<UserProfile>(UserProfileTableName)
               .Find(it => !it.DeletedDate.HasValue && it.Subscriptions.Any())
               .ToEnumerable()
               .Where(it =>
               {
                   var subscriptionIds = it.Subscriptions
                    .Where(subscription => !subscription.DeletedDate.HasValue)
                    .Select(subscription => subscription.ClassRoomId);
                   return classRoomIds.Any(cid => subscriptionIds.Contains(cid));
               });
            return result;
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูลผู้ใช้
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        public void UpsertUserProfile(UserProfile data)
        {
            var update = Builders<UserProfile>.Update
             .Set(it => it.Name, data.Name)
             .Set(it => it.SchoolName, data.SchoolName)
             .Set(it => it.ImageProfileUrl, data.ImageProfileUrl)
             .Set(it => it.IsPrivateAccount, data.IsPrivateAccount)
             .Set(it => it.IsEnableNotification, data.IsEnableNotification)
             .Set(it => it.CourseReminder, data.CourseReminder)
             .Set(it => it.CreatedDate, data.CreatedDate)
             .Set(it => it.DeletedDate, data.DeletedDate)
             .Set(it => it.Subscriptions, data.Subscriptions);

            var updateOption = new UpdateOptions { IsUpsert = true };
            MongoAccess.MongoUtil.Instance.GetCollection<UserProfile>(UserProfileTableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        #endregion IUserProfileRepository members
    }
}
