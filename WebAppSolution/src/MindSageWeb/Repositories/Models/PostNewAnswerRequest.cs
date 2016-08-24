using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class PostNewAnswerRequest
    {
        #region Properties
        
        public string LessonTestId { get; set; }
        public string AssessmentId { get; set; }
        public int Answer { get; set; }

        #endregion Properties
    }
}
