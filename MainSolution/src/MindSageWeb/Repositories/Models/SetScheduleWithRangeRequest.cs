using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class SetScheduleWithRangeRequest
    {
        #region Properties

        public string id { get; set; }
        public string ClassRoomId { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsShift { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        #endregion Properties
    }
}
