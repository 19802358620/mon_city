using System.Web.Mvc;

namespace EKP.Wechat
{
    public class AreaRegister : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Wechat";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.MapRoute(
            //    "Wechat_default",
            //    "Wechat/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
            //注册路由
            context.MapRoute(
                "EKP.Wechat",
                "{siteDomain}/Wechat/{controller}/{action}",
                new { area = "Wechat", controller = "Home", action = "Index" },
                namespaces: new string[] { "EKP.Wechat.Controllers" }
            );
        }
    }
}