using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class CourseMapContentRespond
    {
        #region Properties

        public string SemesterName { get; set; }
        public IEnumerable<CourseMapLessonStatus> LessonStatus { get; set; }

        #endregion Properties
    }
}
