using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ge.Infrastructure.Tree.Menu;

namespace Ge.Infrastructure.Filter
{
    public class FilterManager
    {

        /// <summary>
        /// 获取所有权限集合
        /// </summary>
        public static List<IFilter> GetFilterList()
        {
            var filterList = new List<IFilter>();
            var abstractFilterType = typeof (AbstractFilter);

            foreach (var filterType in abstractFilterType.Assembly.GetTypes())
            {
                if (filterType.BaseType != null && filterType.BaseType.Name == abstractFilterType.Name)
                {
                    var filter = Activator.CreateInstance(filterType);
                    filterList.Add(filter as IFilter);
                }
            }

            return filterList;
        }
        /// <summary>
        /// 获取权限树
        /// </summary>
        public static List<FilterTreeNode> GetFilterTree()
        {
            var menuList = MenuTreeManager.MenuList();//获取所有菜单
            var filterTree = new List<FilterTreeNode>();//权限树

            menuList.ForEach(menu =>
                filterTree.Add(new FilterTreeNode()
                {
                    Mark = menu.ChildsNodes == null ? "filter" : "menu",
                    Expanded = false,
                    Grade = menu.Grade,
                    EntiyName = menu.ChildsNodes == null ? menu.OrderRecord : null,
                    ActionEntiyName = menu.ChildsNodes == null ? menu.OrderRecord : null,
                    DisplayName = menu.DisplayName,
                    OrderRecord = menu.OrderRecord,
                    ChildsNodes = GetSonFiltersDto(menu),
                })
            );

            return filterTree;
        }
        /// <summary>
        /// 递归获取某个权限下面的子权限
        /// </summary>
        private static List<FilterTreeNode> GetSonFiltersDto(MenuTreeNode menu)
        {
            var sonFiltersDtoList = new List<FilterTreeNode>();

            //无子节点返回空
            if (menu.ChildsNodes == null)
                return null;

            //为叶节点菜单添加权限
            if (menu.ChildsNodes != null)
            {
                menu.ChildsNodes.ForEach(menu2 =>
                    sonFiltersDtoList.Add(new FilterTreeNode()
                    {
                        Mark = menu2.ChildsNodes == null ? "filter" : "menu",
                        Expanded = false,
                        Grade = menu2.Grade,
                        EntiyName = menu2.ChildsNodes == null ? menu2.OrderRecord : null,
                        ActionEntiyName = menu2.ChildsNodes == null ? menu2.OrderRecord : null,
                        DisplayName = menu2.DisplayName,
                        OrderRecord = menu2.OrderRecord,
                        ChildsNodes = GetSonFiltersDto(menu2)
                    }));
            }


            return sonFiltersDtoList;
        }
    }
}
