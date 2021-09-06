using System;

namespace Ge.Infrastructure.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 判断是否是同一类型
        /// </summary>
        public static bool GenericEq(this Type type, Type toCompare)
        {
            return type.Namespace == toCompare.Namespace && type.Name == toCompare.Name;
        }
    }
}
