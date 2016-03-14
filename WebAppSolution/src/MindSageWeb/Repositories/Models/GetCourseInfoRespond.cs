using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class GetCourseInfoRespond
    {
        #region Properties

        public string UserProfileId { get; set; }
        public string ClassRoomId { get; set; }
        public bool IsTeacher { get; set; }
        public string ClassName { get; set; }
        public string CurrentStudentCode { get; set; }
        public int NumberOfStudents { get; set; }
        public DateTime? StartDate { get; set; }

        #endregion Properties
    }
}
