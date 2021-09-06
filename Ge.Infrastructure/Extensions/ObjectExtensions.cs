using Ge.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Extensions
{
    /// <summary>
    /// ��    �ƣ�ObjectExtensions
    /// ��    �ߣ�����
    /// ����ʱ�䣺2015-08-22
    /// ��ϵ��ʽ��13436053642
    /// ��    ����Object��չ����
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// object����ת��
        /// </summary>>
        public static TTo ToModel<TFrom, TTo>(this TFrom o) where TFrom : class
        {
            return ObjectMapper.Mapper<TFrom, TTo>(o);
        }
    }
}
