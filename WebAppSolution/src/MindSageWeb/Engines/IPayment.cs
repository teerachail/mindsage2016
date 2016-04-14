using MindSageWeb.Engines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Engines
{
    /// <summary>
    /// มาตรฐานในการจ่ายเงิน
    /// </summary>
    public interface IPayment
    {
        #region Methods

        /// <summary>
        /// ตัดบัตรเครดิต
        /// </summary>
        /// <param name="payment">ข้อมูลบัตรเครดิตที่ต้องการดำเนินการ</param>
        PaymentResult ChargeCreditCard(PaymentInformation payment);

        #endregion Methods
    }
}
