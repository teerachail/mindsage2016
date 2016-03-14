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
        /// <param name="data">ข้อมูล like discussion ที่จะดำเนินการ</param>
        void UpsertLikeDiscussion(LikeDiscussion data);

        /// <summary>
        /// ขอรายการ Like discussion จากรหัสผู้ใช้และรหัส lesson
        /// </summary>
        /// <param name="userprofileId">รหัสผู้ใช้ที่ต้องการค้นหา</param>
        /// <param name="lessonId">รหัส lesson </param>
        IEnumerable<LikeDiscussion> GetLikeDiscussionsByUserProfileIdAndLesson(string userprofileId, string lessonId);

        /// <summary>
        /// ขอรายการ Like discussion จากรหัสผู้ใช้และรหัส class room
        /// </summary>
        /// <param name="userprofileId">รหัสผู้ใช้ที่ต้องการค้นหา</param>
        /// <param name="classRoomId">รหัส class room id</param>
        IEnumerable<LikeDiscussion> GetLikeDiscussionsByUserProfileIdAndClassRoomId(string userprofileId, string classRoomId);

        /// <summary>
        /// ขอรายการ Like discussion ที่ต้องนำไปสร้าง notification
        /// </summary>
        IEnumerable<LikeDiscussion> GetRequireNotifyLikeDiscussions();

        #endregion Methods
    }
}
