using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    public class ReadNoteRequest
    {
        #region Properties

        public string ClassRoomId { get; set; }
        public string UserProfileId { get; set; }

        #endregion Properties
    }
}
