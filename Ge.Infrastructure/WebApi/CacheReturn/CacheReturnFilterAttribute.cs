using System;
using System.Linq;
using System.Web.Http.Filters;
using Ge.Infrastructure.WebApi.CacheReturn.AttributeProvider;


namespace Ge.Infrastructure.WebApi.CacheReturn
{
    /// <summary>
    /// 缓存提供器标签
    /// 可以对控制器的执行返回的结果数进行缓存
    /// </summary>
    public class CacheReturnFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 在进入控制器前检查缓存是否存在，如果存在直接返回缓存数据而不进入控制器
        /// </summary>
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext filterContext)
        {
            //获取Action的所有过滤器标签
            var cacheProviders = filterContext.ActionDescriptor.GetCustomAttributes<BaseCacheReturnProvider>(true);
            if (cacheProviders.Count == 0)
                return;

            if (cacheProviders.Count > 1)
                throw new Exception("\"CacheProviderAttribute\"缓存标签数量不得超过1个");

            var cacheProvider = cacheProviders.First();
            if (string.IsNullOrEmpty(cacheProvider.Key))
                throw new Exception("缓存标识不能为空");

            cacheProvider.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 在返回结果后将数据保存在标识为"Key"缓存中
        /// </summary>
        public override void OnActionExecuted(System.Web.Http.Filters.HttpActionExecutedContext filterContext)
        {
            //获取Action的所有过滤器标签
            var cacheProviders = filterContext.ActionContext.ActionDescriptor.GetCustomAttributes<BaseCacheReturnProvider>(true);
            if (cacheProviders.Count == 0)
                return;

            if (cacheProviders.Count > 1)
                throw new Exception("\"CacheProviderAttribute\"缓存标签数量不得超过1个");

            var cacheProvider = cacheProviders.First();
            if (string.IsNullOrEmpty(cacheProvider.Key))
                throw new Exception("缓存标识不能为空");

            cacheProvider.OnActionExecuted(filterContext);
        }
    }
}