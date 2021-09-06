using System;
using System.Web.Http.Filters;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Ge.Infrastructure.Mvc.Exceptions
{
    public static class ExceptionHandler
    {
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="policy"></param>
        /// <param name="rethrow"></param>
        public static void LogException(HttpActionExecutedContext context, string policy = "Policy", bool rethrow = false)
        {
            //ExceptionPolicy.HandleException(ex, policy);
            var log = log4net.LogManager.GetLogger(context.GetType());
            var webApiError = new WebApiError();

            webApiError.Context = context;
            //记录错误日志
            log.Error(webApiError.GetError());

            if (rethrow)
                throw context.Exception;
        }
    }
}