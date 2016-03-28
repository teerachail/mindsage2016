using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebManagementPortal.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Lesson catalog
    /// </summary>
    public interface ILessonCatalogRepository
    {
        #region Methods

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล lesson catalog
        /// </summary>
        /// <param name="lessonCatalogId">ข้อมูลที่ต้องการดำเนินการ</param>
        Task UpsertLessonCatalog(Models.LessonCatalog data);

        #endregion Methods
    }
}
