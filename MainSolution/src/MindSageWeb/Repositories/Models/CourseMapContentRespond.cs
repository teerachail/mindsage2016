using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class CourseMapContentRespond
    {
        #region Properties

        public string LessonId { get; set; }
        public string SemesterName { get; set; }
        public string LessonWeekName { get; set; }
        public bool IsLocked { get; set; }

        #endregion Properties
    }
}
