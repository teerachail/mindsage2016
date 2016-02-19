using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;

namespace MindSageWeb.Repositories
{
    public class ClassCalendarRepository : IClassCalendarRepository
    {
        #region Fields

        // HACK: Table name
        private const string ClassCalendarsTableName = "test.au.mindsage.ClassCalendars";

        #endregion Fields

        #region IClassCalendarRepository members

        /// <summary>
        /// ขอข้อมูล Class calendar จากรหัส Class room
        /// </summary>
        /// <param name="classRoomId">รหัส Class room ที่ต้องการขอข้อมูล</param>
        public ClassCalendar GetClassCalendarByClassRoomId(string classRoomId)
        {
            var result = MongoAccess.MongoUtil.Instance.GetCollection<ClassCalendar>(ClassCalendarsTableName)
                .Find(it => !it.DeletedDate.HasValue && it.ClassRoomId == classRoomId)
                .ToEnumerable()
                .FirstOrDefault();
            return result;
        }

        #endregion IClassCalendarRepository members
    }
}
