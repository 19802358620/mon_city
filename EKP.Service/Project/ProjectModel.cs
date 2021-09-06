using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnnotationsExtensions;
using EKP.Entity;
using EKP.Service.Base.Tree;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Model.DataAnnotations;

namespace EKP.Service.Project
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class ProjectPagerParam : JqgridSqlParam<T_Project>
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public int? ParentId { get; set; }

        public int? SiteId { get; set; }

        public string Fields { get; set; } = "T_Project.*";
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class ProjectPagerModel : T_Project
    {
        public List<ProjectPagerModel> Children { get; set; }
    }

    /// <summary>
    /// 树模型
    /// </summary>
    public class ProjectTree : TreeNode<ProjectTree>
    {
        public string Type { get; set; }
        public string LearningTarget { get; set; }
        public string LearningImport { get; set; }
        public string TaskDescription { get; set; }
        public string TaskPrepare { get; set; }
        public string TaskImple { get; set; }
        public string ExtendedLearning { get; set; }
        public string ProjectGuidance { get; set; }
        public int? SortIndex { get; set; }
        public string CreateIp { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string IsDeleted { get; set; }
        public int? SiteId { get; set; }
    }

    /// <summary>
    /// jstree树模型
    /// </summary>
    public class ProjectJsTTree : JsTreeNode
    {
        public int? ParentId { get; set; }
        public string Type { get; set; }
        public string LearningTarget { get; set; }
        public string LearningImport { get; set; }
        public string TaskDescription { get; set; }
        public string TaskPrepare { get; set; }
        public string TaskImple { get; set; }
        public string ExtendedLearning { get; set; }
        public string ProjectGuidance { get; set; }
        public int? SortIndex { get; set; }
        public string CreateIp { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string IsDeleted { get; set; }
        public int? SiteId { get; set; }

        public new List<ProjectJsTTree> children { get; set; }
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class ProjectCreateModel : T_Project
    {
        [Required(ErrorMessage = "名称不能为空")]
        public new string Name { get; set; }

        [Required(ErrorMessage = "类型不能为空")]
        public new string Type { get; set; }

        [Required(ErrorMessage = "排序号不能为空")]
        public new int? SortIndex { get; set; }

        [Required(ErrorMessage = "站点Id不能为空")]
        public new int? SiteId { get; set; }
    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class ProjectEditModel : T_Project
    {
        [Required(ErrorMessage = "名称不能为空")]
        public new string Name { get; set; }
    }

    /// <summary>
    /// 编辑某个字段模型
    /// </summary>
    public class ProjectEditFieldModel : T_Project
    {

    }
}
