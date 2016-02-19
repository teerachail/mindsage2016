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
        private const string FriendRequestsTableName = "test.au.mindsage.FriendRequests";

        #endregion Fields

        #region IFriendRequestRepository members

        // <summary>
        /// ขอข้อมูลการขอเป็นเพื่อนจากรหัสบัญชีผู้ใช้
        /// </summary>
        /// <param name="userprofileId">รหัสบัญชีผู้ใช้ที่ต้องการขอข้อมูล</param>
        public IEnumerable<FriendRequest> GetFriendRequestByUserProfileId(string userprofileId)
        {
            var qry = MongoAccess.MongoUtil.Instance.GetCollection<FriendRequest>(FriendRequestsTableName)
                .Find(it => !it.DeletedDate.HasValue && it.FromUserProfileId == userprofileId)
                .ToEnumerable();
            return qry;
        }

        #endregion IFriendRequestRepository members
    }
}
