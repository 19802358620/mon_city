using System;
    using System.Data;
    using System.Data.Common;
    using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web.Http.Tracing;
using Ge.Infrastructure.Sql;

namespace Ge.Infrastructure.DataBase.DbHelper
{
    /// <summary>
    /// 名    称：DbHelper
    /// 作    者：胡政
    /// 来    源：http://www.cnblogs.com/JemBai/archive/2008/09/02/1281864.html
    /// 创建时间：2015-08-21
    /// 联系方式：13436053642
    /// 描    述：数据库帮助类
    /// </summary>
    public class DbHelper : IDbHelper
    {
        private static string dbProviderName = "System.Data.SqlClient";
        private static string dbConnectionString = string.Empty;

        private DbConnection connection;
        public DbHelper()
        {
            this.connection = CreateConnection(DbHelper.dbConnectionString);
        }
        public DbHelper(string connectionString)
        {
            this.connection = CreateConnection(connectionString);
        }
        public static DbConnection CreateConnection()
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbHelper.dbProviderName);
            DbConnection dbconn = dbfactory.CreateConnection();
            dbconn.ConnectionString = DbHelper.dbConnectionString;
            return dbconn;
        }
        public static DbConnection CreateConnection(string connectionString)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbHelper.dbProviderName);
            DbConnection dbconn = dbfactory.CreateConnection();
            dbconn.ConnectionString = connectionString;
            return dbconn;
        }

        public DbCommand GetStoredProcCommond(string storedProcedure)
        {
            DbCommand dbCommand = connection.CreateCommand();
            dbCommand.CommandText = storedProcedure;
            dbCommand.CommandType = CommandType.StoredProcedure;
            return dbCommand;
        }
        public DbCommand GetSqlStringCommond(string sqlQuery)
        {
            DbCommand dbCommand = connection.CreateCommand();
            dbCommand.CommandText = sqlQuery;
            dbCommand.CommandType = CommandType.Text;
            return dbCommand;
        }

        #region 添加参数
        public void AddParameterCollection(DbCommand cmd, DbParameterCollection dbParameterCollection)
        {
            foreach (DbParameter dbParameter in dbParameterCollection)
            {
                cmd.Parameters.Add(dbParameter);
            }
        }
        public void AddOutParameter(DbCommand cmd, string parameterName, DbType dbType, int size)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Size = size;
            dbParameter.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(dbParameter);
        }
        public void AddInParameter(DbCommand cmd, string parameterName, DbType dbType, object value)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value;
            dbParameter.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(dbParameter);
        }
        public void AddReturnParameter(DbCommand cmd, string parameterName, DbType dbType)
        {
            DbParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(dbParameter);
        }
        public DbParameter GetParameter(DbCommand cmd, string parameterName)
        {
            return cmd.Parameters[parameterName];
        }

        #endregion

        #region 执行sql
        /// <summary>
        /// 获取DataSet数据集
        /// </summary>
        public DataSet ExecuteDataSet(DbCommand cmd)
        {
            try
            {
                DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbHelper.dbProviderName);
                DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
                dbDataAdapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                dbDataAdapter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                    throw new SqlExecuteException(cmd.CommandText, ex);
                else
                    throw;
            }
        }
        /// <summary>
        /// 获取DataTable数据集
        /// </summary>
        public DataTable ExecuteDataTable(DbCommand cmd)
        {
            try
            {
                DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbHelper.dbProviderName);
                DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
                dbDataAdapter.SelectCommand = cmd;
                DataTable dataTable = new DataTable();
                dbDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                    throw new SqlExecuteException(cmd.CommandText, ex);
                else
                    throw;
            }
        }
        /// <summary>
        /// 获取DataReader数据对象
        /// </summary>
        public DbDataReader ExecuteReader(DbCommand cmd)
        {
            try
            {
                cmd.Connection.Open();
                DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                    throw new SqlExecuteException(cmd.CommandText, ex);
                else
                    throw;
            }
        }
        /// <summary>
        /// 执行sql并返回影响条目数
        /// </summary>
        public int ExecuteNonQuery(DbCommand cmd)
        {
            try
            {
                cmd.Connection.Open();
                int ret = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                return ret;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                    throw new SqlExecuteException(cmd.CommandText, ex);
                else
                    throw;
            }
        }
        /// <summary>
        /// 执行sql返回唯一对象
        /// </summary>
        public object ExecuteScalar(DbCommand cmd)
        {
            try
            {
                cmd.Connection.Open();
                object ret = cmd.ExecuteScalar();
                cmd.Connection.Close();
                return ret;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                    throw new SqlExecuteException(cmd.CommandText, ex);
                else
                    throw;
            }
        }
        #endregion        

        #region 事务执行sql
        /// <summary>
        /// 获取DataSet数据集
        /// </summary>
        public DataSet ExecuteDataSet(DbCommand cmd, Trans t)
        {
            try
            {
                cmd.Connection = t.DbConnection;
                cmd.Transaction = t.DbTrans;
                DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbHelper.dbProviderName);
                DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
                dbDataAdapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                dbDataAdapter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                    throw new SqlExecuteException(cmd.CommandText, ex);
                else
                    throw;
            }
        }
        /// <summary>
        /// 获取DataTable数据集
        /// </summary>
        public DataTable ExecuteDataTable(DbCommand cmd, Trans t)
        {
            try
            {
                cmd.Connection = t.DbConnection;
                cmd.Transaction = t.DbTrans;
                DbProviderFactory dbfactory = DbProviderFactories.GetFactory(DbHelper.dbProviderName);
                DbDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
                dbDataAdapter.SelectCommand = cmd;
                DataTable dataTable = new DataTable();
                dbDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                    throw new SqlExecuteException(cmd.CommandText, ex);
                else
                    throw;
            }
        }
        /// <summary>
        /// 获取DataReader数据对象
        /// </summary>
        public DbDataReader ExecuteReader(DbCommand cmd, Trans t)
        {
            try
            {
                cmd.Connection.Close();
                cmd.Connection = t.DbConnection;
                cmd.Transaction = t.DbTrans;
                DbDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                return reader;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                    throw new SqlExecuteException(cmd.CommandText, ex);
                else
                    throw;
            }
        }
        /// <summary>
        /// 执行sql并返回影响条目数
        /// </summary>
        public int ExecuteNonQuery(DbCommand cmd, Trans t)
        {
            try
            {
                cmd.Connection.Close();
                cmd.Connection = t.DbConnection;
                cmd.Transaction = t.DbTrans;
                int ret = cmd.ExecuteNonQuery();
                return ret;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                    throw new SqlExecuteException(cmd.CommandText, ex);
                else
                    throw;
            }
        }
        /// <summary>
        /// 执行sql返回唯一对象
        /// </summary>
        public object ExecuteScalar(DbCommand cmd, Trans t)
        {
            try
            {
                cmd.Connection.Close();
                cmd.Connection = t.DbConnection;
                cmd.Transaction = t.DbTrans;
                object ret = cmd.ExecuteScalar();
                return ret;
            }
            catch (Exception ex)
            {
                if (ex is SqlException)
                    throw new SqlExecuteException(cmd.CommandText, ex);
                else
                    throw;
            }
        }
        #endregion

        #region 扩展
        /// <summary>
        /// 获取DataTable分页数据
        /// </summary>
        public DataTable ExecuteDataTablePager(string sql, int take, int skip)
        {
            DbCommand cmd = null;
            DataTable dt = null;
            string pagerSql = SqlHelper.GetPagerSql(sql, take, skip);

            cmd = GetSqlStringCommond(pagerSql);
            dt = ExecuteDataTable(cmd);

            return dt;
        }
        /// <summary>
        /// 获取总条目数
        /// </summary>
        public int ExecuteTotalCount(string sql)
        {
            DbCommand cmd = null;
            DataTable dt = null;
            string totalCountSql = SqlHelper.GetTotalCountSql(sql);

            cmd = GetSqlStringCommond(totalCountSql);
            int count = Convert.ToInt32(ExecuteScalar(cmd));

            return count;
        }
        #endregion
    }
    
}
