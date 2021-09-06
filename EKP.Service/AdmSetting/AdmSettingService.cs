using System.Linq;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Metronicv;

namespace EKP.Service.AdmSetting
{
    /// <summary>
    /// 后台设置管理
    /// </summary>
    public interface IAdmSettingService : IEkpEntityService<T_AdmSetting>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(AdmSettingPagerParam param, params string[] includePath) where T : class, new();
    }

    /// <summary>
    /// 后台设置管理
    /// </summary>
    public class AdmSettingService : EkpEntityService<T_AdmSetting>, IAdmSettingService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(AdmSettingPagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent T_AdmSetting.* {0} from T_AdmSetting  {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_AdmSetting.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询

            //查询
            if (param.SiteId != null)
                sqlWhere += string.Format(" and T_AdmSetting.SiteId = '{0}' ", param.SiteId);
            if (param.UserId != null)
                sqlWhere += string.Format(" and T_AdmSetting.UserId = '{0}' ", param.UserId);

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_AdmSetting.{0} {1} ", param.SortBy, param.SortOrder);

            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);

            var pager = EkpDbService.GetPager<T>(sql, param);
            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };
        }
    }
}
