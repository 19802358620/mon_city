using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ge.Infrastructure.FilterAttribute
{
    /// <summary>
    /// 基础过滤器
    /// </summary>
    public class BaseFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 需要过滤的区域集合，不设置此属性则对所有区域有效
        /// </summary>
        public virtual List<string> Areas { get; set; }

        /// <summary>
        /// 进入action之前过滤操作
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            base.OnActionExecuting(filterContext);

            var area = (filterContext.RequestContext.RouteData.DataTokens["area"] as string ?? string.Empty).ToLower();
            var controller = (filterContext.RequestContext.RouteData.Values["controller"] as string ?? string.Empty).ToLower();
            var action = (filterContext.RequestContext.RouteData.Values["action"] as string ?? string.Empty).ToLower();
            var baseUrl = string.Format("/{0}/{1}/{2}", area, controller, action);

            //检测区域限制
            if (Areas != null && Areas.Any(a => a.ToLower() != area)) return;

            //检测disabled标签限制
            var disabledType = this.GetType().GetMembers().FirstOrDefault(oi => oi.Name == "Disabled");
            if(disabledType != null)
            {
                var disabled = ((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo.GetCustomAttributes(disabledType as Type, true).FirstOrDefault();
                if (disabled != null) return;
            }

            BaseOnActionExecuting(filterContext);
        }

        /// <summary>
        /// 进入action之前过滤操作
        /// </summary>
        public virtual void BaseOnActionExecuting(ActionExecutingContext filterContext)
        {
            
        }

        /// <summary>
        /// 进入action之后返回结果之前过滤操作
        /// </summary>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var area = (filterContext.RequestContext.RouteData.DataTokens["area"] as string ?? string.Empty).ToLower();
            var controller = (filterContext.RequestContext.RouteData.Values["controller"] as string ?? string.Empty).ToLower();
            var action = (filterContext.RequestContext.RouteData.Values["action"] as string ?? string.Empty).ToLower();
            var baseUrl = string.Format("{0}/{1}/{2}", area, controller, action);

            //检测区域限制
            if (Areas != null && Areas.Any(a => a.ToLower() == area)) return;

            //检测disabled标签限制
            var disabledType = this.GetType().GetMembers().FirstOrDefault(oi => oi.Name == "Disabled");
            if (disabledType != null)
            {
                var disabled = ((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo.GetCustomAttributes(disabledType as Type, true).FirstOrDefault();
                if (disabled != null) return;
            }

            BaseOnActionExecuted(filterContext);
        }

        /// <summary>
        /// 进入action之后返回结果之前过滤操作
        /// </summary>
        public virtual void BaseOnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        /// <summary>
        /// 如果添加了此标签，则不会进行进行过滤操作
        /// </summary>
        public abstract class Disabled : System.Attribute {}
    }
}