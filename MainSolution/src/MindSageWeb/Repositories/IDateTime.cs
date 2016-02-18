using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    public interface IDateTime
    {
        #region Methods

        DateTime GetCurrentTime();

        #endregion Methods
    }
}
