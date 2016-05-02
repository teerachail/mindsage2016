using ComputeWebJobsSDKQueue.Repositories.Models;
using System;
using System.Collections.Generic;
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
        #region ICommentRepository members

        /// <summary>
        /// ขอรายการ comment จากรหัสผู้ใช้ที่เกี่ยวข้อง
        /// </summary>
        /// <param name="userprofileId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetCommentByRelatedUserProfileId(string userprofileId)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        #endregion ICommentRepository members
    }
}
