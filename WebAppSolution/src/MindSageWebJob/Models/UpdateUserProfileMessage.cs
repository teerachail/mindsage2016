using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputeWebJobsSDKQueue.Models
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
