using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class SetScheduleWithRangeRespond : GetCourseScheduleRespond
    {
        #region Properties

        public bool IsComplete { get; set; }

        #endregion Properties

    }
}
