using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;
using Ge.Infrastructure.Repository;

namespace EKP.Repository.Ef
{
    /// <summary>
    /// 名    称：IDbContextAdapter
    /// 作    者：胡政
    /// 创建时间：2015-08-21
    /// 联系方式：13436053642
    /// 描    述：数据库操作装饰器接口
    /// </summary>
    public interface IDbContextAdapter<out TDbContext> where TDbContext : DbContext, new()
    {
        #region 基本操作

        /// <summary>
        /// 获取实体的第一个属性名
        /// </summary>
        string GetFirstDefaultPropertyName(Type entityType);

        #endregion

        #region 增加

        /// <summary>
        /// 插入单个实体
        /// </summary>
        int AddObject<TEntity>(TEntity entity, Action<SetEntityState> callDelegate = null) where TEntity : class;

        /// <summary>
        /// 插入多个实体
        /// </summary>
        int AddObject<TEntity>(IEnumerable<TEntity> entityList, Action<SetEntityState> callDelegate = null)
            where TEntity : class;

        #endregion

        #region 修改

        /// <summary>
        /// 修改单个实体
        /// </summary>
        int UpdateObject<TEntity>(TEntity entity, string[] fileds = null, Action<SetEntityState> callDelegate = null)
            where TEntity : class;

        /// <summary>
        /// 修改多个实体
        /// </summary>
        int UpdateObject<TEntity>(IEnumerable<TEntity> entityList, Action<SetEntityState> callDelegate = null)
            where TEntity : class;

        /// <summary>
        /// 修改符合条件的实体
        /// </summary>
        int UpdateObject<TEntity>(Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TEntity>> updateExpression) where TEntity : class;

        /// <summary>
        /// 修改符合条件的实体
        /// </summary>
        int UpdateObject<TEntity>(IEnumerable<UpdateExpressionsEntity<TEntity>> updateExpressions)
            where TEntity : class, new();

        #endregion

        #region 删除

        /// <summary>
        /// 删除单个实体
        /// </summary>
        int DeleteObject<TEntity>(TEntity entity, Action<SetEntityState> callDelegate = null)
            where TEntity : class;

        /// <summary>
        /// 删除多个实体
        /// </summary>
        int DeleteObject<TEntity>(IEnumerable<TEntity> entityList, Action<SetEntityState> callDelegate = null)
            where TEntity : class;

        /// <summary>
        /// 删除符合条件的实体
        /// </summary>
        int DeleteObject<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

        /// <summary>
        /// 删除符合条件的实体
        /// </summary>
        int DeleteObject2<TEntity>(Expression<Func<TEntity, bool>> filterExpression) where TEntity : class;

        #endregion

        #region 查询

        /// <summary>
        /// 通过主键获取单个实体
        /// </summary>
        TEntity GetEntity<TEntity>(object primaryKeyValue) where TEntity : class;

        /// <summary>
        /// 获取符合条件的第一个实体
        /// </summary>
        TEntity GetEntity<TEntity>(Expression<Func<TEntity, bool>> expression, params string[] includePath)
            where TEntity : class;

        /// <summary>
        /// 获取前totalRecord个实体集合
        /// </summary>
        IEnumerable<TEntity> GetTopList<TEntity>(
            Expression<Func<TEntity, bool>> expression,
            string orderBy,
            int totalRecord,
            params string[] includePath) where TEntity : class;

        /// <summary>
        /// 获取符合条件的实体集合
        /// </summary>
        IEnumerable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> expression,
            params string[] includePath)
            where TEntity : class;

        /// <summary>
        /// 获取符合条件的实体集合
        /// </summary>
        IEnumerable<TEntity> GetList<TEntity>(string entsql,
            ObjectParameter[] parameters,
            params string[] includePath)
            where TEntity : class;

        /// <summary>
        /// 获取分页实体集合
        /// </summary>
        IEnumerable<TEntity> GetPager<TEntity>(Expression<Func<TEntity, bool>> expression,
            int pageIndex,
            int pageSize,
            ref long totalCount,
            string orderByProperty,
            params string[] includePath) where TEntity : class;

        /// <summary>
        /// 获取分页实体集合
        /// </summary>
        IEnumerable<TEntity> GetPager<TEntity>(Expression<Func<TEntity, bool>> expression,
            int pageIndex,
            int pageSize,
            ref long totalCount,
            string[] orderByProperties,
            params string[] includePath) where TEntity : class;

        /// <summary>
        /// 获取分页实体集合
        /// </summary>
        IEnumerable<TEntity> GetPager<TEntity>(string entsql,
            ObjectParameter[] parameters,
            int pageIndex,
            int pageSize,
            ref long totalCount,
            string orderByProperty,
            params string[] includePath) where TEntity : class;

        #endregion

        #region 其他扩展操作

        void ExposedDbContext(Action<TDbContext> callDelegate);

        /// <summary>
        /// 执行单条sql操作
        /// </summary>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// 执行单条sql返回查询结果dataset
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataSet ExecuteSqlCommandReturnDataSet(string sql);

        /// <summary>
        /// 执行sql并返回集合
        /// </summary>
        IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters);

        /// <summary>
        /// 执行sql返回DataSet数据集
        /// </summary>
        DataSet ExecuteProcedure(string procedureName, params object[] parameters);

        #endregion
    }
}
