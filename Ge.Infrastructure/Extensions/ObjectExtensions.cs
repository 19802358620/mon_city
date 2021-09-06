using Ge.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Extensions
{
    /// <summary>
    /// 名    称：ObjectExtensions
    /// 作    者：胡政
    /// 创建时间：2015-08-22
    /// 联系方式：13436053642
    /// 描    述：Object扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// object类型转换
        /// </summary>>
        public static TTo ToModel<TFrom, TTo>(this TFrom o) where TFrom : class
        {
            return ObjectMapper.Mapper<TFrom, TTo>(o);
        }
    }
}
