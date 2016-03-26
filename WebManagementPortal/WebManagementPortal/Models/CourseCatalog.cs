using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebManagementPortal.Models
{
    public class CourseCatalog
    {
        #region Properties

        public string id { get; set; }
        public string GroupName { get; set; }
        public string Grade { get; set; }
        public string SideName { get; set; }
        public double PriceUSD { get; set; }
        public string Series { get; set; }
        public string Title { get; set; }
        public string FullDescription { get; set; }
        public string DescriptionImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public IEnumerable<Semester> Semesters { get; set; }
        public IEnumerable<string> Advertisements { get; set; }

        #endregion Properties

        public class Semester
        {
            #region Properties

            public string id { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }

            public IEnumerable<Unit> Units { get; set; }

            #endregion Properties
        }

        public class Unit
        {
            #region Properties

            public string id { get; set; }
            public int Order { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }

            public IEnumerable<Lesson> Lessons { get; set; }

            #endregion Properties
        }

        public class Lesson
        {
            #region Properties

            public string id { get; set; }
            public int Order { get; set; }
            public IEnumerable<LessonContent> Contents { get; set; }

            #endregion Properties
        }

        public class LessonContent
        {
            #region Properties

            public string ImageUrl { get; set; }
            public string Description { get; set; }

            #endregion Properties
        }
    }
}
