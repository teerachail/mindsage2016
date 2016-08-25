using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class LessonTestedResultRequest
    {
        public string UserProfileId { get; set; }
        public string ClassRoomId { get; set; }
        public string LessonId { get; set; }
        public string AssessmentId { get; set; }
        public string Answer { get; set; }
    }
}
