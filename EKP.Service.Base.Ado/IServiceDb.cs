using System.Data.Entity;

namespace EKP.Service.Base.Ado
{
    /// <summary>
    /// 数据库服务管理
    /// </summary>
    public interface IServiceDb<TDbEntitie>
        where TDbEntitie : DbContext, new()
    {
    }
}
