using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.DetectionSetting
{
    /// <summary>
    /// 查看成绩设置
    /// </summary>
    public enum DetectionSettingViewAnswerMode
    {
        [Display(Name = "不允许查看")]
        NotAllowed,

        [Display(Name = "考完后查看")]
        Tested
    }
}
