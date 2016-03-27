using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebManagementPortal.Repositories.Models
{
    public class OwnCarousel
    {
        public IEnumerable<OwnItem> owl { get; set; }
        public class OwnItem
        {
            public string item { get; set; }
        }
    }
}
