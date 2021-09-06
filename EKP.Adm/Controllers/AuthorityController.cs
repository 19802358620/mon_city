using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Role;
using EKP.Service.Authority;
using EKP.Service.User;
using EmitMapper;
using EmitMapper.MappingConfiguration;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Metronicv.Dialog;
using EKP.Service.DictValue;
using EKP.Service.Site;
using EKP.Base.Controllers;
using Ge.Infrastructure.Mvc.Extensions;
using Ge.Infrastructure.Utilities;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public class AuthorityController : EntityController<T_Authority>
    {
        private readonly IUserService userService = Ioc.GetService<IUserService>();
        private readonly ISiteService siteService = Ioc.GetService<ISiteService>();
        private readonly IAuthorityService authorityService = Ioc.GetService<IAuthorityService>();
        private readonly IRoleService roleService = Ioc.GetService<IRoleService>();
        private readonly IMenuManager menuManager = Ioc.GetService<MenuManager>();
        private readonly IDictValueService dictValueService = Ioc.GetService<IDictValueService>();

        public AuthorityController()
            : base(Ioc.GetService<IAuthorityService>())
        {


        }

        /// <summary>
        /// 分页post
        /// </summary>
        [HttpPost]
        public ActionResult Pager(AuthorityPagerParam param)
        {
            return Json(authorityService.GetPager<AuthorityPagerModel>(param));
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        public ActionResult RoleAuthority()
        {
            return View();
        }

        /// <summary>
        /// 获取后台权限post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AuthorityTree(int? roleId, string type)
        {
            var authorityNodes = new List<MenuNode>();
            var role = (T_Role)null;

            //获取所有后台权限
            authorityNodes = menuManager.GetMenus(string.Format("//{0}", type)).First().ChildEntitys;

            //根据角色等级过滤
            if(roleId > 0)
            {
                role = roleService.GetEntiy(Convert.ToInt32(roleId));
                authorityNodes = menuManager.FilterMenus(authorityNodes, 
                    a => string.IsNullOrEmpty(a.minRoleGrade) || EnumHelper.ToInt<RoleGrade>(role.Grade) <= EnumHelper.ToInt<RoleGrade>(a.minRoleGrade));
            }

            //自定义模型映射规则
            var mapConfig = new DefaultMapConfig().MatchMembers((x, y) =>
            {
                if (x == "name" && y == "id")
                {
                    return true;
                }
                else if (x == "title" && y == "text")
                {
                    return true;
                }
                else if (x == "ChildEntitys" && y == "children")
                {
                    return true;
                }
                return x == y;
            });
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<List<MenuNode>, List<JsTreeNode>>(mapConfig);
            var result = mapper.Map(authorityNodes);
            return Json(result);
        }

        /// <summary>
        /// 批量添加后台权限
        /// </summary>
        [HttpPost]
        public ActionResult CreateAuthoritys(int? roleId, string type, List<AuthorityCreateModel> models)
        {
            if (models == null)
            {
                models = new List<AuthorityCreateModel>();
            }

            //模型数据安全验证
            foreach (var model in models)
            {
                if (!this.ModelValidate(model).IsValid)
                {
                    return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
                }
            }

            var role = roleService.GetEntiy(Convert.ToInt32(roleId));
            var deleteAuthoritys = authorityService.GetList("RoleId='{0}' and Type = '{1}'".Format2(roleId, type));
            var addAuthoritys = ObjectMapper.Mapper<List<AuthorityCreateModel>, List<T_Authority>>(models);
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                deleteAuthoritys.ForEach(ra => trans.Delete(ra));
                addAuthoritys.ForEach(ra => trans.Add(ra));
                trans.SaveChange();
            }

            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "操作成功！"));
        }

    }
}