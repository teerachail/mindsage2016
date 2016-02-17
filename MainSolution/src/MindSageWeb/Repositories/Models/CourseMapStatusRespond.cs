using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class CourseMapStatusRespond
    {
        #region Properties

        public string LessonId { get; set; }
        public bool HaveAnyComments { get; set; }
        public bool IsReadedAllContents { get; set; }

        #endregion Properties
    }
}
