using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebManagementPortal.Repositories.Models
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
        public string MoreDescription { get; set; }
        public string ShortTeacherLessonPlan { get; set; }
        public string MoreTeacherLessonPlan { get; set; }
        public string PrimaryContentURL { get; set; }
        public string PrimaryContentDescription { get; set; }
        public bool IsPreviewable { get; set; }
        public IEnumerable<ExtraContent> ExtraContents { get; set; }
        public string CourseCatalogId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public IEnumerable<Ads> Advertisments { get; set; }
        public IEnumerable<TopicOfTheDay> TopicOfTheDays { get; set; }

        #endregion Properties

        public class Ads
        {
            #region Properties

            public string id { get; set; }
            public string ImageUrl { get; set; }
            public string LinkUrl { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? DeletedDate { get; set; }

            #endregion Properties
        }

        public class TopicOfTheDay
        {
            #region Properties

            public string id { get; set; }
            public string Message { get; set; }
            public int SendOnDay { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? DeletedDate { get; set; }

            #endregion Properties
        }

        public class ExtraContent
        {
            #region Properties

            public string id { get; set; }
            public string ContentURL { get; set; }
            public string Description { get; set; }
            public string IconURL { get; set; }

            #endregion Properties
        }
    }
}
