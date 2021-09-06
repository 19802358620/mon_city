using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnnotationsExtensions;
using EKP.Entity;
using EKP.Service.Base.Tree;
using EKP.Service.User;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Model.DataAnnotations;
using Ge.Infrastructure.Utilities;

namespace EKP.Service.Detection
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class DetectionPagerParam : JqgridSqlParam<T_Detection>
    {
        public string Name { get; set; }

        public int? ParentId { get; set; }

        public int? ProjectId { get; set; }

        public int? SiteId { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class DetectionPagerModel : T_Detection
    {
        public List<DetectionPagerModel> Children { get; set; }
    }

    /// <summary>
    /// 树模型
    /// </summary>
    public class DetectionTree : TreeNode<DetectionTree>
    {

    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class DetectionCreateModel : T_Detection
    {
        [Required(ErrorMessage = "名称不能为空")]
        public new string Name { get; set; }

        [Required(ErrorMessage = "任务Id不能为空")]
        public new int? ProjectId { get; set; }

        [Required(ErrorMessage = "排序号不能为空")]
        public new int? SortIndex { get; set; }

        [Required(ErrorMessage = "站点Id不能为空")]
        public new int? SiteId { get; set; }
    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class DetectionEditModel : T_Detection
    {
        [Required(ErrorMessage = "名称不能为空")]
        public new string Name { get; set; }
    }

    /// <summary>
    /// 编辑某个字段模型
    /// </summary>
    public class DetectionEditFieldModel : T_Detection
    {

    }
}
