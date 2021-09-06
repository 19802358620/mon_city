using Ge.Infrastructure.Metronicv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Base.Cache
{
    /// <summary>
    /// 缓存分页参数
    /// </summary>
    public class CachePagerParam : JqgridParam
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
    }

    /// <summary>
    /// 缓存分页模型
    /// </summary>
    public class CachePagerModel
    {
        /// <summary>
        /// 缓存键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 缓存值
        /// </summary>
        public string Value { get; set; }
    }
}
