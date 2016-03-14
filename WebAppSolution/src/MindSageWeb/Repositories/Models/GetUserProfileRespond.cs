using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class GetUserProfileRespond
    {
        #region Properties

        public string UserProfileId { get; set; }
        public bool HasPassword { get; set; }
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public string SchoolName { get; set; }
        public bool IsPrivateAccout { get; set; }
        public bool IsReminderOnceTime { get; set; }
        public string CurrentClassRoomId { get; set; }
        public string CurrentLessonId { get; set; }
        public int CurrentLessonNo { get; set; }

        #endregion Properties
    }
}
