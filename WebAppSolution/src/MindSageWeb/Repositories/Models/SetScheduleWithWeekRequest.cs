using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class SetScheduleWithWeekRequest
    {
        #region Properties

        public string UserProfileId { get; set; }
        public string ClassRoomId { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsShift { get; set; }
        public bool IsSunday { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }

        #endregion Properties
    }
}
