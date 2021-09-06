using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EKP.Base
{
    /// <summary>
    /// Session管理
    /// </summary>
    public class SessionManager
    {
        /// <summary>
        /// 添加session值
        /// </summary>
        public static void Set(Enum sessionType, object value)
        {
            HttpContext.Current.Session[sessionType.ToString()] = value;
        }

        /// <summary>
        /// 获取session值
        /// </summary>
        public static object Get(Enum sessionType)
        {
            return HttpContext.Current.Session[sessionType.ToString()];
        }

        /// <summary>
        /// 移除session值
        /// </summary>
        public static void Remove(Enum sessionType)
        {
            HttpContext.Current.Session.Remove(sessionType.ToString());
        }
    }
}