using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebManagementPortal.Repositories.Models;

namespace WebManagementPortal.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ contract
    /// </summary>
    public interface IContractRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล contract จากรหัสที่ระบุ
        /// </summary>
        /// <param name="contractIds">รหัส contract ที่ต้องการค้นหา</param>
        Task<IEnumerable<Contract>> GetContractsById(IEnumerable<string> contractIds);

        /// <summary>
        /// อัพเดทหรือเพิ่มข้อมูล contract
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการดำเนินการ</param>
        Task UpsertContract(Contract data);

        #endregion Methods
    }
}
