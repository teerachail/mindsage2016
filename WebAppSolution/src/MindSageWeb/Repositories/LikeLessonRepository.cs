using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;
using Microsoft.Extensions.OptionsModel;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ Like lesson
    /// </summary>
    public class LikeLessonRepository : ILikeLessonRepository
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
        public LikeLessonRepository(MongoAccess.MongoUtil mongoUtil, IOptions<DatabaseTableOptions> option)
        {
            _mongoUtil = mongoUtil;
            TableName = option.Value.LikeLessons;
        }

        #endregion Constructors

        #region ILikeLessonRepository members

        /// <summary>
        /// ขอรายการ Like lesson จากรหัส lesson
        /// </summary>
        /// <param name="lessonId">รหัส lesson ที่ต้องการขอข้อมูล</param>
        public IEnumerable<LikeLesson> GetLikeLessonsByLessonId(string lessonId)
        {
            var qry = _mongoUtil.GetCollection<LikeLesson>(TableName)
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
            _mongoUtil.GetCollection<LikeLesson>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        /// <summary>
        /// ขอรายการ Like lesson จากรหัสผู้ใช้และรหัส lesson
        /// </summary>
        /// <param name="userprofileId">รหัสผู้ใช้ที่ต้องการค้นหา</param>
        /// <param name="lessonId">รหัส lesson </param>
        public IEnumerable<LikeLesson> GetLikeLessonsByUserProfileIdAndLesson(string userprofileId, string lessonId)
        {
            var qry = _mongoUtil.GetCollection<LikeLesson>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.LessonId == lessonId && it.LikedByUserProfileId == userprofileId)
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ขอรายการ Like lesson ที่ต้องนำไปสร้าง notification
        /// </summary>
        public IEnumerable<LikeLesson> GetRequireNotifyLikeLessons()
        {
            var qry = _mongoUtil.GetCollection<LikeLesson>(TableName)
               .Find(it => !it.DeletedDate.HasValue && !it.LastNotifyComplete.HasValue)
               .ToEnumerable();
            return qry;
        }

        #endregion ILikeLessonRepository members
    }
}
