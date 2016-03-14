using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class UpdateCommentRequest
    {
        #region Properties

        public string ClassRoomId { get; set; }
        public string LessonId { get; set; }
        public string UserProfileId { get; set; }
        public bool IsDelete { get; set; }
        public string Description { get; set; }

        #endregion Properties
    }
}
