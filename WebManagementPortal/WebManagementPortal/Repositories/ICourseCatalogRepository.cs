using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebManagementPortal.Repositories.Models;

namespace WebManagementPortal.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ course catalog
    /// </summary>
    public interface ICourseCatalogRepository
    {
        #region Methods

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล course catalog
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        Task UpsertCourseCatalog(CourseCatalog data);

        #endregion Methods
    }
}
