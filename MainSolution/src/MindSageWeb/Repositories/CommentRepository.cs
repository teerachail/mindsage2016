using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ Comment
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        #region ICommentRepository members

        /// <summary>
        /// ขอข้อมูล comment จากรหัส lesson และผู้สร้าง comment
        /// </summary>
        /// <param name="lessonId">รหัส lesson ที่ต้องการขอข้อมูล</param>
        /// <param name="creatorProfiles">รายชื่อผู้สร้าง comment ที่ต้องการ</param>
        public Comment GetCommentById(string commentId)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        /// <summary>
        /// เพิ่มหรืออัพเดทข้อมูล comment
        /// </summary>
        /// <param name="data">ข้อมูล comment ที่จะดำเนินการ</param>
        public IEnumerable<Comment> GetCommentsByLessonId(string lessonId, IEnumerable<string> creatorProfiles)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        /// <summary>
        /// ขอข้อมูล comment จากรหัส comment
        /// </summary>
        /// <param name="commentId">รหัส comment ที่ต้องการขอข้อมูล</param>
        public void UpsertComment(Comment data)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        #endregion ICommentRepository members
    }
}
