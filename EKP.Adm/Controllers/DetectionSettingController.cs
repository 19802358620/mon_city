using EKP.Base.Controllers;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.DetectionSetting;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv.Dialog;
using Ge.Infrastructure.Mvc.Extensions;
using Ge.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EKP.Adm.Controllers
{
    /// <summary>
    /// 思考练习设置
    /// </summary>
    public class DetectionSettingController : EntityController<T_DetectionSetting>
    {
        private readonly IDetectionSettingService detectionSettingService = Ioc.GetService<IDetectionSettingService>();

        public DetectionSettingController()
            : base(Ioc.GetService<IDetectionSettingService>())
        {

        }

        /// <summary>
        /// 单条记录
        /// </summary>
        [HttpPost]
        public ActionResult Detail(int? id, int? detectionId, int? userId)
        {
            var model = (T_DetectionSetting)null;
            if(id > 0)
            {
                model = detectionSettingService.GetEntiy(Convert.ToInt32(id));
            }
            else if (detectionId > 0)
            {
                model = detectionSettingService.GetEntiy("DetectionId='{0}' and UserId='{1}' and IsDeleted='{2}'".Format2(detectionId, userId, IsDelete.undeleted));
            }

            var detail = ObjectMapper.Mapper<T_DetectionSetting, DetectionSettingPagerModel>(model);
            return Json(detail);
        }

        /// <summary>
        /// 创建post
        /// </summary>
        [HttpPost]
        public ActionResult Create(DetectionSettingCreateModel model)
        {
            //模型数据安全验证
            if (!this.ModelValidate(model).IsValid)
            {
                return Json(DialogFactory.Create(DialogType.Error, this.ModelValidate(model).FirstError.ErrorMessage));
            }

            var detectionSetting = detectionSettingService.GetEntiy("DetectionId='{0}' and UserId='{1}' and IsDeleted='{2}'".Format2(model.DetectionId, model.UserId, IsDelete.undeleted));
            if (detectionSetting == null)
            {
                detectionSetting = ObjectMapper.Mapper<DetectionSettingCreateModel, T_DetectionSetting>(model);
            }
            else
            {
                detectionSetting.ViewAnswerMode = model.ViewAnswerMode;
                detectionSetting.DetectionId = model.DetectionId;
                detectionSetting.UserId = model.UserId;
            }

            if(detectionSetting.Id > 0)
            {
                detectionSettingService.Update(detectionSetting);
            }
            else
            {
                detectionSettingService.Add(detectionSetting);
            }

            return Json(DialogFactory.Create(DialogType.Success));
        }
    }
}