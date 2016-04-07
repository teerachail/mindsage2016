using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการติดต่อกับ contract
    /// </summary>
    public interface IContractRepository
    {
        #region Methods

        /// <summary>
        /// ขอข้อมูล contract จากรหัส teacher และ grade ที่ระบุ
        /// </summary>
        /// <param name="teacherCode">รหัส teacher ที่ต้องการค้นหา</param>
        /// <param name="grade">Grade</param>
        Task<Contract> GetContractsByTeacherCode(string teacherCode, string grade);

        #endregion Methods
    }
}
