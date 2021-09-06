using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ge.Infrastructure.Tree.Menu
{
    public class MenuTreeManager
    {
        private MenuTreeManager() { }

        /// <summary>
        /// 获取菜单原始数据源
        /// </summary>
        public static List<MenuTreeNode> DataSourse { get { return MenuTreeDataSourse.GetSourse(); } }
        /// <summary>
        /// 获取菜单集合
        /// </summary>
        public static List<MenuTreeNode> MenuList()
        {
            var dataSourse = MenuTreeDataSourse.GetSourse();
            List<MenuTreeNode> menuList = null;//
            if (dataSourse == null || dataSourse.Count == 0)
                return menuList;

            menuList = new List<MenuTreeNode>();
            for (int i = 0; i < dataSourse.Count; i++)
            {
                if (i == 0)
                {
                    dataSourse[i].OrderRecord = "001";
                }  
                else
                {
                    int gradeDiff = dataSourse[i].Grade - dataSourse[i - 1].Grade;
                    if (gradeDiff < 0)
                    {
                        dataSourse[i].OrderRecord = (Convert.ToInt32(dataSourse[i - 1].OrderRecord.Substring(0, dataSourse[i - 1].OrderRecord.Length - 3)) + 1).ToString("000");
                           
                    }
                    else if (gradeDiff == 0)
                    {
                        dataSourse[i].OrderRecord = dataSourse[i - 1].OrderRecord.Substring(0, dataSourse[i - 1].OrderRecord.Length - 3) +
                           (Convert.ToInt32(dataSourse[i - 1].OrderRecord.Substring(dataSourse[i - 1].OrderRecord.Length - 3, 3)) + 1).ToString("000");
                    }
                    else
                    {
                        dataSourse[i].OrderRecord = dataSourse[i - 1].OrderRecord + "001";
                    }
                }

                dataSourse[i].Id = i + 1;
            }

            dataSourse.ForEach(menu =>
            {
                if (menu.OrderRecord.Length == 3)
                {
                    menuList.Add(menu);
                    AddSonMenus(menu, dataSourse);
                }  
            });

            return menuList;
        }
        /// <summary>
        /// 根据datasourse为某个菜单项添加子菜单
        /// </summary>
        private static void AddSonMenus(MenuTreeNode menuNode, List<MenuTreeNode> dataSourse)
        {
            int menuOrderRecordLength = menuNode.OrderRecord.Length;
            for (int i = 0; i < dataSourse.Count; i++)
            {
                if (dataSourse[i].OrderRecord.Length == menuOrderRecordLength + 3
                    && dataSourse[i].OrderRecord.StartsWith(menuNode.OrderRecord))
                {
                    if (menuNode.ChildsNodes == null)
                        menuNode.ChildsNodes = new List<MenuTreeNode>();
                    menuNode.ChildsNodes.Add(dataSourse[i]);
                }
            }
            if (menuNode.ChildsNodes != null)
            {
                for (int i = 0; i < menuNode.ChildsNodes.Count; i++)
                {
                    AddSonMenus(menuNode.ChildsNodes[i], dataSourse);
                }
            }
        }
    }
}
