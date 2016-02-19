using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class GetFriendListRespond
    {
        #region Properties

        public string id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public FriendRequest.RelationStatus Status { get; set; }

        #endregion Properties
    }
}
