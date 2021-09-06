using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKP.Base.Controllers;
using EKP.Base.Identity;
using EKP.Base;
using EKP.Entity;
using EKP.Service.Site;
using Ge.Infrastructure.Ioc;
using EKP.Service.DictValue;
using EKP.Service.Role;
using EKP.Service.Authority;
using EKP.Service.AdmSetting;
using Ge.Infrastructure.Extensions;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Info;

namespace EKP.Front.Controllers
{
    /// <summary>
    /// 共享控制器
    /// </summary>
    public class SharedController : BaseController
    {
        private readonly IInfoService infoService = Ioc.GetService<IInfoService>();
        private readonly ISiteService siteService = Ioc.GetService<ISiteService>();
        private readonly IDictValueService dictValueService = Ioc.GetService<IDictValueService>();
        private readonly IRoleService roleService = Ioc.GetService<IRoleService>();
        private readonly IAuthorityService authorityService = Ioc.GetService<IAuthorityService>();
        private readonly IAdmSettingService admSettingService = Ioc.GetService<IAdmSettingService>();

        /// <summary>
        /// 网站出错跳转页面
        /// </summary>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// 个人中心菜单
        /// </summary>
        public ActionResult _UserCenterMenu()
        {
            return PartialView();
        }

        /// <summary>
        /// 获取App信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAppInfo()
        {
            //登录用户信息
            var loginInUser = ApplicationSignInManager.GetLoginUser();
            var loginUser = (object)null;
            if (loginInUser != null)
            {
                var loginRole = roleService.GetEntiy(Convert.ToInt32(loginInUser.LoginRoleId));
                loginUser = new
                {
                    Id = loginInUser.Id,
                    UserName = loginInUser.UserName,
                    LoginRoleId = loginRole.Id,
                    LoginRoleName = loginRole.Name,
                    LoginRoleKey = loginRole.Key,
                    LoginMethod = loginInUser.LoginMethod,
                    UserInfo = loginInUser.LoginUser
                };
            }

            //可切换角色列表
            var switchRoles = new List<RolePagerModel>();
            if (loginUser != null)
            {
                switchRoles = roleService.IncludeRoles<RolePagerModel>(Convert.ToInt32(loginInUser.LoginUser.RoleId))
                    .OrderBy(r => r.GradeNum).ToList();
            }

            var model = new
            {
                site = App.Site,//当前站点
                area = "Front",//当前区域
                loginUser = loginUser, //当前登录人
                isCache = BaseConfig.IsCache, //是否启用缓存
                switchRoles = switchRoles, //可切换角色列表
            };

            return Json(model);
        }
    }
}