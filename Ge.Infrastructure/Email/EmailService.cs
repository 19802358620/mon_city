using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Ge.Infrastructure.Email
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    public class EmailService : IIdentityMessageService
    {
        /// <summary>
        /// 邮箱服务器
        /// </summary>
        public SmtpClient Smtp { get; set; }

        /// <summary>
        /// 邮件发送者
        /// </summary>
        public string From { get; set; }

        public EmailService(SmtpClient smtp, string from)
        {
            this.Smtp = smtp;
            this.From = from;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendAsync( IdentityMessage message)
        {
            //配置
            var mailMessage = new System.Net.Mail.MailMessage(From,
                message.Destination,
                message.Subject,
                message.Body);
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;

            //发送
            if (Smtp == null)
                throw new Exception("Smtp不能为null");
            Smtp.SendMailAsync(mailMessage);

            return Task.FromResult(0);
        }
    }
}
