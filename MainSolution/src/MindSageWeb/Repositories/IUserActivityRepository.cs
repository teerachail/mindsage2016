using MindsageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ User activity
    /// </summary>
    public interface IUserActivityRepository
    {
        #region Methods

        UserActivity GetUserActivityByUserProfileIdAndClassRoomId(string userprofile, string classRoomId);
        void UpsertUserActivity(UserActivity data);

        #endregion Methods
    }
}
