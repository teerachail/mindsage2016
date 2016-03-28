using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebManagementPortal.Repositories.Models;

namespace WebManagementPortal.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Class room
    /// </summary>
    public interface IClassRoomRepository
    {
        #region Methods

        /// <summary>
        /// อัพเดทข้อมูล Class room
        /// </summary>
        /// <param name="data">ข้อมูลที่จะทำการอัพเดท</param>
        Task UpsertClassRoom(ClassRoom data);

        /// <summary>
        /// ขอข้อมูล public class room จากรหัส course catalog
        /// </summary>
        /// <param name="courseCatalogId">รหัส course catalog ที่ต้องการขอ</param>
        Task<IEnumerable<ClassRoom>> GetPublicClassRoomByCourseCatalogId(IEnumerable<string> courseCatalogIds);

        #endregion Methods
    }
}
