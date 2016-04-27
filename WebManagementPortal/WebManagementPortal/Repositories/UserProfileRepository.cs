using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebManagementPortal.Repositories.Models;

namespace WebManagementPortal.Repositories
{
    // <summary>
    /// ตัวติดต่อกับ User profile
    /// </summary>
    public class UserProfileRepository : IUserProfileRepository
    {
        #region IUserProfileRepository members

        /// <summary>
        /// ขอข้อมูล User profile จากรหัส subscription license id
        /// </summary>
        /// <param name="userprofileId">รหัส User license ที่ต้องการค้นหา</param>
        public IEnumerable<UserProfile> GetUserProfileByLicenseIdInSubscription(IEnumerable<string> licenseIds)
        {
            var qry = MongoAccess.MongoUtil.Instance.GetCollection<UserProfile>(AppConfigOptions.UserProfileTableName)
               .Find(it => it.Subscriptions.Any(s => !s.DeletedDate.HasValue && !string.IsNullOrEmpty(s.LicenseId) && licenseIds.Contains(s.LicenseId)))
               .ToEnumerable();
            return qry;
        }

        // <summary>
        /// ขอข้อมูล User profile จากรหัส subscription license id
        /// </summary>
        /// <param name="userprofileId">รหัส User license ที่ต้องการค้นหา</param>
        public IEnumerable<UserProfile> GetUserProfileByLicenseIdInSubscription(string licenseId)
        {
            var qry = MongoAccess.MongoUtil.Instance.GetCollection<UserProfile>(AppConfigOptions.UserProfileTableName)
               .Find(it => it.Subscriptions.Any(s => !s.DeletedDate.HasValue && !string.IsNullOrEmpty(s.LicenseId) && s.LicenseId == licenseId))
               .ToEnumerable();
            return qry;
        }

        #endregion IUserProfileRepository members
    }
}
