using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories.Models
{
    public class Comment
    {
        #region Properties

        public string id { get; set; }
        public string Description { get; set; }
        public int TotalLikes { get; set; }
        public string CreatorImageUrl { get; set; }
        public string CreatorDisplayName { get; set; }
        public string ClassRoomId { get; set; }
        public string LessonId { get; set; }
        public string CreatedByUserProfileId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public IEnumerable<Discussion> Discussions { get; set; }

        #endregion Properties

        public class Discussion
        {
            #region Properties

            public string id { get; set; }
            public string Description { get; set; }
            public int TotalLikes { get; set; }
            public string CreatorImageUrl { get; set; }
            public string CreatorDisplayName { get; set; }
            public string CreatedByUserProfileId { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? DeletedDate { get; set; }

            #endregion Properties
        }
    }
}
