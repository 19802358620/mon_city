using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ge.Infrastructure.Utilities
{
    /// <summary>
    /// 枚举类型帮助类
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 获取枚举值的描述（注解）。
        /// </summary>
        /// <returns></returns>
        public static string GetEnumDescription(Enum enumValue)
        {
            var str = enumValue.ToString();
            var field = enumValue.GetType().GetField(str);
            var objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;

            var da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }

        /// <summary>
        /// 获取枚举值的本地化名称。
        /// </summary>
        /// <returns></returns>
        public static string GetEnumDisplay(Enum enumValue)
        {
            var str = enumValue.ToString();
            var field = enumValue.GetType().GetField(str);
            var objs = field.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (objs == null || objs.Length == 0) return str;

            var da = (DisplayAttribute)objs[0];
            return da.Name;
        }

        /// <summary>
        /// 检查字符串是否匹配某个枚举值
        /// </summary>
        /// <param name="p">枚举类型</param>
        /// <param name="enumStr">枚举值字符串</param>
        /// <returns>如果找到匹配值，否者返回null</returns>
        public static object IsEnum(Type p, string enumStr)
        {
            foreach (var myCode in Enum.GetValues(p))
            {
                var strName = Enum.GetName(p, myCode);//获取名称
                if (!string.IsNullOrEmpty(strName) && strName == enumStr) return Enum.Parse(p, strName);
            }

            return null;
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T ToEnum<T>(string str)
        {
            T e = default(T);

            e = (T)Enum.Parse(typeof(T), str);

            return e;
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <param name="p"></param>
        /// <param name="enumStr"></param>
        /// <returns></returns>
        public static object GetEnum(Type p, string enumStr)
        {
            foreach (var myCode in Enum.GetValues(p))
            {
                var strName = Enum.GetName(p, myCode);//获取名称
                if (!string.IsNullOrEmpty(strName) && strName == enumStr) return Enum.Parse(p, strName);
            }

            return null;
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <param name="p"></param>
        /// <param name="enumStr"></param>
        /// <returns></returns>
        public static T GetEnum<T>(string enumStr)
        {
            var e = GetEnum(typeof(T), enumStr);
            if (e != null)
                return (T)GetEnum(typeof(T), enumStr);
            return default(T);
        }

        /// <summary>
        /// 枚举转Int类型
        /// </summary>
        public static int ToInt<T>(string str)
        {
            T e = ToEnum<T>(str);

            return Convert.ToInt32(e);
        }

        /// <summary>
        /// 获取枚举为List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">默认以value位值，name为true时，以字段名为值</param>
        /// <returns></returns>
        public static List<SelectListItem> GetEnumSelectList<T>(bool name = false, string defaultValue = "")
        {
            List<SelectListItem> items = new List<SelectListItem>();
            Type type = typeof(T);
            foreach (var field in type.GetFields())
            {
                if (field.IsDefined(typeof(DisplayAttribute), true))
                {
                    DisplayAttribute attribute = field.GetCustomAttribute(typeof(DisplayAttribute), true) as DisplayAttribute;
                    SelectListItem item = new SelectListItem();
                    item.Text = attribute.Name;
                    if (name)
                    {
                        item.Value = field.Name;
                    }
                    else
                    {
                        int value = (int)Enum.Parse(type, field.Name);
                        item.Value = value.ToString();
                    }

                    if (field.IsDefined(typeof(DefaultValueAttribute), true) && defaultValue == "")
                    {
                        DefaultValueAttribute defaultValueAttribute = field.GetCustomAttribute(typeof(DefaultValueAttribute), true) as DefaultValueAttribute;
                        if (Convert.ToBoolean(defaultValueAttribute.Value) && items.Count(m => m.Selected) == 0)
                        {
                            item.Selected = true;
                        }
                    }
                    else if (defaultValue == item.Value)
                    {
                        item.Selected = true;
                    }
                    items.Add(item);
                }
            }
            return items;
        }

        /// <summary>
        /// 获取枚举为JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetEnumJson<T>(bool name = false, string defaultLabel = "", string defaultValue = "")
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if (!string.IsNullOrWhiteSpace(defaultLabel))
            {
                items.Add(new SelectListItem
                {
                    Text = defaultLabel,
                    Value = defaultValue
                });
            }
            Type type = typeof(T);
            foreach (var field in type.GetFields())
            {
                if (field.IsDefined(typeof(DisplayAttribute), true))
                {
                    DisplayAttribute attribute = field.GetCustomAttribute(typeof(DisplayAttribute), true) as DisplayAttribute;
                    SelectListItem item = new SelectListItem();
                    item.Text = attribute.Name;
                    if (name)
                    {
                        item.Value = field.Name;
                    }
                    else
                    {
                        int value = (int)Enum.Parse(type, field.Name);
                        item.Value = value.ToString();
                    }

                    if (field.IsDefined(typeof(DefaultValueAttribute), true))
                    {
                        DefaultValueAttribute defaultValueAttribute = field.GetCustomAttribute(typeof(DefaultValueAttribute), true) as DefaultValueAttribute;
                        if (Convert.ToBoolean(defaultValueAttribute.Value) && items.Count(m => m.Selected) == 0)
                        {
                            item.Selected = true;
                        }
                    }
                    items.Add(item);
                }
            }
            return JsonConvert.SerializeObject(items);
        }

        /// <summary>
        /// 获取枚举值的描述信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetDisplayName<T>(object value, string defaultValue = "", bool name = false)
        {
            List<SelectListItem> items = GetEnumSelectList<T>(name);
            foreach (var item in items)
            {
                if (item.Value == Convert.ToString(value))
                    return item.Text;
            }
            return defaultValue;
        }

        /// <summary>  
        /// 获取枚举的描述  
        /// </summary>  
        /// <param name="en">枚举</param>  
        /// <returns>返回枚举的描述</returns>  
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();   //获取类型  
            MemberInfo[] memberInfos = type.GetMember(en.ToString());   //获取成员  
            if (memberInfos != null && memberInfos.Length > 0)
            {
                DescriptionAttribute[] attrs = memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];   //获取描述特性  

                if (attrs != null && attrs.Length > 0)
                {
                    return attrs[0].Description;    //返回当前描述  
                }
            }
            return en.ToString();
        }

        /// <summary>
        /// 获取枚举所有项列表
        /// </summary>
        /// <returns></returns>
        public static List<T> GetList<T>()
        {
            List<T> list = new List<T>();

            foreach (string str in Enum.GetNames(typeof(T)))
            {
                list.Add(GetEnum<T>(str));
            }

            return list;
        }
    }
}
