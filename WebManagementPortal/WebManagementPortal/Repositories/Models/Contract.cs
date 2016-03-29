using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebManagementPortal.Repositories.Models
{
    public class Contract
    {
        #region Properties

        public string id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string TimeZone { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public IEnumerable<License> Licenses { get; set; }

        #endregion Properties

        public class License
        {
            #region Properties

            public string id { get; set; }
            public string CourseName { get; set; }
            public string Grade { get; set; }
            public string CourseCatalogId { get; set; }
            public int StudentsCapacity { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? DeletedDate { get; set; }
            public IEnumerable<TeacherKey> TeacherKeys { get; set; }

            #endregion Properties
        }

        public class TeacherKey
        {
            #region Properties

            public string id { get; set; }
            public string Grade { get; set; }
            public string Code { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? DeletedDate { get; set; }

            #endregion Properties
        }
    }
}
