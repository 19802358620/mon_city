using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EKP.Service.User;
using EKP.Web.Areas.Base.Application;
using EKP.Base.Identity;
using Ge.Infrastructure.Ioc;
using Microsoft.AspNet.Identity.Owin;
using EKP.Base.Cache;

namespace EKP.Adm
{
    /// <summary>
    /// Ip自动登录过滤器，如果当前访问用户Ip在指定IP段范围内则自动进行登陆
    /// </summary>
    public class GlobalIpLoginInAttribute : ActionFilterAttribute
    {
        private readonly IUserService userService = Ioc.GetService<IUserService>();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var cacheKey = "LoginUserInfo_Detali";
            var area = (filterContext.RequestContext.RouteData.DataTokens["area"] as string ?? string.Empty).ToLower();
            var controller = filterContext.RequestContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RequestContext.RouteData.Values["action"].ToString();
            var postType = filterContext.HttpContext.Request.HttpMethod.ToLower();

            //临时代码
            //var log = log4net.LogManager.GetLogger(this.GetType());
            //log.Error(string.Format("请求地址：/{0}/{1}/{2},请求类型：{3}", area, controller, action, postType));

            //被忽略的请求
            if (postType != "get") return;

            var href = string.Format("/{0}/{1}", controller, action).ToLower();
            if (href == "/user/login") return;
            if (href == "/user/loginout") return;
            if (href == "/user/signinvalidcode") return;
            if (href == "/manager/shared/tempchecklogin") return;
            if (href == "/manager/user/register") return;
            if (href == "/manager/user/forgotpwd") return;
            if (href == "/manager/user/settingpwd") return;

            if (!filterContext.HttpContext.Request.IsAjaxRequest() && !ApplicationSignInManager.IsLogin())
            {
                var signInManager = filterContext.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                signInManager.IpSignIn();
                CacheManager.Remove(cacheKey);
            }
        }
    }
}