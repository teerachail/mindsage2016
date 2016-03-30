using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ Like comment
    /// </summary>
    public class LikeCommentRepository : ILikeCommentRepository
    {
        #region Fields

        // HACK: Table name
        private const string TableName = "test.au.mindsage.LikeComments";
        private MongoAccess.MongoUtil _mongoUtil;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize repository
        /// </summary>
        /// <param name="mongoUtil">Mongo access utility</param>
        public LikeCommentRepository(MongoAccess.MongoUtil mongoUtil)
        {
            _mongoUtil = mongoUtil;
        }

        #endregion Constructors

        #region ILikeCommentRepository members

        /// <summary>
        /// ขอข้อมูลการ like comment จากรหัส comment
        /// </summary>
        /// <param name="commentId">รหัส comment ที่ต้องการขอข้อมูล</param>
        public IEnumerable<LikeComment> GetLikeCommentByCommentId(string commentId)
        {
            var qry = _mongoUtil.GetCollection<LikeComment>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.CommentId == commentId)
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล like lesson
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        public void UpsertLikeComment(LikeComment data)
        {
            var update = Builders<LikeComment>.Update
                .Set(it => it.ClassRoomId, data.ClassRoomId)
                .Set(it => it.CommentId, data.CommentId)
                .Set(it => it.LessonId, data.LessonId)
                .Set(it => it.LikedByUserProfileId, data.LikedByUserProfileId)
                .Set(it => it.LastNotifyRequest, data.LastNotifyRequest)
                .Set(it => it.LastNotifyComplete, data.LastNotifyComplete)
                .Set(it => it.CreatedDate, data.CreatedDate)
                .Set(it => it.DeletedDate, data.DeletedDate);

            var updateOption = new UpdateOptions { IsUpsert = true };
            _mongoUtil.GetCollection<LikeComment>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        /// <summary>
        /// ขอรายการ Like comment จากรหัสผู้ใช้และรหัส lesson
        /// </summary>
        /// <param name="userprofileId">รหัสผู้ใช้ที่ต้องการค้นหา</param>
        /// <param name="lessonId">รหัส lesson </param>
        public IEnumerable<LikeComment> GetLikeCommentsByUserProfileIdAndLesson(string userprofileId, string lessonId)
        {
            var qry = _mongoUtil.GetCollection<LikeComment>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.LessonId == lessonId && it.LikedByUserProfileId == userprofileId)
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ขอรายการ Like comment จากรหัสผู้ใช้และรหัส class room
        /// </summary>
        /// <param name="userprofileId">รหัสผู้ใช้ที่ต้องการค้นหา</param>
        /// <param name="classRoomId">รหัส class room</param>
        public IEnumerable<LikeComment> GetLikeCommentsByUserProfileIdAndClassRoomId(string userprofileId, string classRoomId)
        {
            var qry = _mongoUtil.GetCollection<LikeComment>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.ClassRoomId == classRoomId && it.LikedByUserProfileId == userprofileId)
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ขอรายการ Like comment ที่ต้องนำไปสร้าง notification
        /// </summary>
        public IEnumerable<LikeComment> GetRequireNotifyLikeComments()
        {
            var qry = _mongoUtil.GetCollection<LikeComment>(TableName)
                .Find(it => !it.DeletedDate.HasValue && !it.LastNotifyComplete.HasValue)
                .ToEnumerable();
            return qry;
        }

        #endregion ILikeCommentRepository members
    }
}
