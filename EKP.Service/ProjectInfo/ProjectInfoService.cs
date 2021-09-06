using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.Base.Tree;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Metronicv;
using Newtonsoft.Json2.Linq;

namespace EKP.Service.ProjectInfo
{
    /// <summary>
    /// 项目信管理
    /// </summary>
    public interface IProjectInfoService : IBaseTreeService<T_ProjectInfo>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(ProjectInfoPagerParam param, params string[] includePath) where T : class, new();

    }

    /// <summary>
    /// 项目信管理
    /// </summary>
    public class ProjectInfoService : BaseTreeService<T_ProjectInfo>, IProjectInfoService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(ProjectInfoPagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent T_ProjectInfo.* {0} from T_ProjectInfo {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_ProjectInfo.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询


            //查询
            if (!string.IsNullOrEmpty(param.KeyWord))
                sqlWhere += string.Format(" and T_ProjectInfo.Content like '%{0}%' ", param.KeyWord);
            if (!string.IsNullOrEmpty(param.Picture))
                sqlWhere += string.Format(" and T_ProjectInfo.Picture like '%{0}%' ", param.Picture);
            if (!string.IsNullOrEmpty(param.Video))
                sqlWhere += string.Format(" and T_ProjectInfo.Video like '%{0}%' ", param.Video);
            if (!string.IsNullOrEmpty(param.Type))
                sqlWhere += string.Format(" and T_ProjectInfo.Type = '{0}' ", param.Type);
            if (param.ProjectId != null)
                sqlWhere += string.Format(" and T_ProjectInfo.ProjectId = '{0}' ", param.ProjectId);
            if (param.SiteId != null)
                sqlWhere += string.Format(" and T_ProjectInfo.SiteId = {0} ", param.SiteId);

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_ProjectInfo.{0} {1} ", param.SortBy, param.SortOrder);

            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);

            var pager = EkpDbService.GetPager<T>(sql, param);
            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows.ToList(),
                TotalRecords = pager.TotalRecords,
            };
        }
    }
}
