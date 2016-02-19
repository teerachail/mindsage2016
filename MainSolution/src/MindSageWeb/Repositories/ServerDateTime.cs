using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    public class ServerDateTime : IDateTime
    {
        #region IDateTime members

        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        #endregion IDateTime members
    }
}
