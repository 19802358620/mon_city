using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Pager
{
    /// <summary>
    /// 名    称：PagerParameter
    /// 作    者：胡政
    /// 创建时间：2015-08-22
    /// 联系方式：13436053642
    /// 描    述：分页所需参数
    /// </summary>
    public class PagerParameter
    {
        /// <summary>
        /// 条目起始位置
        /// </summary>
        public virtual int Skip { get; set; }

        /// <summary>
        /// 条目数
        /// </summary>
        public virtual int Take { get; set; }
    }
}
