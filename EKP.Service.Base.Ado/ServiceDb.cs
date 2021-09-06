using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using EKP.Repository.Ado;
using Ge.Infrastructure.DataBase.DbHelper;
using Ge.Infrastructure.Pager;

namespace EKP.Service.Base.Ado
{
    /// <summary>
    /// 数据库服务管理
    /// </summary>
    public class ServiceDb<TDbEntitie> : IServiceDb<TDbEntitie>
        where TDbEntitie : DbContext, new()
    {
        /// <summary>
        /// 创建事务管理器
        /// </summary>
        /// <returns></returns>
        public static EntityTrans CreateEntityTrans()
        {
            var trans = new Trans(EKP.Service.Base.Ef.ServiceDb<TDbEntitie>.CreateContext().Database.Connection.ConnectionString);
            return new EntityTrans(trans);
        }

        /// <summary>
        /// 执行sql返回PagerResult分页结果
        /// </summary>
        public static PagerResult<T> GetPager<T>(string sql, PagerParameter param) where T : class, new()
        {
            return EKP.Service.Base.Ef.ServiceDb<TDbEntitie>.GetPager<T>(sql, param);
        }

        /// <summary>
        /// 执行sql返回List
        /// </summary>
        public static List<T> GetList<T>(string sql)
        {
            return EKP.Service.Base.Ef.ServiceDb<TDbEntitie>.GetList<T>(sql);
        }

        /// <summary>
        /// 获取条目数
        /// </summary>
        public static int GetCount(string sql)
        {
            return EKP.Service.Base.Ef.ServiceDb<TDbEntitie>.GetCount(sql);
        }

        /// <summary>
        /// 获取数据库第一列第一行
        /// </summary>
        public static T GetScalar<T>(string sql)
        {
            return EKP.Service.Base.Ef.ServiceDb<TDbEntitie>.GetScalar<T>(sql);
        }

        /// <summary>
        /// 根据sql返回DataTable数据集
        /// </summary>
        public static DataTable GetDt(string sql)
        {
            return EKP.Service.Base.Ef.ServiceDb<TDbEntitie>.GetDt(sql);
        }

        /// <summary>
        /// 根据sql返回DbDataReader数据集
        /// </summary>
        public static DbDataReader GetDr(string sql)
        {
            return EKP.Service.Base.Ef.ServiceDb<TDbEntitie>.GetDr(sql);
        }
    }
}
