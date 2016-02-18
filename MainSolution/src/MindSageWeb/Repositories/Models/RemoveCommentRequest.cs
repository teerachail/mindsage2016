using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories.Models
{
    public class RemoveCommentRequest
    {
        #region Properties

        public string ClassRoomId { get; set; }
        public string LessonId { get; set; }
        public string UserProfileId { get; set; }

        #endregion Properties
    }
}
