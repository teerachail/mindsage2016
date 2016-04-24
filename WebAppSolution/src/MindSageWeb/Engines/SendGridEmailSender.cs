using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindSageWeb.Engines
{
    /// <summary>
    /// ตัวส่งอีเมล์
    /// </summary>
    public class SendGridEmailSender : IEmailSender
    {
        #region IEmailSender members

        /// <summary>
        /// ส่งอีเมล์
        /// </summary>
        /// <param name="senderEmail">อีเมล์ผู้ส่ง</param>
        /// <param name="receiverEmail">อีเมล์ผู้รับ</param>
        /// <param name="body">เนื้อหาของอีเมล์</param>
        public async Task Send(string senderEmail, string receiverEmail, string body)
        {
            // TODO: Not implemented email sender
            throw new NotImplementedException();
        }

        #endregion IEmailSender members
    }
}
