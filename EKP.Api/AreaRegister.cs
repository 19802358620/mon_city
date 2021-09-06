using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#region 版本信息
/* 
* 名    称：WebSiteAreaRegistration
* 创 建 人：黄海东
* 创建时间：2016-05-09 17:25:22
* 联系方式：电话[15696208839],邮箱[adeledon@foxmail.com]
* 修 改 人：
* 修改时间：
* 描    述：
*/
#endregion

using System.Web.Mvc;
using EKP.Api;

namespace EKP.Api
{
    public class AreaRegister : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Api";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //注册路由
            context.MapRoute(
                "EKP.Api",
                "{siteDomain}/Api/{controller}/{action}",
                new { area = "Api", controller = "Home", action = "Index" },
                namespaces: new string[] { "EKP.Api.Controllers" }
            );
        }
    }
}