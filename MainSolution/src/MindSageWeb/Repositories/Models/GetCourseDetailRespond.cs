using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class GetCourseDetailRespond
    {
        #region Properties

        public string id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DescriptionImageUrl { get; set; }
        public string FullImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalWeeks { get; set; }

        public IEnumerable<Semester> Semesters { get; set; }

        #endregion Properties

        public class Semester
        {
            #region Properties

            public string Name { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int TotalWeeks { get; set; }

            public IEnumerable<Unit> Units { get; set; }

            #endregion Properties
        }

        public class Unit
        {
            #region Properties

            public int UnitNo { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int TotalWeeks { get; set; }

            public IEnumerable<Lesson> Lessons { get; set; }

            #endregion Properties
        }

        public class Lesson
        {
            #region Properties

            public string id { get; set; }
            public int Order { get; set; }
            public string Name { get; set; }
            public string SemesterGroupName { get; set; }

            #endregion Properties
        }
    }
}
