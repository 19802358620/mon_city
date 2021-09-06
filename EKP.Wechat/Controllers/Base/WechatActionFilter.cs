using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EKP.Wechat.Controllers.Base
{
    /// <summary>
    /// 基础过滤器
    /// </summary>
    public class WechatActionFilter : ActionFilterAttribute
    {               
        private bool IsValidate { get; set; }

        public WechatActionFilter(bool isValidate)
        {
            IsValidate = isValidate;
        }

        public WechatActionFilter()
        {
            IsValidate = true;
        }
        //action 执行之前
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (IsValidate)
            {
                if (filterContext.HttpContext.Session["LoginUserId"] == null)
                {
                    filterContext.HttpContext.Session.Remove("LoginUserId");
                    filterContext.HttpContext.Session.RemoveAll();
                    filterContext.HttpContext.Session.Clear();
                    
                    string url = filterContext.HttpContext.Request.Url.AbsolutePath;
                    filterContext.HttpContext.Response.Redirect("/jkt/WeChat/User/Login?url=" + url);
                }                
            }
            base.OnActionExecuting(filterContext);
        }

        //Action执行之后
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            base.OnActionExecuted(filterContext);
        }

        //视图执行成功后
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }        
	}
}