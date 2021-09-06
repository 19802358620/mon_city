using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using EKP.Service.User;
using Ge.Infrastructure.Ioc;
using Ge.Infrastructure.Utilities;
using Ge.Infrastructure.Xml.XmlHelper;
using EKP.Base.Identity;
using EKP.Service.Site;
using EKP.Service.DictValue;
using EKP.Service.Role;
using EKP.Service.Authority;
using Ge.Infrastructure.Extensions;

namespace EKP.Adm
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public interface IMenuManager
    {
        /// <summary>
        /// 获取用户后台菜单
        /// </summary>
        /// <returns></returns>
        List<MenuNode> GetLoginMenus(int? siteId);

        /// <summary>
        /// 获取菜单集合
        /// </summary>
        List<MenuNode> GetMenus(string xpath);

        /// <summary>
        /// 获取菜单集合，并将子菜单集合扁平化
        /// </summary>
        List<MenuNode> GetSomeMenus(string xpath);

        /// <summary>
        /// 将菜单集合扁平化
        /// </summary>
        List<MenuNode> GetSomeMenus(List<MenuNode> dataSourse);

        /// <summary>
        /// 从菜单树过滤掉不包含指定菜单名集合的菜单，并返回过滤后的菜单树
        /// </summary>
        List<MenuNode> FilterMenus(List<string> menuNames, List<MenuNode> sourse);

        /// <summary>
        /// 从菜单树筛选符合条件的树形节点，并返回过滤后的菜单树
        /// </summary>
        List<MenuNode> FilterMenus(List<MenuNode> sourse, Func<MenuNode, bool> func);

        /// <summary>
        /// 检查菜单集合是否存在某个指定菜单名的菜单
        /// </summary>
        bool IsInMenu(string checkName, List<MenuNode> menus);

        /// <summary>
        /// 检查菜单集合是否存在某个菜单名集合表示的菜单集合
        /// </summary>
        bool IsInMenus(List<string> checkNames, List<MenuNode> menus);
    }

    /// <summary>
    /// 菜单管理
    /// </summary>
    public class MenuManager : XmlHelper, IMenuManager
    {
        private readonly IUserService userService = Ioc.GetService<IUserService>();
        private readonly ISiteService siteService = Ioc.GetService<ISiteService>();
        private readonly IDictValueService dictValueService = Ioc.GetService<IDictValueService>();
        private readonly IRoleService roleService = Ioc.GetService<IRoleService>();
        private readonly IAuthorityService authorityService = Ioc.GetService<IAuthorityService>();

        /// <summary>
        /// 菜单Xml文件地址
        /// </summary>
        private static string MenuFileName
        {
            get
            {
                var serverPath = HttpContext.Current.Server.MapPath("/");
                var directory = new FileInfo(serverPath).Directory;
                if (directory != null && directory.Exists)
                {
                    return directory + @"\Areas\Adm\App_Data\Authority.xml";
                }

                throw new Exception("读取菜单配置文件路径出错");
            }
        }

        public MenuManager() : base(MenuFileName) { }

        /// <summary>
        /// 获取用户后台菜单
        /// </summary>
        /// <returns></returns>
        public List<MenuNode> GetLoginMenus(int? siteId)
        {
            var authorityNodes = new List<MenuNode>();
            var loginUser = ApplicationSignInManager.GetLoginUser();
            var site = siteService.GetEntiy(Convert.ToInt32(siteId));
            
            //获取当前角色
            var role = roleService.GetEntiy(Convert.ToInt32(loginUser.LoginRoleId));

            //当前平台的所有菜单
            authorityNodes = this.GetMenus(string.Format("//{0}", site.Type));
            
            //等级过滤
            authorityNodes.DeepRemoveAll("ChildEntitys", m => !string.IsNullOrEmpty(m.minRoleGrade)
                && EnumHelper.ToEnum<RoleGrade>(role.Grade) > EnumHelper.ToEnum<RoleGrade>(m.minRoleGrade));

            //用户权限过滤
            if (EnumHelper.ToEnum<RoleGrade>(role.Grade) > RoleGrade.Administrator)
            {
                var authorityNames = authorityService.GetList(string.Format("RoleId='{0}'", role.Id))
                    .Select(ra => ra.Name).ToList();
                authorityNodes = this.FilterMenus(authorityNames, authorityNodes);
            }

            return authorityNodes;
        }

        /// <summary>
        /// 获取菜单集合
        /// </summary>
        public List<MenuNode> GetMenus(string xpath)
        {
            var menus = this.GetXmlEntityListByXpath<MenuNode>(xpath);

            return menus;
        }

        /// <summary>
        /// 将菜单集合扁平化
        /// </summary>
        public List<MenuNode> GetSomeMenus(string xpath)
        {
            var authoritys = GetMenus(xpath).First().ChildEntitys;
            var result = new List<MenuNode>();
            authoritys.ForEach(a => result.AddRange(GetSomeMenus(a.ChildEntitys)));
            return result;
        }

        /// <summary>
        /// 将菜单集合扁平化
        /// </summary>
        public List<MenuNode> GetSomeMenus(List<MenuNode> dataSourse)
        {
            var list = new List<MenuNode>();

            dataSourse.ForEach(t =>
            {
                list.Add(new MenuNode
                {
                    name = t.name,
                    title = t.title,
                    @class = t.@class,
                    href = t.href,
                    ChildEntitys = null
                });
                if (t.ChildEntitys != null)
                {
                    list.AddRange(GetSomeMenus(t.ChildEntitys));
                }
            });

            return list;
        }

        /// <summary>
        /// 从菜单树过滤掉不包含指定菜单名集合的菜单，并返回过滤后的菜单树
        /// </summary>
        public List<MenuNode> FilterMenus(List<string> menuNames, List<MenuNode> sourse)
        {
            var result = new List<MenuNode>();
            sourse.ForEach(s =>
            {
                if (menuNames.Contains(s.name) || (s.ChildEntitys != null && IsInMenus(menuNames, s.ChildEntitys)))
                    result.Add(s);
                if (s.ChildEntitys != null)
                {
                    var childEntitys = s.ChildEntitys;
                    var newChildEntitys = FilterMenus(menuNames, childEntitys);
                    s.ChildEntitys = newChildEntitys;
                }
            });
            return result;
        }

        /// <summary>
        /// 从菜单树筛选符合条件的树形节点，并返回过滤后的菜单树
        /// </summary>
        public List<MenuNode> FilterMenus(List<MenuNode> sourse, Func<MenuNode, bool> func)
        {
            var result = new List<MenuNode>();
            sourse.ForEach(s =>
            {
                if (func(s))
                {
                    result.Add(s);
                }
                if (s.ChildEntitys != null)
                {
                    var childEntitys = s.ChildEntitys;
                    var newChildEntitys = FilterMenus(childEntitys, func);
                    s.ChildEntitys = newChildEntitys;
                }
            });
            return result;
        }

        /// <summary>
        /// 检查菜单集合是否存在某个指定菜单名的菜单
        /// </summary>
        public bool IsInMenu(string checkName, List<MenuNode> menus)
        {
            if (menus == null || menus.Count == 0) return false;

            if (menus.Count == 1 &&
                (menus[0].ChildEntitys == null || menus[0].ChildEntitys.Count == 0))
            {
                return menus[0].name == checkName;
            }

            foreach (var authority in menus)
            {
                if (authority.name == checkName || IsInMenu(checkName, authority.ChildEntitys))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 检查菜单集合是否存在某个菜单名集合表示的菜单集合
        /// </summary>
        public bool IsInMenus(List<string> checkNames, List<MenuNode> menus)
        {
            foreach (var checkName in checkNames)
            {
                if (IsInMenu(checkName, menus))
                    return true;
            }
            return false;
        }
    }
}