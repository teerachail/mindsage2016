using MindsageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories
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

        #endregion Methods
    }
}
