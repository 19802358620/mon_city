using System.ComponentModel.DataAnnotations;
using EKP.Entity;
using EKP.Service.Base.Tree;
using Ge.Infrastructure.Metronicv;

namespace EKP.Service.DictValue
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class DictValuePagerParam : JqgridSqlParam<T_DictValue>
    {
        public int? KeyId { get; set; }

        public string IsWork { get; set; }

        public string KeyWord { get; set; }

        public string Value { get; set; }
        public string Key { get; set; }

        public int? ParentId { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class DictValuePagerModel : T_DictValue
    {

    }

    /// <summary>
    /// 树模型
    /// </summary>
    public class DictValueTree : TreeNode<DictValueTree>
    {

    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class DictValueCreateModel : T_DictValue
    {
        [Required(ErrorMessage = "数据名称不能为空")]
        public new string Value { get; set; }

        [Required(ErrorMessage = "数据显示名称不能为空")]
        public new string ShowValue { get; set; }

        [Required(ErrorMessage = "是否启用不能为空")]
        public new string IsWork { get; set; }
    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class DictValueEditModel : T_DictValue
    {
        [Required(ErrorMessage = "数据名称不能为空")]
        public new string Value { get; set; }

        [Required(ErrorMessage = "数据显示名称不能为空")]
        public new string ShowValue { get; set; }

        [Required(ErrorMessage = "是否启用不能为空")]
        public new string IsWork { get; set; }
    }
}
