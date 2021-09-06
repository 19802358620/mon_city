using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.DataBase.DbHelper
{
    /// <summary>
    /// 名    称：Trans
    /// 作    者：胡政
    /// 来    源：http://www.cnblogs.com/JemBai/archive/2008/09/02/1281864.html
    /// 创建时间：2015-08-21
    /// 联系方式：13436053642
    /// 描    述：数据库连接操作（执行、释放资源、回滚）等操作
    /// </summary>
    public class Trans : IDisposable
    {
        private DbConnection conn;
        private DbTransaction dbTrans;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public DbConnection DbConnection
        {
            get { return this.conn; }
        }
        /// <summary>
        /// 数据库事务操作对象
        /// </summary>
        public DbTransaction DbTrans
        {
            get { return this.dbTrans; }
        }


        public Trans()
        {
            conn = DbHelper.CreateConnection();
            conn.Open();
            dbTrans = conn.BeginTransaction();
        }
        public Trans(string connectionString)
        {
            conn = DbHelper.CreateConnection(connectionString);
            conn.Open();
            dbTrans = conn.BeginTransaction();
        }


        #region 方法
        /// <summary>
        /// 执行提交
        /// </summary>
        public void Commit()
        {
            dbTrans.Commit();
            this.Colse();
        }
        /// <summary>
        /// 执行回滚
        /// </summary>
        public void RollBack()
        {
            dbTrans.Rollback();
            this.Colse();
        }
        /// <summary>
        /// 释放占用资源
        /// </summary>
        public void Dispose()
        {
            this.Colse();
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Colse()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
        #endregion
    }
}
