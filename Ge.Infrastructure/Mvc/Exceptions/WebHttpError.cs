using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Mvc.Exceptions
{
    /// <summary>
    /// 作    者：胡政
    /// 创建时间：2015-08-28
    /// 联系方式：13436053642
    /// 描    述：获取网站异常信息
    /// </summary>
    public class WebHttpError
    {
        public System.Web.HttpContext HttpContext { get; set; }

        /// <summary>
        /// 获取http请求错误信息
        /// </summary>
        /// <returns></returns>
        public string GetError()
        {
            var ex = HttpContext.Server.GetLastError().GetBaseException();
            var str = new StringBuilder();
            str.Append("\r\n" + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
            str.Append("\r\n.客户信息：");


            string ip = "";
            if (HttpContext.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR") != null)
            {
                ip = HttpContext.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR").ToString().Trim();
            }
            else
            {
                ip = HttpContext.Request.ServerVariables.Get("Remote_Addr").ToString().Trim();
            }
            str.Append("\r\n\tIp:" + ip);
            str.Append("\r\n\t浏览器:" + HttpContext.Request.Browser.Browser.ToString());
            str.Append("\r\n\t浏览器版本:" + HttpContext.Request.Browser.MajorVersion.ToString());
            str.Append("\r\n\t操作系统:" + HttpContext.Request.Browser.Platform.ToString());
            str.Append("\r\n.错误信息：");
            str.Append("\r\n\t记录来源：Application_Error");
            str.Append("\r\n\t页面：" + HttpContext.Request.Url.ToString());
            str.Append("\r\n\t错误信息：" + ex.Message);
            str.Append("\r\n\t错误源：" + ex.Source);
            str.Append("\r\n\t异常方法：" + ex.TargetSite);
            str.Append("\r\n\t堆栈信息：" + ex.StackTrace);
            str.Append("\r\n\r\n\r\n\r\n--------------------------------------------------------------------------------------------------");
            return str.ToString();
        }
    }
}
