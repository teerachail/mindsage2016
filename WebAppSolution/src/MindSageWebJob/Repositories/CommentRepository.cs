using ComputeWebJobsSDKQueue.Repositories.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputeWebJobsSDKQueue.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Comment
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        #region Fields

        private readonly string PrimaryConnectionString;
        private readonly string PrimaryDatabaseName;
        private readonly string TableName;

        #endregion Fields

        #region Constructors

        public CommentRepository()
        {
            PrimaryConnectionString = ConfigurationManager.ConnectionStrings["PrimaryDBConnectionString"].ConnectionString;
            PrimaryDatabaseName = ConfigurationManager.AppSettings["PrimaryDBName"];
            TableName = ConfigurationManager.AppSettings["CommentTableName"];
        }

        #endregion Constructors

        #region ICommentRepository members

        /// <summary>
        /// ขอรายการ comment จากรหัสผู้ใช้ที่เกี่ยวข้อง
        /// </summary>
        /// <param name="userprofileId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Comment>> GetCommentByRelatedUserProfileId(string userprofileId)
        {
            var mongoUtil = new MongoAccess.MongoUtil(PrimaryConnectionString, PrimaryDatabaseName);
            var qry = (await mongoUtil.GetCollection<Comment>(TableName)
                .FindAsync(it => !it.DeletedDate.HasValue && (it.CreatedByUserProfileId == userprofileId || it.Discussions.Any(d => !d.DeletedDate.HasValue && d.CreatedByUserProfileId == userprofileId))))
                .ToEnumerable();
            return qry;
        }

        /// <summary>
        /// อัพเดท comment
        /// </summary>
        /// <param name="data">Comment ที่จะทำการอัพเดท</param>
        public async Task UpdateComment(Comment data)
        {
            var mongoUtil = new MongoAccess.MongoUtil(PrimaryConnectionString, PrimaryDatabaseName);
            var filter = Builders<Comment>.Filter.Eq(it => it.id, data.id);
            var update = Builders<Comment>.Update
                .Set(it => it.CreatorDisplayName, data.CreatorDisplayName)
                .Set(it => it.CreatorImageUrl, data.CreatorImageUrl)
                .Set(it => it.Discussions, data.Discussions);
            await mongoUtil.GetCollection<Comment>(TableName).UpdateOneAsync(filter, update);
        }

        #endregion ICommentRepository members
    }
}
