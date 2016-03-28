using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebManagementPortal.Repositories.Models
{
    public class ClassRoom
    {
        #region Properties

        public string id { get; set; }
        public string Name { get; set; }
        public string CourseCatalogId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string Message { get; set; }
        public bool IsPublic { get; set; }
        public DateTime LastUpdatedMessageDate { get; set; }

        public IEnumerable<Lesson> Lessons { get; set; }

        #endregion Properties

        public class Lesson
        {
            #region Properties

            public string id { get; set; }
            public int TotalLikes { get; set; }
            public string LessonCatalogId { get; set; }

            #endregion Properties
        }
    }
}
