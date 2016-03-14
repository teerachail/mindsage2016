using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories.Models
{
    /// <summary>
    /// ข้อมูลการ like lesson
    /// </summary>
    public class LikeLessonRequest
    {
        #region Properties

        /// <summary>
        /// รหัส class room
        /// </summary>
        public string ClassRoomId { get; set; }

        /// <summary>
        /// รหัส lesson
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// ชื่อบัญชีผู้ใช้ที่ดำเนินการ
        /// </summary>
        public string UserProfileId { get; set; }

        #endregion Properties
    }
}
