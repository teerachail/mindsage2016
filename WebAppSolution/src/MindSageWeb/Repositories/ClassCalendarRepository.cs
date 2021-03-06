﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;
using Microsoft.Extensions.OptionsModel;

namespace MindSageWeb.Repositories
{
    public class ClassCalendarRepository : IClassCalendarRepository
    {
        #region Fields

        private readonly string TableName;
        private MongoAccess.MongoUtil _mongoUtil;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="mongoUtil">Mongo access utility</param>
        public ClassCalendarRepository(MongoAccess.MongoUtil mongoUtil, IOptions<DatabaseTableOptions> option)
        {
            _mongoUtil = mongoUtil;
            TableName = option.Value.ClassCalendars;
        }

        #endregion Constructors

        #region IClassCalendarRepository members

        /// <summary>
        /// ขอข้อมูล Class calendar จากรหัส Class calendar
        /// </summary>
        /// <param name="classCalendarId">รหัส Class calendar ที่ต้องการขอข้อมูล</param>
        public async Task<ClassCalendar> GetClassCalendarById(string classCalendarId)
        {
            var result = await _mongoUtil.GetCollection<ClassCalendar>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.id == classCalendarId)
                .FirstOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// ขอข้อมูล Class calendar จากรหัส Class room
        /// </summary>
        /// <param name="classRoomId">รหัส Class room ที่ต้องการขอข้อมูล</param>
        public ClassCalendar GetClassCalendarByClassRoomId(string classRoomId)
        {
            var result = _mongoUtil.GetCollection<ClassCalendar>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.ClassRoomId == classRoomId)
                .ToEnumerable()
                .OrderBy(it => it.CreatedDate)
                .LastOrDefault();
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
        /// สร้าง Class calendar ใหม่
        /// </summary>
        /// <param name="data">ข้อมูล Class calendar ที่ต้องการสร้าง</param>
        public async Task CreateNewClassCalendar(ClassCalendar data)
        {
            await _mongoUtil.GetCollection<ClassCalendar>(TableName).InsertOneAsync(data);
        }

        /// <summary>
        /// ขอรายการ topic of the day ที่ต้องนำไปสร้าง notification
        /// </summary>
        /// <param name="currentTime">Current time</param>
        public IEnumerable<ClassCalendar> GetRequireNotifyTopicOfTheDay(DateTime currentTime)
        {
            var qry = _mongoUtil.GetCollection<ClassCalendar>(TableName)
                .Find(it => !it.DeletedDate.HasValue && !it.CloseDate.HasValue)
                .ToEnumerable()
                .Where(c => c.LessonCalendars.SelectMany(lc => lc.TopicOfTheDays).Any(it => !it.SendTopicOfTheDayDate.HasValue && it.RequiredSendTopicOfTheDayDate.HasValue && it.RequiredSendTopicOfTheDayDate.Value.Date.ToUniversalTime() >= currentTime.Date));
            return qry;
        }

        #endregion IClassCalendarRepository members
    }
}
