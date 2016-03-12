using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class SetStartDateRequest
    {
        #region Properties

        public string UserProfileId { get; set; }
        public string ClassRoomId { get; set; }
        public DateTime BeginDate { get; set; }

        #endregion Properties
    }
}
