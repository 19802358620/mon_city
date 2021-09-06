using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Ge.Infrastructure.Sms
{
    public class SmsService : IIdentityMessageService
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendAsync(IdentityMessage message)
        {
            return Task.FromResult(0);
        }
    }
}
