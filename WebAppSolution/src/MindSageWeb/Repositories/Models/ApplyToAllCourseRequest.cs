using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class ApplyToAllCourseRequest
    {
        #region Properties

        public string UserProfileId { get; set; }
        public string ClassRoomId { get; set; }

        #endregion Properties
    }
}
