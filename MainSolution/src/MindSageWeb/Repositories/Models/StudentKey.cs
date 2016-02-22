using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class StudentKey
    {
        #region Properties

        public string id { get; set; }
        public string Code { get; set; }
        public string CourseCatalogId { get; set; }
        public string ClassRoomId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        #endregion Properties
    }
}
