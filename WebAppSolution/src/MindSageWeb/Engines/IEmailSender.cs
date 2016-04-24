using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Engines
{
    /// <summary>
    /// มาตรฐานในการส่งอีเมล์
    /// </summary>
    public interface IEmailSender
    {
        #region Methods

        /// <summary>
        /// ส่งอีเมล์
        /// </summary>
        /// <param name="senderEmail">อีเมล์ผู้ส่ง</param>
        /// <param name="receiverEmail">อีเมล์ผู้รับ</param>
        /// <param name="body">เนื้อหาของอีเมล์</param>
        Task Send(string senderEmail, string receiverEmail, string body);

        #endregion Methods
    }
}
