using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Utilities
{
    /// <summary>
    /// 名    称：描述枚举的属性
    /// 作    者：付正勇
    /// 创建时间：2014/7/18 13:38:22
    /// 联系方式：电话[15086925108],邮件[fxy6781349@qq.com]
    /// 描    述：
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnumAttribute : Attribute
    {
        private string _name;
        private string _description;

        /// <summary>
        /// 枚举名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 枚举描述
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">枚举名称</param>
        public EnumAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">枚举名称</param>
        /// <param name="description">枚举描述</param>
        public EnumAttribute(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}
