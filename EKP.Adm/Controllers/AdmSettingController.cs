using System;
using System.Collections.Generic;
using System.Web.Mvc;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.AdmSetting;
using EKP.Base.Controllers;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Utilities;
using Ge.Infrastructure.Extensions;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 后台设置管理
    /// </summary>
    public class AdmSettingController : EntityController<T_AdmSetting>
    {
        private readonly IAdmSettingService admSettingService = Ioc.GetService<IAdmSettingService>();

        public AdmSettingController()
            : base(Ioc.GetService<IAdmSettingService>())
        {

        }

        /// <summary>
        /// 更改模板设置post
        /// </summary>
        [HttpPost]
        public ActionResult ChangeTemplate(AdmSettingCreateModel model)
        {
            if (model.UserId == null)
            {
                return Json(DialogFactory.Create(DialogType.Error, "参数错误"));
            }

            if (string.IsNullOrEmpty(model.Layout))
            {
                return Json(DialogFactory.Create(DialogType.Error, "参数错误"));
            }

            var admSettings = admSettingService.GetList("UserId='{0}' and IsDeleted='{1}'".Format2(model.UserId, IsDelete.undeleted));
            var admSetting = admSettingService.GetEntiy("UserId='{0}' and Layout='{1}' and IsDeleted='{2}'".Format2(model.UserId, model.Layout, IsDelete.undeleted));
            admSettings.ForEach(ad => ad.IsWork = IsYes.no.ToString());

            if (admSetting == null)
            {
                using (var trans = EkpDbService.CreateEntityTrans())
                {
                    admSetting = ObjectMapper.Mapper<AdmSettingCreateModel, T_AdmSetting>(model);
                    admSetting.IsWork = IsYes.yes.ToString();
                    admSettings.ForEach(trans.Update);
                    trans.Add(admSetting);
                    trans.SaveChange();
                }
            }
            else
            {
                using (var trans = EkpDbService.CreateEntityTrans())
                {
                    admSetting.IsWork = IsYes.yes.ToString();
                    admSettings.ForEach(trans.Update);
                    trans.Update(admSetting);
                    trans.SaveChange();
                }
            }

            return Json(DialogFactory.Create(DialogType.Success));
        }

        /// <summary>
        /// 更改模板设置post
        /// </summary>
        [HttpPost]
        public ActionResult ChangeTemplateSetting(AdmSettingCreateModel model)
        {
            if(model.UserId == null)
            {
                return Json(DialogFactory.Create(DialogType.Error, "参数错误"));
            }
            if (string.IsNullOrEmpty(model.Layout))
            {
                return Json(DialogFactory.Create(DialogType.Error, "参数错误"));
            }

            return Json(base.CreateSingle(model, "Layout='{0}' and UserId='{1}' and IsDeleted = '{2}'"
                .Format2(model.Layout, model.UserId, IsDelete.undeleted), new string[] { "Setting" }));
        }

        /// <summary>
        /// 删除post
        /// </summary>
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            var AdmSettings = admSettingService.GetList(ids.ToArray());
            return Json(base.Delete(ids.ToArray()));
        }
    }
}