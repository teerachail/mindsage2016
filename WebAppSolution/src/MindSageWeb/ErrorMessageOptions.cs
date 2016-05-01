using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb
{
    public class ErrorMessageOptions
    {
        public string CanNotConnectToTheDatabase { get; set; }
        public string CanNotChargeACreditCard { get; set; }
        public string CourseNotFound { get; set; }
        public string AccountNotFound { get; set; }
        public string SelectedCourseIsNotAvailableForPurchase { get; set; }
        public string NoLastActivatedCourse { get; set; }
        public string EntireCourseIsIncomplete { get; set; }
    }
}
