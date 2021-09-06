using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EKP.Web.Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.asmx/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{siteDomain}/{controller}/{action}",
                new {controller = "Home", action = "Index"},
                namespaces: new string[] {"EKP.Front.Controllers"}
                ).DataTokens.Add("area", "Front");
        }
    }
}
