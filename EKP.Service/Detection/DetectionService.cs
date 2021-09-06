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

namespace EKP.Service.Detection
{
    /// <summary>
    /// 自我检测管理
    /// </summary>
    public interface IDetectionService : IBaseTreeService<T_Detection>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(DetectionPagerParam param, params string[] includePath) where T : class, new();
        
    }

    /// <summary>
    /// 自我检测管理
    /// </summary>
    public class DetectionService : BaseTreeService<T_Detection>, IDetectionService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(DetectionPagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent T_Detection.* {0} from T_Detection {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_Detection.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询

            //查询
            if (!string.IsNullOrEmpty(param.Name))
                sqlWhere += string.Format(" and T_Detection.Name like '%{0}%' ", param.Name);
            if (param.ParentId != null)
                sqlWhere += string.Format(" and T_Detection.ParentId = '{0}' ", param.ParentId);
            if (param.ProjectId != null)
                sqlWhere += string.Format(" and T_Detection.ProjectId = '{0}' ", param.ProjectId);
            if (param.SiteId != null)
                sqlWhere += string.Format(" and T_Detection.SiteId = {0} ", param.SiteId);

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_Detection.{0} {1} ", param.SortBy, param.SortOrder);

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
