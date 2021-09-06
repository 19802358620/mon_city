using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ge.Infrastructure.WebApi.CacheReturn.AttributeProvider
{
    /// <summary>
    /// 抽象缓存标签
    /// </summary>
    public abstract class BaseCacheReturnProvider: Attribute
    {
        /// <summary>
        /// 缓存标识
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// 在进入控制器前检查缓存是否存在，如果存在直接返回缓存数据而不进入控制器
        /// </summary>
        public abstract void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext filterContext);


        /// <summary>
        /// 在返回结果后将数据保存在标识为"Key"缓存中
        /// </summary>
        public abstract void OnActionExecuted(System.Web.Http.Filters.HttpActionExecutedContext filterContext);
    }
}