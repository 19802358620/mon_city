using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Sql
{
    /// <summary>
    /// 名    称：SqlHelper
    /// 作    者：胡政
    /// 创建时间：2015-08-22
    /// 联系方式：13436053642
    /// 描    述：sql语句拼接帮助类（包括分页、获取总条数等）
    /// </summary>
   public  class SqlHelper
    {
       /// <summary>
       /// 获取从skip至skip+take的take条数据记录
       /// </summary>
       public static string GetPagerSql(string sql, int skip, int take)
       {
           return string.Format(PagerSqlTemplate(),
               take, sql, skip);
       }
       /// <summary>
       /// 将sql语句转化为获取总条目数的sql语句
       /// </summary>
       /// <param name="sql"></param>
       /// <returns></returns>
        public static string GetTotalCountSql(string sql)
       {
           return string.Format(TotalCountSqlTemplate(), sql);
       }

       #region // 私有方法
       /// <summary>
       /// 分页sql语句模板
       /// </summary>
        private static string PagerSqlTemplate()
        {
            return
                "select * from (" +
                "select  row_number()over(order by tempcolumn)Rownumber,* from (" +
                "select top {0} tempcolumn=0 , * from (" +
                "{1})temp1)temp2 " +
                ")temp3 " +
                "where Rownumber >= {2}";
        }
       /// <summary>
       /// 获取总条目数sql语句模板
       /// </summary>
       /// <returns></returns>
        private static string TotalCountSqlTemplate()
        {
            return "select count(*)totalCount from (" +
                 "{0}" +
                ")a";
        }
        #endregion
    }
}
