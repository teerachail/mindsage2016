using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class AddCourseRequest
    {
        #region Properties

        public string UserProfileId { get; set; }
        public string Code { get; set; }
        public string Grade { get; set; }

        #endregion Properties
    }
}
