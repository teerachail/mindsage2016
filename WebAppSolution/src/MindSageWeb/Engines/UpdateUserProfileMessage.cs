using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Engines
{
    public class UpdateUserProfileMessage
    {
        #region Properties

        public string UserProfileId { get; set; }
        public string DisplayName { get; set; }
        public string ProfileImageUrl { get; set; }

        #endregion Properties
    }
}
