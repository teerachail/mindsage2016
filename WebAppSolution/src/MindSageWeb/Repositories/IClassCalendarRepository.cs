﻿using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Class calendar
    /// </summary>
    public interface IClassCalendarRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล Class calendar จากรหัส Class calendar
        /// </summary>
        /// <param name="classCalendarId">รหัส Class calendar ที่ต้องการขอข้อมูล</param>
        Task<ClassCalendar> GetClassCalendarById(string classCalendarId);

        /// <summary>
        /// ขอข้อมูล Class calendar จากรหัส Class room
        /// </summary>
        /// <param name="classRoomId">รหัส Class room ที่ต้องการขอข้อมูล</param>
        ClassCalendar GetClassCalendarByClassRoomId(string classRoomId);

        /// <summary>
        /// ขอข้อมูล Class calendar จากรหัส Class room
        /// </summary>
        /// <param name="classRoomId">รหัส Class room ที่ต้องการขอข้อมูล</param>
        IEnumerable<ClassCalendar> GetClassCalendarByClassRoomId(IEnumerable<string> classRoomIds);

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล Class calendar
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        void UpsertClassCalendar(ClassCalendar data);

        /// <summary>
        /// สร้าง Class calendar ใหม่
        /// </summary>
        /// <param name="data">ข้อมูล Class calendar ที่ต้องการสร้าง</param>
        Task CreateNewClassCalendar(ClassCalendar data);

        /// <summary>
        /// ขอรายการ topic of the day ที่ต้องนำไปสร้าง notification
        /// </summary>
        /// <param name="currentTime">Current time</param>
        IEnumerable<ClassCalendar> GetRequireNotifyTopicOfTheDay(DateTime currentTime);

        #endregion Methods
    }
}
