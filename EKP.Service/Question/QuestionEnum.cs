using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.SubjectQuestion
{
    /// <summary>
    /// 问题类型
    /// </summary>
    public enum QuestionType
    {
        single, //单选
        multi, //多选
        bit, //判断题
        fill, //填空题
        shortAnswer,//简单题
    }
}
