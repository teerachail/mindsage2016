using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการดึงข้อมูลเวลา
    /// </summary>
    public interface IDateTime
    {
        #region Methods

        /// <summary>
        /// ขอวันที่และเวลาในปัจจุบัน
        /// </summary>
        DateTime GetCurrentTime();

        #endregion Methods
    }
}
