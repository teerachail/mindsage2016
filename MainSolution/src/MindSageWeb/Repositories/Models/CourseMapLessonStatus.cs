using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class CourseMapLessonStatus
    {
        #region Properties

        public string LessonId { get; set; }
        public bool IsLocked { get; set; }
        public string LessonWeekName { get; set; }

        #endregion Properties
    }
}
