using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class SendFriendRequest
    {
        #region Properties

        public string FromUserProfileId { get; set; }
        public string ToUserProfileId { get; set; }
        public string RequestId { get; set; }
        public bool IsAccept { get; set; }

        #endregion Properties
    }
}
