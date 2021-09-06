using System.Linq;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using Ge.Infrastructure.Metronicv;
using EKP.Service.Base.Tree;

namespace EKP.Service.DictValue
{
    /// <summary>
    /// 字典选项管理
    /// </summary>
    public interface IDictValueService : IBaseTreeService<T_DictValue>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(DictValuePagerParam param, params string[] includePath) where T : class, new();
    }

    /// <summary>
    /// 字典选项管理
    /// </summary>
    public class DictValueService : BaseTreeService<T_DictValue>, IDictValueService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(DictValuePagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent T_DictValue.* {0} from T_DictValue {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_DictValue.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()), 
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_DictKey"))
            {
                sqlSelect += " ,(T_DictKey.[Key])[Key] ";
                sqlJoin += " left join T_DictKey on T_DictKey.Id = T_DictValue.KeyId ";
            }

            //查询
            if (!string.IsNullOrEmpty(param.IsWork))
            {
                sqlWhere += string.Format(" and T_DictValue.IsWork='{0}'", param.IsWork);
            }
            if (param.ParentId != null)
            {
                sqlWhere += string.Format(" and T_DictValue.ParentId='{0}' ", param.ParentId);
            }
            if (param.KeyId != null)
                sqlWhere += string.Format(" and T_DictValue.KeyId = {0} ", param.KeyId);
            if (!string.IsNullOrEmpty(param.KeyWord))
                sqlWhere += string.Format(" and (T_DictValue.Value like '%{0}%' or T_DictValue.ShowValue like '%{0}%') ", param.KeyWord);
            else if (!string.IsNullOrEmpty(param.Value))
                sqlWhere += string.Format(" and T_DictValue.Value like '%{0}%' ", param.Value);
            else if (!string.IsNullOrEmpty(param.Key))
                sqlWhere += string.Format(" and T_DictKey.[Key] = '{0}' ", param.Key);

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_DictValue.{0} {1} ", param.SortBy, param.SortOrder);

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
