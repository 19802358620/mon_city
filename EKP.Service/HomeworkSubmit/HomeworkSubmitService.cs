using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Metronicv;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.HomeworkSubmit
{
    public interface IHomeworkSubmitService : IEkpEntityService<T_HomeworkSubmit>
    {

        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(HomeworkSubmitPagerParm param, params string[] includePath) where T : class, new();
        HomeworkSubmitPagerModel getHomeworkSubmitModel(int id);
    }

    public class IHomeworkClassServiceService : EkpEntityService<T_HomeworkSubmit>, IHomeworkSubmitService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(HomeworkSubmitPagerParm param, params string[] includePath) where T : class, new()
        {

            var sql = "select top 99.99999999 percent T_HomeworkSubmit.*, T_Homework.Name HomeworkName,T_Homework.ScoreDegree ScoreDegree ,(T_User.RealName) as StudentName " +
                      "from T_HomeworkSubmit,T_Homework, T_User {0} {1} {2}";
            string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_User.Id=T_HomeworkSubmit.UserId and T_Homework.Id=T_HomeworkSubmit.HomeworkId and T_HomeworkSubmit.HomeworkId=" + param.HomeworkId),
                sqlOrderBy = string.Empty;

            //连接查询 {0}, (T_User.RealName) as TeacherName
            if (param.KeyWord != null)
                sqlWhere += string.Format(" and (T_Homework.Name like '%{0}%' or T_User.RealName like '%{0}%') ", param.KeyWord);
           
            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_Homework.{0} {1} ", param.SortBy, param.SortOrder);

            sql = string.Format(sql, sqlWhere, sqlJoin , sqlOrderBy);
            var rows = EkpDbService.GetDt(sql).ToList<T>();
            var count = EkpDbService.GetCount(sql);
            
            return new JqgridResult<T>(param)
            {
                Rows = rows,
                TotalRecords = count,
            };
        }

        public HomeworkSubmitPagerModel getHomeworkSubmitModel(int id)
        {
            try
            {
                var sql = "select T_HomeworkSubmit.*, T_Homework.Name HomeworkName " +
                          " from T_HomeworkSubmit,T_Homework" +
                          " where T_Homework.Id=T_HomeworkSubmit.HomeworkId and T_HomeworkSubmit.Id=" + id;

                DataTable dt = EkpDbService.GetDt(sql);
                if (dt != null && dt.Rows.Count > 0)
                    return dt.Rows[0].ToModel<HomeworkSubmitPagerModel>();
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
    }
}