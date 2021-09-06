using System.Web.Mvc;

namespace EKP.Adm
{
    /// <summary>
    /// 注册区域
    /// </summary>
    public class AreaRegister : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Adm";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //注册路由
            context.MapRoute(
                "EKP.Adm",
                "{siteDomain}/Adm/{controller}/{action}",
                new { area = "Adm", controller = "Home", action = "Index" },
                namespaces: new string[] { "EKP.Adm.Controllers" }
            );
        }
    }
}
