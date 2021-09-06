using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;
using Ge.Infrastructure.EasyUi;
using Ge.Infrastructure.Metronicv;
using Ge.Infrastructure.Repository;

namespace EKP.Service.Base.Ef
{
    internal interface IServiceBase<TEntity> where TEntity : class, new()
    {
        bool AddObject(TEntity entity);
        bool AddObject(IEnumerable<TEntity> data);

        bool UpdateObject(TEntity entity, string[] fileds = null);
        bool UpdateObject(IEnumerable<TEntity> data);
        bool UpdateObject(Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TEntity>> updateExpression);
        bool UpdateObject(IEnumerable<UpdateExpressionsEntity<TEntity>> updateExpressions);

        void DeleteObject(TEntity entity);
        void DeleteObject(IEnumerable<TEntity> data);
        void DeleteObject(Expression<Func<TEntity, bool>> expression);
        void DeleteObject2(Expression<Func<TEntity, bool>> expression);

        TEntity GetEntity(Expression<Func<TEntity, bool>> where, params string[] path);
        TDEntity GetTdEntity<TDEntity>(Expression<Func<TEntity, bool>> where, params string[] path);
        List<TEntity> GetTopList(Expression<Func<TEntity, bool>> expression, string orderBy, int totalRecord,
            params string[] includePath);
        List<TEntity> GetList(Expression<Func<TEntity, bool>> where, params string[] path);
        List<TEntity> GetList(string entsql, ObjectParameter[] parameters, params string[] includePath);
        List<TDEntity> GetTdList<TDEntity>(Expression<Func<TEntity, bool>> where, params string[] path);

        EasyGridResult<TModel> DataGridPager<TModel>(EasyGridParam<TEntity> param);
        JqgridResult<TModel> JqgridPager<TModel>(JqgridParam<TEntity> param, params string[] path);

        IEnumerable<TEntity2> SqlQuery<TEntity2>(string sql, params object[] parameter) where TEntity2 : class;
        DataSet ExecuteSqlCommandReturnDataSet(string sql);
        bool ExecuteSqlCommand(string sql, params object[] parameters);
        DataSet ExecuteProcedure(string procedureName, params object[] parameters);
        bool Exists(Expression<Func<TEntity, bool>> where, params string[] path);
        object ExecuteService(ExecuteServiceCallBackHandler callBackHandler, bool isOpenTransaction = true);
    }
}