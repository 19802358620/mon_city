using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using EKP.Repository.Ef;
using Ge.Infrastructure.DataBase.DbHelper;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Pager;
using Ge.Infrastructure.Sql;

namespace EKP.Service.Base.Ef
{
    /// <summary>
    /// 数据库服务管理
    /// </summary>
    public class ServiceDb<TDbEntitie> : IServiceDb<TDbEntitie>
        where TDbEntitie : DbContext, new()
    {
        /// <summary>
        /// 创建上下文
        /// </summary>
        public static TDbEntitie CreateContext()
        {
            return new TDbEntitie();
        }

        /// <summary>
        /// 创建DbHelper
        /// </summary>
        /// <returns></returns>
        public static DbHelper CreateDbHelper()
        {
            return new DbHelper(CreateContext().Database.Connection.ConnectionString);
        }

        /// <summary>
        /// 创建事务操作对象
        /// </summary>
        public static SetEntityState CreateSetEntityState()
        {
            return new SetEntityState(CreateContext());
        }

        /// <summary>
        /// 执行sql返回PagerResult分页结果
        /// </summary>
        public static PagerResult<T> GetPager<T>(string sql, PagerParameter param) where T : class, new()
        {
            using (var context = CreateContext())
            {
                var result = new PagerResult<T>(param);
                result.Rows = context.Database.SqlQuery<T>(sql).Skip(param.Skip).Take(param.Take).ToList();
                result.TotalRecords = context.Database.SqlQuery<T>(sql).Count();
                return result;
            }
        }

        /// <summary>
        /// 执行sql返回List
        /// </summary>
        public static List<T> GetList<T>(string sql)
        {
            using (var context = CreateContext())
            {
                var result = context.Database.SqlQuery<T>(sql).ToList();
                return result;
            }
        }

        /// <summary>
        /// 获取条目数
        /// </summary>
        public static int GetCount(string sql)
        {
            var db = CreateDbHelper();
            var cmd = db.GetSqlStringCommond(SqlHelper.GetTotalCountSql(sql));
            var count = db.ExecuteScalar(cmd);

            return (int)count;
        }

        /// <summary>
        /// 获取数据库第一列第一行
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static T GetScalar<T>(string sql)
        {
            var db = CreateDbHelper();
            var cmd = db.GetSqlStringCommond(sql);
            var count = db.ExecuteScalar(cmd);

            if (count == DBNull.Value)
                return default(T);
            return (T)count;
        }

        /// <summary>
        /// 执行sql返回DataTable数据集
        /// </summary>
        public static DataTable GetDt(string sql)
        {
            DbHelper db = null;
            DbCommand cmd = null;
            DataTable dt = null;

            db = CreateDbHelper();
            cmd = db.GetSqlStringCommond(sql);
            dt = db.ExecuteDataTable(cmd);

            return dt;
        }

        /// <summary>
        /// 执行sql返回DbDataReader数据集
        /// </summary>
        public static DbDataReader GetDr(string sql)
        {
            DbHelper db = null;
            DbCommand cmd = null;
            DbDataReader dr = null;

            db = CreateDbHelper();
            cmd = db.GetSqlStringCommond(sql);
            dr = db.ExecuteReader(cmd);

            return dr;
        }
    }
}
