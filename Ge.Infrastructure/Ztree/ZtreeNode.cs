using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Ztree
{
    /// <summary>
    /// 名    称：ZtreeNode
    /// 作    者：胡政
    /// 创建时间：2015-08-29
    /// 联系方式：13436053642
    /// 描    述：Ztree插件节点dto模型
    /// </summary>
    public class ZtreeNode<T>
    {
        public T id { get; set; }

        public T parentId { get; set; }

        public string name { get; set; }

        public bool? open { get; set; }

        public bool? isParent { get; set; }

        public List<ZtreeNode<T>> children { get; set; }
    }
}
