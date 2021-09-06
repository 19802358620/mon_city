using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using EntityFramework.Extensions;
using Ge.Infrastructure.Extensions.EFExtension;
using Ge.Infrastructure.Repository;

namespace EKP.Repository.Ef
{
    /// <summary>
    /// 名    称：DbContextAdapter
    /// 作    者：胡政
    /// 创建时间：2015-08-21
    /// 联系方式：13436053642
    /// 描    述：数据库操作装饰器
    /// </summary>
    public class DbContextAdapter<TDbContext> : IDbContextAdapter<TDbContext> where TDbContext : DbContext, new()
    {
        #region 基本操作

        /// <summary>
        /// 获取实体的第一个属性名
        /// </summary>
        public string GetFirstDefaultPropertyName(Type entityType)
        {
            var firstOrDefault = entityType.GetProperties().FirstOrDefault();
            if (firstOrDefault != null)
            {
                var propertyName = firstOrDefault.Name;
                return propertyName;
            }
            return string.Empty;
        }

        /// <summary>
        /// 创建上下文
        /// </summary>
        /// <returns></returns>
        private TDbContext CreateDbContext()
        {
            return new TDbContext();
        }

        /// <summary>
        /// 创建连接查询（left join）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="includePath">连接的表集合</param>
        /// <returns></returns>
        private DbQuery<TEntity> CreatePathQuery<TEntity>(DbSet<TEntity> dbSet, string[] includePath)
            where TEntity : class
        {
            DbQuery<TEntity> dbQuery = dbSet.Include(includePath[0]);
            for (int i = 1; i < includePath.Length; i++)
            {
                dbQuery = dbQuery.Include(includePath[i]);
            }
            return dbQuery;
        }

        private ObjectContext DbContextToObjectContext(DbContext dbContext)
        {
            return ((IObjectContextAdapter)dbContext).ObjectContext;
        }

        #endregion

        #region 增加

        /// <summary>
        /// 插入单个实体
        /// </summary>
        public int AddObject<TEntity>(TEntity entity, Action<SetEntityState> callDelegate = null) where TEntity : class
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    var setting = new SetEntityState(context);
                    setting.Add(entity);
                    if (callDelegate != null)
                        callDelegate(setting);
                    return context.SaveChanges();
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 插入多个实体
        /// </summary>
        public int AddObject<TEntity>(IEnumerable<TEntity> entityList, Action<SetEntityState> callDelegate = null)
            where TEntity : class
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    var setting = new SetEntityState(context);
                    foreach (TEntity entity in entityList)
                    {
                        setting.Add(entity);
                    }

                    if (callDelegate != null)
                        callDelegate(setting);
                    return context.SaveChanges();
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改单个实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="fileds">设置应该被修改的字段，如果为null则默认修改所有的字段</param>
        /// <param name="callDelegate"></param>
        /// <returns></returns>
        public int UpdateObject<TEntity>(TEntity entity, string[] fileds = null, Action<SetEntityState> callDelegate = null)
            where TEntity : class
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    var setting = new SetEntityState(context);

                    if (callDelegate != null)
                        callDelegate(setting);
                    setting.Update(entity, fileds);
                    return context.SaveChanges();
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改多个实体
        /// </summary>
        public int UpdateObject<TEntity>(IEnumerable<TEntity> entityList, Action<SetEntityState> callDelegate = null)
            where TEntity : class
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    var setting = new SetEntityState(context);

                    if (callDelegate != null)
                        callDelegate(setting);

                    foreach (TEntity entity in entityList)
                    {
                        setting.Update(entity);
                        context.Entry(entity).CurrentValues.SetValues(entity);
                    }

