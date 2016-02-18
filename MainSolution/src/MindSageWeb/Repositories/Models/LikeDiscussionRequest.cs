using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class LikeDiscussionRequest
    {
        #region Properties

        public string ClassRoomId { get; set; }
        public string LessonId { get; set; }
        public string CommentId { get; set; }
        public string DiscussionId { get; set; }
        public string UserProfileId { get; set; }

        #endregion Properties
    }
}
