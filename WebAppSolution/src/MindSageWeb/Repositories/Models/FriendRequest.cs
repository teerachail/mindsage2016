using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class FriendRequest
    {
        #region Properties

        public string id { get; set; }
        public string FromUserProfileId { get; set; }
        public string ToUserProfileId { get; set; }
        public RelationStatus Status { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        #endregion Properties

        public enum RelationStatus
        {
            SendRequest,
            ReceiveRequest,
            Friend,
            Unfriend,
        }
    }
}
