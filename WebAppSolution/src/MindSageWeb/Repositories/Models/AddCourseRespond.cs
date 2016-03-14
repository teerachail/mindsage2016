using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class AddCourseRespond
    {
        #region Properties

        public string Code { get; set; }
        public string Grade { get; set; }
        public bool IsSuccess { get; set; }

        #endregion Properties
    }
}
