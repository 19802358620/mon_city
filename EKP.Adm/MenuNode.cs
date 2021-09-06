using System.Collections.Generic;
using Ge.Infrastructure.Xml.XmlHelper;

namespace EKP.Adm
{
    /// <summary>
    /// 菜单节点
    /// </summary>
    public class MenuNode : XmlEntity
    {
        /// <summary>
        /// 子菜单集合
        /// </summary>
        public new List<MenuNode> ChildEntitys
        {
            get { return base.ChildEntitys as List<MenuNode>; }
            set { base.ChildEntitys = value; }
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string @class { get; set; }

        /// <summary>
        /// 跳转链接
        /// </summary>
        public string href { get; set; }

        /// <summary>
        /// 最小角色等级
        /// </summary>
        public string minRoleGrade { get; set; }

        /// <summary>
        /// 是否是顶部菜单
        /// </summary>
        public string isTop { get; set; }
    }
}