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
        private MongoAccess.MongoUtil _mongoUtil;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="mongoUtil">Mongo access utility</param>
        public ClassCalendarRepository(MongoAccess.MongoUtil mongoUtil)
        {
            _mongoUtil = mongoUtil;
        }

        #endregion Constructors

        #region IClassCalendarRepository members

        /// <summary>
        /// ขอข้อมูล Class calendar จากรหัส Class room
        /// </summary>
        /// <param name="classRoomId">รหัส Class room ที่ต้องการขอข้อมูล</param>
        public ClassCalendar GetClassCalendarByClassRoomId(string classRoomId)
        {
            var result = _mongoUtil.GetCollection<ClassCalendar>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.ClassRoomId == classRoomId)
                .ToEnumerable()
                .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// ขอข้อมูล Class calendar จากรหัส Class room
        /// </summary>
        /// <param name="classRoomId">รหัส Class room ที่ต้องการขอข้อมูล</param>
        public IEnumerable<ClassCalendar> GetClassCalendarByClassRoomId(IEnumerable<string> classRoomIds)
        {
            var qry = _mongoUtil.GetCollection<ClassCalendar>(TableName)
                .Find(it => !it.DeletedDate.HasValue && classRoomIds.Contains(it.ClassRoomId))
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล Class calendar
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        public void UpsertClassCalendar(ClassCalendar data)
        {
            var update = Builders<ClassCalendar>.Update
             .Set(it => it.BeginDate, data.BeginDate)
             .Set(it => it.ExpiredDate, data.ExpiredDate)
             .Set(it => it.CloseDate, data.CloseDate)
             .Set(it => it.ClassRoomId, data.ClassRoomId)
             .Set(it => it.CreatedDate, data.CreatedDate)
             .Set(it => it.DeletedDate, data.DeletedDate)
             .Set(it => it.LessonCalendars, data.LessonCalendars)
             .Set(it => it.Holidays, data.Holidays)
             .Set(it => it.ShiftDays, data.ShiftDays);

            var updateOption = new UpdateOptions { IsUpsert = true };
            _mongoUtil.GetCollection<ClassCalendar>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        /// <summary>
        /// ขอรายการ topic of the day ที่ต้องนำไปสร้าง notification
        /// </summary>
        /// <param name="currentTime">Current time</param>
        public IEnumerable<ClassCalendar> GetRequireNotifyTopicOfTheDay(DateTime currentTime)
        {
            var qry = _mongoUtil.GetCollection<ClassCalendar>(TableName)
                .Find(it => !it.DeletedDate.HasValue && !it.CloseDate.HasValue && it.LessonCalendars.SelectMany(lc => lc.TopicOfTheDays).Any(l => !l.SendTopicOfTheDayDate.HasValue))
                .ToEnumerable()
                .Where(c => c.LessonCalendars.SelectMany(lc => lc.TopicOfTheDays).Any(it => !it.SendTopicOfTheDayDate.HasValue && it.RequiredSendTopicOfTheDayDate.Date >= currentTime.Date));
            return qry;
        }

        #endregion IClassCalendarRepository members
    }
}
