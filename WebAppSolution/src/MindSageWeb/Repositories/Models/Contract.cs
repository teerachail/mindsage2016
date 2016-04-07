using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class Contract
    {
        #region Properties

        public string id { get; set; }
        public string SchoolName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PrimaryContractName { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryContractName { get; set; }
        public string SecondaryPhoneNumber { get; set; }
        public string SecondaryEmail { get; set; }
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
