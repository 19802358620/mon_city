using System.Data.Entity;

namespace EKP.Service.Base.Ef
{
    /// <summary>
    /// 数据库服务管理
    /// </summary>
    internal interface IServiceDb<TDbEntitie> 
        where TDbEntitie : DbContext, new()
    {
    }
}
