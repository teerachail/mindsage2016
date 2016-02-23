using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ Like comment
    /// </summary>
    public class LikeCommentRepository : ILikeCommentRepository
    {
        #region ILikeCommentRepository members

        /// <summary>
        /// ขอข้อมูลการ like comment จากรหัส comment
        /// </summary>
        /// <param name="commentId">รหัส comment ที่ต้องการขอข้อมูล</param>
        public IEnumerable<LikeComment> GetLikeCommentByCommentId(string commentId)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล like lesson
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        public void UpsertLikeComment(LikeComment data)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        #endregion ILikeCommentRepository members
    }
}
