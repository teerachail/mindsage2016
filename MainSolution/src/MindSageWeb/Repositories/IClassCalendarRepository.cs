using MindsageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Class calendar
    /// </summary>
    public interface IClassCalendarRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล Class calendar จากรหัส Class room
        /// </summary>
        /// <param name="classRoomId">รหัส Class room ที่ต้องการขอข้อมูล</param>
        ClassCalendar GetClassCalendarByClassRoomId(string classRoomId);

        #endregion Methods
    }
}
