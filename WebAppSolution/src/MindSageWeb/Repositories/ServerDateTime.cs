using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวดึงข้อมูลเวลา
    /// </summary>
    public class ServerDateTime : IDateTime
    {
        #region IDateTime members

        /// <summary>
        /// ขอวันที่และเวลาในปัจจุบัน
        /// </summary>
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        #endregion IDateTime members
    }
}
