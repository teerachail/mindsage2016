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
    /// ตัวติดต่อกับ Comment
    /// </summary>
    public class CommentRepository : ICommentRepository
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
        public CommentRepository(MongoAccess.MongoUtil mongoUtil, IOptions<DatabaseTableOptions> option)
        {
            _mongoUtil = mongoUtil;
            TableName = option.Value.Comments;
        }

        #endregion Constructors

        #region ICommentRepository members

        /// <summary>
        /// ขอข้อมูล comment จากรหัส lesson และผู้สร้าง comment
        /// </summary>
        /// <param name="lessonId">รหัส lesson ที่ต้องการขอข้อมูล</param>
        /// <param name="creatorProfiles">รายชื่อผู้สร้าง comment ที่ต้องการ</param>
        public Comment GetCommentById(string commentId)
        {
            var result = _mongoUtil.GetCollection<Comment>(TableName)
                .Find(it => !it.DeletedDate.HasValue && it.id == commentId)
                .ToEnumerable()
                .FirstOrDefault();
            return result;
        }

        /// <summary>
        /// ขอข้อมูล comment จากรหัส comment
        /// </summary>
        /// <param name="commentIds">รหัส comment ที่ต้องการขอข้อมูล</param>
        public IEnumerable<Comment> GetCommentById(IEnumerable<string> commentIds)
        {
            var qry = _mongoUtil.GetCollection<Comment>(TableName)
                .Find(it => !it.DeletedDate.HasValue && commentIds.Contains(it.id))
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ขอข้อมูล comment จากรหัส lesson และผู้สร้าง comment
        /// </summary>
        /// <param name="classRoomId">รหัส class room ที่ต้องการขอข้อมูล</param>
        /// <param name="lessonId">รหัส lesson ที่ต้องการขอข้อมูล</param>
        /// <param name="creatorProfiles">รายชื่อผู้สร้าง comment ที่ต้องการ</param>
        public IEnumerable<Comment> GetCommentsByClassRoomAndLessonId(string classRoomId, string lessonId, IEnumerable<string> creatorProfiles)
        {
            var qry = _mongoUtil.GetCollection<Comment>(TableName)
               .Find(it => !it.DeletedDate.HasValue && it.ClassRoomId == classRoomId && it.LessonId == lessonId && creatorProfiles.Contains(it.CreatedByUserProfileId))
               .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// เพิ่มหรืออัพเดทข้อมูล comment
        /// </summary>
        /// <param name="data">ข้อมูล comment ที่จะดำเนินการ</param>
        public void UpsertComment(Comment data)
        {
            var update = Builders<Comment>.Update
             .Set(it => it.Description, data.Description)
             .Set(it => it.TotalLikes, data.TotalLikes)
             .Set(it => it.CreatorImageUrl, data.CreatorImageUrl)
             .Set(it => it.CreatorDisplayName, data.CreatorDisplayName)
             .Set(it => it.ClassRoomId, data.ClassRoomId)
             .Set(it => it.LessonId, data.LessonId)
             .Set(it => it.CreatedByUserProfileId, data.CreatedByUserProfileId)
             .Set(it => it.LastNotifyRequest, data.LastNotifyRequest)
             .Set(it => it.LastNotifyComplete, data.LastNotifyComplete)
             .Set(it => it.CreatedDate, data.CreatedDate)
             .Set(it => it.DeletedDate, data.DeletedDate)
             .Set(it => it.Discussions, data.Discussions);

            var updateOption = new UpdateOptions { IsUpsert = true };
            _mongoUtil.GetCollection<Comment>(TableName)
               .UpdateOne(it => it.id == data.id, update, updateOption);
        }

        /// <summary>
        /// ขอข้อมูล comment จากรหัสผู้ใช้
        /// </summary>
        /// <param name="userprofileId">รหัสผู้ใช้ที่ต้องการค้นหา</param>
        /// <param name="classRoomId">รหัส Class room ที่ต้องการค้นหา</param>
        public IEnumerable<Comment> GetCommentsByUserProfileId(string userprofileId, string classRoomId)
        {
            var qry = _mongoUtil.GetCollection<Comment>(TableName)
               .Find(it => !it.DeletedDate.HasValue && it.ClassRoomId == classRoomId && it.CreatedByUserProfileId == userprofileId)
               .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ขอรายการ Like comment ที่ต้องนำไปสร้าง notification
        /// </summary>
        public IEnumerable<Comment> GetRequireNotifyComments()
        {
            var qry = _mongoUtil.GetCollection<Comment>(TableName)
               .Find(it => !it.DeletedDate.HasValue && !it.LastNotifyComplete.HasValue)
               .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// ขอรายการ Like discussion ที่ต้องนำไปสร้าง notification
        /// </summary>
        public IEnumerable<Comment> GetRequireNotifyDiscussions()
        {
            var qry = _mongoUtil.GetCollection<Comment>(TableName)
               .Find(it => !it.DeletedDate.HasValue && it.Discussions.Any(d => !it.DeletedDate.HasValue && !it.LastNotifyComplete.HasValue))
               .ToEnumerable();
            return qry;
        }

        #endregion ICommentRepository members
    }
}
