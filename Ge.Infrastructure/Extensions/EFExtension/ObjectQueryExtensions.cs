//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;

namespace Ge.Infrastructure.Extensions.EFExtension
{
    /// <summary>
    ///     When using LINQ to Entities, queries are instances of ObjectQuery but are statically
    ///     typed as IQueryable. This makes it difficult to access members of ObjectQuery, particularly
    ///     when the element type is anonymous. These extensions make it easier to access ObjectQuery
    ///     members.
    /// </summary>
    /// <example>
    ///     <code>
    /// var q = from p in context.Products
    ///         where p.ProductName.StartsWith("Foo")
    ///         select p;
    /// 
    /// // before
    /// string commandText = ((ObjectQuery&lt;Product&gt;)q).ToTraceString();
    /// 
    /// // after
    /// string commandText = q.ToTraceString();
    /// </code>
    /// </example>
    public static class ObjectQueryExtensions
    {
        /// <summary>
        ///     Returns the given IQuerable instance as an ObjectQuery instance.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">IQueryable instance.</param>
        /// <returns>source as an ObjectQuery</returns>
        public static ObjectQuery<T> AsObjectQuery<T>(this IQueryable<T> source)
        {
            return source as ObjectQuery<T>;
        }

        /// <summary>
        ///     Retrieves Entity Framework trace information for the given query.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">IQueryable instance. Must be an ObjectQuery instance at runtime.</param>
        /// <returns>Trace string for the query.</returns>
        public static string ToTraceString<T>(this IQueryable<T> source)
        {
            return source.ToObjectQuery("source").ToTraceString();
        }

        /// <summary>
        ///     Includes navigation path in query result.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">IQueryable instance. Must be an ObjectQuery instance at runtime.</param>
        /// <param name="path">Navigation path.</param>
        /// <returns>Query with spanned navigations.</returns>
        public static IQueryable<T> Include<T>(this IQueryable<T> source, string path)
        {
            return source.ToObjectQuery("source").Include(path);
        }

        /// <summary>
        ///     Sets merge option for a query.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">IQueryable instance. Must be an ObjectQuery instance at runtime.</param>
        /// <param name="mergeOption">Merge option to use when evaluating the query.</param>
        /// <returns>Query with merge option set.</returns>
        public static IQueryable<T> SetMergeOption<T>(this IQueryable<T> source, MergeOption mergeOption)
        {
            ObjectQuery<T> result = source.ToObjectQuery("source");
            result.MergeOption = mergeOption;
            return result;
        }

        /// <summary>
        ///     Returns binding list for the given query instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IBindingList ToBindingList<T>(this IQueryable<T> source)
        {
            Utility.CheckArgumentNotNull(source, "source");
            var listSource = source as IListSource;
            if (null == listSource)
            {
                throw new ArgumentException(Messages.UnableToGetBindingList, "source");
            }
            var bindingList = listSource.GetList() as IBindingList;
            if (null == bindingList)
            {
                throw new ArgumentException(Messages.UnableToGetBindingList, "source");
            }
            return bindingList;
        }

        public static ObjectQuery<T> ToObjectQuery<T>(this IQueryable<T> source, string argumentName)
        {
            Utility.CheckArgumentNotNull(source, "source");
            var result = source as ObjectQuery<T>;
            if (null == result)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Messages.OperationRequiresObjectQuery, argumentName));
            }
            return result;
        }
    }
}