using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class PostNewCourseMessageRequest
    {
        #region Properties

        public string ClassRoomId { get; set; }
        public string UserProfileId { get; set; }
        public string Message { get; set; }

        #endregion Properties
    }
}
