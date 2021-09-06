using System.ComponentModel.DataAnnotations;
using EKP.Entity;
using Ge.Infrastructure.Metronicv;

namespace EKP.Service.Class
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class ClassPagerParam : JqgridSqlParam<T_Class>
    {
        public string KeyWord { get; set; }

        public string Name { get; set; }

        public string ClassIds { get; set; }

        public int? CreateBy { get; set; }

        public int? SiteId { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public int UserId { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class ClassPagerModel : T_Class
    {
        public string SiteName { get; set; }
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class ClassCreateModel : T_Class
    {
        [Required(ErrorMessage = "班级名不能为空")]
        public new string Name { get; set; }

        [Required(ErrorMessage = "站点Id不能为空")]
        public new int? SiteId { get; set; }

    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class ClassEditModel : T_Class
    {
        [Required(ErrorMessage = "班级名不能为空")]
        public new string Name { get; set; }
    }

    /// <summary>
    /// 教师班级分页参数
    /// </summary>
    public class TeacherClassPagerParam : JqgridSqlParam<T_Class>
    {
        public string KeyWord { get; set; }

        public string Name { get; set; }

        public int? UserId { get; set; }

        public int? SiteId { get; set; }
    }
}