                    return context.SaveChanges();
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改符合条件的实体
        /// </summary>
        public int UpdateObject<TEntity>(Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TEntity>> updateExpression) where TEntity : class
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    return context.Set<TEntity>().Where(filterExpression).Update(updateExpression);
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 修改符合条件的实体
        /// </summary>
        public int UpdateObject<TEntity>(IEnumerable<UpdateExpressionsEntity<TEntity>> updateExpressions)
            where TEntity : class, new()
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    int count;
                    using (var tx = context.Database.BeginTransaction())
                    {
                        try
                        {
                            count = updateExpressions.Sum(item => context.Set<TEntity>().Where(item.FilterExpression).Update(item.UpdateExpression));
                            tx.Commit();
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();
                            throw ex;
                        }
                    }
                    return count;
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除单个实体
        /// </summary>
        public int DeleteObject<TEntity>(TEntity entity, Action<SetEntityState> callDelegate = null)
            where TEntity : class
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    var setting = new SetEntityState(context);

                    if (callDelegate != null)
                        callDelegate(setting);

                    setting.Delete(entity);
                    return context.SaveChanges();
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除多个实体
        /// </summary>
        public int DeleteObject<TEntity>(IEnumerable<TEntity> entityList, Action<SetEntityState> callDelegate = null)
            where TEntity : class
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    var setting = new SetEntityState(context);

                    if (callDelegate != null)
                        callDelegate(setting);
                    foreach (TEntity entity in entityList)
                    {
                        setting.Delete(entity);
                    }
                    return context.SaveChanges();
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除符合条件的实体
        /// </summary>
        public int DeleteObject<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    IEnumerable<TEntity> data = GetList(expression);
                    foreach (TEntity entity in data)
                    {
                        context.Entry(entity).State = EntityState.Deleted;
                    }
                    return context.SaveChanges();
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除符合条件的实体
        /// </summary>
        public int DeleteObject2<TEntity>(Expression<Func<TEntity, bool>> filterExpression) where TEntity : class
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    return context.Set<TEntity>().Where(filterExpression).Delete();
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 查询

        /// <summary>
        /// 通过主键获取单个实体
        /// </summary>
        public TEntity GetEntity<TEntity>(object primaryKeyValue) where TEntity : class
        {
            using (TDbContext context = CreateDbContext())
            {
                var entity = context.Set<TEntity>().Find(primaryKeyValue);
                return entity;
            }
        }

        /// <summary>
        /// 获取符合条件的第一个实体
        /// </summary>
        public TEntity GetEntity<TEntity>(Expression<Func<TEntity, bool>> expression, params string[] includePath)
            where TEntity : class
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    context.Configuration.LazyLoadingEnabled = true;
                    context.Configuration.ProxyCreationEnabled = false;
                    DbSet<TEntity> dbSet = context.Set<TEntity>();
                    if (includePath.Length > 0)
                    {
                        DbQuery<TEntity> query = CreatePathQuery(dbSet, includePath);
                        return query.FirstOrDefault(expression);
                    }
                    return dbSet.FirstOrDefault(expression);
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取前totalRecord个实体集合
        /// </summary>
        public IEnumerable<TEntity> GetTopList<TEntity>(
            Expression<Func<TEntity, bool>> expression,
            string orderBy,
            int totalRecord,
            params string[] includePath) where TEntity : class
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    context.Configuration.LazyLoadingEnabled = true;
                    context.Configuration.ProxyCreationEnabled = false;
                    DbSet<TEntity> dbSet = context.Set<TEntity>();

                    if (string.IsNullOrEmpty(orderBy))
                    {
                        orderBy = this.GetFirstDefaultPropertyName(typeof(TEntity));
                    }

                    if (includePath.Length > 0)
                    {
                        DbQuery<TEntity> query = this.CreatePathQuery(dbSet, includePath);
                        return query.Where(expression).OrderBy(orderBy).Take(totalRecord).ToList();
                    }

