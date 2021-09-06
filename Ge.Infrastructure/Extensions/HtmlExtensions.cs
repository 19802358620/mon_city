using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ge.Infrastructure.Extensions
{
    /// <summary>
    /// Html扩展方法
    /// </summary>
    public static class HtmlExtensions
    {
        /// <summary>
        /// 获取指定Url路径的页面的内容并输出到前台
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static HtmlString Url(this HtmlHelper helper, string url)
        {
            var path = HttpContext.Current.Server.MapPath(url);
            var html = string.Empty;

            if (!File.Exists(path))
            {
                return new HtmlString("<h2>没有找到指定路径的文件：“{0}”！</h2>".Format2(url));
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fs, Encoding.Default);

            try
            {
                html = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                html = ex.Message.ToString();
            }
            finally
            {
                sr.Close();
                fs.Close();
            }

            return new HtmlString(html);
        }
    }
}
