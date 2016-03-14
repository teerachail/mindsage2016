using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class UpdateCourseInfoRequest
    {
        #region Properties

        public string ClassRoomId { get; set; }
        public string ClassName { get; set; }
        public string ChangedStudentCode { get; set; }
        public DateTime? BeginDate { get; set; }

        #endregion Properties
    }
}
