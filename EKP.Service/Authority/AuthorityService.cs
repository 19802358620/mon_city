using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EKP.Entity;
using EKP.Service.Base;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Metronicv;

namespace EKP.Service.Authority
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public interface IAuthorityService : IEkpEntityService<T_Authority>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(AuthorityPagerParam param, params string[] includePath) where T : class, new();
    }

    /// <summary>
    /// 权限管理
    /// </summary>
    public class AuthorityService : EkpEntityService<T_Authority>, IAuthorityService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(AuthorityPagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent T_Authority.* {0} from T_Authority  {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where 1=1 "),
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_Role"))
            {
                sqlSelect += " ,(T_Role.Name)RoleName ";
                sqlJoin += " left join T_Role on T_Role.Id = T_Authority.RoleId ";
            }

            //查询
            if (param.Type != null)
                sqlWhere += string.Format(" and T_Authority.Type = '{0}' ", param.Type);
            if (param.RoleId != null)
                sqlWhere += string.Format(" and T_Authority.RoleId = '{0}' ", param.RoleId);

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_Authority.{0} {1} ", param.SortBy, param.SortOrder);

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
