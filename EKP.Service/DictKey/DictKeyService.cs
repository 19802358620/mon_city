using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EKP.Entity;
using EKP.Service.Base;
using Ge.Infrastructure.Metronicv;

namespace EKP.Service.DictKey
{
    /// <summary>
    /// 字典键管理接口
    /// </summary>
    public interface IDictKeyService : IEkpEntityService<T_DictKey>
    {

    }

    /// <summary>
    /// 字典键管理
    /// </summary>
    public class DictKeyService : EkpEntityService<T_DictKey>, IDictKeyService
    {

    }
}
