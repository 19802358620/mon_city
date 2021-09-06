using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using EKP.Web.Areas.Base.Application;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Mvc.Exceptions;
using Newtonsoft.Json2;
using EKP.Base;
using EKP.Web.Start;

namespace EKP.Web.Start
{
    public class Global : System.Web.HttpApplication
    {
        static Global()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"\config\log4net.config"));
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            InitializeIoc.Register();//注册Ioc容器
            log4net.Config.XmlConfigurator.Configure();//注册日志记录
            AreaRegistration.RegisterAllAreas();//区域注册
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);//注册全局过滤器
            RouteConfig.RegisterRoutes(RouteTable.Routes);//注册路由
            AuthConfig.RegisterAuth();//注册证书
            ValueProviderConfig.RegisterValueProvider();//注册自定义模型绑定器
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var log = log4net.LogManager.GetLogger(sender.GetType());
            var webHttpError = Ioc.GetService<WebHttpError>();
            webHttpError.HttpContext = HttpContext.Current;

            //统一记录错误日志
            log.Error(webHttpError.GetError());

            //统一处理ajax请求错误
            var lastError = Server.GetLastError();
            if (lastError != null)
            {
                var httpStatusCode = (lastError is HttpException) ? (lastError as HttpException).GetHttpCode() : 500; //这里仅仅区分两种错误 
                //ajax请求错误
                if (new HttpRequestWrapper(Request).IsAjaxRequest())
                {
                    var dialog = DialogFactory.Create(DialogType.Error, "服务器异常",
                        string.Format(
                            string.Format("{0}", lastError)));

                    Response.Clear();
                    Response.ContentType = "application/json; charset=utf-8";
                    Response.Write(JsonConvert.SerializeObject(dialog));
                    Response.Flush();
                    Server.ClearError();
                }
                //非ajax请求错误
                else
                {
                    Response.Clear();
                    Response.ContentType = "text/html; charset=utf-8";
                    Response.Redirect(string.Format("/{0}/Adm/Shared/Error?returnUrl={1}", App.Domain, Request.RawUrl));
                    Response.Flush();
                    Server.ClearError();
                }
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}