/// <summary>
/// 开发团队：JsonsTeam
/// 官方主页:http://www.jsons.cn
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Ge.Infrastructure.Utilities
{
    public class Html2Helper
    {
        /// <summary>
        /// 格式化输出到页面的字符串，包括转换回车符
        /// </summary>
        /// <param name="htmlstr">要格式化的字符串</param>
        /// <param name="replace">是否替换换行符</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatHtmlString(string htmlstr, bool replace)
        {
            if (string.IsNullOrEmpty(htmlstr)) return "";
            htmlstr = HttpContext.Current.Server.HtmlEncode(htmlstr);
            htmlstr = htmlstr.Replace(" ", "&nbsp;");
            if (replace) { htmlstr = htmlstr.Replace("\r\n", "<br />"); }
            return htmlstr;
        }

        /// <summary>
        /// 格式化输出到页面的字符串，包括转换回车符
        /// </summary>
        /// <param name="htmlstr">要格式化的字符串</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatHtmlString(string htmlstr)
        {
            htmlstr = HttpContext.Current.Server.HtmlEncode(htmlstr);
            htmlstr = htmlstr.Replace(" ", "&nbsp;");
            htmlstr = htmlstr.Replace("<", "&lt;");
            htmlstr = htmlstr.Replace(">", "&gt;");
            return htmlstr;
        }

        /// <summary>
        /// 返回过滤掉所有的Html标签后的字符串
        /// </summary>
        /// <param name="html">Html源码</param>
        /// <returns>过滤Html标签后的字符串</returns>
        public static string ClearAllHtml(string html)
        {
            string filter = "<[\\s\\S]*?>";
            if (Regex.IsMatch(html, filter))
            {
                html = Regex.Replace(html, filter, "");
            }
            filter = "[<>][\\s\\S]*?";
            if (Regex.IsMatch(html, filter))
            {
                html = Regex.Replace(html, filter, "");
            }
            return html;
        }

        /// <summary>
        /// 检查是否有Html标签注入
        /// </summary>
        /// <param name="html">Html源码</param>
        /// <returns>存在为True</returns>
        public static bool IsIncludeHtmlInjec(string html)
        {
            string filter = "<[\\s\\S]*?>";

            if (Regex.IsMatch(html, filter))
            {
                return true;
            }
            filter = "[<>][\\s\\S]*?";
            if (Regex.IsMatch(html, filter))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查是有否有sql注入
        /// </summary>
        /// <param name="html">Html源码</param>
        /// <returns>存在为True</returns>
        public static bool IsIncludeSqlInjec(string html)
        {
            if (String.IsNullOrWhiteSpace(html))return false;

            string pattern = @"select|insert|delete|from|iframe|'|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec|netlocalgroup administrators";
            Match m = Regex.Match(html, pattern, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查是有否脚本注入
        /// </summary>
        /// <param name="html">Html源码</param>
        /// <returns>存在为True</returns>
        public static bool IsIncludeScriptInjec(string html)
        {
            if (String.IsNullOrWhiteSpace(html)) return false;

            string pattern = @"<script>|alert|%27+|function|onmouseover|\\|onclick|window|document|break|location|eval|==|javascript|expression|xpression|catch|'";
            Match m = Regex.Match(html, pattern, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查是有Url地址
        /// </summary>
        /// <param name="html">Html源码</param>
        /// <returns>存在为True</returns>
        public static bool IsIncludeUrlInjec(string html)
        {
            if (String.IsNullOrWhiteSpace(html)) return false;

            string pattern = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&$%\$#\=~])*$";
            Match m = Regex.Match(html, pattern, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                return true;
            }
            return false;
        }
    }
}