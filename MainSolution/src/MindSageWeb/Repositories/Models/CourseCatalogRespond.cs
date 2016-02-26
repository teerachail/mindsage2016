using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class CourseCatalogRespond
    {
        #region Properties

        public string id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }

        #endregion Properties
    }
}
