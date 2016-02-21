using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ User profile
    /// </summary>
    public interface IUserProfileRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล User profile จากรหัส
        /// </summary>
        /// <param name="userprofileId">รหัส User profile ที่จะทำการขอข้อมูล</param>
        UserProfile GetUserProfileById(string userprofileId);

        /// <summary>
        /// ขอข้อมูล User profile จากรหัส
        /// </summary>
        /// <param name="userprofileId">รหัส User profile ที่จะทำการขอข้อมูล</param>
        IEnumerable<UserProfile> GetUserProfileById(IEnumerable<string> userprofileIds);

        /// <summary>
        /// ขอข้อมูล User profile จากรหัส class room
        /// </summary>
        /// <param name="classRoomId">รหัส class room ที่จะทำการขอข้อมูล</param>
        IEnumerable<UserProfile> GetUserProfilesByClassRoomId(string classRoomId);

        #endregion Methods
    }
}
