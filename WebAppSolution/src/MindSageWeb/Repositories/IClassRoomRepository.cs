using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ Class room
    /// </summary>
    public interface IClassRoomRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล Class room จากรหัส
        /// </summary>
        /// <param name="classRoomId">รหัส Class room ที่ต้องการขอ</param>
        ClassRoom GetClassRoomById(string classRoomId);

        /// <summary>
        /// อัพเดทข้อมูล Class room
        /// </summary>
        /// <param name="data">ข้อมูลที่จะทำการอัพเดท</param>
        void UpsertClassRoom(ClassRoom data);

        /// <summary>
        /// ขอข้อมูล public class room จากรหัส course catalog
        /// </summary>
        /// <param name="courseCatalogId">รหัส course catalog ที่ต้องการขอ</param>
        ClassRoom GetPublicClassRoomByCourseCatalogId(string courseCatalogId);

        #endregion Methods
    }
}
