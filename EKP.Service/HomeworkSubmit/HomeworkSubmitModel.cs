using EKP.Entity;
using Ge.Infrastructure.Metronicv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.HomeworkSubmit
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class HomeworkSubmitPagerParm : JqgridSqlParam<T_HomeworkSubmit>
    {
        public string KeyWord { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public string StduentName { get; set; }

        public int HomeworkId { get; set; }

        public string ScoreDegree { get; set; }



    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class HomeworkSubmitPagerModel : T_HomeworkSubmit
    {
        public string StudentName { get; set; }
        public string HomeworkName { get; set; }

        public string ScoreDegree { get; set; }
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class HomeworkSubmitCreateModel : T_HomeworkSubmit
    {

    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class HomeworkSubmitEditModel : T_HomeworkSubmit
    {

    }
}
