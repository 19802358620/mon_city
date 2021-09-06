using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Role;
using EKP.Service.Site;
using EKP.Base.Identity;
using EKP.Base.Controllers;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Mvc.Extensions;
using Ge.Infrastructure.Utilities;
using Microsoft.AspNet.Identity.Owin;
using EKP.Web.Areas.Base.Application;
using EKP.Adm;
using EKP.Base;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 站点管理
    /// </summary>
    public class SiteController : EntityController<T_Site>
    {
        private readonly ISiteService siteService = Ioc.GetService<ISiteService>();

        public SiteController()
            : base(Ioc.GetService<ISiteService>())
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
        public ActionResult Pager(SitePagerParam param)
        {
            return Json(siteService.GetPager<SitePagerModel>(param, new string[] { "T_DictValue" }));
        }

        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int id)
        {
            var site = siteService.GetEntiy(id);
            return Json(site);
        }

        /// <summary>
        /// 创建
        /// </summary>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 创建post
        /// </summary>
        [HttpPost]
        public ActionResult Create(SiteCreateModel model)
        {
            //模型数据安全验证
            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }
            var site = ObjectMapper.Mapper<SiteCreateModel, T_Site>(model);

            //事务
            using (var trans = EkpDbService.CreateEntityTrans())
            {
                trans.Add(site);
                trans.SaveChange();
            }

            return Json(DialogFactory.Create(DialogType.Success, string.Empty, "创建成功！"));
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public ActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// 编辑post
        /// </summary>
        [HttpPost]
        public ActionResult Edit(SiteEditModel model)
        {
            return Json(Edit("id = {0}".Format2(model.Id), model,
                "Name", "Logo", "TopBanner", "Favicon", "Province", "City",
                "Telephone", "Fax", "Email", "Copyright", "FundSupport", "Technical", "EnWeChat"));
        }

        /// <summary>
        /// 编辑Seo优化post
        /// </summary>
        [HttpPost]
        public ActionResult EditSeo(SiteSeoEditModel model)
        {
            return Json(Edit("id = {0}".Format2(model.Id), model, "MetaKeywords", "MetaDescription"));
        }

        /// <summary>
        /// 编辑关于我们
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditAboutUs(SiteAboutUsEditModel model)
        {
            return Json(Edit("id = {0}".Format2(model.Id), model, "AboutUs"));
        }

        /// <summary>
        /// 编辑帮助中心
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditHelper(SiteHelperEditModel model)
        {
            return Json(Edit("id = {0}".Format2(model.Id), model, "Helper"));
        }

        /// <summary>
        /// 编辑帮助中心
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditCourseIntroduce(SiteCourseIntroduceEditModel model)
        {
            return Json(Edit("id = {0}".Format2(model.Id), model, "CourseIntroduce"));
        }

        /// <summary>
        /// 删除post
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            return Json(base.Delete(ids.ToArray()));
        }

        /// <summary>
        /// 网站首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            return View();
        }

    }
}