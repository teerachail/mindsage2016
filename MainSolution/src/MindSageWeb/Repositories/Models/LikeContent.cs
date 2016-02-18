using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public abstract class LikeContent
    {
        #region Properties

        public string id { get; set; }
        public string LessonId { get; set; }
        public string LikedByUserProfileId { get; set; }
        public DateTime LastNotifyRequest { get; set; }
        public DateTime? LastNotifyComplete { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        #endregion Properties
    }
}
