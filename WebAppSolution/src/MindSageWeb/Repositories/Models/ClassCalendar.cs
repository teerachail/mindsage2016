using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class ClassCalendar
    {
        #region Properties

        public string id { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public string ClassRoomId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public IEnumerable<LessonCalendar> LessonCalendars { get; set; }
        public IEnumerable<DateTime> Holidays { get; set; }
        public IEnumerable<DateTime> ShiftDays { get; set; }

        #endregion Properties

        public class LessonCalendar
        {
            #region Properties

            public string id { get; set; }
            public int Order { get; set; }
            public string SemesterGroupName { get; set; }
            public DateTime BeginDate { get; set; }
            public IEnumerable<TopicOfTheDay> TopicOfTheDays { get; set; }
            public string LessonId { get; set; }
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
            public DateTime? RequiredSendTopicOfTheDayDate { get; set; }
            public DateTime? SendTopicOfTheDayDate { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? DeletedDate { get; set; }

            #endregion Properties
        }
    }
}
