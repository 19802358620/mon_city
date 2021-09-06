using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.Authority
{
    //权限类型
    public enum AuthorityType
    {
        [Display(Name = "中心站")]
        System,

        [Display(Name = "分站")]
        Substation,

        [Display(Name = "资源")]
        Resource
    }
}
