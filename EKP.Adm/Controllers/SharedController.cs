using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKP.Base.Controllers;
using EKP.Base.Identity;
using EKP.Base;
using EKP.Entity;
using EKP.Adm;
using EKP.Service.Site;
using Ge.Infrastructure.Ioc;
using EKP.Service.DictValue;
using EKP.Service.Role;
using EKP.Service.Authority;
using EKP.Service.AdmSetting;
using Ge.Infrastructure.Extensions;
using EKP.Service.Base.EkpBaseModel;
using EKP.Base.Cache;
using EKP.Service.Info;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 共享控制器
    /// </summary>
    public class SharedController : BaseController
    {
        private readonly ISiteService siteService = Ioc.GetService<ISiteService>();
        private readonly IMenuManager menuManager = Ioc.GetService<MenuManager>();
        private readonly IDictValueService dictValueService = Ioc.GetService<IDictValueService>();
        private readonly IRoleService roleService = Ioc.GetService<IRoleService>();
        private readonly IAuthorityService authorityService = Ioc.GetService<IAuthorityService>();
        private readonly IAdmSettingService admSettingService = Ioc.GetService<IAdmSettingService>();

        /// <summary>
        /// 建设中
        /// </summary>
        public ActionResult Building()
        {
            return View();
        }

        /// <summary>
        /// 错误页面
        /// </summary>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// 获取公共信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAppInfo()
        {
            var appInfo = CacheManager.GetByCache<object>("_{0}.Adm.Shared.GetAppInfo_".Format2(App.Site.Domain), s =>
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
                        LoginRoleKey = loginRole.Key,
                        LoginRoleName = loginRole.Name,
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

                //后台风格风格配置
                var admSetting = (T_AdmSetting)null;
                if (loginInUser != null)
                {
                    admSetting = admSettingService.GetEntiy("UserId = '{0}' and IsWork = '{1}' and IsDeleted = '{2}'"
                        .Format2(loginInUser.Id, IsYes.yes, IsDelete.undeleted));
                }

                
                return new
                {
                    site = App.Site,//当前站点
                    area = "Adm",//当前区域
                    loginUser = loginUser, //当前登录人
                    isCache = BaseConfig.IsCache, //是否启用缓存
                    menus = loginInUser == null ? null : menuManager.GetLoginMenus(App.Site.Id), //后台菜单
                    switchRoles = switchRoles, //可切换角色列表
                    admSetting = admSetting, //后台风格设置
                };
            }, 30);

            return Json(appInfo);
        }
    }
}