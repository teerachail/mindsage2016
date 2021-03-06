﻿using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Like lesson
    /// </summary>
    public interface ILikeLessonRepository
    {
        #region Methods

        /// <summary>
        /// ขอรายการ Like lesson จากรหัส lesson
        /// </summary>
        /// <param name="lessonId">รหัส lesson ที่ต้องการขอข้อมูล</param>
        IEnumerable<LikeLesson> GetLikeLessonsByLessonId(string lessonId);

        /// <summary>
        /// แก้ไขหรือเพิ่มข้อมูล Like lesson
        /// </summary>
        /// <param name="data">ข้อมูลที่จะทำการอัพเดทหรือเพิ่ม</param>
        void UpsertLikeLesson(LikeLesson data);

        /// <summary>
        /// ขอรายการ Like lesson จากรหัสผู้ใช้และรหัส lesson
        /// </summary>
        /// <param name="userprofileId">รหัสผู้ใช้ที่ต้องการค้นหา</param>
        /// <param name="lessonId">รหัส lesson </param>
        IEnumerable<LikeLesson> GetLikeLessonsByUserProfileIdAndLesson(string userprofileId, string lessonId);

        /// <summary>
        /// ขอรายการ Like lesson ที่ต้องนำไปสร้าง notification
        /// </summary>
        IEnumerable<LikeLesson> GetRequireNotifyLikeLessons();

        #endregion Methods
    }
}
