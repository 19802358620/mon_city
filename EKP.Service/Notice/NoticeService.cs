using System;
using System.Collections.Generic;
using System.Linq;
using EKP.Entity;
using EKP.Service.Base;
using EKP.Service.Base.EkpBaseModel;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Utilities;

namespace EKP.Service.Notice
{

    /// <summary>
    /// 通知管理接口
    /// </summary>
    public interface INoticeService : IEkpEntityService<T_Notice>
    {

        /// <summary>
        /// 分页
        /// </summary>
        JqgridResult<T> GetPager<T>(NoticePagerParam param, params string[] includePath) where T : class, new();

        JqgridResult<T> SudentGetNoticePager<T>(NoticePagerParam param, params string[] includePath) where T : class, new();
    }
    
    public class NoticeService : EkpEntityService<T_Notice>, INoticeService
    {
        /// <summary>
        /// 分页
        /// </summary>
        public JqgridResult<T> GetPager<T>(NoticePagerParam param, params string[] includePath) where T : class, new()
        {
            string sqlOrderBy = "";

            string sql2 = "select top 99.99999999 percent T_Notice.*,(T_User.RealName) as TeacherName, tempT.ClassIds, tempT.ClassNames " +
                           " from T_Notice inner join (" +
                           " SELECT A.NoticeId," +
                           "((SELECT CONVERT(nvarchar(20), ClassId),+',' FROM V_NoticeClass WHERE NoticeId = A.NoticeId FOR XML PATH(''))) AS ClassIds," +
                           "(SELECT ClassName + ',' FROM V_NoticeClass WHERE NoticeId = A.NoticeId FOR XML PATH('')) AS ClassNames " +
                           " FROM V_NoticeClass A  {1} " +
                           " GROUP BY A.NoticeId) tempT on T_Notice.Id = tempT.NoticeId" +
                           " left join T_User on T_Notice.UserId = T_User.Id" +
                           " where 1 = 1 {0} {2}";

            //查询
            string sqlWhere = string.Empty;
            if (param.KeyWord != null)
                sqlWhere += string.Format(" and (T_Notice.Title like '%{0}%') ", param.KeyWord);

            if (param.UserId != 0)
                sqlWhere += string.Format(" and (T_Notice.UserId = '{0}') ", param.UserId);

            string sqlWhereClass = "";
            if (!string.IsNullOrEmpty(param.ClassIds))
                sqlWhereClass = string.Format(" where A.ClassId = '{0}' ", param.ClassIds);
 
            //排序
            if (!string.IsNullOrEmpty(param.SortBy))
                sqlOrderBy = string.Format(" order by T_Notice.{0} {1} ", param.SortBy, param.SortOrder);
            
            sql2 = string.Format(sql2, sqlWhere, sqlWhereClass, sqlOrderBy);

            var pager = EkpDbService.GetPager<T>(sql2, param);
            return new JqgridResult<T>(param)
            {
                Rows = pager.Rows,
                TotalRecords = pager.TotalRecords,
            };

            //var sql = " select T_Notice.*,T_User.RealName TeacherName from T_Notice,T_User,T_NoticeClass where T_Notice.UserId=T_User.Id and T_NoticeClass.NoticeId=T_Notice.Id";
            //string
            //    sqlJoin = string.Empty,
            //    sqlWhere = string.Format(" where T_Notice.IsDeleted = '{0}' ", IsDelete.undeleted.ToString()),
            //    sqlOrderBy = string.Empty;

            ////连接查询
            //if (includePath.Contains("T_Site"))
            //{
            //    sqlJoin += " left join T_Site on T_Site.Id = T_Notice.SiteId ";
            //    sqlWhere += string.Format(" and (T_Site.IsDeleted = '{0}') ", IsDelete.undeleted.ToString());
            //    sqlWhere += string.Format(" and T_NoticeClass.NoticeId=T_Notice.Id");
            //}

            ////查询
            //if (param.KeyWord != null)
            //    sqlWhere += string.Format(" and (T_Notice.Name like '%{0}%' or T_Notice.Remark like '%{0}%'or T_Notice.Remark like '%{0}%' or T_Notice.Accessory like '%{0}%' or T_Notice.AccessoryName like '%{0}%'  or T_Notice.LinkName like '%{0}%') ", param.KeyWord);

            ////排序
            //if (!string.IsNullOrEmpty(param.SortBy))
            //    sqlOrderBy = string.Format(" order by T_Notice.{0} {1} ", param.SortBy, param.SortOrder);
            //sql = string.Format(sql/*, sqlJoin, sqlWhere*/);


            //var rows = EkpDbService.GetDt(sql).ToList<T>();
            //var count = EkpDbService.GetCount(sql);
            //return new JqgridResult<T>(param)
            //{
            //    Rows = rows,
            //    TotalRecords = count,
            //};

        }

        public JqgridResult<T> SudentGetNoticePager<T>(NoticePagerParam param, params string[] includePath) where T : class, new()
        {
            try
            {
                string sqlWhere = "";
                string sqlOrderBy = "";
                string sql = "select top 99.99999999 percent T_Notice.*,T_Notice.State NoticeState,T_Notice.Type stuType,T_User.RealName TeacherName  " +
                             " from T_Notice inner join T_NoticeClass on T_Notice.id = T_NoticeClass.NoticeId "+
                             " inner join T_User on T_Notice.UserId = T_User.Id  "+
                             " where T_User.IsDeleted = 'undeleted'  and T_NoticeClass.ClassId in({0}) {1} {2}";
                //查询
                if (param.KeyWord != null)
                     sqlWhere += string.Format(" and (T_Notice.Title like '%{0}%') ", param.KeyWord);

                //排序
                if (!string.IsNullOrEmpty(param.SortBy))
                    sqlOrderBy = string.Format(" order by T_Notice.{0} {1} ", param.SortBy, param.SortOrder);


                sql = string.Format(sql, param.ClassIds, sqlWhere, sqlOrderBy);

                var pager = EkpDbService.GetPager<T>(sql, param);
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