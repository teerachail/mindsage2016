﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ Like lesson
    /// </summary>
    public class LikeLessonRepository : ILikeLessonRepository
    {
        #region Fields

        // HACK: Table name
        private const string TableName = "test.au.mindsage.LikeLessons";

        #endregion Fields

        #region ILikeLessonRepository members

        /// <summary>
        /// ขอรายการ Like lesson จากรหัส lesson
        /// </summary>
        /// <param name="lessonId">รหัส lesson ที่ต้องการขอข้อมูล</param>
        public IEnumerable<LikeLesson> GetLikeLessonsByLessonId(string lessonId)
        {
            var qry = MongoAccess.MongoUtil.Instance.GetCollection<LikeLesson>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.LessonId == lessonId)
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// แก้ไขหรือเพิ่มข้อมูล Like lesson
        /// </summary>
        /// <param name="data">ข้อมูลที่จะทำการอัพเดทหรือเพิ่ม</param>
        public void UpsertLikeLesson(LikeLesson data)
        {
            var update = Builders<LikeLesson>.Update
               .Set(it => it.ClassRoomId, data.ClassRoomId)
               .Set(it => it.LessonId, data.LessonId)
               .Set(it => it.LikedByUserProfileId, data.LikedByUserProfileId)
               .Set(it => it.LastNotifyRequest, data.LastNotifyRequest)
               .Set(it => it.LastNotifyComplete, data.LastNotifyComplete)
               .Set(it => it.CreatedDate, data.CreatedDate)
               .Set(it => it.DeletedDate, data.DeletedDate);

            var updateOption = new UpdateOptions { IsUpsert = true };
            MongoAccess.MongoUtil.Instance.GetCollection<LikeLesson>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        #endregion ILikeLessonRepository members
    }
}
