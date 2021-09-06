using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.Role;
using EKP.Service.Authority;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.FilterAttribute;
using EKP.Base.Identity;

namespace EKP.Api
{
    public class GlobalLogAttribute : BaseFilterAttribute
    {

        //需要验证的区域
        public override List<string> Areas { get { return new List<string>() { "Api" }; } }
        
        /// <summary>
        /// 全局权限验证
        /// </summary>
        public override void BaseOnActionExecuting(ActionExecutingContext filterContext)
        {
            base.BaseOnActionExecuting(filterContext);

            var area = (filterContext.RequestContext.RouteData.DataTokens["area"] as string ?? string.Empty).ToLower();
            var controller = (filterContext.RequestContext.RouteData.Values["controller"] as string ?? string.Empty).ToLower();
            var action = (filterContext.RequestContext.RouteData.Values["action"] as string ?? string.Empty).ToLower();
            var postType = filterContext.HttpContext.Request.HttpMethod.ToLower();

            if (postType.Equals("post")) return;//过滤掉post请求

            var loginUser = ApplicationSignInManager.GetLoginUser();
            if(loginUser == null) return;
        }

    }
}