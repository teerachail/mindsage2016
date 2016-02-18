using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class CourseCatalog
    {
        #region Properties

        public string id { get; set; }
        public string Grade { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string DescriptionImageUrl { get; set; }
        public string FullImageUrl { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public IEnumerable<Semester> Semesters { get; set; }

        #endregion Properties

        public class Semester
        {
            #region Properties

            public string id { get; set; }
            public int Order { get; set; }
            public string Title { get; set; }
            public string ShortDescription { get; set; }
            public string FullDescription { get; set; }

            public IEnumerable<Unit> Units { get; set; }

            #endregion Properties
        }

        public class Unit
        {
            #region Properties

            public string id { get; set; }
            public int Order { get; set; }
            public string Title { get; set; }
            public string ShortDescription { get; set; }
            public string FullDescription { get; set; }

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
