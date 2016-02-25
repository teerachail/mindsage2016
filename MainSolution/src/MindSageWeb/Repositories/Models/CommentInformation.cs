using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class CommentInformation
    {
        #region Properties

        public string id { get; set; }
        public int LessonWeek { get; set; }
        public string Description { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDiscussions { get; set; }
        public string CreatorImageUrl { get; set; }
        public string CreatorDisplayName { get; set; }
        public string ClassRoomId { get; set; }
        public string LessonId { get; set; }
        public string CreatedByUserProfileId { get; set; }
        public DateTime CreatedDate { get; set; }

        #endregion Properties
    }
}
