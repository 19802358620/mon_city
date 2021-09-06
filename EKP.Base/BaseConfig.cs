using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EKP.Base
{
    /// <summary>
    /// 站点基础配置
    /// </summary>
    public class BaseConfig
    {
        /// <summary>
        /// 是否启用缓存（前台和后台缓存）
        /// </summary>
        public static bool IsCache { get { return false; } }
    }
}