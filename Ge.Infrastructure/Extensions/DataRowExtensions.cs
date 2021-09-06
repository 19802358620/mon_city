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
    public static class DataRowExtensions
    {
        /// <summary>
        /// 将DataRow转化为model
        /// </summary>>
        public static TModel ToModel<TModel>(this DataRow dr) where TModel : class, new()
        {
            return ObjectMapper.Mapper<TModel>(dr);
        }
    }
}
