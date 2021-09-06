using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using EKP.Web.Areas.Base.Application;
using Ge.Infrastructure.Metronicv.Dialog;
using Newtonsoft.Json;
using JsonSerializerSettings = Newtonsoft.Json2.JsonSerializerSettings;
using ReferenceLoopHandling = Newtonsoft.Json2.ReferenceLoopHandling;
using EKP.Base.Cache;

namespace EKP.Base.FilterAttribute
{
    /// <summary>
    /// 名    称：缓存过滤器
    /// </summary>
    public class CacheAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 通过前台传入的参数判断是否返回缓存数据
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!IsAllowesCache(filterContext.RequestContext)) return;

            string key = CreateKeyByParams(filterContext.RequestContext);
            var data = CacheManager.Get(key);
            if (data != null)
            {
                if(data is JsonResult)
                    filterContext.Result = data as JsonResult;
                else if(data is JavaScriptResult)
                    filterContext.Result = data as JavaScriptResult;
            }
        }

        /// <summary>
        /// 返回结果前对结果进行缓存
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!IsAllowesCache(filterContext.RequestContext)) return;

            var area = (filterContext.RequestContext.RouteData.DataTokens["area"] as string ?? string.Empty).ToLower();
            var controller = (filterContext.RequestContext.RouteData.Values["controller"] as string?? string.Empty).ToLower();
            var action = (filterContext.RequestContext.RouteData.Values["action"] as string?? string.Empty).ToLower();
            string key = CreateKeyByParams(filterContext.RequestContext);

            var result = filterContext.Result;
            //对JsonResult进行缓存
            if (result is JsonResult)
            {
                var jsonResult = result as JsonResult;
                var data = jsonResult.Data;
                //如果是弹出框并且弹出框类型为错误则不进行缓存
                if(data is Dialog && (data as Dialog).DialogType == DialogType.Error) return;

                //将结果转化为JavaScriptResult的方式返回结果
                CacheManager.Set(key, jsonResult);
            }
            //对JavaScriptResult进行缓存
            else if (result is JavaScriptResult)
            {
                var javaScriptResult = result as JavaScriptResult;
                var data = javaScriptResult.Script;
                Dialog dialog = null;
                try
                {
                    dialog = JsonConvert.DeserializeObject<Dialog>(data);
                }
                catch (Exception ex)
                {
                    
                }
                ////如果是弹出框并且弹出框类型为错误则不进行缓存
                if (dialog != null && (dialog as Dialog).DialogType == DialogType.Error) return;
                CacheManager.Set(key, javaScriptResult);
            }
        }

        /// <summary>
        /// 创建唯一的缓存key
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected string CreateKeyByParams(RequestContext context)
        {
            var area = (context.RouteData.DataTokens["area"] as string ?? string.Empty).ToLower();
            var controller = (context.RouteData.Values["controller"] as string ?? string.Empty).ToLower();
            var action = (context.RouteData.Values["action"] as string ?? string.Empty).ToLower();

            var keyParam = string.Empty;
            context.HttpContext.Request.QueryString.AllKeys.ToList().ForEach(k =>
            {
                keyParam += string.Format("{0}:{1};", k, context.HttpContext.Request.QueryString[k]);
            });
            context.HttpContext.Request.Form.AllKeys.ToList().ForEach(k =>
            {
                keyParam += string.Format("{0}:{1};", k, context.HttpContext.Request.Form[k]);
            });
            var key = string.Format("{0}.{1}.{2}.{3}.{4}", "AspNetMVC", area, controller, action, keyParam);
            return key;
        }

        /// <summary>
        /// 是否允许缓存
        /// </summary>
        /// <returns></returns>
        protected bool IsAllowesCache(RequestContext context)
        {
            return BaseConfig.IsCache && (context.HttpContext.Request.QueryString["cache"] == "true" ||
                   context.HttpContext.Request.Form["cache"] == "true");
        }
    }
}