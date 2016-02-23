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

        #endregion Fields

        #region ILikeCommentRepository members

        /// <summary>
        /// ขอข้อมูลการ like comment จากรหัส comment
        /// </summary>
        /// <param name="commentId">รหัส comment ที่ต้องการขอข้อมูล</param>
        public IEnumerable<LikeComment> GetLikeCommentByCommentId(string commentId)
        {
            var qry = MongoAccess.MongoUtil.Instance.GetCollection<LikeComment>(TableName)
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
              .Set(it => it.CommentId, data.CommentId)
              .Set(it => it.LessonId, data.LessonId)
              .Set(it => it.LikedByUserProfileId, data.LikedByUserProfileId)
              .Set(it => it.LastNotifyRequest, data.LastNotifyRequest)
              .Set(it => it.LastNotifyComplete, data.LastNotifyComplete)
              .Set(it => it.CreatedDate, data.CreatedDate)
              .Set(it => it.DeletedDate, data.DeletedDate);

            var updateOption = new UpdateOptions { IsUpsert = true };
            MongoAccess.MongoUtil.Instance.GetCollection<LikeComment>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        #endregion ILikeCommentRepository members
    }
}
