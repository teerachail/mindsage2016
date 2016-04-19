using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;
using Microsoft.Extensions.OptionsModel;

namespace MindSageWeb.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        #region Fields

        private readonly string TableName;
        private MongoAccess.MongoUtil _mongoUtil;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="mongoUtil">Mongo access utility</param>
        public UserProfileRepository(MongoAccess.MongoUtil mongoUtil, IOptions<DatabaseTableOptions> option)
        {
            _mongoUtil = mongoUtil;
            TableName = option.Value.UserProfiles;
        }

        #endregion Constructors

        #region IUserProfileRepository members

        /// <summary>
        /// ขอข้อมูล User profile จากรหัส
        /// </summary>
        /// <param name="userprofileId">รหัส User profile ที่จะทำการขอข้อมูล</param>
        public UserProfile GetUserProfileById(string userprofileId)
        {
            var result = _mongoUtil.GetCollection<UserProfile>(TableName)
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
            var qry = _mongoUtil.GetCollection<UserProfile>(TableName)
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
            var result = _mongoUtil.GetCollection<UserProfile>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.Subscriptions.Any())
                .ToEnumerable()
                .Where(it => it.Subscriptions.Where(subscription => !subscription.DeletedDate.HasValue).Select(s => s.ClassRoomId).Contains(classRoomId));
            return result;
        }

        /// <summary>
        /// ขอข้อมูล User profile จากรหัส class room ที่ใช้งานล่าสุด
        /// </summary>
        /// <param name="classRoomId">รหัส class room ที่จะทำการขอข้อมูล</param>
        public IEnumerable<UserProfile> GetUserProfilesByLastActivateOnClassRoomId(IEnumerable<string> classRoomIds)
        {
            var qry = _mongoUtil.GetCollection<UserProfile>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.Subscriptions.Any(s => classRoomIds.Contains(s.ClassRoomId)))
                .ToEnumerable()
                .Where(it => classRoomIds.Contains(it.Subscriptions.Where(s => !s.DeletedDate.HasValue && s.LastActiveDate.HasValue).OrderBy(s => s.LastActiveDate).LastOrDefault().ClassRoomId));
            return qry;
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
            _mongoUtil.GetCollection<UserProfile>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        #endregion IUserProfileRepository members
    }
}
