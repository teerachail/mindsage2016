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

        #endregion IUserProfileRepository members
    }
}
