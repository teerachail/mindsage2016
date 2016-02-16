using MindsageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Lesson catalog
    /// </summary>
    public interface ILessonCatalogRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล Lesson catalog จากรหัส
        /// </summary>
        /// <param name="lessonCatalogId">รหัส Lesson catalog ที่ต้องการ</param>
        LessonCatalog GetLessonCatalogById(string lessonCatalogId);

        #endregion Methods
    }
}
