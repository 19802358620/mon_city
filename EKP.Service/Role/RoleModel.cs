using System.ComponentModel.DataAnnotations;
using EKP.Entity;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Utilities;

namespace EKP.Service.Role
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class RolePagerParam : JqgridSqlParam<T_Role>
    {
        public string KeyWord { get; set; }

        public string Name { get; set; }

        public int? SiteId { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class RolePagerModel : T_Role
    {
        public string SiteName { get; set; }

        public int? GradeNum { get {
                if (!string.IsNullOrEmpty(this.Grade))
                    return EnumHelper.ToInt<RoleGrade>(this.Grade);

                return null;
        }}
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class RoleCreateModel : T_Role
    {
        private string grade = RoleGrade.User.ToString();

        [Required(ErrorMessage = "角色名不能为空")]
        public new string Name { get; set; }

        [Required(ErrorMessage = "角色等级不能为空")]
        public new string Grade { get { return grade;  } set { grade = value; } }

        [Required(ErrorMessage = "站点Id不能为空")]
        public new int? SiteId { get; set; }

    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class RoleEditModel : T_Role
    {
        [Required(ErrorMessage = "角色名不能为空")]
        public new string Name { get; set; }

        [Required(ErrorMessage = "角色等级不能为空")]
        public new string Grade { get; set; }
    }
}
