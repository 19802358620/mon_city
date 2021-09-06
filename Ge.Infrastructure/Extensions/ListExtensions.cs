using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ge.Infrastructure.Utilities;

namespace Ge.Infrastructure.Extensions
{
    /// <summary>
    /// 名    称：ListExtensions
    /// 作    者：胡政
    /// 创建时间：2015-08-22
    /// 联系方式：13436053642
    /// 描    述：List<T>扩展方法
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// List类型转换
        /// </summary>>
        public static List<TTo> ToList<TFrom, TTo>(this List<TFrom> list)where TFrom  : class, new()
        {
            return ObjectMapper.Mapper<List<TFrom>, List<TTo>>(list);
        }

        /// <summary>
        /// 添加一个item并返回新的List
        /// </summary>
        public static List<T> AddItem<T>(this List<T> items, T item)
        {
            items.Add(item);
            return items;
        }

        /// <summary>
        /// 在指定位置插入item并返回新List
        /// </summary>
        public static List<T> InsertItem<T>(this List<T> items, int index, T item)
        {
            items.Insert(index, item);
            return items;
        }

        /// <summary>
        /// 将List中每个元素用指定字符串分割开
        /// 返回示例：1,2,3
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="splitStr">分隔符</param>
        /// <param name="quote">引号</param>
        /// <returns></returns>
        public static string ToStringBySplit<T>(this List<T> items, string splitStr, string quote = "")
        {
            var str = string.Empty;
            if (items == null || items.Count == 0) return str;

            items.ForEach(it => str += string.Format("{2}{0}{2}{1}", it, splitStr, quote));
            str = str.Remove(str.Length - 1);

            return str;
        }

        //深度foreach
        public static void DeepForeach<T>(this List<T> items, string propertyName, Action<T> action)
        {
            items.ForEach(action);
            //递归对子对象进行循环操作
            items.ForEach(item =>
            {
                var children = item.GetType().GetProperty(propertyName).GetValue(item, null) as List<T>;
                if (children != null)
                {
                    children.DeepForeach(propertyName, action);
                }
            });
        }

        //深度remove
        public static void DeepRemoveAll<T>(this List<T> items, string propertyName, Predicate<T> match)
        {
            items.RemoveAll(match);
            //递归查找符合bool表达式的元素进行删除
            items.ForEach(item =>
            {
                var propertie = item.GetType().GetProperties().FirstOrDefault(p => p.Name == propertyName);
                if (propertie == null) return;
                var children = propertie.GetValue(item, null) as List<T>;
                if (children != null)
                {
                    children.DeepRemoveAll(propertyName, match);
                }
            });
        }
    }
}
