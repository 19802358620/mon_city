using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Ge.Infrastructure.Utilities
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public class AppSettingHelper
    {
        /// <summary>
        /// 通过key获取配置文件值，如果不存在键则返回null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            if (appSetting == null) return null;

            return ConfigurationManager.AppSettings[key].ToString();
        }
    }
}
