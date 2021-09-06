using System.Web;
using System.Web.Http.Filters;
using Ge.Infrastructure.Ioc;

namespace Ge.Infrastructure.Mvc.Exceptions
{
    /// <summary>
    /// 处理webapi异常信息
    /// </summary>
    public class WebApiHandleErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            ExceptionHandler.LogException(context);
            base.OnException(context);
        }
    }
}
