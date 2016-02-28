using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class CommentNotification : Notification
    {
        #region Properties

        public string CommentId { get; set; }

        #endregion Properties
    }
}
