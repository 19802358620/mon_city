using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EKP.Entity;
using Ge.Infrastructure.Metronicv;

namespace EKP.Service.LearningResourceClass
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class ResourceClassPagerParam : JqgridSqlParam<T_LearningResourceClass>
    {
        public string KeyWord { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string TearcherName { get; set; }
    }
    /// <summary>
    /// 分页模型
    /// </summary>
    public class ResourceClassPagerModel : T_LearningResourceClass
    {
        public string TeacherName { get; set; }
    }

    public class ResourceClassCreateModel : T_LearningResourceClass
    { 
    
    }
    public class ResourceClassEditModel:T_LearningResourceClass
    {

    }

}
