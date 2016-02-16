using MindsageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Like lesson
    /// </summary>
    public interface ILikeLessonRepository
    {
        #region Methods

        /// <summary>
        /// ขอรายการ Like lesson จากรหัส lesson
        /// </summary>
        /// <param name="id">รหัส lesson ที่ต้องการขอข้อมูล</param>
        IEnumerable<LikeLesson> GetLikeLessonsByLessonId(string id);

        /// <summary>
        /// แก้ไขหรือเพิ่มข้อมูล Like lesson
        /// </summary>
        /// <param name="data">ข้อมูลที่จะทำการอัพเดทหรือเพิ่ม</param>
        void UpsertLikeLesson(LikeLesson data);

        #endregion Methods
    }
}
