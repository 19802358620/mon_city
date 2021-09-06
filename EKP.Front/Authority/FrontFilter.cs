using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EKP.Base.Identity;
using EKP.Entity;
using EKP.Service.Authority;
using EKP.Service.Role;
using Ge.Infrastructure.Filter;
using Ge.Infrastructure.FilterAttribute;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv.Dialog;

namespace EKP.Front.Authority
{
    /// <summary>
    /// 全局过滤器
    /// </summary>
    public class FrontFilter : BaseFilterAttribute
    {
        /// <summary>
        /// 只验证该区域
        /// </summary>
        public override List<string> Areas
        {
            get
            {
                return new List<string>() { "Front" };
            }
        }

        /// <summary>
        /// 全局权限验证
        /// </summary>
        public override void BaseOnActionExecuting(ActionExecutingContext filterContext)
        {
            var area = (filterContext.RequestContext.RouteData.DataTokens["area"] as string ?? string.Empty).ToLower();//Area
            var controller = (filterContext.RequestContext.RouteData.Values["controller"] as string ?? string.Empty).ToLower();//controller
            var action = (filterContext.RequestContext.RouteData.Values["action"] as string ?? string.Empty).ToLower();//action
            var postType = filterContext.HttpContext.Request.HttpMethod.ToLower();//请求方式(post或get)
            var loginUser = ApplicationSignInManager.GetLoginUser();//当前登录用户

            //请求方式验证
            if (postType == "post")
            {
                return;
            }
        }

        /// <summary>
        /// 转跳到登录页面进行登录
        /// </summary>
        protected void GotoLoginPage(ActionExecutingContext filterContext)
        {
            var postType = filterContext.HttpContext.Request.HttpMethod.ToLower();
            if (postType == "post")
            {
                CreateLoginDialog(filterContext);
            }
            else
            {
                var returnUrl = filterContext.HttpContext.Request.RawUrl;
                filterContext.RequestContext.HttpContext.Response.RedirectToRoute("EKP.Front", new
                {
                    Controller = "User",
                    Action = "Login",
                    returnUrl = returnUrl
                });
            }
        }

        /// <summary>
        /// 弹出登录框进行登录
        /// </summary>
        protected void CreateLoginDialog(ActionExecutingContext filterContext)
        {
            //用action拦截的方式在客户端弹出登陆框
            var jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jsonResult.Data = DialogFactory.Create(DialogType.Login, "登陆过期，请重新登陆", string.Empty, "AuthorityAttribute.error");
            filterContext.Result = jsonResult;
        }

        /// <summary>
        /// 如果添加了此标签，则不会进行进行过滤操作
        /// </summary>
        public new class Disabled : System.Attribute { }
    }
}
