using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class UpdateProfileRequest
    {
        #region Properties

        public string Name { get; set; }
        public string SchoolName { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsReminderOnceTime { get; set; }

        #endregion Properties
    }
}
