using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Metronicv
{
    /// <summary>
    /// 名    称：JsTreeNode
    /// 作    者：胡政
    /// 创建时间：2015-08-29
    /// 联系方式：13436053642
    /// 描    述：JsTree插件节点dto模型
    /// </summary>
    public class JsTreeNode
    {
        public string id { get; set; }

        public string text { get; set; }

        public List<JsTreeNode> children { get; set; }

        public object info { get; set; }
    }
}
