using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Filter
{
    /// <summary>
    /// 权限接口
    /// </summary>
    public interface IFilter
    {

    }
    /// <summary>
    /// 抽象权限
    /// </summary>
    public abstract class AbstractFilter : IFilter
    {
        /// <summary>
        /// 获取权限名字（唯一）
        /// </summary>
        public static string EntiyName { get { return typeof(AbstractFilter).Name; } }
    }
}
