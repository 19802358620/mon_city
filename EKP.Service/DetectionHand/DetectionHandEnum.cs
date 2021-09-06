using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.DetectionHand
{
    /// <summary>
    /// 练习状态
    /// </summary>
    public enum DetectionHandStatus
    {
        [Description("已交卷")]
        Examed,

        [Description("已批改")]
        MakeScore
    }
}
