using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Role;
using EKP.Service.User;
using EKP.Web.Areas.Base.Application;
using EKP.Front;
using Ge.Infrastructure.Email;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Mvc.Extensions;
using Ge.Infrastructure.Utilities;
using Microsoft.AspNet.Identity;
using EKP.Base;
using EKP.Base.Controllers;
using Ge.Infrastructure.Sms;
using GeetestSDK;
using EKP.Base.Identity;
using Ge.Infrastructure.Extensions;
using Microsoft.AspNet.Identity.Owin;

namespace EKP.Front.Controllers
{
    /// <summary>
    /// 用户登录、注册、密码找回
    /// </summary>
    public class UserCenterController : BaseController
    {
        private readonly IUserService userService = Ioc.GetService<IUserService>();
        private readonly IRoleService roleService = Ioc.GetService<IRoleService>();

        #region View
        /// <summary>
        /// 主页
        /// </summary>
        public ActionResult Home()
        {
            return View();
        }

        /// <summary>
        /// 个人资料
        /// </summary>
        public ActionResult UserInfo()
        {
            return View();
        }

        /// <summary>
        /// 资料订阅
        /// </summary>
        public ActionResult Subscribe()
        {
            return View();
        }

        /// <summary>
        /// 我的网盘
        /// </summary>
        public ActionResult MyDisk()
        {
            return View();
        }

        /// <summary>
        /// 参考咨询
        /// </summary>
        public ActionResult Consult()
        {
            return View();
        }

        /// <summary>
        /// 个人收藏
        /// </summary>
        public ActionResult Collect()
        {
            return View();
        }

        /// <summary>
        /// 定题服务
        /// </summary>
        public ActionResult SDI()
        {
            return View();
        }

        /// <summary>
        /// 账户统计
        /// </summary>
        public ActionResult Statistics()
        {
            return View();
        }
        #endregion
    }
}