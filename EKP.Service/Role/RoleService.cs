using System;
using System.Collections.Generic;
using System.Linq;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Utilities;

namespace EKP.Service.Role
{
    /// <summary>
    /// 角色管理接口
    /// </summary>
    public interface IRoleService : IEkpEntityService<T_Role>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(RolePagerParam param, params string[] includePath) where T : class, new();

        /// <summary>
        /// 获取某角色包含的所有角色
        /// </summary>
        List<T> IncludeRoles<T>(int roleId) where T : RolePagerModel, new();
    }

    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleService : EkpEntityService<T_Role>, IRoleService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(RolePagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent T_Role.* {0} from T_Role  {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_Role.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_Site"))
            {
                sqlSelect += " ,(T_Site.Name)SiteName ";
                sqlJoin += " left join T_Site on T_Site.Id = T_Role.SiteId ";
                sqlWhere += string.Format(" and (T_Site.IsDeleted = '{0}') ", IsDelete.undeleted.ToString());
            }

            //查询
            if (param.KeyWord != null)
                sqlWhere += string.Format(" and (T_Role.Name like '%{0}%' or T_Role.Description like '%{0}%') ", param.KeyWord);
            if (param.Name != null)
                sqlWhere += string.Format(" and T_Role.Name like '%{0}%' ", param.Name);
            if (param.SiteId != null)
                sqlWhere += string.Format(" and T_Role.SiteId = {0} ", param.SiteId);

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_Role.{0} {1} ", param.SortBy, param.SortOrder);

            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);

            var pager = EkpDbService.GetPager<T>(sql, param);
            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };
        }

        /// <summary>
        /// 获取某角色包含的所有角色
        /// </summary>
        public List<T> IncludeRoles<T>(int roleId) where T : RolePagerModel, new()
        {
            var role = this.GetEntiy(roleId);
            var roles = this.GetPager<T>(new RolePagerParam {
                Page = 1,
                PageSize = Int32.MaxValue,
                SiteId = role.SiteId
            }).Rows;
            var includeRoles = roles.FindAll(r => EnumHelper.ToInt<RoleGrade>(r.Grade) >= EnumHelper.ToInt<RoleGrade>(role.Grade));

            return includeRoles;
        }
    }
}
