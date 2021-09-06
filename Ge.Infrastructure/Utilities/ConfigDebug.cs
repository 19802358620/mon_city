using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Utilities
{
    /// <summary>
    /// 获取站点调试状态
    /// </summary>
    public static class ConfigDebug
    {
        /// <summary>
        /// 是否处于调试状态
        /// </summary>
        public static bool IsDebug{
            get
            {

                var compilationSection = (System.Web.Configuration.CompilationSection)System.Configuration.ConfigurationManager.GetSection("system.web/compilation");
                if (compilationSection == null) return true;

                return compilationSection.Debug;
            }
        }
    }
}
