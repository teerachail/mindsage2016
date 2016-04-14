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
        public string UserProfileNotFound { get; set; }
        public string CourseInformationIncorrect { get; set; }
    }
}
