using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MindSageWeb.Repositories.Models;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// ตัวติดต่อกับ Like lesson
    /// </summary>
    public class LikeLessonRepository : ILikeLessonRepository
    {
        #region ILikeLessonRepository members

        /// <summary>
        /// ขอรายการ Like lesson จากรหัส lesson
        /// </summary>
        /// <param name="id">รหัส lesson ที่ต้องการขอข้อมูล</param>
        public IEnumerable<LikeLesson> GetLikeLessonsByLessonId(string id)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        /// <summary>
        /// แก้ไขหรือเพิ่มข้อมูล Like lesson
        /// </summary>
        /// <param name="data">ข้อมูลที่จะทำการอัพเดทหรือเพิ่ม</param>
        public void UpsertLikeLesson(LikeLesson data)
        {
            // TODO: Not implemented
            throw new NotImplementedException();
        }

        #endregion ILikeLessonRepository members
    }
}
