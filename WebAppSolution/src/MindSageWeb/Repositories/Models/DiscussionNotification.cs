using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class DiscussionNotification : Notification
    {
        #region Properties

        public string CommentId { get; set; }
        public string DiscussionId { get; set; }

        #endregion Properties
    }
}
