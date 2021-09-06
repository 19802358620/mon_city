using EKP.Entity;
using EKP.Service.Base.Ado;

namespace EKP.Service.Base
{
    /// <summary>
    /// Ekp数据库管理
    /// </summary>
    internal interface IEkpDbService : IServiceDb<EKP_JSEntities>
    {

    }

    /// <summary>
    /// Ekp数据库管理
    /// </summary>
    public class EkpDbService : ServiceDb<EKP_JSEntities>, IEkpDbService
    {

    }
}
