using EKP.Service.Base.Ado;

namespace EKP.Service.Base
{
    /// <summary>
    /// Ekp数据库某个实体类管理
    /// </summary>
    public interface IEkpEntityService<T> : IServiceBase<T> where T : class, new()
    {

    }

    /// <summary>
    /// Ekp数据库某个实体类管理
    /// </summary>
    public class EkpEntityService<T> : ServiceBase<T>, IEkpEntityService<T> where T : class, new()
    {

    }
}