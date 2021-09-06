using System;
using EKP.Entity;
using Ge.Infrastructure.Metronicv;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EKP.Service.LearningResource
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class ResourcePagerParam : JqgridSqlParam<T_LearningResource>
    {
        public string TeacherName { get; set; }
        public string ClassIds { get; set; }
    }
    /// <summary>
    /// 分页模型
    /// </summary>
    public class ResourcePagerModel:T_LearningResource
    {
        public string TeacherName { get; set; }   
        public string ClassName { get; set; }
        public string ClassId { get; set; }
    }
    /// <summary>
    /// 创建模型
    /// </summary>
    public class ResourceCreateModel:T_LearningResource
    {
        [Required(ErrorMessage ="资源名称不能为空")]
        public int ResourceId { get; set; }
        public string ClassNames { get; set; }
        public string ClassIds { get; set; }

    }

    public class ResourceEditModel:T_LearningResource
    {
        public string classId { get; set; }
    }
}
