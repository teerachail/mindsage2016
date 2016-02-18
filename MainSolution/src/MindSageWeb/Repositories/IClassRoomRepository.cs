using MindsageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindsageWeb.Repositories
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
        /// <param name="id">รหัส Class room ที่ต้องการขอ</param>
        ClassRoom GetClassRoomById(string id);

        /// <summary>
        /// อัพเดทข้อมูล Class room
        /// </summary>
        /// <param name="data">ข้อมูลที่จะทำการอัพเดท</param>
        void UpdateClassRoom(ClassRoom data);

        #endregion Methods
    }
}
