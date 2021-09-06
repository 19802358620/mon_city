using System;
using System.Collections.Generic;
using System.Linq;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using EKP.Service.User;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Metronicv;

namespace EKP.Service.Class
{
    /// <summary>
    /// 班级管理
    /// </summary>
    public interface IClassService : IEkpEntityService<T_Class>
    {
        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(ClassPagerParam param, params string[] includePath) where T : class, new();

        /// <summary>
        /// 班级分页
        /// </summary>
        JqgridResult<T> GetTeacherClassPager<T>(TeacherClassPagerParam param, params string[] includePath) where T : class, new();
    }

    /// <summary>
    /// 班级管理
    /// </summary>
    public class ClassService : EkpEntityService<T_Class>, IClassService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(ClassPagerParam param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent T_Class.* {0} from T_Class  {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_Class.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_Site"))
            {
                sqlSelect += " ,(T_Site.Name)SiteName ";
                sqlJoin += " left join T_Site on T_Site.Id = T_Class.SiteId ";
                sqlWhere += string.Format(" and (T_Site.IsDeleted = '{0}') ", IsDelete.undeleted.ToString());
            }

            //查询
            if (param.KeyWord != null)
            {
                sqlWhere += string.Format(" and (T_Class.Name like '%{0}%' or T_Class.Remark like '%{0}%') ", param.KeyWord);
            }
            if (param.Name != null)
            {
                sqlWhere += string.Format(" and T_Class.Name like '%{0}%' ", param.Name);
            }
            if (param.ClassIds != null)
            {
                sqlWhere += string.Format(" and ',{0},' like '%,' + cast(T_Class.Id as varchar) + ',%' ", param.ClassIds);
            }
            if (param.CreateBy != null)
            {
                sqlWhere += string.Format(" and T_Class.CreateBy = '{0}' ", param.CreateBy);
            }
            if (param.SiteId != null)
            {
                sqlWhere += string.Format(" and T_Class.SiteId = {0} ", param.SiteId);
            }
            //if (param.UserId != 0)
            //    sqlWhere += string.Format(" and T_Class.Id in (select ClassIds from T_User where Id = '{0}') ", param.UserId);

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_Class.{0} {1} ", param.SortBy, param.SortOrder);

            sql = string.Format(sql, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);

            var rows = EkpDbService.GetDt(sql).ToList<T>();
            var count = EkpDbService.GetCount(sql);

            return new JqgridResult<T>(param)
            {
                Rows = rows,
                TotalRecords = count,
            };
        }

        /// <summary>
        /// 班级分页
        /// </summary>
        public JqgridResult<T> GetTeacherClassPager<T>(TeacherClassPagerParam param, params string[] includePath)
            where T : class, new()
        {
            var userService = Ioc.GetService<IUserService>();

            var sql = "select top 99.99999999 percent T_Class.* {0} from T_Class  {1} {2} {3}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_Class.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_Site"))
            {
                sqlSelect += " ,(T_Site.Name)SiteName ";
                sqlJoin += " left join T_Site on T_Site.Id = T_Class.SiteId ";
                sqlWhere += string.Format(" and (T_Site.IsDeleted = '{0}') ", IsDelete.undeleted.ToString());
            }

            //查询
            if (param.KeyWord != null)
            {
                sqlWhere += string.Format(" and (T_Class.Name like '%{0}%' or T_Class.Remark like '%{0}%') ", param.KeyWord);
            }
            if (param.Name != null)
            {
                sqlWhere += string.Format(" and T_Class.Name like '%{0}%' ", param.Name);
            }
            if (param.UserId != null)
            {
                var user = userService.GetEntiy(Convert.ToInt32(param.UserId));
                sqlWhere += string.Format(" and (',{0},' like '%,' + convert(varchar(20),T_Class.Id) + ',%') ", user.ClassIds);
            }
            if (param.SiteId != null)
            {
                sqlWhere += string.Format(" and T_Class.SiteId = {0} ", param.SiteId);
            }

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_Class.{0} {1} ", param.SortBy, param.SortOrder);

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
