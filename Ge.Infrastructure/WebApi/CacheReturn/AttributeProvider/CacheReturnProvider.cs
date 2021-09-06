using System;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Ge.Infrastructure.WebApi.CacheReturn.AttributeProvider
{
    public class CacheReturnProvider : BaseCacheReturnProvider
    {
        /// <summary>
        /// 在进入控制器前检查缓存是否存在，如果存在直接返回缓存数据而不进入控制器
        /// </summary>
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext filterContext)
        {
            var value = HttpContext.Current.Cache.Get(Key);
            if (value != null)
            {
                filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.OK, value);
            }
        }

        /// <summary>
        /// 在返回结果后将数据保存在标识为"Key"缓存中
        /// </summary>
        public override void OnActionExecuted(System.Web.Http.Filters.HttpActionExecutedContext filterContext)
        {
            if((filterContext.Response.Content as ObjectContent) == null) return;

            var returnValue = (filterContext.Response.Content as ObjectContent).Value;
            HttpContext.Current.Cache[Key] = returnValue;
        }
    }
}