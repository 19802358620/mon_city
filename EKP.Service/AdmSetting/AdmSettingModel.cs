using System.ComponentModel.DataAnnotations;
using EKP.Entity;
using Ge.Infrastructure.Metronicv;

namespace EKP.Service.AdmSetting
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class AdmSettingPagerParam : JqgridSqlParam<T_AdmSetting>
    {
        public int? SiteId { get; set; }

        public int? UserId { get; set; }
    }

    /// <summary>
    /// 分页模型
    /// </summary>
    public class AdmSettingPagerModel : T_AdmSetting
    {

    }

    /// <summary>
    /// 创建模型
    /// </summary>
    public class AdmSettingCreateModel : T_AdmSetting
    {
        
    }

    /// <summary>
    /// 编辑模型
    /// </summary>
    public class AdmSettingEditModel : T_AdmSetting
    {
        
    }
}
