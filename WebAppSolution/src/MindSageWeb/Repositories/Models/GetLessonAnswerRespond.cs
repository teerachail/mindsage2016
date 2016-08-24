using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class GetLessonAnswerRespond
    {
        #region Properties

        public string id { get; set; }
        public string UserProfileId { get; set; }
        public string LessonId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public IEnumerable<LessonAnswerInformation> Answer { get; set; }

        #endregion Properties
    }
}
