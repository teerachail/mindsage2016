using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class LessonTestResult
    {
        #region Properties

        public string id { get; set; }
        public string UserProfileId { get; set; }
        public string ClassRoomId { get; set; }
        public string LessonId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public IEnumerable<AnswerInformation> Answers { get; set; }

        #endregion Properties

        #region Inner classes

        public class AnswerInformation
        {
            public string AssessmentId { get; set; }
            public string Answer { get; set; }
        }

        #endregion Inner classes
    }
}
