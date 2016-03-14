using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;
using MongoDB.Driver;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ Like discussion
    /// </summary>
    public class LikeDiscussionRepository : ILikeDiscussionRepository
    {
        #region Fields

        // HACK: Table name
        private const string TableName = "test.au.mindsage.LikeDiscussions";

        #endregion Fields

        #region ILikeDiscussionRepository members

        /// <summary>
        /// ขอข้อมูล like discussion จากรหัส discussion
        /// </summary>
        /// <param name="discussionId">รหัส discussion ที่ต้องการขอข้อมูล</param>
        public IEnumerable<LikeDiscussion> GetLikeDiscussionByDiscusionId(string discussionId)
        {
            var qry = MongoAccess.MongoUtil.Instance.GetCollection<LikeDiscussion>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.DiscussionId == discussionId)
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล like discussion
        /// </summary>
        /// <param name="item">ข้อมูล like discussion ที่จะดำเนินการ</param>
        public void UpsertLikeDiscussion(LikeDiscussion data)
        {
            var update = Builders<LikeDiscussion>.Update
                .Set(it => it.ClassRoomId, data.ClassRoomId)
                .Set(it => it.CommentId, data.CommentId)
                .Set(it => it.DiscussionId, data.DiscussionId)
                .Set(it => it.LessonId, data.LessonId)
                .Set(it => it.LikedByUserProfileId, data.LikedByUserProfileId)
                .Set(it => it.LastNotifyRequest, data.LastNotifyRequest)
                .Set(it => it.LastNotifyComplete, data.LastNotifyComplete)
                .Set(it => it.CreatedDate, data.CreatedDate)
                .Set(it => it.DeletedDate, data.DeletedDate);

            var updateOption = new UpdateOptions { IsUpsert = true };
            MongoAccess.MongoUtil.Instance.GetCollection<LikeDiscussion>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        /// <summary>
        /// ขอรายการ Like discussion จากรหัสผู้ใช้และรหัส lesson
        /// </summary>
        /// <param name="userprofileId">รหัสผู้ใช้ที่ต้องการค้นหา</param>
        /// <param name="lessonId">รหัส lesson </param>
        public IEnumerable<LikeDiscussion> GetLikeDiscussionsByUserProfileIdAndLesson(string userprofileId, string lessonId)
        {
            var qry = MongoAccess.MongoUtil.Instance.GetCollection<LikeDiscussion>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.LessonId == lessonId && it.LikedByUserProfileId == userprofileId)
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ขอรายการ Like discussion จากรหัสผู้ใช้และรหัส class room
        /// </summary>
        /// <param name="userprofileId">รหัสผู้ใช้ที่ต้องการค้นหา</param>
        /// <param name="classRoomId">รหัส class room id</param>
        public IEnumerable<LikeDiscussion> GetLikeDiscussionsByUserProfileIdAndClassRoomId(string userprofileId, string classRoomId)
        {
            var qry = MongoAccess.MongoUtil.Instance.GetCollection<LikeDiscussion>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.ClassRoomId == classRoomId && it.LikedByUserProfileId == userprofileId)
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ขอรายการ Like discussion ที่ต้องนำไปสร้าง notification
        /// </summary>
        public IEnumerable<LikeDiscussion> GetRequireNotifyLikeDiscussions()
        {
            var qry = MongoAccess.MongoUtil.Instance.GetCollection<LikeDiscussion>(TableName)
                .Find(it => !it.DeletedDate.HasValue && !it.LastNotifyComplete.HasValue)
                .ToEnumerable();
            return qry;
        }

        #endregion ILikeDiscussionRepository members
    }
}
