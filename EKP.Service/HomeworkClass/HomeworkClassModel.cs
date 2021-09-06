using EKP.Entity;
using Ge.Infrastructure.Metronicv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.HomeworkClass
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class HomeworkClassPagerParm : JqgridSqlParam<T_HomeworkClass>
    {
        public string KeyWord { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public string TeacherName { get; set; }

    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class HomeworkClassPagerModel : T_HomeworkClass
    {
        public string TeacherName { get; set; }
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class HomeworkClassCreateModel : T_HomeworkClass
    {

    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class HomeworkClassEditModel : T_HomeworkClass
    {

    }
}
