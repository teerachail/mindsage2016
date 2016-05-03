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
    public interface ICommentRepository
    {
        #region Methods

        /// <summary>
        /// ขอรายการ comment จากรหัสผู้ใช้ที่เกี่ยวข้อง
        /// </summary>
        /// <param name="userprofileId"></param>
        /// <returns></returns>
        Task<IEnumerable<Comment>> GetCommentByRelatedUserProfileId(string userprofileId);

        /// <summary>
        /// อัพเดท comment
        /// </summary>
        /// <param name="data">Comment ที่จะทำการอัพเดท</param>
        Task UpdateComment(Comment data);

        #endregion Methods
    }
}
