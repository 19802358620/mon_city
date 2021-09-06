using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ge.Infrastructure.Utilities;

namespace Ge.Infrastructure.Extensions
{
    /// <summary>
    /// 名    称：DataTableExtensions
    /// 作    者：胡政
    /// 创建时间：2015-08-22
    /// 联系方式：13436053642
    /// 描    述：DataTable扩展方法
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// 将DataTable转化为List
        /// </summary>>
        public static List<TModel> ToList<TModel>(this DataTable dt) where TModel : class, new()
        {
            return ObjectMapper.Mapper<TModel>(dt);
        }

        /// <summary>
        /// 将DataTable转化为List
        /// </summary>>
        public static List<DataRow> ToList(this DataTable dt)
        {
            var list = new List<DataRow>();
            for(var i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(dt.Rows[i]);
            }

            return list;
        }
    }
}
