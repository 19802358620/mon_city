using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using EKP.Entity;
using Ge.Infrastructure.Metronicv;

namespace EKP.Service.Site
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class SitePagerParam : JqgridSqlParam<T_Site>
    {
        public string KeyWord { get; set; }

        public string Domain { get; set; }

        public int? ParentId { get; set; }

        public int? SiteId { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class SitePagerModel : T_Site
    {
        public string TypeShowValue { get; set; }
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class SiteCreateModel : T_Site
    {
        [Required(ErrorMessage = "父站点Id不能为空")]
        public new int? ParentId { get; set; }

        [Required(ErrorMessage = "域名不能为空")]
        public new string Domain { get; set; }
    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class SiteEditModel : T_Site
    {
        [Required(ErrorMessage = "域名不能为空")]
        public new string Domain { get; set; }
    }

    /// <summary>
    /// 编辑Seo模型
    /// </summary>
    public class SiteSeoEditModel
    {
        public int Id { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }
    }

    /// <summary>
    /// 编辑关于我们
    /// </summary>
    public class SiteAboutUsEditModel
    {
        public int Id { get; set; }

        public string AboutUs { get; set; }
    }

    /// <summary>
    /// 编辑帮助中心
    /// </summary>
    public class SiteHelperEditModel
    {
        public int Id { get; set; }

        public string Helper { get; set; }
    }

    /// <summary>
    /// 编辑课程介绍
    /// </summary>
    public class SiteCourseIntroduceEditModel
    {
        public int Id { get; set; }

        public string CourseIntroduce { get; set; }
    }

    /// <summary>
    /// 编辑皮肤模型
    /// </summary>
    public class SiteThemeEditModel
    {
        public int Id { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }
    }

}
