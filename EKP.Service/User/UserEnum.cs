using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.User
{
    /// <summary>
    /// 账号状态
    /// </summary>
    public enum UserStatus
    {
        [Description("未激活")]
        NotActivate,
        [Description("正常")]
        Normal, 
        [Description("停用")]
        Stop 
    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserType
    {
        [Description("个人用户")]
        Person,
        [Description("企业用户")]
        Company
    }

}
