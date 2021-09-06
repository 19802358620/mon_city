using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.Base.EkpBaseModel;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using EKP.Web.Areas.Base.Application;
using EKP.Base.Controllers;
using EKP.Base;


namespace EKP.Api.Controllers
{
    /// <summary>
    /// 共享控制器
    /// </summary>
    public class SharedController : BaseController
    {

        /// <summary>
        /// 网站出错跳转页面
        /// </summary>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// 网站菜单
        /// </summary>
        public ActionResult _LayoutMenu()
        {
            return PartialView();
        }
    }
}