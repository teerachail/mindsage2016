using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories.Models
{
    public class UserActivity
    {
        #region Properties

        public string id { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsPrivateAccount { get; set; }
        public string UserProfileName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public DateTime? HideClassRoomMessageDate { get; set; }
        public string UserProfileId { get; set; }
        public string ClassRoomId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public IEnumerable<LessonActivity> LessonActivities { get; set; }

        #endregion Properties

        public class LessonActivity
        {
            #region Properties

            public string id { get; set; }
            public DateTime BeginDate { get; set; }
            public int TotalContentsAmount { get; set; }
            public IEnumerable<string> SawContentIds { get; set; }
            public int CreatedCommentAmount { get; set; }
            public int ParticipationAmount { get; set; }
            public string LessonId { get; set; }

            #endregion Properties
        }
    }
}
