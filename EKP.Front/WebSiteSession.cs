using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EKP.Front
{

    /// <summary>
    /// Session值类型
    /// </summary>
    public enum WebSiteSessionType
    {
        FindPasswordToken = 1,//找回密码往客户端邮箱发送的token串
        FindPasswordTime = 2,//找回密码往客户端邮箱的时间
        ValidCode = 3,//验证码
        ValidCodeTime = 4,//验证码发送时间
        BindValidCodeToken = 3,//绑定验证码
        BindValidCodeTime = 4//绑定验证码发送时间
    }
}