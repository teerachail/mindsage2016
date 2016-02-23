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
        private const string TableName = "test.au.mindsage.ClassCalendars";

        #endregion Fields

        #region IClassCalendarRepository members

        /// <summary>
        /// ขอข้อมูล Class calendar จากรหัส Class room
        /// </summary>
        /// <param name="classRoomId">รหัส Class room ที่ต้องการขอข้อมูล</param>
        public ClassCalendar GetClassCalendarByClassRoomId(string classRoomId)
        {
            var result = MongoAccess.MongoUtil.Instance.GetCollection<ClassCalendar>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.ClassRoomId == classRoomId)
                .ToEnumerable()
                .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล Class calendar
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        public void UpsertClassCalendar(ClassCalendar data)
        {
            var update = Builders<ClassCalendar>.Update
             .Set(it => it.BeginDate, data.BeginDate)
             .Set(it => it.IsWeekendHoliday, data.IsWeekendHoliday)
             .Set(it => it.ExpiredDate, data.ExpiredDate)
             .Set(it => it.CloseDate, data.CloseDate)
             .Set(it => it.ClassRoomId, data.ClassRoomId)
             .Set(it => it.CreatedDate, data.CreatedDate)
             .Set(it => it.DeletedDate, data.DeletedDate)
             .Set(it => it.LessonCalendars, data.LessonCalendars)
             .Set(it => it.Holidays, data.Holidays);

            var updateOption = new UpdateOptions { IsUpsert = true };
            MongoAccess.MongoUtil.Instance.GetCollection<ClassCalendar>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        #endregion IClassCalendarRepository members
    }
}
