using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Metronicv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.HomeworkClass
{
    public interface IHomeworkClassService : IEkpEntityService<T_HomeworkClass>
    {

        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(HomeworkClassPagerParm param, params string[] includePath) where T : class, new();
    }

    public class IHomeworkClassServiceService : EkpEntityService<T_HomeworkClass>, IHomeworkClassService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(HomeworkClassPagerParm param, params string[] includePath) where T : class, new()
        {
            var sql = "select top 99.99999999 percent T_Homework.*, (T_User.RealName) as TeacherName from T_Homework {0}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_Class.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

            //连接查询
            if (includePath.Contains("T_User"))
            {
                sqlSelect += " ,(T_Site.Name)SiteName ";
                sqlJoin += " left join T_User on T_User.Id = T_Homework.UserId ";
                sqlWhere += string.Format(" and (T_Site.IsDeleted = '{0}') ", IsDelete.undeleted.ToString());
            }

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_Homework.{0} {1} ", param.SortBy, param.SortOrder);

            sql = string.Format(sql/*, sqlSelect*/, sqlJoin/*, sqlWhere, sqlOrderBy*/);
            var rows = EkpDbService.GetDt(sql).ToList<T>();
            var count = EkpDbService.GetCount(sql);

            return new JqgridResult<T>(param)
            {
                Rows = rows,
                TotalRecords = count,
            };
        }
    }
}
