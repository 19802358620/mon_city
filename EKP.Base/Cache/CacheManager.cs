using Ge.Infrastructure.Metronicv;
using Newtonsoft.Json2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EKP.Base.Cache
{
    /// <summary>
    /// 服务端缓存管理
    /// </summary>
    public class CacheManager
    {
        /// <summary>
        /// 设置cache值
        /// 默认缓存半小时
        /// </summary>
        public static void Set<T>(string key, T value, int minutes = 30)
        {
            if (!BaseConfig.IsCache) return;

            if (HttpContext.Current.Cache[key] != null)
                Remove(key);
            HttpContext.Current.Cache.Insert(key, value, null,
                DateTime.Now.AddMinutes(minutes), System.Web.Caching.Cache.NoSlidingExpiration);//缓存半小时后过期
        }

        /// 获取cache值，如果存在直接返回，不存在设置缓存值后返回
        /// </summary>
        public static T GetByCache<T>(string key, T nullValue, int minutes = 30)
        {
            if (!BaseConfig.IsCache) return nullValue;

            if (Get(key) != null) return (T)Get(key);

            Set(key, nullValue, minutes);
            return nullValue;
        }

        /// <summary>
        /// 获取cache值，如果存在直接返回，不存在设置缓存值饭后返回
        /// </summary>
        public static T GetByCache<T>(string key, Func<T, T> cacheValue, int minutes = 30)
        {
            if (!BaseConfig.IsCache) return cacheValue(default(T));

            if (Get(key) != null) return (T)Get(key);

            if (cacheValue != null)
            {
                Set(key, cacheValue(default(T)), minutes);
            }
            return (T)Get(key);
        }

        /// <summary>
        /// 获取cache值
        /// </summary>
        public static object Get(string key)
        {
            if (!BaseConfig.IsCache) return null;

            return HttpContext.Current.Cache[key];  
        }

        /// <summary>
        /// 移除cache值
        /// </summary>
        public static void Remove(string key)
        {
            if (HttpContext.Current.Cache[key] != null)
                HttpContext.Current.Cache.Remove(key);
        }

        /// <summary>
        /// 移除cache值
        /// </summary>
        public static void Remove(List<string> keys)
        {
            keys.ForEach(CacheManager.Remove);
        }

        /// <summary>
        /// 获取所有缓存分页
        /// </summary>
        public static JqgridResult<T> GetPager<T>(CachePagerParam param, params string[] includePath) where T : CachePagerModel, new()
        {
            var caches = new List<T>();
            var cacheEnum = System.Web.HttpRuntime.Cache.GetEnumerator(); 
            while (cacheEnum.MoveNext())
            {
                caches.Add(new T
                {
                    Key = cacheEnum.Key.ToString(),
                    Value = JsonConvert.SerializeObject(cacheEnum.Value),
                });
            }

            //筛选
            if (!string.IsNullOrEmpty(param.KeyWord))
            {
                caches = caches.FindAll(c => c.Key.Contains(param.KeyWord) || c.Value.Contains(param.KeyWord));
            }

            //排序
            caches = caches.OrderBy(c => c.Key).ToList();

            return new JqgridResult<T>(param)
            {
                Rows = caches.Take(param.Take).Skip(param.Skip).ToList(),
                TotalRecords = caches.Count,
            };
        }
    }
}