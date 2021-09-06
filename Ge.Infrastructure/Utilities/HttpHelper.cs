using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.Win32;

namespace Ge.Infrastructure.Utilities
{
    /// <summary>
    /// 对客户端请求的操作类
    /// </summary>
    public static class HttpHelper
    {


        #region 私有方法
        /// <summary>
        /// 格式化IP，每个段为三位数字
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        private static string FormatIP(string IP)
        {
            string[] IPs = IP.Split('.');
            IP = "";
            for (int i = 0; i < IPs.Length; i++)
            {
                IPs[i] = IPs[i].PadLeft(3, '0');
                IP += IPs[i] + ".";
            }
            IP = IP.Remove(IP.Length - 1, 1);
            return IP;
        }

        public static decimal EncodeIP(string IPstr)
        {
            decimal IP = 0;
            if (IPstr == string.Empty)
            {
                return 0;
            }
            string[] IPstrs = IPstr.Split('.');
            if (IPstrs.Length == 4)
            {
                try
                {
                    for (int i = 3; i >= 0; i--)
                    {
                        IP += (Convert.ToDecimal(IPstrs[i]) * (decimal)Math.Pow(256, 3 - i));
                    }
                }
                catch
                {
                    IP = 0;
                }
                return IP;
            }
            else
            {
                return 0;
            }
        }

        #endregion
        /// <summary>
        /// 取得格式化IP，格式为为192.168.020.080。这样可以从数据库中直接用字符串进行比较
        /// </summary>
        /// <returns></returns>
        /// 
        public static string mapPath(string url)
        {
            return HttpContext.Current.Server.MapPath(url);
        }