                    return dbSet.Where(expression).OrderBy(orderBy).Take(totalRecord).ToList();
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取符合条件的实体集合
        /// </summary>
        public IEnumerable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> expression,
            params string[] includePath)
            where TEntity : class
        {
            try
            {
                var list = new List<TEntity>();
                using (TDbContext context = CreateDbContext())
                {
                    context.Configuration.LazyLoadingEnabled = true;
                    context.Configuration.ProxyCreationEnabled = false;
                    DbSet<TEntity> dbSet = context.Set<TEntity>();
                    if (includePath.Length > 0)
                    {
                        DbQuery<TEntity> query = CreatePathQuery(dbSet, includePath);
                        list = query.Where(expression).ToList();
                    }
                    else
                    {
                        list = dbSet.Where(expression).ToList();
                    }
                }

                return list;
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取符合条件的实体集合
        /// </summary>
        public IEnumerable<TEntity> GetList<TEntity>(string entsql,
            ObjectParameter[] parameters,
            params string[] includePath)
            where TEntity : class
        {
            try
            {
                var list = new List<TEntity>();
                using (TDbContext context = CreateDbContext())
                {
                    context.Configuration.LazyLoadingEnabled = true;
                    context.Configuration.ProxyCreationEnabled = false;
                    DbSet<TEntity> dbSet = context.Set<TEntity>();
                    if (includePath.Length > 0)
                    {
                        ObjectQuery<TEntity> query = CreatePathQuery(dbSet, includePath).AsObjectQuery();
                        list = query.Where(entsql, parameters).ToList();
                    }
                    else
                    {
                        list = dbSet.AsObjectQuery().Where(entsql, parameters).ToList();
                    }
                }

                return list;
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取分页实体集合
        /// </summary>
        public IEnumerable<TEntity> GetPager<TEntity>(Expression<Func<TEntity, bool>> expression,
            int pageIndex,
            int pageSize,
            ref long totalCount,
            string orderByProperty,
            params string[] includePath) where TEntity : class
        {
            try
            {
                if (string.IsNullOrEmpty(orderByProperty))
                    orderByProperty = GetFirstDefaultPropertyName(typeof(TEntity));
                using (TDbContext context = CreateDbContext())
                {
                    context.Configuration.LazyLoadingEnabled = true;
                    context.Configuration.ProxyCreationEnabled = false;
                    DbSet<TEntity> dbSet = context.Set<TEntity>();
                    if (includePath != null && includePath.Length > 0)
                    {
                        DbQuery<TEntity> query = CreatePathQuery(dbSet, includePath);
                        totalCount = query.LongCount(expression);

                        return query.Where(expression)
                            .OrderBy(orderByProperty)
                            .Skip(pageIndex * pageSize)
                            .Take(pageSize)
                            .ToList();
                    }

                    totalCount = (int)dbSet.LongCount(expression);

                    return dbSet.Where(expression)
                        .OrderBy(orderByProperty)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .ToList();
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取分页实体集合
        /// </summary>
        public IEnumerable<TEntity> GetPager<TEntity>(Expression<Func<TEntity, bool>> expression,
            int pageIndex,
            int pageSize,
            ref long totalCount,
            string[] orderByProperties,
            params string[] includePath) where TEntity : class
        {
            try
            {
                string orderByProperty;
                if (orderByProperties != null && orderByProperties.Length > 0)
                    orderByProperty = orderByProperties[0];
                else
                    orderByProperty = GetFirstDefaultPropertyName(typeof(TEntity));
                using (TDbContext context = CreateDbContext())
                {
                    context.Configuration.LazyLoadingEnabled = true;
                    context.Configuration.ProxyCreationEnabled = false;
                    DbSet<TEntity> dbSet = context.Set<TEntity>();
                    if (includePath != null && includePath.Length > 0)
                    {
                        DbQuery<TEntity> query = CreatePathQuery(dbSet, includePath);
                        totalCount = query.LongCount(expression);

                        var queryable = query.Where(expression)
                            .OrderBy(orderByProperty)
                            .Skip(pageIndex * pageSize)
                            .Take(pageSize)
                            ;
                        if (orderByProperties != null)
                        {
                            queryable = orderByProperties.Aggregate(queryable,
                                (current, property) => current.OrderBy(property));
                        }
                        return queryable.ToList();
                    }

                    totalCount = (int)dbSet.LongCount(expression);

                    var querytemp = dbSet.Where(expression)
                        .OrderBy(orderByProperty)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize);
                    if (orderByProperties != null)
                    {
                        querytemp = orderByProperties.Aggregate(querytemp,
                            (current, property) => current.OrderBy(property));
                    }
                    return querytemp.ToList();
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取分页实体集合
        /// </summary>
        public IEnumerable<TEntity> GetPager<TEntity>(string entsql,
            ObjectParameter[] parameters,
            int pageIndex,
            int pageSize,
            ref long totalCount,
            string orderByProperty,
            params string[] includePath) where TEntity : class
        {
            try
            {
                if (string.IsNullOrEmpty(orderByProperty))
                    orderByProperty = GetFirstDefaultPropertyName(typeof(TEntity));
                using (TDbContext context = CreateDbContext())
                {
                    context.Configuration.LazyLoadingEnabled = true;
                    context.Configuration.ProxyCreationEnabled = false;
                    DbSet<TEntity> dbSet = context.Set<TEntity>();

                    if (includePath != null && includePath.Length > 0)
                    {
                        ObjectQuery<TEntity> query = CreatePathQuery(dbSet, includePath).AsObjectQuery();
                        totalCount = query.Where(entsql, parameters).LongCount();

                        return query.Where(entsql, parameters)
                            .OrderBy(orderByProperty)
                            .Skip(pageIndex * pageSize)
                            .Take(pageSize)
                            .ToList();
                    }

                    var objQuery = dbSet.AsObjectQuery();
                    totalCount = (int)objQuery.Where(entsql, parameters).LongCount();

                    return objQuery.Where(entsql, parameters)
                        .OrderBy(orderByProperty)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize)
                        .ToList();
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 其他扩展操作

        public void ExposedDbContext(Action<TDbContext> callDelegate)
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    if (callDelegate != null)
                    {
                        callDelegate(context);
                    }
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 执行单条sql操作
        /// </summary>
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    ObjectContext objectContext = DbContextToObjectContext(context);
                    using (objectContext.Connection.CreateConnectionScope())
                    {
                        DbCommand cmd = objectContext.CreateStoreCommand(sql, CommandType.Text, parameters);
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 执行单条sql操作,返回查询dataset
        /// </summary>
        public DataSet ExecuteSqlCommandReturnDataSet(string sql)
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    ObjectContext objectContext = DbContextToObjectContext(context);
                    using (objectContext.Connection.CreateConnectionScope())
                    {
                        DbCommand cmd = objectContext.CreateStoreCommand(sql, CommandType.Text);
                        var da = new SqlDataAdapter(cmd as SqlCommand);
                        var ds = new DataSet();
                        da.Fill(ds);
                        da.Dispose();
                        return ds;
                    }
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 执行sql并返回集合
        /// </summary>
        public IEnumerable<TEntity> SqlQuery<TEntity>(string sql, params object[] parameters)
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    ObjectContext objectContext = DbContextToObjectContext(context);
                    return objectContext.CreateStoreCommand(sql, CommandType.Text, parameters).Materialize<TEntity>();
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 执行sql返回DataSet数据集
        /// </summary>
        public DataSet ExecuteProcedure(string procedureName, params object[] parameters)
        {
            try
            {
                using (TDbContext context = CreateDbContext())
                {
                    DbCommand cmd = DbContextToObjectContext(context)
                        .CreateStoreCommand(procedureName, CommandType.StoredProcedure, parameters);
                    var da = new SqlDataAdapter(cmd as SqlCommand);
                    var ds = new DataSet();
                    da.Fill(ds);
                    da.Dispose();
                    return ds;
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
