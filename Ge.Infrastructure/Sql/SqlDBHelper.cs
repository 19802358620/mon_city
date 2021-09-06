using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Transactions;
using System.Collections;

namespace Ge.Infrastructure.Sql
{
    /// <summary>
    /// 名    称：SqlDBHelper
    /// 作    者：刘亚辉
    /// 创建时间：2015-12-25
    /// 描    述：数据库操作类
    /// </summary>
    public class SqlDBHelper
    {
        private string connectionString = "";
        protected SqlConnection conn = null;

        #region  //构造函数
        public SqlDBHelper()
        {
            this.connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
            this.conn = new SqlConnection(connectionString);
        }
       
        #endregion

        #region //打开连接
        public void OpenDb()
        {
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region //关闭连接
        public void CloseDb()
        {
            if (!object.Equals(conn, null) && (conn.State != ConnectionState.Closed))
            {
                conn.Close();
            }
        }
        #endregion

        #region //释放连接
        public void Dispose()
        {
            if (conn != null)
            {
                conn.Dispose();
                conn = null;
            }
        }
        #endregion

        #region  //执行单条SQL(插入、更新、删除)
        /// <summary>
        /// 执行单条SQL(插入、更新、删除)
        /// </summary>
        /// <param name="sql_"></param>
        public void ExecuteNonQuery(string sql_)
        {
            try
            {
                OpenDb();
                SqlCommand cm = new SqlCommand(sql_, conn);
                cm.ExecuteNonQuery();
                cm.Dispose();
                cm = null;
                CloseDb();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "  " + sql_);
            }
        }
        #endregion

        #region  //执行SQL返回首行首列的值
        /// <summary>
        /// 执行SQL返回首行首列的值,不存在返回""
        /// </summary>
        public string GetSingleVal(string sql_)
        {
            string RetStr = null;
            try
            {
                OpenDb();
                SqlCommand cm = new SqlCommand(sql_, conn);
                RetStr = cm.ExecuteScalar() == null ? "" : cm.ExecuteScalar().ToString();
                cm.Dispose();
                cm = null;
                CloseDb();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + ", " + sql_);
            }

            return RetStr;
        }
        #endregion

        #region  //判断是否存在对应的数据
        /// <summary>
        /// 根据SQL判断是否存在对应的数据
        /// </summary>
        public bool YNExistData(string sql_)
        {
            bool ynExist = false;
            try
            {
                DataTable dt = GetDataTable(sql_);
                if (dt != null && dt.Rows.Count > 0)
                    ynExist = true;
            }
            catch (Exception e)
            {
                ynExist = false;
                throw new Exception(e.ToString() + ", " + sql_);
            }

            return ynExist;
        }
        #endregion

