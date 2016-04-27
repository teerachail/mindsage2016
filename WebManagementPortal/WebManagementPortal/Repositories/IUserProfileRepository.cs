using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebManagementPortal.Repositories.Models;

namespace WebManagementPortal.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ User profile
    /// </summary>
    public interface IUserProfileRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล User profile จากรหัส subscription license id
        /// </summary>
        /// <param name="userprofileId">รหัส User license ที่ต้องการค้นหา</param>
        IEnumerable<UserProfile> GetUserProfileByLicenseIdInSubscription(IEnumerable<string> licenseIds);

        /// <summary>
        /// ขอข้อมูล User profile จากรหัส subscription license id
        /// </summary>
        /// <param name="userprofileId">รหัส User license ที่ต้องการค้นหา</param>
        IEnumerable<UserProfile> GetUserProfileByLicenseIdInSubscription(string licenseId);

        #endregion Methods
    }
}
