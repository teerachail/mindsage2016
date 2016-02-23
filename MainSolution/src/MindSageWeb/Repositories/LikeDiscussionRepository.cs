using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ Like discussion
    /// </summary>
    public class LikeDiscussionRepository : ILikeDiscussionRepository
    {
        #region ILikeDiscussionRepository members

        /// <summary>
        /// ขอข้อมูล like discussion จากรหัส discussion
        /// </summary>
        /// <param name="discussionId">รหัส discussion ที่ต้องการขอข้อมูล</param>
        public IEnumerable<LikeDiscussion> GetLikeDiscussionByDiscusionId(string discussionId)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล like discussion
        /// </summary>
        /// <param name="item">ข้อมูล like discussion ที่จะดำเนินการ</param>
        public void UpsertLikeDiscussion(LikeDiscussion item)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        #endregion ILikeDiscussionRepository members
    }
}
