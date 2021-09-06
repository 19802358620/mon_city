using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace EKP.Web.Areas.Base.Application
{
    /// <summary>
    /// 客户端Cookie管理
    /// </summary>
    public  class CookieManager
    {
        /// <summary>
        /// 获取CookieName
        /// </summary>
        /// <returns></returns>
        public static string GetCookieName(Enum cookieType)
        {
            return string.Format("{0}{1}", ".AspNet.", cookieType.GetType().FullName);
        }

        /// <summary>
        /// 添加cookie值
        /// </summary>
        public static void Set(Enum cookieType, string value)
        {
            var cookieName = GetCookieName(cookieType);
            var cookie = HttpContext.Current.Request.Cookies[cookieName];
            var key = CreateCookieKey(cookieType);
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
            }
            cookie.Expires = DateTime.Now.AddMonths(1);
            cookie[key] = value;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 获取cookie值
        /// </summary>
        public static string Get(Enum cookieType)
        {
            var cookieName = GetCookieName(cookieType);
            var cookie = HttpContext.Current.Request.Cookies[cookieName];
            var key = CreateCookieKey(cookieType);
            if (cookie == null) return null;
            return cookie[key];
        }

        /// <summary>
        /// 移除cookie值
        /// </summary>
        public static void Remove(Enum cookieType)
        {
            var cookieName = GetCookieName(cookieType);
            var cookie = HttpContext.Current.Request.Cookies[cookieName];
            var key = CreateCookieKey(cookieType);
            if (cookie == null) return;
            cookie.Values.Remove(key);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 创建Cookie键
        /// </summary>
        /// <returns></returns>
        private static string CreateCookieKey(Enum cookieType)
        {
            return cookieType.ToString();
        }
    }
}