using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Role;
using EKP.Base.Controllers;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Utilities;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleController : EntityController<T_Role>
    {
        private readonly IRoleService roleService = Ioc.GetService<IRoleService>();

        public RoleController()
            : base(Ioc.GetService<IRoleService>())
        {

        }

        /// <summary>
        /// 分页
        /// </summary>
        public ActionResult Pager()
        {
            return View();
        }

        /// <summary>
        /// 分页post
        /// </summary>
        [HttpPost]
        public ActionResult Pager(RolePagerParam param)
        {
            return Json(roleService.GetPager<RolePagerModel>(param, "T_Site"));
        }

        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var role = roleService.GetEntiy(id);
            var detail = ObjectMapper.Mapper<T_Role, RolePagerModel>(role);
            return Json(detail);
        }

        /// <summary>
        /// 创建
        /// </summary>
        public ActionResult Create()
        {
            return PartialView();
        }

        /// <summary>
        /// 创建post
        /// </summary>
        [HttpPost]
        public ActionResult Create(RoleCreateModel model)
        {
            return Json(base.Create(model));
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public ActionResult Edit()
        {
            return PartialView();
        }

        /// <summary>
        /// 编辑post
        /// </summary>
        [HttpPost]
        public ActionResult Edit(RoleEditModel model)
        {
            return Json(Edit(string.Format("id = {0}", model.Id), model,
                "Name", "Description"));
        }

        /// <summary>
        /// 删除post
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            var roles = roleService.GetList(ids.ToArray());
            return Json(base.Delete(ids.ToArray()));
        }

        /// <summary>
        /// 角色权限
        /// </summary>
        public ActionResult RoleAuth()
        {
            return View();
        }
    }
}