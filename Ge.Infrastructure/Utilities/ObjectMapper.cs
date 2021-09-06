using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using EmitMapper;
using EmitMapper.MappingConfiguration;
using Microsoft.Practices.ObjectBuilder2;

namespace Ge.Infrastructure.Utilities
{
    /// <summary>
    /// 名    称：DbHelper
    /// 作    者：胡政
    /// 参    考：http://www.mamicode.com/info-detail-95629.html
    /// 创建时间：2015-08-21
    /// 联系方式：13436053642
    /// 描    述：EmitMapper实体映射框架封装
    /// </summary>
    public static class ObjectMapper
    {
        /// <summary>
        /// 将TFrom模型属性映射到TTo模型
        /// </summary>
        public static TTo Mapper<TFrom, TTo>(TFrom from) where TFrom : class
        {
            var mapConfig = new DefaultMapConfig();
            return Mapper<TFrom, TTo>(from, mapConfig);
        }

        /// <summary>
        /// 用自定义映射规则进行模型映射
        /// </summary>
        /// <param name="from"></param>
        /// <param name="config">映射规则</param>
        public static TTo Mapper<TFrom, TTo>(TFrom from, IMappingConfigurator mapConfig) where TFrom : class
        {
            var ignores = new List<string>();

            //初始化忽略映射的属性
            typeof(TFrom).GetProperties().ForEach(p =>
            {
                if (p.GetCustomAttributes(typeof(IgnoreMapperAttribute), false).Length > 0)
                    ignores.Add(p.Name);
            });
            typeof(TTo).GetProperties().ForEach(p =>
            {
                if (p.GetCustomAttributes(typeof(IgnoreMapperAttribute), false).Length > 0)
                    ignores.Add(p.Name);
            });

            if (mapConfig != null && mapConfig is MapConfigBase<TFrom>)
                (mapConfig as MapConfigBase<TFrom>).IgnoreMembers<TFrom, TTo>(ignores.ToArray());
            ObjectsMapper<TFrom, TTo> mapper = ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>(mapConfig);
            return mapper.Map(from);
        }

        /// <summary>
        /// 将TFrom模型属性映射到TTo模型
        /// </summary>
        public static TTo Mapper<TFrom, TTo>(TFrom from, TTo tto, IMappingConfigurator mapConfig) where TFrom : class
        {
            ObjectsMapper<TFrom, TTo> mapper = ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>(mapConfig);
            return mapper.Map(from, tto);
        }

        /// <summary>
        /// 将TFrom模型属性映射到TTo模型，未映射字段属性不变
        /// </summary>
        public static TTo Mapper<TFrom, TTo>(TFrom from, TTo tto) where TFrom : class
        {
            var mapConfig = new DefaultMapConfig();
            var ignores = new List<string>();

            //初始化忽略映射的属性
            typeof(TFrom).GetProperties().ForEach(p =>
            {
                if (p.GetCustomAttributes(typeof(IgnoreMapperAttribute), false).Length > 0)
                    ignores.Add(p.Name);
            });
            typeof(TTo).GetProperties().ForEach(p =>
            {
                if (p.GetCustomAttributes(typeof(IgnoreMapperAttribute), false).Length > 0)
                    ignores.Add(p.Name);
            });

            mapConfig.IgnoreMembers<TFrom, TTo>(ignores.ToArray());
            ObjectsMapper<TFrom, TTo> mapper = ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>(mapConfig);
            return mapper.Map(from, tto);
        }

        /// <summary>
        /// 将TFrom模型属性映射到TTo模型
        /// </summary>
        public static TTo Mapper<TFrom, TTo>(TFrom from, TTo tto, string[] fields, string[] ignores) where TFrom : class
        {
            var mapConfig = new DefaultMapConfig();

            if (fields != null)
            {
                mapConfig.MatchMembers((f, t) => fields.Contains(f));
            }
            if (ignores != null)
            {
                mapConfig.IgnoreMembers<TFrom, TTo>(ignores.ToArray());
            }

            return Mapper(from, tto, mapConfig);
        }

        /// <summary>
        /// 将datatable映射为list集合
        /// </summary>
        public static List<TModel> Mapper<TModel>(DataTable dt) where TModel : class, new()
        {
            var list = new List<TModel>();
            var propertys = typeof(TModel).GetProperties();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var model = new TModel();
                for (int j = 0; j < propertys.Count(); j++)
                {
                    string propertysName = propertys[j].Name;//columnName
                    if (dt.Columns.Contains(propertysName))
                    {
                        var modelProrperty = model.GetType().GetProperty(propertysName);
                        modelProrperty.SetValue(model, ChangeType(dt.Rows[i][propertysName], modelProrperty.PropertyType), null);
                    }
                }
                list.Add(model);
            }

            return list;
        }
        /// <summary>
        /// 将datarow映射为model
        /// </summary>
        public static TModel Mapper<TModel>(DataRow dr) where TModel : class, new()
        {
            var model = new TModel();
            var propertys = typeof(TModel).GetProperties();

            for (int i = 0; i < propertys.Count(); i++)
            {
                string propertysName = propertys[i].Name;//columnName
                if (dr.Table.Columns.Contains(propertysName))
                {
                    var modelProrperty = propertys[i];
                    if (modelProrperty.GetCustomAttributes(typeof(DateTimeToStrAttribute), false).Length > 0)
                    {
                        var dateTimeToStrAttribute =
                            (DateTimeToStrAttribute)modelProrperty.GetCustomAttributes(typeof(DateTimeToStrAttribute), false).First();
                        if (dr[propertysName] is DBNull)
                        {
                            modelProrperty.SetValue(model, null, null);
                        }
                        else
                        {
                            var dateTime = (DateTime)dr[propertysName];
                            if (string.IsNullOrEmpty(dateTimeToStrAttribute.Formatter))
                                modelProrperty.SetValue(model, dateTime.ToString(), null);
                            else
                                modelProrperty.SetValue(model, dateTime.ToString(dateTimeToStrAttribute.Formatter), null);
                        }
                    }
                    else
                    {
                        modelProrperty.SetValue(model, ChangeType(dr[propertysName], modelProrperty.PropertyType), null);
                    }
                }
            }

            return model;
        }



        /// <summary>
        /// 类型转换
        /// </summary>
        /// <returns></returns>
        public static object ChangeType(object value, Type conversionType)
        {
            if (conversionType == null)
            {
                throw new ArgumentNullException("conversionType");
            }

            //转换DbNull
            if (value == null || (value as DBNull) != null || (value as Nullable) != null)
            {
                return null;
            }
            //转换Nullable<>
            else if (conversionType.IsGenericType &&
              conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                var nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }

            return Convert.ChangeType(value, conversionType);
        }
    }
}