using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ge.Infrastructure.Filter;
using Ge.Infrastructure.FilterAttribute;


namespace EKP.Adm.Authority
{
    /// <summary>
    /// 全局过滤器
    /// </summary>
    public class AdmFilter : BaseFilterAttribute
    {
        /// <summary>
        /// 只验证该区域
        /// </summary>
        public override List<string> Areas
        {
            get
            {
                return new List<string>() { "Adm" };
            }
        }

        /// <summary>
        /// 全局权限验证
        /// </summary>
        public override void BaseOnActionExecuting(ActionExecutingContext filterContext)
        {
            var isThrow = true;
        }

        /// <summary>
        /// 如果添加了此标签，则不会进行进行过滤操作
        /// </summary>
        public new class Disabled : System.Attribute { }
    }
}