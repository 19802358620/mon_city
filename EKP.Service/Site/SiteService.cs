using System;
using System.Linq;
using System.Runtime.InteropServices;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Role;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;

namespace EKP.Service.Site
{
    /// <summary>
    /// 站点管理
    /// </summary>
    public interface ISiteService : IEkpEntityService<T_Site>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(SitePagerParam param, params string[] includePath) where T : class, new();
    }

    /// <summary>
    /// 站点管理
    /// </summary>
    public class SiteService : EkpEntityService<T_Site>, ISiteService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(SitePagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent T_Site.* {0} from T_Site {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_Site.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_DictValue"))
            {
                sqlSelect += " ,(T_DictValue.ShowValue)TypeShowValue ";
                sqlJoin += " left join T_DictValue on T_DictValue.Value = T_Site.Type ";
                sqlWhere += string.Format(" and T_DictValue.KeyId = (select Id from T_DictKey where [key] = 'SiteType' and T_DictKey.IsDeleted = '{0}') and T_DictValue.IsDeleted = '{0}' ", IsDelete.undeleted.ToString());
            }

            //查询
            if (!string.IsNullOrEmpty(param.KeyWord))
            {
                sqlWhere += string.Format(" and (T_Site.Name like '%{0}%') ", param.KeyWord);
            }
            if (!string.IsNullOrEmpty(param.Domain))
            {
                sqlWhere += string.Format(" and (T_Site.Domain like '%{0}%') ", param.Domain);
            }
            if (param.ParentId != null)
            {
                sqlWhere += string.Format(" and (T_Site.ParentId = '{0}') ", param.ParentId);
            }
            if (param.SiteId != null)
            {
                sqlWhere += string.Format(" and (T_Site.SiteId = '{0}') ", param.SiteId);
            }

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by {0}{1} {2} ",
                    param.SortBy.Contains(".") ? string.Empty : "T_Site.", param.SortBy, param.SortOrder);

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
