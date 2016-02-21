using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Friend request
    /// </summary>
    public interface IFriendRequestRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูลการขอเป็นเพื่อนจากรหัสบัญชีผู้ใช้
        /// </summary>
        /// <param name="userprofileId">รหัสบัญชีผู้ใช้ที่ต้องการขอข้อมูล</param>
        IEnumerable<FriendRequest> GetFriendRequestByUserProfileId(string userprofileId);

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูลการขอเป็นเพื่อน
        /// </summary>
        /// <param name="request">ข้อมูลที่ต้องการดำเนินการ</param>
        void UpsertFriendRequest(FriendRequest request);

        #endregion Methods
    }
}
