﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ notification
    /// </summary>
    public class NotificationRepository : INotificationRepository
    {
        #region Fields

        // HACK: Table name
        private const string TableName = "test.au.mindsage.Notifications";

        #endregion Fields

        #region INotificationRepository members

        /// <summary>
        /// ขอข้อมูล notification จากรหัสผู้ใช้และ class room
        /// </summary>
        /// <param name="userprofileId">ชื่อผู้ใช้ที่ต้องการค้นหาข้อมูล</param>
        /// <param name="classRoomId">Class room id</param>
        public IEnumerable<Notification> GetNotificationByUserIdAndClassRoomId(string userprofileId, string classRoomId)
        {
            var qry = MongoAccess.MongoUtil.Instance.GetCollection<Notification>(TableName)
              .Find(it => !it.HideDate.HasValue && it.ClassRoomId == classRoomId && it.ToUserProfileId == userprofileId)
              .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล notification
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        public void Upsert(Notification data)
        {
            var update = Builders<Notification>.Update
               .Set(it => it.LastReadedDate, data.LastReadedDate)
               .Set(it => it.LastUpdateDate, data.LastUpdateDate)
               .Set(it => it.ToUserProfileId, data.ToUserProfileId)
               .Set(it => it.ClassRoomId, data.ClassRoomId)
               .Set(it => it.LessonId, data.LessonId)
               .Set(it => it.CreatedDate, data.CreatedDate)
               .Set(it => it.HideDate, data.HideDate)
               .Set(it => it.Message, data.Message)
               .Set(it => it.ByUserProfileId, data.ByUserProfileId)
               .Set(it => it.TotalLikes, data.TotalLikes)
               .Set(it => it.Tag, data.Tag);

            var updateOption = new UpdateOptions { IsUpsert = true };
            MongoAccess.MongoUtil.Instance.GetCollection<Notification>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        /// <summary>
        /// เพิ่มข้อมูล notification
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        public void Insert(IEnumerable<Notification> data)
        {
            if (!data.Any()) return;

            MongoAccess.MongoUtil.Instance.GetCollection<Notification>(TableName)
                .InsertMany(data);
        }

        #endregion INotificationRepository members
    }
}
