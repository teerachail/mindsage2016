using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class GetLikeRespond
    {
        #region Properties

        public string LessonId { get; set; }
        public bool IsLikedLesson { get; set; }
        public IEnumerable<string> LikeCommentIds { get; set; }
        public IEnumerable<string> LikeDiscussionIds { get; set; }

        #endregion Properties
    }
}