        public static string GetFormatIP()
        {
            return FormatIP(GetIP());
        }
        /// <summary>
        /// 取得客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];
            }
            else
            {
                if (result.IndexOf(",") > 0)
                {
                    string[] ips = result.Split(',');
                    result = ips[0].Trim();
                }
                if (result.IndexOf("127.") == 0 || result.IndexOf("192.168") == 0 || result.IndexOf("172.") == 0 || result.IndexOf("10.") == 0)
                {
                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"];
                }
            }

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(result) || result == "::1")
                return "127.0.0.1";

            return result;
        }

        #region 对cookie的一组操作
        /// <summary>
        /// 获取指定cookie的值 
        /// </summary>
        /// <param name="cookieName">cookie名</param>
        /// <returns></returns>
        public static string GetCookieValue(string cookieName)
        {

            return GetCookieValue(cookieName, null);
        }
        /// <summary>
        /// 获取指定cookie的值 
        /// </summary>
        /// <param name="cookieName">cookie名</param>
        /// <param name="valueKey">值集合的键</param>
        /// <returns></returns>
        public static string GetCookieValue(string cookieName, string valueKey)
        {
            if (null == HttpContext.Current.Request.Cookies[cookieName])
            {
                return null;
            }
            else
            {
                if (string.IsNullOrEmpty(valueKey))
                {
                    return HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[cookieName].Value);
                }
                else
                {
                    return HttpUtility.UrlDecode(HttpContext.Current.Request.Cookies[cookieName].Values[valueKey]);
                }
            }
        }
        /// <summary>
        /// 根据指定的名称和值,创建一个cookie,默认没有指定过期时间的cookie
        /// </summary>
        /// <param name="name">cookie名</param>
        /// <param name="value">cookie值 </param>
        public static void SetCookie(string cookieName, string cookieValue)
        {
            SetCookie(cookieName, cookieValue, null);
        }
        /// <summary>
        /// 根据指定的名称和值,创建一个cookie,可以指定过期时间
        /// </summary>
        /// <param name="name">cookie名</param>
        /// <param name="value">cookie值 </param>
        /// <param name="expires">cookie的过期时间</param>
        /// <param name="hasExpire">标识是否有过期时间</param>
        public static void SetCookie(string cookieName, string cookieValue, DateTime? expires)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Value = HttpUtility.UrlEncode(cookieValue);
            if (expires != null)
            {
                cookie.Expires = (DateTime)expires;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 根据指定的名称和多个值,创建一个cookie,默认没有指定过期时间的cookie
        /// </summary>
        /// <param name="name">cookie名</param>
        /// <param name="values">cookie值 </param>
        public static void SetCookie(string cookieName, IDictionary<string, string> cookieValues)
        {

            SetCookie(cookieName, cookieValues, null);
        }
        /// <summary>
        /// 根据指定的名称和多个值,创建一个cookie,可以指定过期时间
        /// </summary>
        /// <param name="name">cookie名</param>
        /// <param name="values">cookie值 </param>
        /// <param name="expires">cookie的过期时间</param>
        /// <param name="hasExpire">标识是否有过期时间</param>
        public static void SetCookie(string cookieName, IDictionary<string, string> cookieValues, DateTime? expires)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            foreach (KeyValuePair<string, string> pair in cookieValues)
            {
                cookie.Values.Add(pair.Key, HttpUtility.UrlEncode(pair.Value));
            }
            if (expires != null)
            {
                cookie.Expires = (DateTime)expires;
            }
            HttpContext.Current.Response.Cookies.Add(cookie);

        }
        /// <summary>
        /// 根据指定的名/值对，创建多个cookie,可以指定过期时间
        /// </summary>
        /// <param name="dics">字典对象，包含要创建cookie名和值</param>
        /// <param name="expires">cookie的过期时间</param>
        /// <param name="hasExpire">标识是否有过期时间</param>
        public static void SetCookies(Dictionary<string, string> dics)
        {
            SetCookies(dics, null);
        }
        /// <summary>
        /// 根据指定的名/值对，创建多个cookie,可以指定过期时间
        /// </summary>
        /// <param name="dics">字典对象，包含要创建cookie名和值</param>
        /// <param name="expires">cookie的过期时间</param>
        /// <param name="hasExpire">标识是否有过期时间</param>
        public static void SetCookies(Dictionary<string, string> dics, DateTime? expires)
        {
            foreach (KeyValuePair<string, string> pair in dics)
            {
                HttpCookie cookie = new HttpCookie(pair.Key);
                cookie.Value = HttpUtility.UrlEncode(pair.Value);
                if (expires != null)
                {
                    cookie.Expires = (DateTime)expires;
                }
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        /// <summary>
        /// 删除指定名字的cookie
        /// </summary>
        /// <param name="arrCookieName">可选的cookie名数组</param>
        public static void DeleteCookie(params string[] arrCookieName)
        {

            if (0 != arrCookieName.Length)
            {
                foreach (string name in arrCookieName)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
                    if (null == cookie) continue;
                    cookie.Expires = DateTime.Now.AddSeconds(-1);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }
        /// <summary>
        /// 清空所有cookie,必须用一个新集合来接收过期cookie
        /// </summary>
        public static void ClearCookie()
        {
            HttpCookieCollection newColCookies = new HttpCookieCollection();
            HttpCookieCollection oldColCookies = HttpContext.Current.Request.Cookies;
            if (null == oldColCookies) return;
            foreach (string name in oldColCookies)
            {
                oldColCookies[name].Expires = DateTime.Now.AddHours(-1);
                newColCookies.Add(oldColCookies[name]);
            }
            for (int i = 0; i < newColCookies.Count; i++)
            {
                HttpContext.Current.Response.Cookies.Add(newColCookies[i]);
            }
        }
        #endregion

        /// <summary>
        /// 下载文件，用上了注册表来获取文件的类型
        /// </summary>
        /// <param name="saveFileName">保存文件名</param>
        /// <param name="physicalsPath">物理路径</param>
        /// <param name="charset">输出文件编码</param>
        public static void DownloadFile(string saveFileName, string physicalsPath, string charset)
        {
            string charType = charset ?? "gb2312";
            System.IO.FileInfo file = new System.IO.FileInfo(physicalsPath);
            string extendName = file.Extension;//文件扩展名
            string DEFAULT_CONTENT_TYPE = "application/unknown";//默认文件类型，
            RegistryKey regkey, fileExtKey;
            string fileContentType;
            try
            {
                regkey = Registry.ClassesRoot;
                fileExtKey = regkey.OpenSubKey(extendName);
                fileContentType = fileExtKey.GetValue("Content Type", DEFAULT_CONTENT_TYPE).ToString();
            }
            catch
            {
                fileContentType = DEFAULT_CONTENT_TYPE;
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = charType;
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding(charType);
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + saveFileName);
            HttpContext.Current.Response.ContentType = fileContentType;
            HttpContext.Current.Response.WriteFile(physicalsPath);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();
        }
    }
}
