using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using EKP.Entity;
using EKP.Service.Class;
using EKP.Service.Role;
using EKP.Service.Subject;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Utilities;

namespace EKP.Service.User
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class UserPagerParam : JqgridSqlParam<T_User>
    {
        public string KeyWord { get; set; }
        public string Gender { get; set; }
        public int? RoleId { get; set; }
        public int? SiteId { get; set; }
        public string Status { get; set; }
        public string Grade { get; set; }
        public int? ClassId { get; set; }
        public string ClassIds { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class UserPagerModel : T_User
    {
        public string RoleName { get; set; }
        public string RoleKey { get; set; }
        public string SiteName { get; set; }
        public string ShowStatus
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Status))
                {
                    var userStatus = EnumHelper.GetEnum<UserStatus>(this.Status);
                    return EnumHelper.GetEnumDescription(userStatus);
                }

                return string.Empty;
            }
        }

        public string ShowClasses { get; set; }

        public List<ClassPagerModel> Classes { get; set; } 
    }

    /// <summary>
    /// 用户登录模型
    /// </summary>
    public class UserLoginModel
    {
        [Required(ErrorMessage = "账号不能为空")]
        public string Name { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        [Required(ErrorMessage = "登录方式不能为空")]
        public int LoginMethod { get; set; } //枚举类型

        [Required(ErrorMessage = "平台Id不能为空")]
        public int CompanyId { get; set; }
    }

    /// <summary>
    /// 用户注册模型
    /// </summary>
    public class UserRegisterModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "账号不能为空")]
        [MinLength(6, ErrorMessage = "账号应在 6-20 个字符之间")]
        [MaxLength(20, ErrorMessage = "账号应在 6-20 个字符之间")]
        [RegularExpression("^[\u4e00-\u9fa5_a-zA-Z0-9]+$", ErrorMessage = "账号只能由字母、数字、下划线组成")]
        public string Account { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(6, ErrorMessage = "密码应在 6-20 个字符之间")]
        [MaxLength(20, ErrorMessage = "密码应在 6-20 个字符之间")]
        [RegularExpression("^[0-9a-zA-Z_]*$", ErrorMessage = "密码只能由字母、数字、下划线组成")]
        public string PassWord { get; set; }

        [Required(ErrorMessage = "请再次输入密码")]
        [EqualTo("PassWord", ErrorMessage = "前后密码不一致")]
        public string PassWord2 { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessage = "注册方式不能为空")]
        public string RegisterType { get; set; }

        public int SiteId { get; set; }
        public int CreateBy { get; set; }
        public string CreateIp { get; set; }
        public DateTime CreateTime { get; set; }
        public string IsDeleted { get; set; }
        public string ValidCode { get; set; }
    }

    /// <summary>
    /// 修改密码模型
    /// </summary>
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "账号不能为空")]
        public string Account { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(6, ErrorMessage = "密码应在 6-20 个字符之间")]
        [MaxLength(20, ErrorMessage = "密码应在 6-20 个字符之间")]
        [RegularExpression("^[0-9a-z_]*$", ErrorMessage = "账号只能由字母、数字、下划线组成")]
        public string Password { get; set; }

        [EqualTo("Password", ErrorMessage = "前后密码不一致")]
        public string Password2 { get; set; }
    }

    /// <summary>
    /// 修改密码模型
    /// </summary>
    public class ChangePwdModel
    {
        [Required(ErrorMessage = "账号不能为空")]
        public string Account { get; set; }

        [Required(ErrorMessage = "旧密码不能为空")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(6, ErrorMessage = "密码应在 6-20 个字符之间")]
        [MaxLength(20, ErrorMessage = "密码应在 6-20 个字符之间")]
        [RegularExpression("^[0-9a-z_]*$", ErrorMessage = "账号只能由字母、数字、下划线组成")]
        public string Password { get; set; }

        [EqualTo("Password", ErrorMessage = "前后密码不一致")]
        public string Password2 { get; set; }
    }

    #region 管理员

    /// <summary>
    /// 创建模型
    /// </summary>
    public class UserCreateModel : T_User
    {
        [Required(ErrorMessage = "账号不能为空")]
        [MinLength(5, ErrorMessage = "账号应在 5-20 个字符之间")]
        [MaxLength(20, ErrorMessage = "账号应在 5-20 个字符之间")]
        [RegularExpression("^[0-9a-zA-Z_]*$", ErrorMessage = "账号只能由字母、数字、下划线组成")]
        public new string Account { get; set; }

        [Required(ErrorMessage = "状态不能为空")]
        public new string Status { get; set; }

        [Required(ErrorMessage = "角色不能为空")]
        public new int? RoleId { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(6, ErrorMessage = "密码应在 6-20 个字符之间")]
        [MaxLength(20, ErrorMessage = "密码应在 6-20 个字符之间")]
        [RegularExpression("^[0-9a-zA-Z_]*$", ErrorMessage = "密码只能由字母、数字、下划线组成")]
        public new string PassWord { get; set; }

        [Required(ErrorMessage = "请再次输入密码")]
        [EqualTo("PassWord", ErrorMessage = "前后密码不一致")]
        public string PassWord2 { get; set; }

        [Required(ErrorMessage = "平台Id不能为空")]
        public new int? SiteId { get; set; }
    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class UserEditModel : T_User
    {
        [Phone(ErrorMessage = "请输入有效电话号码")]
        public new string Phone { get; set; }

        [Required(ErrorMessage = "角色不能为空")]
        public new int? RoleId { get; set; }
    }

    #endregion

    #region 教师

    /// <summary>
    /// 创建模型
    /// </summary>
    public class TeacherCreateModel : T_User
    {
        [Required(ErrorMessage = "账号不能为空")]
        [MinLength(5, ErrorMessage = "账号应在 5-20 个字符之间")]
        [MaxLength(20, ErrorMessage = "账号应在 5-20 个字符之间")]
        [RegularExpression("^[0-9a-zA-Z_]*$", ErrorMessage = "账号只能由字母、数字、下划线组成")]
        public new string Account { get; set; }

        [Required(ErrorMessage = "状态不能为空")]
        public new string Status { get; set; }

        [Phone(ErrorMessage = "请输入有效电话号码")]
        public new string MobileNo { get; set; }

        [Required(ErrorMessage = "角色不能为空")]
        public new int? RoleId { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(6, ErrorMessage = "密码应在 6-20 个字符之间")]
        [MaxLength(20, ErrorMessage = "密码应在 6-20 个字符之间")]
        [RegularExpression("^[0-9a-zA-Z_]*$", ErrorMessage = "密码只能由字母、数字、下划线组成")]
        public new string PassWord { get; set; }

        [Required(ErrorMessage = "请再次输入密码")]
        [EqualTo("PassWord", ErrorMessage = "前后密码不一致")]
        public string PassWord2 { get; set; }

        [Required(ErrorMessage = "平台Id不能为空")]
        public new int? SiteId { get; set; }
    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class TeacherEditModel : T_User
    {
        [Phone(ErrorMessage = "请输入有效电话号码")]
        public new string Phone { get; set; }

        [Required(ErrorMessage = "角色不能为空")]
        public new int? RoleId { get; set; }
    }

    #endregion

    #region 学生

    /// <summary>
    /// 创建模型
    /// </summary>
    public class StudentCreateModel : T_User
    {
        [Required(ErrorMessage = "账号不能为空")]
        [MinLength(5, ErrorMessage = "账号应在 5-20 个字符之间")]
        [MaxLength(20, ErrorMessage = "账号应在 5-20 个字符之间")]
        [RegularExpression("^[0-9a-zA-Z_]*$", ErrorMessage = "账号只能由字母、数字、下划线组成")]
        public new string Account { get; set; }

        [Required(ErrorMessage = "状态不能为空")]
        public new string Status { get; set; }

        [Phone(ErrorMessage = "请输入有效电话号码")]
        public new string MobileNo { get; set; }

        [Required(ErrorMessage = "角色不能为空")]
        public new int? RoleId { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(6, ErrorMessage = "密码应在 6-20 个字符之间")]
        [MaxLength(20, ErrorMessage = "密码应在 6-20 个字符之间")]
        [RegularExpression("^[0-9a-zA-Z_]*$", ErrorMessage = "密码只能由字母、数字、下划线组成")]
        public new string PassWord { get; set; }

        [Required(ErrorMessage = "请再次输入密码")]
        [EqualTo("PassWord", ErrorMessage = "前后密码不一致")]
        public string PassWord2 { get; set; }

        [Required(ErrorMessage = "平台Id不能为空")]
        public new int? SiteId { get; set; }
    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class StudentEditModel : T_User
    {
        [Phone(ErrorMessage = "请输入有效电话号码")]
        public new string Phone { get; set; }

        [Required(ErrorMessage = "角色不能为空")]
        public new int? RoleId { get; set; }
    }

    #endregion
}