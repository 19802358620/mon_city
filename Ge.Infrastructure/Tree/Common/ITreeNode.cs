using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Tree.Common{
    /// <summary>
    /// 树形节点接口
    /// </summary>
    public interface ITreeNode<T>
    {
        /// <summary>
        /// 获取或设置节点Id（唯一）
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// 获取或设置节点深度
        /// </summary>
        int Grade { get; set; }
        /// <summary>
        /// 获取或设置节点名（唯一）
        /// </summary>
        string EntiyName { get; set; }
        /// <summary>
        /// 获取或设置节点显示名
        /// </summary>
        string DisplayName { get; set; }
        /// <summary>
        /// 获取或设置节点顺序号（唯一）
        /// </summary>
        string OrderRecord { get; }
        /// <summary>
        /// 获取或设置子节点集合
        /// </summary>
        List<T> ChildsNodes { get; }
    }
}
