using System.Web;
using System.Web.Mvc;
using EKP.Web.Areas;
using EKP.Base;
using EKP.Base.Cache;
using EKP.Adm;
using EKP.Base.FilterAttribute;
using EKP.Adm.Authority;

namespace EKP.Web.Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute(), 10);//异常过滤器
            filters.Add(new CacheAttribute(), 11);//缓存过滤器
            filters.Add(new GlobalIpLoginInAttribute(), 12);//Ip自动登录过滤器
            filters.Add(new AdmFilter(), 13);//全局参数过滤器
            filters.Add(new ModelFillAttribute(), 14);//模型绑定器
        }
    }
}