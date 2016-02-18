using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Like discussion
    /// </summary>
    public interface ILikeDiscussionRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล like discussion จากรหัส discussion
        /// </summary>
        /// <param name="discussionId">รหัส discussion ที่ต้องการขอข้อมูล</param>
        IEnumerable<LikeDiscussion> GetLikeDiscussionByDiscusionId(string discussionId);

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล like discussion
        /// </summary>
        /// <param name="item">ข้อมูล like discussion ที่จะดำเนินการ</param>
        void UpsertLikeDiscussion(LikeDiscussion item);

        #endregion Methods
    }
}
