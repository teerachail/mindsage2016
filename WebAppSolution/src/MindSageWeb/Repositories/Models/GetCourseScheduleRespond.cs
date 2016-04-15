using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class GetCourseScheduleRespond
    {
        #region Properties

        public bool IsComplete { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<LessonSchedule> Lessons { get; set; }
        public IEnumerable<DateTime> Holidays { get; set; }
        public IEnumerable<DateTime> ShiftDays { get; set; }

        #endregion Properties
    }

    public class LessonSchedule
    {
        #region Properties

        public string Name { get; set; }
        public DateTime BeginDate { get; set; }

        #endregion Properties
    }
}
