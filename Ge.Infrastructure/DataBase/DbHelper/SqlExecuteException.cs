using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.DataBase.DbHelper
{
    /// <summary>
    /// 名    称：SqlExecuteException
    /// 作    者：胡政
    /// 创建时间：2015-08-21
    /// 联系方式：13436053642
    /// 描    述：执行sql错误时抛出错误的sql语句
    /// </summary>
    public class SqlExecuteException : Exception
    {
         public SqlExecuteException(string message, Exception inner)
            : base(String.Format("sql语句<span style='color:red'>“{0}”</span>执行错误；错误位置：<span style='color:red'>“{1}”</span>", message, inner.Message), inner)
         {

         }
    }
}
