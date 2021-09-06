using System.Web.Mvc;
using EKP.Service.User;
using EKP.Web.Areas.Base.Application;
using EKP.Adm;
using Ge.Infrastructure.Ioc;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 名    称：HomeController
    /// 作    者：胡政
    /// 创建时间：2015-08-28
    /// 联系方式：13436053642
    /// 描    述：主页
    /// </summary>
    public class SystemController : Controller
    {
        private readonly IUserService _userService = Ioc.GetService<IUserService>();


        #region 视图
        /// <summary>
        /// 中心管理
        /// </summary>
        public ActionResult Center()
        {
            return View();
        }

        /// <summary>
        /// 关于中心
        /// </summary>
        public ActionResult AboutCenter()
        {
            return View();
        }

        /// <summary>
        /// 通知公告
        /// </summary>
        public ActionResult Notice()
        {
            return View();
        }

        /// <summary>
        /// 友情链接
        /// </summary>
        public ActionResult Blogroll()
        {
            return View();
        }

        /// <summary>
        /// 阅读工具管理
        /// </summary>
        public ActionResult Tools()
        {
            return View();
        }

        /// <summary>
        /// 在线咨询
        /// </summary>
        public ActionResult Consult()
        {
            return View();
        }
        #endregion
    }
}