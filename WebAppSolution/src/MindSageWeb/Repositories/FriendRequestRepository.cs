using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ Friend request
    /// </summary>
    public class FriendRequestRepository : IFriendRequestRepository
    {
        #region Fields

        // HACK: Table name
        private const string TableName = "test.au.mindsage.FriendRequests";
        private MongoAccess.MongoUtil _mongoUtil;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="mongoUtil">Mongo access utility</param>
        public FriendRequestRepository(MongoAccess.MongoUtil mongoUtil)
        {
            _mongoUtil = mongoUtil;
        }

        #endregion Constructors

        #region IFriendRequestRepository members

        // <summary>
        /// ขอข้อมูลการขอเป็นเพื่อนจากรหัสบัญชีผู้ใช้
        /// </summary>
        /// <param name="userprofileId">รหัสบัญชีผู้ใช้ที่ต้องการขอข้อมูล</param>
        public IEnumerable<FriendRequest> GetFriendRequestByUserProfileId(string userprofileId)
        {
            var qry = _mongoUtil.GetCollection<FriendRequest>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.FromUserProfileId == userprofileId)
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ขอข้อมูลการขอเป็นเพื่อนจากรหัสบัญชีผู้ใช้
        /// </summary>
        /// <param name="userprofileId">รหัสบัญชีผู้ใช้ที่ต้องการขอข้อมูล</param>
        public IEnumerable<FriendRequest> GetFriendRequestByUserProfileId(IEnumerable<string> userprofileIds)
        {
            var qry = _mongoUtil.GetCollection<FriendRequest>(TableName)
                .Find(it => !it.DeletedDate.HasValue && userprofileIds.Contains(it.FromUserProfileId))
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูลการขอเป็นเพื่อน
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        public void UpsertFriendRequest(FriendRequest data)
        {
            var update = Builders<FriendRequest>.Update
             .Set(it => it.FromUserProfileId, data.FromUserProfileId)
             .Set(it => it.ToUserProfileId, data.ToUserProfileId)
             .Set(it => it.Status, data.Status)
             .Set(it => it.AcceptedDate, data.AcceptedDate)
             .Set(it => it.CreatedDate, data.CreatedDate)
             .Set(it => it.DeletedDate, data.DeletedDate);

            var updateOption = new UpdateOptions { IsUpsert = true };
            _mongoUtil.GetCollection<FriendRequest>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        #endregion IFriendRequestRepository members
    }
}
