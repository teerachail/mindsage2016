using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Like comment
    /// </summary>
    public interface ILikeCommentRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูลการ like comment จากรหัส comment
        /// </summary>
        /// <param name="commentId">รหัส comment ที่ต้องการขอข้อมูล</param>
        IEnumerable<LikeComment> GetLikeCommentByCommentId(string commentId);

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล like lesson
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        void UpsertLikeComment(LikeComment data);

        /// <summary>
        /// ขอรายการ Like comment จากรหัสผู้ใช้และรหัส lesson
        /// </summary>
        /// <param name="userprofileId">รหัสผู้ใช้ที่ต้องการค้นหา</param>
        /// <param name="lessonId">รหัส lesson </param>
        IEnumerable<LikeComment> GetLikeCommentsByUserProfileIdAndLesson(string userprofileId, string lessonId);

        #endregion Methods
    }
}
