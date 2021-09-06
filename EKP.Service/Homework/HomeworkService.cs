//using EKP.Entity;
//using EKP.Service.Base;
//using EKP.Service.Base.EkpBaseModel;
//using Ge.Infrastructure.Extensions;
//using Ge.Infrastructure.Metronicv;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
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

namespace EKP.Service.Homework
{

    public interface IHomeworkService : IEkpEntityService<T_Homework>
    {

        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(HomeworkPagerParm param, params string[] includePath) where T : class, new();

        JqgridResult<T> SudentGetHomeworkPager<T>(HomeworkPagerParm param, params string[] includePath) where T : class, new();

    }

    public class HomeworkService : EkpEntityService<T_Homework>, IHomeworkService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(HomeworkPagerParm param, params string[] includePath) where T : class, new()
        {
            string sqlOrderBy = string.Empty;
            string sql2 = " select top 99.99999999 percent T_Homework.*,(T_User.RealName) as TeacherName,tempT.ClassId, tempT.ClassNames" +
                            " from T_Homework"+                           
                            " inner join(" +
                            " SELECT A.HomeworkId," +
                            " ((SELECT ClassId + ',' FROM V_HomeworkClass WHERE HomeworkId = A.HomeworkId FOR XML PATH(''))) AS ClassId," +
                            "   (SELECT ClassName + ',' FROM V_HomeworkClass WHERE HomeworkId = A.HomeworkId FOR XML PATH('')) AS ClassNames" +
                            " FROM V_HomeworkClass A {1}" +
                            " GROUP BY A.HomeworkId" +
                            " ) tempT on T_Homework.Id = tempT.HomeworkId" +
                            " left join T_User on T_Homework.UserId = T_User.Id" +
                            " where T_Homework.IsDeleted = 'undeleted' {0} {2}";
            //查询
            string sqlWhere = string.Empty;
            if (param.KeyWord != null)
                sqlWhere += string.Format(" and (T_Homework.Name like '%{0}%' or T_Homework.LinkName like '%{0}%' or T_Homework.AttachmentName like '%{0}%') ", param.KeyWord);
            if (param.UserId != 0)
                sqlWhere += string.Format(" and (T_Homework.UserId = '{0}') ", param.UserId);
            string sqlWhereClass = "";
            if (!string.IsNullOrEmpty(param.ClassId))
                sqlWhereClass =  string.Format(" where A.ClassId = '{0}' ", param.ClassId);

            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_Homework.{0} {1} ", param.SortBy, param.SortOrder);

            sql2 = string.Format(sql2, sqlWhere, sqlWhereClass, sqlOrderBy/*, sqlSelect, sqlJoin, sqlWhere*//*, sqlOrderBy*/);

            var pager = EkpDbService.GetPager<T>(sql2, param);
            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };            
        }

        /// <summary>
        ///  学生查看自己班的班级
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="includePath"></param>
        /// <returns></returns>
        public JqgridResult<T> SudentGetHomeworkPager<T>(HomeworkPagerParm param, params string[] includePath) where T : class, new()
        {
            try
            {
                string
                sqlSelect = string.Empty,
                sqlJoin = string.Empty,
                sqlWhere = string.Format(" where T_User.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
                sqlOrderBy = string.Empty;

                string sql2 = " select {0} {1} {2} {3}";
                sqlSelect = " T_Homework.*, T_HomeworkSubmit.Score StuScore, T_HomeworkSubmit.Status SubmitSatatus,T_User.RealName TeacherName " +
                            " from T_Homework " +
                            " inner join T_HomeworkClass" +
                            " on T_Homework.Id = T_HomeworkClass.HomeworkId " +
                            " inner join T_User on T_Homework.UserId = T_User.Id " +
                            " left join T_HomeworkSubmit on T_Homework.Id = T_HomeworkSubmit.HomeworkId ";               
                sqlWhere += " and T_Homework.IsDeleted='undeleted' ";
                sqlWhere += " and T_HomeworkClass.ClassId in({0})";
                sqlWhere = string.Format(sqlWhere, param.ClassId);

                //查询 
                if (param.KeyWord != null)
                    sqlWhere += string.Format(" and (T_Homework.Name like '%{0}%' or RealName like '%{0}%') ", param.KeyWord);
                
                //排序
                if (!string.IsNullOrEmpty(param.SortBy))
                    sqlOrderBy = string.Format(" order by T_Homework.{0} {1} ", param.SortBy, param.SortOrder);

                sql2 = string.Format(sql2, sqlSelect, sqlJoin, sqlWhere, sqlOrderBy);

                var pager = EkpDbService.GetPager<T>(sql2, param);
                return new JqgridResult<T>(param)
                {
                    Rows = pager.Rows,
                    TotalRecords = pager.TotalRecords,
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}