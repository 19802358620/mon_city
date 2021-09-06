using System.ComponentModel.DataAnnotations;

namespace EKP.Service.Role
{
    //角色等级
    public enum RoleGrade
    {
        [Display(Name = "Root")]
        Root, 

        [Display(Name = "超级管理员")]
        Administrator,

        [Display(Name = "普通用户")]
        User 
    }

    public enum RoleKey
    {
        [Display(Name = "普通用户")]
        General,
        [Display(Name = "普通用户")]
        Service
    }
}
