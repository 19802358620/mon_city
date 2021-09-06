using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using Ge.Infrastructure.DataBase.DbHelper;
using Ge.Infrastructure.Extensions.EFExtension;
using Ge.Infrastructure.Pager;
using Ge.Infrastructure.Sql;

namespace EKP.Repository.Ado
{
    /// <summary>
    /// 名    称：DbSqlAdapter
    /// 作    者：胡政
    /// 创建时间：2015-08-21
    /// 联系方式：13436053642
    /// 描    述：数据库操作装饰器类(基于sql语句)
    /// </summary>
    public class DbContextAdapter<TDbContext> : IDbContextAdapter<TDbContext> where TDbContext : DbContext, new()
    {
        /// <summary>
        /// 创建事务管理器
        /// </summary>
        /// <returns></returns>
        private EntityTrans CreateEntityTrans()
        {
            var trans = new Trans(new TDbContext().Database.Connection.ConnectionString);
            return new EntityTrans(trans);
        }

        /// <summary>
        /// 创建数据库操作帮助类
        /// </summary>
        /// <returns></returns>
        private DbHelper CreateDbHelper()
        {
            var context = new TDbContext();
            return new DbHelper(context.Database.Connection.ConnectionString);
        }

        #region 查询
        /// <summary>
        /// 获取DataSet数据集
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        public DataSet GetDs<TEntity>(string where) where TEntity : class
        {
            var sql = EntitySql.SelectSql<TEntity>(where);
            var db = CreateDbHelper();
            var cmd = db.GetSqlStringCommond(sql);
            var ds = db.ExecuteDataSet(cmd);
            return ds;
        }
        /// <summary>
        /// 获取DataSet数据集
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="top">获取条数</param>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        /// <returns></returns>
        public DataSet GetDs<TEntity>(int top, string where) where TEntity : class
        {
            var sql = EntitySql.SelectSql<TEntity>(top, where);
            var db = CreateDbHelper();
            var cmd = db.GetSqlStringCommond(sql);
            var ds = db.ExecuteDataSet(cmd);
            return ds;
        }

        /// <summary>
        /// 获取DataTable数据集
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        public DataTable GetDt<TEntity>(string where) where TEntity : class
        {
            using (var dbSqlTrans = CreateEntityTrans())
            {
                var sql = EntitySql.SelectSql<TEntity>(where);
                var dt = dbSqlTrans.GetDt(sql);
                return dt;
            }
        }
        /// <summary>
        /// 获取DataTable数据集
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="top">获取条数</param>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        /// <returns></returns>
        public DataTable GetDt<TEntity>(int top, string where) where TEntity : class
        {
            using (var dbSqlTrans = CreateEntityTrans())
            {
                var sql = EntitySql.SelectSql<TEntity>(top, where);
                var dt = dbSqlTrans.GetDt(sql);
                return dt;
            }
        }

        /// <summary>
        /// 获取DataReader数据集
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        public DbDataReader GetDr<TEntity>(string where) where TEntity : class
        {
            var sql = EntitySql.SelectSql<TEntity>(where);
            var db = CreateDbHelper();
            var cmd = db.GetSqlStringCommond(sql);
            var dr = db.ExecuteReader(cmd);
            return dr;
        }
        /// <summary>
        /// 获取DataReader数据集
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="top">获取条数</param>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        /// <returns></returns>
        public DbDataReader GetDr<TEntity>(int top, string where) where TEntity : class
        {
            var sql = EntitySql.SelectSql<TEntity>(top, where);
            var db = CreateDbHelper();
            var cmd = db.GetSqlStringCommond(sql);
            var dr = db.ExecuteReader(cmd);
            return dr;
        }

        /// <summary>
        /// 获取DataTable翻页数据集，并输出总条数
        /// </summary>
        public DataTable GetDtPager<TEntity>(PagerParameter param, string where, out int totalCount) where TEntity : class
        {
            var tabelName = typeof(TEntity).Name;
            var sql_data = SqlHelper.GetPagerSql(string.Format("select top 99.99999999 percent * from {0}  {1}", tabelName, where), param.Skip, param.Take);
            var sql_totalCount = SqlHelper.GetTotalCountSql(string.Format("select top 100 percent * from {0}  {1}", tabelName, where));
            var db = CreateDbHelper();

            var cmd = db.GetSqlStringCommond(sql_data);
            var dt = db.ExecuteDataTable(cmd);

            cmd = db.GetSqlStringCommond(sql_totalCount);
            totalCount = Convert.ToInt32(db.ExecuteScalar(cmd));

            return dt;
        }

        /// <summary>
        /// 查询是否存在实体
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        public bool IsExist<TEntity>(string where) where TEntity : class
        {
            var dt = this.GetDt<TEntity>(where);

            return dt != null && dt.Rows.Count > 0;
        }
        #endregion

        #region 插入
        /// <summary>
        /// 插入实体
        /// </summary>
        public void Add<TEntity>(params TEntity[] entities) where TEntity : class
        {
            if (entities == null || entities.Length == 0) return;

            using (var dbSqlTrans = CreateEntityTrans())
            {
                entities.ToList().ForEach(entity => dbSqlTrans.Add(entity));
                dbSqlTrans.SaveChange();
            }
        }
        #endregion

        #region 更新
        /// <summary>
        /// 更新实体
        /// </summary>
        public void Update<TEntity>(params TEntity[] entities) where TEntity : class
        {
            if (entities == null || entities.Length == 0) return;

            using (var dbSqlTrans = CreateEntityTrans())
            {
                entities.ToList().ForEach(entity => dbSqlTrans.Update(entity));
                dbSqlTrans.SaveChange();
            }
        }
        /// <summary>
        /// 更新实体，并指定哪些列能被更新
        /// </summary>
        public void Update<TEntity>(List<TEntity> entities, params string[] fileds) where TEntity : class
        {
            if (entities == null || entities.Count == 0) return;

            using (var dbSqlTrans = CreateEntityTrans())
            {
                entities.ForEach(entity => dbSqlTrans.Update(entity, fileds));
                dbSqlTrans.SaveChange();
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除单个实体
        /// </summary>
        public void Delete<TEntity>(params TEntity[] entities) where TEntity : class
        {
            if (entities == null || entities.Length == 0) return;

            using (var dbSqlTrans = CreateEntityTrans())
            {
                entities.ToList().ForEach(entity => dbSqlTrans.Delete(entity));
                dbSqlTrans.SaveChange();
            }
        }
        #endregion
    }
}
