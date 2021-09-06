using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.Project
{
    /// <summary>
    /// 项目类型
    /// </summary>
    public enum ProjectType
    {
        [Description("项目")]
        project,

        [Description("任务")]
        task
    }
}
