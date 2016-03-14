using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class NotificationRespond
    {
        #region Properties

        public string id { get; set; }
        public NotificationTag Tag { get; set; }
        public string Message { get; set; }
        public IEnumerable<ActionInformation> FromUserProfiles { get; set; }

        #endregion Properties

        public enum NotificationTag
        {
            Reminder, TOTD, Like, Comment, Discussion
        }

        public class ActionInformation
        {
            #region Properties

            public string FromUserProfileId { get; set; }
            public string FromUserProfileName { get; set; }

            #endregion Properties
        }
    }
}
