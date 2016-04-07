using MindSageWeb.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Repositories
{
    /// <summary>
    /// มาตรฐานในการเชื่อมต่อกับ payment
    /// </summary>
    public interface IPaymentRepository
    {
        #region Methods

        /// <summary>
        /// เพิ่ม payment ใหม่
        /// </summary>
        /// <param name="data">ข้อมูลที่ต้องการเพิ่ม</param>
        Task CreateNewPayment(Payment data);

        /// <summary>
        /// ขอข้อมูล payment จากรหัส
        /// </summary>
        /// <param name="id">รหัส payment ที่ต้องการขอข้อมูล</param>
        Task<Payment> GetPaymentById(string id);

        #endregion Methods
    }
}
