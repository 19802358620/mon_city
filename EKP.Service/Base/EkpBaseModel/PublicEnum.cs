using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.Base.EkpBaseModel
{
    public class PublicEnum
    {
        #region 系统默认枚举
        /// <summary>
        /// 状态
        /// </summary>
        public enum Status
        {
            [Display(Name = "未激活")]
            notactivate,
            [Display(Name = "正常")]
            normal,
            [Display(Name = "停用")]
            stop
        }

        /// <summary>
        /// 用户属性
        /// </summary>
        public enum Attribute
        {
            [Display(Name = "个人用户")]
            person,
            [Display(Name = "机构用户")]
            organ,
            [Display(Name = "企业平台用户")]
            ekp
        }
        #endregion
    }
}
