using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb
{
    public class AppConfigOptions
    {
        #region Properties

        public string PrimaryDBName { get; set; }
        public string GoogleClinetId { get; set; }
        public string GoogleClientSecret { get; set; }
        public string ManagementPortalUrl { get; set; }
        public string MindSageUrl { get; set; }
        public string PaypalClientId { get; set; }
        public string PaypalClientSecret { get; set; }

        #endregion Properties
    }
}
