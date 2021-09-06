using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.Subject
{
    /// <summary>
    /// 题目类型
    /// </summary>
    public enum SubjectType
    {
        [Description("单选题")]
        single, 
        
        [Description("多选题")]
        multi, 

        [Description("判断题")]
        bit,

        [Description("填空题")]
        fill,

        [Description("简答题")]
        shortAnswer,
    }

    /// <summary>
    /// 题目权限
    /// </summary>
    public enum SubjectViewAuthority
    {
        [Description("完全开放")]
        pub,

        [Description("仅考试开放")]
        Test
    }
}