        #region  //执行SQL返回DataTable数据集
        /// <summary>
        /// 执行SQL返回DataTable数据集
        /// </summary>
        /// <param name="sql_"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql_)
        {
            if (sql_ == "")
                return null;
            DataTable dt = null;
            DataSet ds = null;
            try
            {
                OpenDb();
                SqlDataAdapter myad = new SqlDataAdapter(sql_, conn);
                ds = new DataSet();
                myad.Fill(ds);//用数据适配器填充数据集
                myad.Dispose();
                myad = null;
                CloseDb();

                if (ds.Tables.Count <= 0)
                    return null;
                dt = ds.Tables[0];
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "  " + sql_);
            }
            return dt;
        }
        #endregion

        #region //使用事务执行多条SQL(插入、更新、删除)
        /// <summary>
        /// 使用事务执行多条SQL(插入、更新、删除)
        /// </summary>
        /// <param name="sqls"></param>
        public void ExecTansaction(List<string> sqls)
        {
            if (sqls.Count == 0) return;

            OpenDb();

            // 启动一个事务。 
            SqlTransaction myTran = conn.BeginTransaction();
            // 为事务创建一个命令
            SqlCommand myCom = new SqlCommand();
            myCom.Connection = conn;
            myCom.Transaction = myTran;
            try
            {
                foreach (string sql in sqls)
                {
                    myCom.CommandText = sql;
                    myCom.ExecuteNonQuery();
                }
                myTran.Commit();//提交事务
            }
            catch (Exception Ex)
            {
                myTran.Rollback();

                //返回异常的错误信息 
                //MessageBox.Show("提交数据失败!\n" + ex.Message.ToString(), "异常信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw new Exception("提交数据失败!\n" + Ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region //使用事务执行多条SQL(插入、更新、删除)-插入语句返回插入后的Id
        /// <summary>
        /// 使用事务执行多条SQL(插入、更新、删除)-插入语句返回插入后的Id
        /// </summary>
        /// <param name="sqls"></param>
        /// <param name="ids">插入语句返回插入后的Id</param>
        public void ExecTansaction(List<string> sqls, ref List<string> ids)
        {
            if (sqls.Count == 0) return;

            OpenDb();

            // 启动一个事务。 
            SqlTransaction myTran = conn.BeginTransaction();
            // 为事务创建一个命令
            SqlCommand myCom = new SqlCommand();
            myCom.Connection = conn;
            myCom.Transaction = myTran;
            try
            {
                ids.Clear();
                foreach (string sql in sqls)
                {
                    if (sql.Substring(0, 6).ToLower() == "insert")
                    {
                        myCom.CommandText = sql;
                        object id = myCom.ExecuteScalar();
                        ids.Add(id.ToString().Trim());
                    }
                    else
                    {
                        myCom.CommandText = sql;
                        myCom.ExecuteNonQuery();
                    }
                }
                myTran.Commit();//提交事务
            }
            catch (Exception Ex)
            {
                myTran.Rollback();
                
                //返回异常的错误信息 
                //MessageBox.Show("提交数据失败!\n" + ex.Message.ToString(), "异常信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw new Exception("提交数据失败!\n" + Ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region //使用事务执行多条SQL(插入、更新、删除)-含有主表子表依赖关系
        /// <summary>
        /// 使用事务执行多条SQL(插入、更新、删除)
        /// </summary>
        /// <param name="sqlMain">主表</param>
        /// <param name="sqlDetails">子表（含有一个{0}）</param>
        /// <param name="sqls">其它sql</param>
        /// <returns>返回主表的Id</returns>
        public object ExecTansaction(string sqlMain, List<string> sqlDetails, List<string> sqls)
        {
            OpenDb();
            object id;

            // 启动一个事务。 
            SqlTransaction myTran = conn.BeginTransaction();
            // 为事务创建一个命令
            SqlCommand myCom = new SqlCommand();
            myCom.Connection = conn;
            myCom.Transaction = myTran;
            try
            {
                //执行主表
                myCom.CommandText = sqlMain + "; select SCOPE_IDENTITY() as Id";
                id = myCom.ExecuteScalar();

                //执行子表
                foreach (string sql in sqlDetails)
                {
                    string sqlDetail = string.Format(sql, id);
                    myCom.CommandText = sqlDetail;
                    myCom.ExecuteNonQuery();
                }

                foreach (string sql in sqls)
                {
                    myCom.CommandText = sql;
                    myCom.ExecuteNonQuery();
                }
                myTran.Commit();//提交事务
            }
            catch (Exception Ex)
            {
                myTran.Rollback();

                //返回异常的错误信息 
                throw new Exception("提交数据失败!\n" + Ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return id;
        }
        /// <summary>
        /// 使用事务执行多条SQL(插入、更新、删除)
        /// </summary>
        /// <param name="sqls">其它sql</param>
        /// <param name="sqls_ref">多个主表，每个主表又包含多个子表（含有一个{0}）</param>
        public void ExecTansaction(List<string> sqls, Dictionary<string, List<string>> sqls_ref)
        {
            OpenDb();
            // 启动一个事务。 
            SqlTransaction myTran = conn.BeginTransaction();
            // 为事务创建一个命令
            SqlCommand myCom = new SqlCommand();
            myCom.Connection = conn;
            myCom.Transaction = myTran;
            try
            {
                foreach (string sql in sqls)
                {
                    myCom.CommandText = sql;
                    myCom.ExecuteNonQuery();
                }

                foreach (string sqlMain in sqls_ref.Keys)
                {
                    //执行主表
                    myCom.CommandText = sqlMain + "; select SCOPE_IDENTITY() as Id";
                    object id = myCom.ExecuteScalar();

                    for (int i = 0; i < sqls_ref[sqlMain].Count; i++)
                    {
                        //执行子表
                        string sqlDetail = string.Format(sqls_ref[sqlMain][i], id);
                        myCom.CommandText = sqlDetail;
                        myCom.ExecuteNonQuery();
                    }
                }

                myTran.Commit();//提交事务
            }
            catch (Exception Ex)
            {
                myTran.Rollback();

                //返回异常的错误信息 
                throw new Exception("提交数据失败!\n" + Ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }


        #region 使用事务执行多条SQL(插入、更新、删除)--逻辑层次-3层       
        /// <summary>
        /// 使用事务执行多条SQL(插入、更新、删除)--逻辑层次-3层
        /// </summary>
        /// <param name="sqls">其它sql</param>
        /// <param name="sqls_ref">多个主表，每个主表又包含多个子表（含有一个{0}），每个子表又包含数个子表</param>
        /// 
        public void ExecTansactionTree(List<string> sqls, List<Firstmodel> sqls_F)
        {
            OpenDb();
            // 启动一个事务。 
            SqlTransaction myTran = conn.BeginTransaction();
            // 为事务创建一个命令
            SqlCommand myCom = new SqlCommand();
            myCom.Connection = conn;
            myCom.Transaction = myTran;
            try
            {
                foreach (string sql in sqls)
                {
                    myCom.CommandText = sql;
                    myCom.ExecuteNonQuery();
                }
                foreach (var sqlFirst in sqls_F)
                {
                    //执行主表
                    myCom.CommandText = sqlFirst.sqlFirst;
                    object id = myCom.ExecuteScalar();
                    for (int i = 0; i < sqlFirst.Second.Count; i++)
                    {
                        //执行子表
                        string sqlSecond = string.Format(sqlFirst.Second[i].sqlSecond, id);
                        myCom.CommandText = sqlSecond;
                        var  id2 = myCom.ExecuteScalar();
                        for (int j=0;j< sqlFirst.Second[i].sqlThird.Count; j++)
                        {
                            string sqlThird = string.Format(sqlFirst.Second[i].sqlThird[j], id2);
                            myCom.CommandText = sqlThird;
                            myCom.ExecuteNonQuery();
                        }
                    }
                }
                myTran.Commit();//提交事务
            }
            catch (Exception Ex)
            {
                myTran.Rollback();

                //返回异常的错误信息 
                throw new Exception("提交数据失败!\n" + Ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        /// <summary>
        /// 使用事务执行多条主子及附带SQL并返回插入后的Id列表
        /// </summary>
        /// <param name="sqls_ref">主子SQL集</param>
        /// <param name="sqls">附带SQL</param>
        /// <param name="ids">返回Id列表</param>
        public void ExecTansaction(Dictionary<string, List<string>> sqls_ref, List<string> sqls, ref List<object> ids)
        {
            OpenDb();

            // 启动一个事务。 
            SqlTransaction myTran = conn.BeginTransaction();
            // 为事务创建一个命令
            SqlCommand myCom = new SqlCommand();
            myCom.Connection = conn;
            myCom.Transaction = myTran;
            try
            {
                foreach (string sqlMain in sqls_ref.Keys)
                {
                    //执行主表
                    myCom.CommandText = sqlMain + "; select SCOPE_IDENTITY() as Id";
                    object id = myCom.ExecuteScalar();
                    ids.Add(id);

                    for (int i = 0; i < sqls_ref[sqlMain].Count; i++)
                    {
                        //执行子表
                        string sqlDetail = string.Format(sqls_ref[sqlMain][i], id);
                        myCom.CommandText = sqlDetail;
                        myCom.ExecuteNonQuery();
                    }
                }

                foreach (string sql in sqls)
                {
                    myCom.CommandText = sql;
                    myCom.ExecuteNonQuery();
                }

                myTran.Commit();//提交事务
            }
            catch (Exception Ex)
            {
                myTran.Rollback();

                //返回异常的错误信息 
                throw new Exception("提交数据失败!\n" + Ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 使用事务执行带参数多条SQL(插入、更新、删除)
        /// </summary>
        /// <param name="sqlMain">主表</param>
        /// <param name="cmdParms">主表参数</param>
        /// <param name="sqlDetails">子表（含有一个{0}）</param>
        /// <param name="sqls">其它sql</param>
        /// <returns>返回主表的Id</returns>
        public object ExecTansaction(string sqlMain, SqlParameter[] cmdParms, List<string> sqlDetails, List<string> sqls)
        {
            OpenDb();
            object id;

            // 启动一个事务。 
            SqlTransaction myTran = conn.BeginTransaction();
            // 为事务创建一个命令
            SqlCommand myCom = new SqlCommand();
            myCom.Connection = conn;
            myCom.Transaction = myTran;
            try
            {
                //执行主表
                myCom.CommandText = sqlMain + "; select SCOPE_IDENTITY() as Id";
                PrepareCommand(myCom, conn, myTran, myCom.CommandText, cmdParms);
                id = myCom.ExecuteScalar();

                //执行子表
                foreach (string sql in sqlDetails)
                {
                    string sqlDetail = string.Format(sql, id);
                    myCom.CommandText = sqlDetail;
                    myCom.ExecuteNonQuery();
                }

                foreach (string sql in sqls)
                {
                    myCom.CommandText = sql;
                    myCom.ExecuteNonQuery();
                }
                myTran.Commit();//提交事务
            }
            catch (Exception Ex)
            {
                myTran.Rollback();

                //返回异常的错误信息 
                throw new Exception("提交数据失败!\n" + Ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return id;
        }

        /// <summary>
        /// 使用事务执行带参数多条SQL(插入、更新、删除)
        /// </summary>
        /// <param name="sqlMain">主表</param>
        /// <param name="cmdParms">主表参数</param>
        /// <param name="subSql">子表（含有一个{0}）</param>
        /// <param name="subParams">子表参数</param>
        /// <param name="sqls">其它sql</param>
        /// <returns>返回主表的Id</returns>
        public object ExecTansaction(string sqlMain, SqlParameter[] cmdParms, List<string> subSql, List<SqlParameter[]> subParams, List<string> sqls)
        {
            OpenDb();
            object id;

            // 启动一个事务。 
            SqlTransaction myTran = conn.BeginTransaction();
            // 为事务创建一个命令
            SqlCommand myCom = new SqlCommand();
            myCom.Connection = conn;
            myCom.Transaction = myTran;
            try
            {
                //执行主表
                myCom.CommandText = sqlMain + "; select SCOPE_IDENTITY() as Id";
                PrepareCommand(myCom, conn, myTran, myCom.CommandText, cmdParms);
                id = myCom.ExecuteScalar();
                myCom.Parameters.Clear();

                //执行子表
                for (int i = 0; i < subSql.Count; i++)
                {
                    string sqlDetail = string.Format(subSql[i], id);
                    myCom.CommandText = sqlDetail;
                    PrepareCommand(myCom, conn, myTran, myCom.CommandText, subParams[i]);
                    myCom.ExecuteNonQuery();
                    myCom.Parameters.Clear();
                }

                foreach (string sql in sqls)
                {
                    myCom.CommandText = sql;
                    myCom.ExecuteNonQuery();
                }
                myTran.Commit();//提交事务
            }
            catch (Exception Ex)
            {
                myTran.Rollback();

                //返回异常的错误信息 
                throw new Exception("提交数据失败!\n" + Ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return id;
        }

        #endregion

        #region //使用分布式事务执行多条SQL(插入、更新、删除)
        /// <summary>
        /// 使用分布式事务执行多条SQL(插入、更新、删除)
        /// </summary>
        /// <param name="sqls"></param>
        public static void ExecTransactionScope(List<string> localSqls, List<string> globalSqls)
        {
            try
            {
                string connectionStringLocal = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
                string connectionStringGlobal = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DBConnectionString_Global"].ToString();

                //创建TransactionScope
                using (TransactionScope tsCope = new TransactionScope())
                {
                    using (SqlConnection cnLocal = new SqlConnection(connectionStringLocal))
                    {
                        cnLocal.Open();
                        foreach (string sql in localSqls)
                        {
                            SqlCommand cmd = new SqlCommand(sql, cnLocal);                            
                            cmd.ExecuteNonQuery();                           
                        }
                        cnLocal.Close();
                    }
                    using (SqlConnection cnGlobal = new SqlConnection(connectionStringGlobal))
                    {
                        cnGlobal.Open();

                        foreach (string sql in globalSqls)
                        {
                            SqlCommand cmd = new SqlCommand(sql, cnGlobal);                            
                            cmd.ExecuteNonQuery();
                        }
                        cnGlobal.Close();
                    }

                    tsCope.Complete();                    
                }
            }
            catch (Exception Ex)
            {
                //返回异常的错误信息 
                throw new Exception("提交数据失败!\n" + Ex.Message.ToString());
            }
            
        }
        /// <summary>
        /// 使用分布式事务执行多条SQL(插入、更新、删除)
        /// </summary>
        /// <param name="localSqlMain">主表</param>
        /// <param name="localSqlDetails">子表集合（含有一个{0}）</param>
        /// <param name="localSqls">其它sql集合</param>
        /// <param name="globalSqls">全局库sql集合</param>
        /// <param name="sqls"></param>
        public static void ExecTransactionScope(string localSqlMain, List<string> localSqlDetails, List<string> localSqls, List<string> globalSqls)
        {
            try
            {
                string connectionStringLocal = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
                string connectionStringGlobal = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DBConnectionString_Global"].ToString();

                //创建TransactionScope
                using (TransactionScope tsCope = new TransactionScope())
                {
                    using (SqlConnection cnLocal = new SqlConnection(connectionStringLocal))
                    {
                        cnLocal.Open();
                        //执行主表
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cnLocal;

                        cmd.CommandText = localSqlMain;
                        object id = cmd.ExecuteScalar();

                        //执行子表
                        foreach (string sql in localSqlDetails)
                        {
                            string sqlDetail = string.Format(sql, id);
                            cmd.CommandText = sqlDetail;
                            cmd.ExecuteNonQuery();
                        }

                        foreach (string sql in localSqls)
                        {
                            //SqlCommand cmd = new SqlCommand(sql, cnLocal);
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                        cnLocal.Close();
                    }
                    using (SqlConnection cnGlobal = new SqlConnection(connectionStringGlobal))
                    {
                        cnGlobal.Open();

                        foreach (string sql in globalSqls)
                        {
                            SqlCommand cmd = new SqlCommand(sql, cnGlobal);
                            cmd.ExecuteNonQuery();
                        }
                        cnGlobal.Close();
                    }

                    tsCope.Complete();
                }
            }
            catch (Exception Ex)
            {
                //返回异常的错误信息 
                throw new Exception("提交数据失败!\n" + Ex.Message.ToString());
            }

        }
        /// <summary>
        /// 使用分布式事务执行多条SQL(插入、更新、删除)---以全局库为主
        /// </summary>
        /// <param name="globalSqlMain">主表</param>
        /// <param name="globalSqlDetails">子表集合（含有一个{0}）</param>
        /// <param name="sqls"></param>
        public static void ExecTransactionScope_Global(string globalSqlMain, List<string> globalSqlDetails, List<string> globalSqls, List<string> localSqls)
        {
            try
            {
                string connectionStringLocal = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
                string connectionStringGlobal = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DBConnectionString_Global"].ToString();

                //创建TransactionScope
                using (TransactionScope tsCope = new TransactionScope())
                {
                    using (SqlConnection cnGlobal = new SqlConnection(connectionStringGlobal))
                    {
                        cnGlobal.Open();
                        //执行主表
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = cnGlobal;

                        cmd.CommandText = globalSqlMain;
                        object id = cmd.ExecuteScalar();

                        //执行子表
                        foreach (string sql in globalSqlDetails)
                        {
                            string sqlDetail = string.Format(sql, id);
                            cmd.CommandText = sqlDetail;
                            cmd.ExecuteNonQuery();
                        }

                        foreach (string sql in globalSqls)
                        {
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                        cnGlobal.Close();
                    }
                    using (SqlConnection cnLocal = new SqlConnection(connectionStringLocal))
                    {
                        cnLocal.Open();

                        foreach (string sql in localSqls)
                        {
                            SqlCommand cmd = new SqlCommand(sql, cnLocal);
                            cmd.ExecuteNonQuery();
                        }
                        cnLocal.Close();
                    }

                    tsCope.Complete();
                }
            }
            catch (Exception Ex)
            {
                //返回异常的错误信息 
                throw new Exception("提交数据失败!\n" + Ex.Message.ToString());
            }

        }
        #endregion
        
        #region //获得数据库服务器当前时间
        /// <summary>
        /// 获得数据库服务器当前时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetDbServerTime()
        {
            SqlDataReader rs = null;
            DateTime dt = new DateTime();
            try
            {
                OpenDb();
                SqlCommand cm = new SqlCommand("select getdate() ", conn);
                rs = cm.ExecuteReader(CommandBehavior.CloseConnection);
                cm.Dispose();
                cm = null;
                
                if (rs.Read())
                { dt = DateTime.Parse(rs[0].ToString()); }

                CloseDb();
            }
            catch
            {
                throw new Exception("取服务器时间出错！");
            }
            return dt;
        }
        #endregion

        #region  //执行对单个Entity的更新
        /// <summary>
        /// 执行对单个Entity的更新
        /// </summary>
        /// <param name="baseEntity"></param>
        public void ExecuteObjectUpdate(object baseEntity)
        {
            ExecuteObjectUpdate(baseEntity, "");
        }
        #endregion

        #region  //执行对单个Entity的更新(这个可以添加额外的约束条件)
        /// <summary>
        /// 执行对单个Entity的更新
        /// </summary>
        /// <param name="baseEntity">由控制器组装而来的实体</param>
        /// <param name="otherConditions">其他约束条件用(and开头,例如:"and name='王斐'"</param>
        public void ExecuteObjectUpdate(object baseEntity, string otherConditions)
        {
            string sql_ = "";//values 之前
            try
            {
                Type type = baseEntity.GetType();
                IList<CustomAttributeData> tableAttribute = type.GetCustomAttributesData();//该方法是根据Type属性找到的
                string tableName = "";//表名
                tableName = (String)tableAttribute[0].ConstructorArguments[0].Value;//解刨Type属性中偶然发现

                string operation = "update";//执行操作方法,此处可以考虑将操作类型operation由调用者确定
                PropertyInfo[] propertyList = type.GetProperties();//返回PropertyInfo类型，用于取得该类的属性的信息

                //生成SQL语句
                string propertyName = "";//属性名
                string propertyValue = "";//属性值

                sql_ = operation + " " + tableName + " set ";

                int updateCount = 0;//用于计数更新

                for (int i = 1; i < propertyList.Length; i++)
                //这里考虑并没有写死跟添加一样,也从0开始,有一段时间考虑想从i=1开始,
                //但是存在部分情况主键可以改变的情况,因此,这里会默认的将首字段进行拼接,
                //会出现update tableName set PKName=value.... where  PKName=value;的情况
                //个人认为这并不影响


                //2015年5月28日02:14:26
                //由于部分主键是自增的,如果默认主键参与跟新,则会因为自增字段不能update而报错
                //但是没有找到合适方法
                //这里还是从第2个开始判断
                {
                    //propertyList.Add("");
                    propertyName = propertyList[i].Name;
                    propertyValue = GetObjectPropertyValue(type, baseEntity, propertyName);//获取属性值
                    if (propertyValue != null)
                    {
                        if (updateCount != 0)//判断是否是第一个更新的字段如果是则不添加逗号
                        {
                            sql_ += " , ";
                        }
                        sql_ += "[" + propertyName + "]" + "= N'{0}'";
                        sql_ = string.Format(sql_, propertyValue);
                        updateCount++;
                    }

                }
                string PKPropertyName = propertyList[0].Name;//此处还在考虑 是否需要解析一下 ,可以改为支持两个主键或者以上
                string PKPropertyValue = GetObjectPropertyValue(type, baseEntity, PKPropertyName);

                sql_ = sql_ + " where " + PKPropertyName + "=N'{0}'" + " " + otherConditions;//此处添加而外约束条件跟主键
                sql_ = String.Format(sql_, PKPropertyValue);

                OpenDb();
                SqlCommand cm = new SqlCommand(sql_, conn);
                cm.ExecuteNonQuery();
                cm.Dispose();
                cm = null;
                CloseDb();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "  " + sql_);
            }

        }
        #endregion

        #region  //执行对单个Entity的插入（多表操作，表中含有自增字段慎用） 返回值是Entity的第一个属性
        /// <summary>
        /// 执行对单个Entity的插入（多表操作，表中含有自增字段慎用） 返回值是Entity的第一个属性
        /// </summary>
        /// <param name="sql_"></param>
        public string ExecuteObjectInput(object baseEntity)
        {
            string sql_ = "";//values 之前
            try
            {
                Type type = baseEntity.GetType();//得到对象类型
                IList<CustomAttributeData> tableAttribute = type.GetCustomAttributesData();//该方法是根据Type属性找到的
                string tableName = "";//表名
                tableName = (String)tableAttribute[0].ConstructorArguments[0].Value;//解刨Type属性中偶然发现
                string operation = "insert into ";//执行操作方法
                PropertyInfo[] propertyList = type.GetProperties();//返回PropertyInfo类型，用于取得该类的属性的信息

                //生成SQL语句
                string values = "";//values( 之后 最后一起拼装起来
                string propertyName = "";//属性名
                string propertyValue = "";//属性值

                sql_ = operation + " " + tableName + " ( ";

                int inputCount = 0;//用于计数新增
                for (int i = 0; i < propertyList.Length; i++)//这个从Entity第一个属性开始就解析,主要因为添加时候只要主属性不是自增的就需要填写
                {
                    //propertyList.Add("");
                    propertyName = propertyList[i].Name;
                    propertyValue = GetObjectPropertyValue(type, baseEntity, propertyName);//获取属性值
                    if (propertyValue != null)
                    {
                        if (inputCount != 0)
                        {
                            sql_ += " , ";
                            values += " , ";
                        }
                        sql_ += "[" + propertyName + "]";

                        values += " N'{0}' ";
                        values = string.Format(values, propertyValue);
                        inputCount++;
                    }

                }
                sql_ = sql_ + ") output inserted.{0} values(" + values + ")";

                sql_ = string.Format(sql_, propertyList[0].Name);

                OpenDb();
                SqlCommand cm = new SqlCommand(sql_, conn);
                //ulong returnId = cm.ExecuteNonQuery();
                string returnId = cm.ExecuteScalar().ToString();
                cm.Dispose();
                cm = null;
                CloseDb();
                return returnId;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "  " + sql_);
            }

        }
        #endregion

        #region  //返回对单个Entity的插入的SQL语句
        /// <summary>
        /// 返回对单个Entity的插入的SQL语句（多表操作，表中含有自增字段慎用） 返回“插入记录的自增Id”
        /// </summary>
        /// <param name="sql_"></param>
        public string GetObjectInputSql(object baseEntity)
        {
            string sql_ = "";//values 之前
            try
            {
                Type type = baseEntity.GetType();//得到对象类型
                string tableName = type.Name;
                
                string operation = "insert into ";//执行操作方法
                PropertyInfo[] propertyList = type.GetProperties();//返回PropertyInfo类型，用于取得该类的属性的信息

                //生成SQL语句
                string values = "";//values( 之后 最后一起拼装起来
                string propertyName = "";//属性名
                string propertyValue = "";//属性值

                sql_ = operation + " " + tableName + " ( ";

                int inputCount = 0;//用于计数新增
                for (int i = 0; i < propertyList.Length; i++)//这个从Entity第一个属性开始就解析,主要因为添加时候只要主属性不是自增的就需要填写
                {
                    //propertyList.Add("");
                    propertyName = propertyList[i].Name;
                    propertyValue = GetObjectPropertyValue(type, baseEntity, propertyName);//获取属性值
                    if (propertyValue != null)
                    {
                        if (inputCount != 0)
                        {
                            sql_ += " , ";
                            values += " , ";
                        }
                        sql_ += "[" + propertyName + "]";

                        values += " N'{0}' ";
                        values = string.Format(values, propertyValue);
                        inputCount++;
                    }
                }
                sql_ = sql_ + ") values(" + values + "); select SCOPE_IDENTITY() as Id "; //同时返回插入记录的自增Id

                sql_ = string.Format(sql_, propertyList[0].Name);

                return sql_;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "  " + sql_);
                return "";
            }

        }
        /// <summary>
        /// 返回对单个Entity的插入的SQL语句（多表操作，表中含有自增字段慎用） 返回值是Entity的第一个属性
        /// </summary>
        /// <param name="tableName">表名</param>
        public string GetObjectInputSql(object baseEntity, string tableName)
        {
            string sql_ = "";//values 之前
            try
            {
                Type type = baseEntity.GetType();//得到对象类型
                /*
                IList<CustomAttributeData> tableAttribute = type.GetCustomAttributesData();//该方法是根据Type属性找到的
                string tableName = "";//表名
                tableName = (String)tableAttribute[0].ConstructorArguments[0].Value;//解刨Type属性中偶然发现
                 * */
                string operation = "insert into ";//执行操作方法
                PropertyInfo[] propertyList = type.GetProperties();//返回PropertyInfo类型，用于取得该类的属性的信息

                //生成SQL语句
                string values = "";//values( 之后 最后一起拼装起来
                string propertyName = "";//属性名
                string propertyValue = "";//属性值

                sql_ = operation + " " + tableName + " ( ";

                int inputCount = 0;//用于计数新增
                for (int i = 0; i < propertyList.Length; i++)//这个从Entity第一个属性开始就解析,主要因为添加时候只要主属性不是自增的就需要填写
                {
                    //propertyList.Add("");
                    propertyName = propertyList[i].Name;
                    propertyValue = GetObjectPropertyValue(type, baseEntity, propertyName);//获取属性值
                    if (propertyValue != null)
                    {
                        if (inputCount != 0)
                        {
                            sql_ += " , ";
                            values += " , ";
                        }
                        sql_ += "[" + propertyName + "]";

                        values += " N'{0}' ";
                        values = string.Format(values, propertyValue);
                        inputCount++;
                    }

                }
                sql_ = sql_ + ") output inserted.{0} values(" + values + ")";

                sql_ = string.Format(sql_, propertyList[0].Name);

                return sql_;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "  " + sql_);
                return "";
            }

        }
        #endregion

        #region  //C#利用反射获取对象属性值
        /// <summary>
        /// C#利用反射获取对象属性值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseEntity"></param>
        /// <param name="propertyname">需要更改的值</param>
        /// <returns></returns>
        public static string GetObjectPropertyValue(Type type, object baseEntity, string propertyname)
        {

            PropertyInfo property = type.GetProperty(propertyname);//根据变量名得到变量对象

            //if (property == null) return null;//BaseEntity.cs属性名与BaseModel.cs中属性名不相同时,并不进行编辑

            object o = property.GetValue(baseEntity, null);//从实体中获取具体值 重要

            if (o == null) return null;

            return o.ToString();
        }
        #endregion

        #region  //将sr_readStr数组存入数据库image类型的字段中
        /// <summary>
        /// 将sr_readStr数组存入数据库image类型的字段中
        /// </summary>
        /// <param name="sql_"></param>
        public void ExecuteNonQuery_Byte(string sql_, byte[] sr_readStr)
        {
            try
            {
                OpenDb();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql_;
                SqlParameter par = new SqlParameter("@imgfile", SqlDbType.Image);
                par.Value = sr_readStr;
                cmd.Parameters.Add(par);
                cmd.ExecuteNonQuery();
                CloseDb();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "  " + sql_);
            }
        }
        #endregion

        #region //执行SQL返回DataReader数据集
        public SqlDataReader getDataReader(String sql_)
        {
            if (sql_ == "")
                return null;
            SqlDataReader returnReader = null;
            try
            {
                OpenDb();
                SqlCommand command = new SqlCommand(sql_, conn);
                returnReader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "  " + sql_);
            }
            return returnReader;
        }
        #endregion

        #region 获取DataTable分页数据
        /// <summary>
        /// 获取DataTable分页数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="from">开始的行数</param>
        /// <param name="pagerCount">每页的条数（为0表示获取所有数据）</param>
        /// <returns></returns>
        public DataTable GetDataTablePager(string sql, int from, int pagerCount, params SqlParameter[] cmdParms)
        {
            //pagerCount == 0, 表示获取所有数据
            if (pagerCount == 0)
                return GetDataTable(sql, cmdParms);

            return ExecuteDataTablePager(sql, from, pagerCount - 1, cmdParms);
        }
        /// <summary>
        /// 获取DataTable分页数据(从take开始的skip+1条数据记录)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="take">开始的行数</param>
        /// <param name="skip">向后的条数</param>
        /// <returns></returns>
        public DataTable ExecuteDataTablePager(string sql, int take, int skip, params SqlParameter[] cmdParms)
        {
            DataTable dt = null;
            string pagerSql = GetPagerSql(sql, take, skip);

            dt = GetDataTable(pagerSql, cmdParms);

            return dt;
        }
        /// <summary>
        /// 获取从take至skip+take的skip条数据记录
        /// </summary>
        public static string GetPagerSql(string sql, int take, int skip)
        {
            return string.Format(PagerSqlTemplate(), skip, sql, take + 1);
        }
        /// <summary>
        /// 分页sql语句模板
        /// </summary>
        private static string PagerSqlTemplate()
        {
            return
                "select * from (" +
                "select  row_number()over(order by tempcolumn)Rownumber,* from (" +
                "select top {0} tempcolumn=0 , * from (" +
                "{1})temp1)temp2 " +
                ")temp3 " +
                "where Rownumber >= {2}";
        }
        /// <summary>
        /// 获取DataTable分页数据
        /// </summary>
        /// <param name="sql">sql语句</param>        
        /// <param name="from">开始的行数</param>
        /// <param name="pagerCount">每页的条数（为0表示获取所有数据）</param>
        /// <param name="orderColumnName">排序的列名</param>
        /// <param name="order">升序或降序（asc, desc）</param> 
        /// <returns></returns>
        public DataTable GetDataTablePager(string sql, int from, int pagerCount, string orderColumnName, string order, params SqlParameter[] cmdParms)
        {
            //pagerCount == 0, 表示获取所有数据
            if (pagerCount == 0)
            {
                sql += " order by " + orderColumnName + " " + order;
                return GetDataTable(sql, cmdParms);
            }

            return ExecuteDataTablePager(sql, from, pagerCount - 1, orderColumnName, order, cmdParms);
        }
        /// <summary>
        /// 获取DataTable分页数据(从take开始的skip+1条数据记录)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="take">开始的行数</param>
        /// <param name="skip">向后的条数</param>
        /// <returns></returns>
        public DataTable ExecuteDataTablePager(string sql, int take, int skip, string orderColumnName, string order, params SqlParameter[] cmdParms)
        {
            DataTable dt = null;
            string pagerSql = GetPagerSql(sql, take, skip, orderColumnName, order);

            dt = GetDataTable(pagerSql, cmdParms);

            return dt;
        }
        /// <summary>
        /// 获取从take至skip+take的skip条数据记录
        /// </summary>
        public static string GetPagerSql(string sql, int take, int skip, string orderColumnName, string order)
        {
            //string pagerSql = PagerSqlTemplate_order(sql, take, skip, orderColumnName, order);
            string pagerSql = PagerSqlTemplate_orders(sql, take, skip, orderColumnName, order);
            return pagerSql;
        }
        /// <summary>
        /// 分页sql语句模板(含排序)
        /// </summary>
        /// <param name="orderColumnName">排序的列名</param>
        /// <param name="order">升序或降序（asc, desc）</param>
        /// <returns></returns>
        private static string PagerSqlTemplate_order(string sql, int take, int skip, string orderColumnName, string order)
        {
            string str = "select top {0} * from (" +
                        "select  row_number()over(order by tempcolumn {1})Rownumber,* from (" +
                        "select  tempcolumn={2} , * from (" +
                        "{3})temp1)temp2 " +
                        ")temp3 " +
                        "where Rownumber >= {4}";
            str = string.Format(str, skip, order, orderColumnName, sql, take + 1);
            return str;
        }
        /// <summary>
        /// 分页sql语句模板(含排序)多字段排序
        /// </summary>
        /// <param name="orderColumnNames">排序的列名(多个之间用;间隔)</param>
        /// <param name="orders">升序或降序（asc, desc）(多个之间用;间隔)</param>
        /// <returns></returns>
        private static string PagerSqlTemplate_orders(string sql, int take, int skip, string orderColumnNames, string orders)
        {
            string[] columnNameArr = orderColumnNames.Split(';');
            string[] orderArr = orders.Split(';');
            string str1 = "";
            string str2 = "";
            for (int i = 0; i < columnNameArr.Length; i++)
            {
                if (columnNameArr[i] == "") continue;
                str1 += "tempcolumn" + i + "=" + columnNameArr[i] + ",";
                str2 += "tempcolumn" + i + " " + orderArr[i] + ",";
            }
            if (str1 != "")
                str1 = str1.Substring(0, str1.Length - 1); //去除最后一个,
            if (str2 != "")
                str2 = str2.Substring(0, str2.Length - 1); //去除最后一个,

            string str = "select top {0} * from (" +
                        "select  row_number()over(order by {1})Rownumber,* from (" +
                        "select {2} , * from (" +
                        "{3})temp1)temp2 " +
                        ")temp3 " +
                        "where Rownumber >= {4}";
            str = string.Format(str, skip, str2, str1, sql, take + 1);
            return str;
        }
        #endregion

        #region 执行带参数的SQL语句
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sqlString">SQL语句</param>
        /// <param name="cmdParms"></param>
        /// <returns>影响的记录数</returns>
        public int ExecuteNonQuery(string sqlString, params SqlParameter[] cmdParms)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return rows;
                }
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlList">SQL语句列表[MyDictionary对象]（key为sql语句，value是该语句的SqlParameter[]）</param>
        public void ExecTansaction(List<MyDictionary> sqlList)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var cmd = new SqlCommand();
                    try
                    {
                        //循环
                        foreach (MyDictionary model in sqlList)
                        {
                            string cmdText = model.Key;
                            var cmdParms = model.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public void ExecTansaction(Hashtable sqlStringList)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var cmd = new SqlCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDe in sqlStringList)
                        {
                            string cmdText = myDe.Key.ToString();
                            var cmdParms = (SqlParameter[])myDe.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public void ExecuteSqlTranWithIndentity(Hashtable sqlStringList)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var cmd = new SqlCommand();
                    try
                    {
                        int indentity = 0;
                        //循环
                        foreach (DictionaryEntry myDe in sqlStringList)
                        {
                            string cmdText = myDe.Key.ToString();
                            var cmdParms = (SqlParameter[])myDe.Value;
                            foreach (var q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.InputOutput)
                                {
                                    q.Value = indentity;
                                }
                            }
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            cmd.ExecuteNonQuery();
                            foreach (SqlParameter q in cmdParms)
                            {
                                if (q.Direction == ParameterDirection.Output)
                                {
                                    indentity = Convert.ToInt32(q.Value);
                                }
                            }
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="sqlString">计算查询结果语句</param>
        /// <param name="cmdParms"></param>
        /// <returns>查询结果（object）</returns>
        public object GetSingleVal(string sqlString, params SqlParameter[] cmdParms)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if ((Equals(obj, null)) || (Equals(obj, DBNull.Value)))
                    {
                        return null;
                    }
                    return obj;
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：调用该方法后，一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="cmdParms"></param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader ExecuteReader(string sqlString, params SqlParameter[] cmdParms)
        {
            var connection = new SqlConnection(connectionString);
            var cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, sqlString, cmdParms);
            SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return myReader;

        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="cmdParms"></param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string sqlString, params SqlParameter[] cmdParms)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                using (var da = new SqlDataAdapter(cmd))
                {
                    var ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString">查询语句</param>
        /// <param name="cmdParms"></param>
        /// <returns>DataSet</returns>
        public DataTable GetDataTable(string sqlString, params SqlParameter[] cmdParms)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    var ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        if (ds.Tables.Count <= 0)
                            return null;
                        dt = ds.Tables[0];

                        cmd.Parameters.Clear();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return dt;
                }
            }
        }
        /// <summary>
        /// 获取数据总条数
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public int GetRecordCount(string sqlString, params SqlParameter[] cmdParms)
        {
            DataTable dt = GetDataTable(sqlString, cmdParms);
            if (dt == null || dt.Rows.Count <= 0)
                return 0;

            return dt.Rows.Count;
        }

        private void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, IEnumerable<SqlParameter> cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (var parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
        /// <summary>
        /// 根据SQL判断是否存在对应的数据
        /// </summary>
        public bool YNExistData2(string sqlString, params SqlParameter[] cmdParms)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    var ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                        dt = ds.Tables[0];
                        if (dt != null && dt.Rows.Count > 0)
                            return true;
                        else
                            return false;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        #endregion
    }
    public class MyDictionary
    {
        public string Key { set; get; }
        public SqlParameter[] Value { set; get; }
    }


    public class Firstmodel
    {
        public string sqlFirst { get; set; }
        public List<Secondmodel> Second { get; set; }
    }
    public class Secondmodel
    {
        public string sqlSecond { get; set; }
        public List<string> sqlThird { get; set; }
    }
}
