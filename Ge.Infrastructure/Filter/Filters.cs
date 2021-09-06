using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Filter
{
    /// <summary>
    /// 院系
    /// </summary>
    public class CollegeFilter : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(CollegeFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction Create = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Create", DisplayName = "添加" };
        public static FilterAction Update = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Edit", DisplayName = "编辑" };
        public static FilterAction Delete = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Delete", DisplayName = "删除" };
    }
    /// <summary>
    /// 专业
    /// </summary>
    public class MajorFilter : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(MajorFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction Create = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Create", DisplayName = "添加" };
        public static FilterAction Update = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Edit", DisplayName = "编辑" };
        public static FilterAction Delete = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Delete", DisplayName = "删除" };
    }
    /// <summary>
    /// 修改密码
    /// </summary>
    public class PasswordModify : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(PasswordModify).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "修改" };
    }
    /// <summary>
    /// 个人设置
    /// </summary>
    public class PersonalConfig : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(PersonalConfig).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction WordConfig = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "WordConfig", DisplayName = "自定义文字" };
        public static FilterAction ThemeConfig = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "ThemeConfig", DisplayName = "自定义主题" };
    }
    /// <summary>
    /// 注册人员
    /// </summary>
    public class UserFilter : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(UserFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction Update = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Edit", DisplayName = "编辑" };
        public static FilterAction ExportExcel = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "ExportExcel", DisplayName = "导出Excel" };
    }
    /// <summary>
    /// 学生
    /// </summary>
    public class StudentFilter : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(StudentFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction Create = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Create", DisplayName = "添加" };
        public static FilterAction Update = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Edit", DisplayName = "编辑" };
        public static FilterAction Delete = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Delete", DisplayName = "删除" };
        public static FilterAction ExportExcel = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "ExportExcel", DisplayName = "导出Excel" };
    }
    /// <summary>
    /// 教师
    /// </summary>
    public class TeacherFilter : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(TeacherFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction Create = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Create", DisplayName = "添加" };
        public static FilterAction Update = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Edit", DisplayName = "编辑" };
        public static FilterAction Delete = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Delete", DisplayName = "删除" };
        public static FilterAction ExportExcel = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "ExportExcel", DisplayName = "导出Excel" };
    }
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleFilter : AbstractFilter
    {
        public new static string EntiyName
        {
            get { return typeof(RoleFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction Create = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Create", DisplayName = "添加" };
        public static FilterAction Update = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Edit", DisplayName = "编辑" };
        public static FilterAction Delete = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Delete", DisplayName = "删除" };
        public static FilterAction RoleFilterConfig = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "RoleFilterConfig", DisplayName = "权限配置" };
    }
    /// <summary>
    /// 绑定用户
    /// </summary>
    public class RoleUserFilter : AbstractFilter
    {
        public new static string EntiyName
        {
            get { return typeof(RoleUserFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction Bind = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Bind", DisplayName = "绑定" };
        public static FilterAction RecallBind = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "RecallBind", DisplayName = "罢免" };
    }
    /// <summary>
    /// 培养方案
    /// </summary>
    public class MajorTrainPlanFilter : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(MajorTrainPlanFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction Create = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Create", DisplayName = "添加" };
        public static FilterAction Update = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Edit", DisplayName = "编辑" };
        public static FilterAction Delete = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Delete", DisplayName = "删除" };
        public static FilterAction MajorTrainPlanCourses = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "MajorTrainPlanCourses", DisplayName = "管理实验课程" };
    }
    /// <summary>
    /// 实验课程
    /// </summary>
    public class CourseFilter : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(CourseFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction Create = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Create", DisplayName = "添加" };
        public static FilterAction Update = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Edit", DisplayName = "编辑" };
        public static FilterAction Delete = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Delete", DisplayName = "删除" };
        public static FilterAction ExperimentCourses = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "ExperimentCourses", DisplayName = "管理实验项目" };
    }
    /// <summary>
    /// 教室
    /// </summary>
    public class ClassRoomFilter : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(ClassRoomFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction Create = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Create", DisplayName = "添加" };
        public static FilterAction Update = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Edit", DisplayName = "编辑" };
        public static FilterAction Delete = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Delete", DisplayName = "删除" };
    }
    /// <summary>
    /// 开课
    /// </summary>
    public class OpenCourseFilter : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(OpenCourseFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction OpenCourseManage = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "OpenCourseManage", DisplayName = "开课管理" };
    }
    /// <summary>
    /// 设备登记
    /// </summary>
    public class DevicesFilter : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(DevicesFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction Create = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Create", DisplayName = "添加" };
        public static FilterAction Update = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Edit", DisplayName = "编辑" };
        public static FilterAction Delete = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Delete", DisplayName = "删除" };

        public static FilterAction Using = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Using", DisplayName = "使用" };
        public static FilterAction Repair = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Repair", DisplayName = "维修" };
        public static FilterAction Scrap = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Scrap", DisplayName = "报废" };
        public static FilterAction ExportExcel = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "ExportExcel", DisplayName = "导出Excel" };
    }
    /// <summary>
    /// 选课系统
    /// </summary>
    public class BookExperimentFilter : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(BookExperimentFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "选课" };
    }
    /// <summary>
    /// 审核选课
    /// </summary>
    public class ExamineOpenCourseFilter : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(ExamineOpenCourseFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction Examine = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Examine", DisplayName = "审核" };
        public static FilterAction CancelExamine = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "CancelExamine", DisplayName = "取消审核" };
    }
    /// <summary>
    /// 课程管理
    /// </summary>
    public class OwnCourseFilter : AbstractFilter
    {
        /// 获取用户权限显示名
        public new static string EntiyName
        {
            get { return typeof(OwnCourseFilter).Name; }
        }

        //权限拥有的操作
        public static FilterAction View = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "查看" };
        public static FilterAction MyCocurseManager = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "View", DisplayName = "我的课程" };
        public static FilterAction BusyWorkManager = new FilterAction { FilterEntiyName = EntiyName, EntiyName = "Examine", DisplayName = "课堂作业" };
    }
}