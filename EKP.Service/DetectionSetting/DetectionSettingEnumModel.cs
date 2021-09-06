using EKP.Entity;
using EKP.Service.Class;
using EKP.Service.Detection;
using EKP.Service.DetectionReply;
using EKP.Service.User;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EKP.Service.DetectionSetting
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class DetectionSettingPagerParam : JqgridSqlParam<T_DetectionSetting>
    {
        public string KeyWord { get; set; }

        public int? DetectionId { get; set; }

        public int? UserId { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class DetectionSettingPagerModel : T_DetectionSetting
    {
        public string ShowViewAnswerMode
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ViewAnswerMode))
                {
                    var viewAnswerMode = EnumHelper.GetEnum<DetectionSettingViewAnswerMode>(this.ViewAnswerMode);
                    return EnumHelper.GetEnumDisplay(viewAnswerMode);
                }

                return string.Empty;
            }
        }
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class DetectionSettingCreateModel : T_DetectionSetting
    {
        [Required(ErrorMessage = "查看答案设置不能为空")]
        public new string ViewAnswerMode { get; set; }

        [Required(ErrorMessage = "项目Id不能为空")]
        public new int? DetectionId { get; set; }

        [Required(ErrorMessage = "用户Id不能为空")]
        public new int? UserId { get; set; }
    }
}
