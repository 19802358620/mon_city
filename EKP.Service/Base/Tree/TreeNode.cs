using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKP.Service.Base.Tree
{
    /// <summary>
    /// 节点对象
    /// </summary>
    public class TreeNode
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public List<TreeNode> Children { get; set; }
    }

    /// <summary>
    /// 泛型节点对象
    /// </summary>
    public class TreeNode<T> where T: TreeNode<T>
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public new List<T> Children { get; set; }
    }
}
