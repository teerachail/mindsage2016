using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
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

        /// <summary>
        /// ขอข้อมูล Lesson catalog จากรหัส
        /// </summary>
        /// <param name="lessonCatalogId">รหัส Lesson catalog ที่ต้องการ</param>
        IEnumerable<LessonCatalog> GetLessonCatalogById(IEnumerable<string> lessonCatalogIds);

        /// <summary>
        /// ขอข้อมูล Lesson catalog จากรหัส Course catalog
        /// </summary>
        /// <param name="courseCatalogId">รหัส Course catalog ที่ต้องการ</param>
        IEnumerable<LessonCatalog> GetLessonCatalogByCourseCatalogId(string courseCatalogId);

        #endregion Methods
    }
}
