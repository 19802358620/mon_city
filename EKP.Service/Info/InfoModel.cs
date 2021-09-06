using System.ComponentModel.DataAnnotations;
using EKP.Entity;
using Ge.Infrastructure.Metronicv;

namespace EKP.Service.Info
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class InfoPagerParam : JqgridSqlParam<T_Info>
    {
        public int? KeyId { get; set; }

        public string IsWork { get; set; }

        public string KeyWord { get; set; }

        public string Value { get; set; }
        public string Key { get; set; }

        public string Type { get; set; }

        public string ContentType { get; set; }

        public string IsCover { get; set; } // 是否存在图片

        public int? SiteId { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class InfoPagerModel : T_Info
    {
        public string FirstType { get; set; }

        public string SecondType { get; set; }

        public string CreateTimeStr { get { return CreateTime.ToString("yyyy-MM-dd hh:mm:ss"); } }
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class InfoCreateModel : T_Info
    {
        [Required(ErrorMessage = "标题不能为空")]
        public new string Title { get; set; }

        [Required(ErrorMessage = "摘要不能为空")]
        public new string Abstract { get; set; }

        [Required(ErrorMessage = "内容不能为空")]
        public new string Content { get; set; }

        [Required(ErrorMessage = "数据来源不能为空")]
        public new string Resource { get; set; }

        [Required(ErrorMessage = "一级类型不能为空")]
        public new string Type { get; set; }

        [Required(ErrorMessage = "二级类型不能为空")]
        public new string ContentType { get; set; }
    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class InfoEditModel : T_Info
    {
        [Required(ErrorMessage = "标题不能为空")]
        public new string Title { get; set; }

        [Required(ErrorMessage = "摘要不能为空")]
        public new string Abstract { get; set; }

        [Required(ErrorMessage = "内容不能为空")]
        public new string Content { get; set; }

        [Required(ErrorMessage = "数据来源不能为空")]
        public new string Resource { get; set; }
    }
}
