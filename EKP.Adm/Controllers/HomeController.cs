using System.Web.Mvc;
using EKP.Service.User;
using EKP.Web.Areas.Base.Application;
using EKP.Adm;
using Ge.Infrastructure.Ioc;
using EKP.Service.Role;
using EKP.Base.Cache;
using System.Collections.Generic;
using Ge.Infrastructure.Metronicv.Dialog;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 名    称：HomeController
    /// 作    者：胡政
    /// 创建时间：2015-08-28
    /// 联系方式：13436053642
    /// 描    述：主页
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IUserService _userService = Ioc.GetService<IUserService>();

        /// <summary>
        /// 首页
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 缓存分页
        /// </summary>
        public ActionResult CachePager()
        {
            return View();
        }

        /// <summary>
        /// 缓存分页post
        /// </summary>
        [HttpPost]
        public ActionResult CachePager(CachePagerParam param)
        {
            return Json(CacheManager.GetPager<CachePagerModel>(param));
        }

        /// <summary>
        /// 删除缓存post
        /// </summary>
        [HttpPost]
        public ActionResult CacheDelate(List<string> keys)
        {
            CacheManager.Remove(keys);
            return Json(DialogFactory.Create(DialogType.Success));
        }
    }
}