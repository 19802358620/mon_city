using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ge.Infrastructure.Filter;

namespace Ge.Infrastructure.Tree.Menu
{
    public class MenuTreeDataSourse
    {
        public static List<MenuTreeNode> GetSourse()
        {
            var menuList = new List<MenuTreeNode>();

            #region 系统配置

            menuList.Add(new MenuTreeNode()
            {
                Expanded = true,
                Grade = 1,
                DisplayName = "系统配置",
                Src = "#"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "院系",
                Src = "BusinessDectionaryWords/List?businessDectionaryID=005"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "专业",
                Src = "Major/List"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "修改密码",
                Src = "Account/ChangePassWard"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "网站主题",
                Src = "AccountSetting/Index"
            });

            #endregion

            #region 人员管理

            menuList.Add(new MenuTreeNode()
            {
                Grade = 1,
                DisplayName = "人员管理",
                Src = "#"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "学生信息管理",
                Src = "Student/StudentList"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "教师信息管理",
                Src = "Teacher/TeacherList"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "基本信息管理",
                Src = "Account/AccountList"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "教师职称",
                Src = "BusinessDectionaryWords/List?businessDectionaryID=008"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "学位",
                Src = "BusinessDectionaryWords/List?businessDectionaryID=004"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "班级职务",
                Src = "BusinessDectionaryWords/List?businessDectionaryID=002"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "政治面貌",
                Src = "BusinessDectionaryWords/List?businessDectionaryID=003"
            });

            #endregion

            #region 角色权限

            menuList.Add(new MenuTreeNode()
            {
                Grade = 1,
                DisplayName = "角色权限",
                Src = "#"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "角色",
                Src = "Role/RoleList"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "绑定用户",
                Src = "Role/AccountRole"
            });

            #endregion

            #region 专业培养方案

            menuList.Add(new MenuTreeNode()
            {
                Grade = 1,
                DisplayName = "专业培养方案",
                Src = "#",
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "培养方案",
                Src = "MajorTrainPlan/List"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "实验课程",
                Src = "Course/List"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "实验类型",
                Src = "BusinessDectionaryWords/List?businessDectionaryID=011"
            });

            #endregion

            #region 实验开放控制

            menuList.Add(new MenuTreeNode()
            {
                Grade = 1,
                DisplayName = "实验开放控制",
                Src = "#",
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "开设课程",
                Src = "OpenCourse/List"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "实验教室",
                Src = "ClassRoom/List"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "教室类型",
                Src = "BusinessDectionaryWords/List?businessDectionaryID=012"
            });

            #endregion

            #region 实验环节管理

            menuList.Add(new MenuTreeNode()
            {
                Grade = 1,
                DisplayName = "实验环节管理",
                Src = "#",
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "实验选课",
                Src = "BookExperiment/List"
            });
            menuList.Add(new MenuTreeNode()
            {
                Grade = 2,
                DisplayName = "课程查询",
                Src = "AccountCourse/Index"
            });

            #endregion

            return menuList;
        }
    }
}
