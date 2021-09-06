using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using Ge.Infrastructure.Pager;

namespace EKP.Repository.Ado
{
    /// <summary>
    /// 名    称：IDbContextAdapter
    /// 作    者：胡政
    /// 创建时间：2015-08-21
    /// 联系方式：13436053642
    /// 描    述：数据库操作装饰器接口(基于sql语句)
    /// </summary>
    public interface IDbContextAdapter<out TDbContext> where TDbContext : DbContext, new()
    {
        #region 查询
        /// <summary>
        /// 获取DataSet数据集
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        DataSet GetDs<TEntity>(string where) where TEntity : class;
        DataSet GetDs<TEntity>(int top, string where) where TEntity : class;

        /// <summary>
        /// 获取DataTable数据集
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        DataTable GetDt<TEntity>(string where) where TEntity : class;
        DataTable GetDt<TEntity>(int top, string where) where TEntity : class;

        /// <summary>
        /// 获取DataReader数据集
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        DbDataReader GetDr<TEntity>(string where) where TEntity : class;
        DbDataReader GetDr<TEntity>(int top, string where) where TEntity : class;

        /// <summary>
        /// 获取DataTable翻页数据集，并输出总条数
        /// </summary>
        DataTable GetDtPager<TEntity>(PagerParameter param, string where, out int totalCount) where TEntity : class;

        /// <summary>
        /// 查询是否存在实体
        /// </summary>
        /// <param name="where">查询条件（也可以包括聚类、排列顺序等条件）</param>
        bool IsExist<TEntity>(string where) where TEntity : class;
        #endregion

        #region 插入
        /// <summary>
        /// 插入单个实体
        /// </summary>
        void Add<TEntity>(params TEntity[] entities) where TEntity : class;
        #endregion

        #region 更新
        /// <summary>
        /// 更新单个实体
        /// </summary>
        void Update<TEntity>(params TEntity[] entities) where TEntity : class;
        /// <summary>
        /// 更新实体，并指定哪些列能被更新
        /// </summary>
        void Update<TEntity>(List<TEntity> entities, params string[] fileds) where TEntity : class;
        #endregion

        #region 删除
        /// <summary>
        /// 删除单个实体
        /// </summary>
        void Delete<TEntity>(params TEntity[] entities) where TEntity : class;

        #endregion
    }
}
