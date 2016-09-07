using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class LessonCatalog
    {
        #region Properties

        public string id { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public int UnitNo { get; set; }
        public string SemesterName { get; set; }
        public string CourseCatalogId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public IEnumerable<Ads> Advertisments { get; set; }
        public IEnumerable<TopicOfTheDay> TopicOfTheDays { get; set; }
        public IEnumerable<LessonItem> TeacherItems { get; set; }
        public IEnumerable<LessonItem> StudentItems { get; set; }
        public IEnumerable<AssessmentItem> PreAssessments { get; set; }
        public IEnumerable<AssessmentItem> PostAssessments { get; set; }

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

        public class LessonItem
        {
            #region Properties

            public string id { get; set; }
            public int Order { get; set; }
            public string Name { get; set; }
            public string ContentType { get; set; }
            public bool IsPreviewable { get; set; }
            public string IconURL { get; set; }
            public string Description { get; set; }
            public string ContentURL { get; set; }

            #endregion Properties
        }

        public class AssessmentItem
        {
            #region Properties

            public string id { get; set; }
            public int Order { get; set; }
            public string Name { get; set; }
            public bool IsPreviewable { get; set; }
            public string IconURL { get; set; }
            public IEnumerable<Assessment> Assessments { get; set; }

            #endregion Properties
        }

        public class Assessment
        {
            #region Properties

            public string id { get; set; }
            public int Order { get; set; }
            public string ContentType { get; set; }
            public string Question { get; set; }
            public string StatementBefore { get; set; }
            public string StatementAfter { get; set; }
            public IEnumerable<Choice> Choices { get; set; }

            #endregion Properties
        }

        public class Choice
        {
            #region Properties

            public string id { get; set; }
            public string Name { get; set; }
            public bool IsCorrect { get; set; }

            #endregion Properties
        }
    }
}
