using System.ComponentModel.DataAnnotations;
using EKP.Entity;
using Ge.Infrastructure.Metronicv;

namespace EKP.Service.ProjectInfo
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class ProjectInfoPagerParam : JqgridSqlParam<T_ProjectInfo>
    {
        public string KeyWord { get; set; }

        public string Picture { get; set; }

        public string Video { get; set; }

        public string Type { get; set; }

        public int? ProjectId { get; set; }

        public int? SiteId { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class ProjectInfoPagerModel : T_ProjectInfo
    {
        public string SiteName { get; set; }
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class ProjectInfoCreateModel : T_ProjectInfo
    {
        [Required(ErrorMessage = "类型不能为空")]
        public new string Type { get; set; }

        [Required(ErrorMessage = "ProjectId不能为空")]
        public new int? ProjectId { get; set; }

        [Required(ErrorMessage = "站点Id不能为空")]
        public new int? SiteId { get; set; }

        [Required(ErrorMessage = "图片名字不能为空")]
        public new string Name { get; set; }

    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class ProjectInfoEditModel : T_ProjectInfo
    {
        [Required(ErrorMessage = "类型不能为空")]
        public new string Type { get; set; }
    }
}
