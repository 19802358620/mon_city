using System;
using System.Data;
using System.Linq;
using Ge.Infrastructure.DataBase.DbHelper;
using Ge.Infrastructure.Extensions;

namespace EKP.Repository.Ado
{
    /// <summary>
    /// 实体事务管理器
    /// </summary>
    public class EntityTrans : IDisposable
    {
        private Trans trans = null;
        private DbHelper db = null;

        public EntityTrans(Trans trans)
        {
            this.trans = trans;
            this.db = new DbHelper(trans.DbConnection.ConnectionString);
        }

        public DataTable GetDt(string sql) 
        {
            var cmd = db.GetSqlStringCommond(sql);
            var dt = db.ExecuteDataTable(cmd, trans);
            return dt;
        }

        public T Get<T>(int id) where T : class, new()
        {
            var sql = EntitySql.SelectSql<T>(id);
            var cmd = db.GetSqlStringCommond(sql);
            var dt = db.ExecuteDataTable(cmd, trans);
            var entity = dt.ToList<T>().FirstOrDefault();
            return entity;
        }

        public void Add<T>(T entity) where T : class
        {
            var sql = EntitySql.AddSql(entity);
            var cmd = db.GetSqlStringCommond(sql);
            var id = db.ExecuteScalar(cmd, trans);
            var entityAttribute_id = entity.GetType().GetProperty("Id");
            entityAttribute_id.SetValue(entity, Convert.ToInt32(id));
        }

        public void Update<T>(T entity) where T : class
        {
            Update(entity, null);
        }

        public void Update<T>(T entity, params string[] fileds) where T : class
        {
            var sql = EntitySql.UpdateSql(entity, fileds);
            var cmd = db.GetSqlStringCommond(sql);
            db.ExecuteNonQuery(cmd, trans);
        }

        public void Delete<T>(T entity) where T : class
        {
            var sql = EntitySql.DeleteTemplate(entity);
            var cmd = db.GetSqlStringCommond(sql);
            db.ExecuteNonQuery(cmd, trans);
        }

        /// <summary>
        /// 提交所有操作
        /// </summary>
        public void SaveChange()
        {
            trans.Commit();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            trans.Colse();
            trans.Dispose();
        }

    }
}
