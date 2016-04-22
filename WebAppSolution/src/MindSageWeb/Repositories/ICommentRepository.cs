using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Comment
    /// </summary>
    public interface ICommentRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล comment จากรหัส lesson และผู้สร้าง comment
        /// </summary>
        /// <param name="classRoomId">รหัส class room ที่ต้องการขอข้อมูล</param>
        /// <param name="lessonId">รหัส lesson ที่ต้องการขอข้อมูล</param>
        /// <param name="creatorProfiles">รายชื่อผู้สร้าง comment ที่ต้องการ</param>
        /// <param name="getAllComments">ขอ comment ทั้งหมดหรือไม่</param>
        IEnumerable<Comment> GetCommentsByClassRoomAndLessonId(string classRoomId, string lessonId, IEnumerable<string> creatorProfiles, bool getAllComments);

        /// <summary>
        /// เพิ่มหรืออัพเดทข้อมูล comment
        /// </summary>
        /// <param name="data">ข้อมูล comment ที่จะดำเนินการ</param>
        void UpsertComment(Comment data);

        /// <summary>
        /// ขอข้อมูล comment จากรหัส comment
        /// </summary>
        /// <param name="commentId">รหัส comment ที่ต้องการขอข้อมูล</param>
        Comment GetCommentById(string commentId);

        /// <summary>
        /// ขอข้อมูล comment จากรหัส comment
        /// </summary>
        /// <param name="commentIds">รหัส comment ที่ต้องการขอข้อมูล</param>
        IEnumerable<Comment> GetCommentById(IEnumerable<string> commentIds);

        /// <summary>
        /// ขอข้อมูล comment จากรหัสผู้ใช้
        /// </summary>
        /// <param name="userprofileId">รหัสผู้ใช้ที่ต้องการค้นหา</param>
        /// <param name="classRoomId">รหัส Class room ที่ต้องการค้นหา</param>
        IEnumerable<Comment> GetCommentsByUserProfileId(string userprofileId, string classRoomId);

        /// <summary>
        /// ขอรายการ Like comment ที่ต้องนำไปสร้าง notification
        /// </summary>
        IEnumerable<Comment> GetRequireNotifyComments();

        /// <summary>
        /// ขอรายการ Like discussion ที่ต้องนำไปสร้าง notification
        /// </summary>
        IEnumerable<Comment> GetRequireNotifyDiscussions();

        #endregion Methods
    }
}
