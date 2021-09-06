using System.Collections.Generic;
using Ge.Infrastructure.Filter;
using Ge.Infrastructure.Tree.Common;

namespace Ge.Infrastructure.Tree.Menu
{
    /// <summary>
    /// 菜单树中的一个节点
    /// </summary>
    public class MenuTreeNode : ITreeNode<MenuTreeNode>
    {
        /// <summary>
        /// 获取或设置菜单ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 获取或设置菜单是否展开
        /// </summary>
        public bool Expanded { get; set; }
        /// <summary>
        /// 获取或设置菜单的等级
        /// </summary>
        public int Grade { get; set; }
        /// <summary>
        /// 获取或设置菜单名字
        /// </summary>
        public string EntiyName { get; set; }
        /// <summary>
        /// 获取或设置菜单显示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 获取或设置跳转路径
        /// </summary>
        public string Src{ get; set; }
        /// <summary>
        /// 获取或设置菜单顺序编号
        /// </summary>
        public string OrderRecord{ get; set; }
        /// <summary>
        /// 获取或设置获取所有子菜单节点
        /// </summary>
        public List<MenuTreeNode> ChildsNodes { get; set; }
        /// <summary>
        /// 获取或设置菜单附加的权限
        /// </summary>
        public List<FilterAction> FiltersList { get; set; }
    }
}
