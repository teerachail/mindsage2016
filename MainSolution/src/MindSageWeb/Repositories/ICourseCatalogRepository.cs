using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ course catalog
    /// </summary>
    public interface ICourseCatalogRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล course catalog จากรหัส
        /// </summary>
        /// <param name="courseCatalogId">รหัส course catalog ที่ต้องการข้อมูล</param>
        CourseCatalog GetCourseCatalogById(string courseCatalogId);

        /// <summary>
        /// ขอข้อมูล course ทั้งหมดที่สามารถเลือกได้
        /// </summary>
        IEnumerable<CourseCatalog> GetAvailableCourses();

        /// <summary>
        /// ขอข้อมูล course ที่อยู่ในกลุ่มเดียวกัน
        /// </summary>
        /// <param name="groupName">ชื่อกลุ่มที่ต้องการค้นหา</param>
        IEnumerable<CourseCatalog> GetRelatedCoursesByGroupName(string groupName);

        #endregion Methods
    }
}
