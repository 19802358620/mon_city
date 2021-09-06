using EKP.Entity;
using EKP.Service.Site;
using Ge.Infrastructure.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ge.Infrastructure.Extensions;

namespace EKP.Base
{
    /// <summary>
    /// 当前的http请求
    /// </summary>
    public class App
    {
        private static readonly ISiteService siteService = Ioc.GetService<ISiteService>();

        /// <summary>
        /// 当前的站点名
        /// </summary>
        public static string Domain {
            get
            {
                var urlsp = HttpContext.Current.Request.Path.Split('/');

                return urlsp.Length > 1 ? urlsp[1].ToString() : string.Empty;
            }
        }

        /// <summary>
        /// 当前站点实体
        /// </summary>
        public static T_Site Site
        {
            get
            {
                var site = siteService.GetEntiy("Domain='{0}'".Format2(App.Domain));
                return site;
            }
        }
    }
}
