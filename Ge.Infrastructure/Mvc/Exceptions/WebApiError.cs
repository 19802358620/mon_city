using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Ge.Infrastructure.Mvc.Exceptions
{
    /// <summary>
    /// 作    者：胡政
    /// 创建时间：2015-08-28
    /// 联系方式：13436053642
    /// 描    述：获取WebApi异常信息
    /// </summary>
    public class WebApiError
    {
        public HttpActionExecutedContext Context { get; set; }

        /// <summary>
        /// 获取WebApi异常信息
        /// </summary>
        /// <returns></returns>
        public string GetError()
        {
            var ex = Context.Exception;
            var str = new StringBuilder();
            str.Append("\r\n" + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
            str.Append("\r\n.客户信息：");

            str.Append("\r\n.错误信息：");
            str.Append("\r\n\t记录来源：WebApi异常过滤器OnException");
            str.Append("\r\n\t页面：" + Context.Request.RequestUri.ToString());
            str.Append("\r\n\t错误信息：" + ex.Message);
            str.Append("\r\n\t错误源：" + ex.Source);
            str.Append("\r\n\t异常方法：" + ex.TargetSite);
            str.Append("\r\n\t堆栈信息：" + ex.StackTrace);
            str.Append("\r\n\r\n\r\n\r\n--------------------------------------------------------------------------------------------------");
            return str.ToString();
        }
    }
}
