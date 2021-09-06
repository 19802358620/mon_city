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

namespace EKP.Service.Project
{
    /// <summary>
    /// 项目管理
    /// </summary>
    public interface IProjectService : IBaseTreeService<T_Project>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(ProjectPagerParam param, params string[] includePath) where T : class, new();
        
    }

    /// <summary>
    /// 项目管理
    /// </summary>
    public class ProjectService : BaseTreeService<T_Project>, IProjectService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(ProjectPagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent {4} {0} from T_Project {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_Project.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_DictValue"))
            {
                sqlSelect += " ,(T_DictValue.Value)TypeName ";
                sqlJoin += " left join T_DictValue on T_DictValue.Id = T_Project.Type ";
            }

            //查询
            if (!string.IsNullOrEmpty(param.Name))
                sqlWhere += string.Format(" and T_Project.Name like '%{0}%' ", param.Name);
            if (!string.IsNullOrEmpty(param.Type))
                sqlWhere += string.Format(" and T_Project.Type = '{0}' ", param.Type);
            if (param.ParentId != null)
                sqlWhere += string.Format(" and T_Project.ParentId = '{0}' ", param.ParentId);
            if (param.SiteId != null)
                sqlWhere += string.Format(" and T_Project.SiteId = {0} ", param.SiteId);

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_Project.{0} {1} ", param.SortBy, param.SortOrder);

            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy, param.Fields);

            var pager = EkpDbService.GetPager<T>(sql, param);
            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows.ToList(),
                TotalRecords = pager.TotalRecords,
            };
        }
    }
}
