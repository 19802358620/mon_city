using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EKP.Api
{

    /// <summary>
    /// Session值类型
    /// </summary>
    public enum WebSiteSessionType
    {
        FindPasswordToken = 1,//找回密码往客户端邮箱发送的token串
        FindPasswordTime = 2,//找回密码往客户端邮箱的时间
    }
}