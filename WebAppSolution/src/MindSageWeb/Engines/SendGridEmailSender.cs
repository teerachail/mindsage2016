using Microsoft.Extensions.OptionsModel;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MindSageWeb.Engines
{
    /// <summary>
    /// ตัวส่งอีเมล์
    /// </summary>
    public class SendGridEmailSender : IEmailSender
    {
        #region Fields

        private readonly AppConfigOptions _appConfig;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="appConfig">App configruation</param>
        public SendGridEmailSender(IOptions<AppConfigOptions> appConfig)
        {
            _appConfig = appConfig.Value;
        }

        #endregion Constructors

        #region IEmailSender members

        /// <summary>
        /// ส่งอีเมล์ในการขอตั้งรหัสผ่านใหม่
        /// </summary>
        /// <param name="receiverEmail">อีเมล์ผู้รับ</param>
        public async Task SendResetPasswordEmail(string receiverEmail)
        {
            var resetPasswordURL = "www.google.com";
            var emailBody = $"<center><table cellspacing='0' cellpadding='0' width='480px' style='border-collapse:collapse;max-width:480px;width:auto'><tbody><tr><td align='left' style='background-color:#ffffff;border-color:#c1c2c4;border-style:solid;display:block;border:none'><table cellspacing='0' cellpadding='0' width='100%' style='border-collapse:collapse'><tbody><tr><td><table cellspacing='0' cellpadding='0' width='100%' style='border-collapse:collapse'><tbody><tr><td height='24px' style='padding:20px 24px 0px 24px'><span style='font-size:22px;line-height:32px;font-weight:500;font-family:Helvetica Neue,Helvetica,Arial,sans-serif'>Hi  { receiverEmail },</span></td></tr></tbody></table><table cellspacing='0' cellpadding='0' width='100%' style='border-collapse:collapse'><tbody><tr><td height='20px' style='padding:16px 24px 0px 24px'><span style='font-size:16px;line-height:20px;font-family:Helvetica Neue,Helvetica,Arial,sans-serif'>We got a request to reset your MindSage password.</span></td></tr></tbody></table><table cellspacing='0' cellpadding='0' width='100%' style='border-collapse:collapse'><tbody><tr><td height='17px' style='padding:30px 0px 30px 0px'><center><a style='color:#2b5a83;text-decoration:none;border-radius:3px;border:1px solid #2b5a83;padding:10px 19px 12px 19px;font-size:17px;font-weight:500;white-space:nowrap;border-collapse:collapse;display:inline-block;font-family:Helvetica Neue,Helvetica,Arial,sans-serif' href='{ resetPasswordURL }' target='_blank'>Reset Password</a></center></td></tr></tbody></table><table cellspacing='0' cellpadding='0' width='100%' style='border-collapse:collapse'><tbody><tr><td height='20px' style='padding:0px 24px 0px 24px'><span style='font-size:16px;line-height:20px;font-family:Helvetica Neue,Helvetica,Arial,sans-serif'>If you ignore this message, your password won't be changed.</span></td></tr></tbody></table><table cellspacing='0' cellpadding='0' width='100%' style='border-collapse:collapse'><tbody><tr></tr></tbody></table></td></tr></tbody></table></td></tr></tbody></table></center>";
            await Send("miolynet@perfenterprise.com", receiverEmail, "Reset Your Password", emailBody); // HACK: Sender email
        }

        /// <summary>
        /// ส่งอีเมล์
        /// </summary>
        /// <param name="senderEmail">อีเมล์ผู้ส่ง</param>
        /// <param name="receiverEmail">อีเมล์ผู้รับ</param>
        /// <param name="subject">หัวเรื่อง</param>
        /// <param name="body">เนื้อหาของอีเมล์</param>
        public async Task Send(string senderEmail, string receiverEmail, string subject, string body)
        {
            var message = new SendGridMessage();
            message.AddTo(receiverEmail);
            message.From = new MailAddress(senderEmail);
            message.Subject = subject;
            message.Html = body;

            // HACK: test send email
            const string Username = "azure_f4f61c8971b9650ce3e99ee8e01a25e3@azure.com";
            const string Password = "@mefuwfhfuoT0";
            var credentials = new System.Net.NetworkCredential(Username, Password);
            var transportWeb = new Web(credentials);
            await transportWeb.DeliverAsync(message);
        }

        #endregion IEmailSender members
    }
}
