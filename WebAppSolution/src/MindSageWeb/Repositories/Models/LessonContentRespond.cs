using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class LessonContentRespond : LessonCatalog
    {
        #region Properties

        public string CourseMessage { get; set; }
        public int TotalLikes { get; set; }
        public bool IsTeacher { get; set; }

        #endregion Properties
    }
}
