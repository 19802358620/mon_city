using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using Ge.Infrastructure.EasyUi;
using Ge.Infrastructure.Extensions;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Repository;
using Ge.Infrastructure.Utilities;

namespace EKP.Service.Base.Ef
{
    internal class ServiceBase<TDbEntitie, TEntity> : ServiceAdapter<TDbEntitie>, IServiceBase<TEntity>
        where TDbEntitie : DbContext, new()
        where TEntity : class, new()
    {
        #region message

        private const string InsertFail = "The new operation could not be completed, failure see system error log!";
        private const string UpdateFail = "The update operation failed to complete, failure see system error log!";
        private const string DeleteFail = "Delete operation failed to complete, failure see system error log!";
        private const string OtherFail = "The operation failed!";

        #endregion

        #region Insert

        public virtual bool AddObject(TEntity entity)
        {
            try
            {
                object result = base.ExecuteService(() => base.Adapter.AddObject(entity), false);
                return ((int)result) > 0;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public virtual bool AddObject(IEnumerable<TEntity> data)
        {
            try
            {
                object result = base.ExecuteService(() => base.Adapter.AddObject(data), true);
                return ((int)result) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Update

        public virtual bool UpdateObject(TEntity entity, string[] fileds = null)
        {
            try
            {
                object result = base.ExecuteService(() => base.Adapter.UpdateObject(entity, fileds), false);
                return ((int)result) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual bool UpdateObject(IEnumerable<TEntity> data)
        {
            try
            {
                object result = base.ExecuteService(() => base.Adapter.UpdateObject(data, null), true);
                return ((int)result) > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Update operation for EntityFramework Extensions

        public bool UpdateObject(Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TEntity>> updateExpression)
        {
            try
            {
                object result = base.ExecuteService(
                    () => base.Adapter.UpdateObject(filterExpression, updateExpression), false);
                return ((int)result) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateObject(IEnumerable<UpdateExpressionsEntity<TEntity>> updateExpressions)
        {
            try
            {
                object result = base.ExecuteService(() => base.Adapter.UpdateObject(updateExpressions), false);
                return ((int)result) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Delete

        public virtual void DeleteObject(TEntity entity)
        {
            try
            {
                base.ExecuteService(() => base.Adapter.DeleteObject(entity), false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void DeleteObject(IEnumerable<TEntity> data)
        {
            try
            {
                base.ExecuteService(() => base.Adapter.DeleteObject(data), true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void DeleteObject(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                base.ExecuteService(() => base.Adapter.DeleteObject(expression), true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Delete operation for EntityFramework Extensions

        public virtual void DeleteObject2(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                base.ExecuteService(() => base.Adapter.DeleteObject2(expression), true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Select
        
        public virtual TEntity GetEntity(Expression<Func<TEntity, bool>> where, params string[] path)
        {
            return base.Adapter.GetEntity(where, path);
        }

        public virtual TDEntity GetTdEntity<TDEntity>(Expression<Func<TEntity, bool>> where, params string[] path)
        {
            return ObjectMapper.Mapper<TEntity, TDEntity>(this.GetEntity(where, path));
        }

        public List<TEntity> GetTopList(
            Expression<Func<TEntity, bool>> expression,
            string orderBy,
            int totalRecord, 
            params string[] includePath)
        {
            return this.Adapter.GetTopList(expression, orderBy, totalRecord, includePath).ToList();
        }

        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> where, params string[] path)
        {
            return base.Adapter.GetList(@where, path).ToList();
        }

        public virtual List<TEntity> GetList(string entsql,
            ObjectParameter[] parameters,
            params string[] includePath)
        {
            return base.Adapter.GetList<TEntity>(entsql, parameters, includePath).ToList();
        }

        public virtual List<TDEntity> GetTdList<TDEntity>(Expression<Func<TEntity, bool>> where, params string[] path)
        {
            return ObjectMapper.Mapper<List<TEntity>, List<TDEntity>>(this.GetList(where, path));
        }

        public virtual EasyGridResult<TModel> DataGridPager<TModel>(EasyGridParam<TEntity> param)
        {
            long totalCount = 0;
            var result = base.ExecuteService(() => base.Adapter.GetPager(
                param.Where,
                param.Page - 1,
                param.Rows,
                ref totalCount,
                param.Order = (param.Order == null) ?
                null : string.Format("{0} {1}", param.Sort, param.Order),
                param.IncludePath));
            var dataGridResult = new EasyGridResult<TModel>(param)
            {
                TotalRecords = Convert.ToInt32(totalCount),
                Rows = ((List<TEntity>)result).ToList<TEntity, TModel>()
            };
            return dataGridResult;
        } 

        public virtual JqgridResult<TModel> JqgridPager<TModel>(JqgridParam<TEntity> param, params string[] path)
        {
            var dataGridResult = DataGridPager<TModel>(new EasyGridParam<TEntity>()
            {
                Where = param.Where,
                Page = param.Page,
                Rows = param.PageSize,
                Sort = param.SortBy,
                Order = param.SortOrder,
            });
            var jqgridResult = new JqgridResult<TModel>(param)
            {
                Rows = dataGridResult.rows,
                TotalRecords = Convert.ToInt32(dataGridResult.total)
            };
            return jqgridResult;
        }

        #endregion
        
        #region Other

        public IEnumerable<TEntity2> SqlQuery<TEntity2>(string sql, params object[] parameter) where TEntity2 : class
        {
            object result = base.ExecuteService(() => base.Adapter.SqlQuery<TEntity2>(sql, parameter));
            return result as List<TEntity2>;
        }

        public bool ExecuteSqlCommand(string sql, params object[] parameters)
        {
            try
            {
                object result = base.ExecuteService(() => base.Adapter.ExecuteSqlCommand(sql, parameters), false);
                return ((int)result) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet ExecuteSqlCommandReturnDataSet(string sql)
        {
            try
            {
                object result = base.ExecuteService(() => base.Adapter.ExecuteSqlCommandReturnDataSet(sql), false);
                return result as DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet ExecuteProcedure(string procedureName, params object[] parameters)
        {
            try
            {
                object result = base.ExecuteService(() => base.Adapter.ExecuteProcedure(procedureName, parameters),
                    false);
                return result as DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> where, params string[] path)
        {
            return this.GetEntity(where, path) != null;
        }

        #endregion
    }
}