using System.Linq;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using Ge.Infrastructure.Metronicv;
using EKP.Service.Base.Tree;
using System.Collections.Generic;
using Ge.Infrastructure.Extensions;

namespace EKP.Service.Info
{
    /// <summary>
    /// 信息管理
    /// </summary>
    public interface IInfoService : IBaseTreeService<T_Info>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(InfoPagerParam param, params string[] includePath) where T : class, new();

        /// <summary>
        /// 从多个类型中，抽取top条数据出来并返回列表
        /// </summary>
        List<T> GetFast<T>(List<string> types, int top, string where = "") where T : class, new();
    }

    /// <summary>
    /// 信息管理
    /// </summary>
    public class InfoService : BaseTreeService<T_Info>, IInfoService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(InfoPagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent T_Info.* {0} from T_Info {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_Info.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()), 
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_DictValue"))
            {
                sqlSelect += " ,(T_DictValueFirst.ShowValue) FirstType ";
                sqlJoin += " left join T_DictValue T_DictValueFirst on T_DictValueFirst.Value = T_Info.Type ";

                sqlSelect += " ,(T_DictValueSecond.ShowValue) SecondType ";
                sqlJoin += " left join T_DictValue T_DictValueSecond on T_DictValueSecond.Value = T_Info.ContentType ";
            }

            //查询
            if (!string.IsNullOrEmpty(param.KeyWord))
            {
                sqlWhere += string.Format(" and (T_Info.title like '%{0}%' or T_Info.Content like '%{0}%' or T_Info.Abstract like '%{0}%') ", param.KeyWord);
            }
            if (!string.IsNullOrEmpty(param.Type) && param.Type != "0")
            {
                sqlWhere += string.Format(" and T_Info.Type='{0}' ", param.Type);
            }
            if (!string.IsNullOrEmpty(param.ContentType) && param.ContentType != "0")
            {
                sqlWhere += string.Format(" and T_Info.ContentType='{0}' ", param.ContentType);
            }
            if (param.IsCover == "1")
            {
                sqlWhere += string.Format(" and (T_Info.Cover is not null and T_Info.Cover != '') ");
            }
            if (param.SiteId != null)
            {
                sqlWhere += string.Format(" and T_Info.SiteId='{0}' ", param.SiteId);
            }

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_Info.{0} {1} ", param.SortBy, param.SortOrder);

            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);

            var pager = EkpDbService.GetPager<T>(sql, param);
            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };
        }

        /// <summary>
        /// 从多个类型中，抽取top条数据出来并返回列表
        /// </summary>
        public List<T> GetFast<T>(List<string> contentTypes, int top, string where = "") where T : class, new()
        { 
            var sql = "select top {0} T_Info.* from T_Info {1} order by createTime desc";
            var sqlWhere = string.Format(" where T_Info.IsDeleted = '{0}' ", IsDelete.undeleted.ToString());

            if(contentTypes != null && contentTypes.Count > 0)
            {
                var contentTypesStr = " 1=0 ";
                contentTypes.ForEach(t => contentTypesStr += " OR ContentType='{0}' ".Format2(t));
                contentTypesStr = "({0})".Format2(contentTypesStr);
                sqlWhere += " and {0} ".Format2(contentTypesStr);
            }
            if (!string.IsNullOrEmpty(where))
            {
                sqlWhere += " and {0} ".Format2(where);
            }

            sql = string.Format(sql, top, sqlWhere);

            var list = EkpDbService.GetDt(sql).ToList<T>();
            return list;
        }
    }
}
