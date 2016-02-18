using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories.Models
{
    public class LessonCatalog
    {
        #region Properties

        public string id { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public int UnitNo { get; set; }
        public string SemesterName { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string ShortTeacherLessonPlan { get; set; }
        public string FullTeacherLessonPlan { get; set; }
        public string PrimaryContentURL { get; set; }
        public IEnumerable<string> ExtraContentUrls { get; set; }
        public string CourseCatalogId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public IEnumerable<Ads> Advertisments { get; set; }

        #endregion Properties

        public class Ads
        {
            #region Properties

            public string id { get; set; }
            public string ImageUrl { get; set; }
            public string LinkUrl { get; set; }

            #endregion Properties
        }
    }
}
