using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EKP.Entity;
using Ge.Infrastructure.Metronicv;

namespace EKP.Service.Authority
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class AuthorityPagerParam : JqgridSqlParam<T_Authority>
    {
        public string Type { get; set; }

        public int? RoleId { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class AuthorityPagerModel : T_Authority
    {
        public string RoleName { get; set; }
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class AuthorityCreateModel : T_Authority
    {
        [Required(ErrorMessage = "权限类型不能为空")]
        public new string Type { get; set; }

        [Required(ErrorMessage = "权限名称不能为空")]
        public new string Name { get; set; }

    }
}
