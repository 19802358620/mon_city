using System;
using System.Linq;
using Microsoft.Practices.Unity;
using Ge.Infrastructure.Extensions;
using System.Reflection;

namespace Ge.Infrastructure.Ioc
{
    public static class UnityContainerExtensions
    {

        /// <summary>
        /// 注入泛型T的子类对象
        /// </summary>
        public static void RegisterInheritedTypes<T>(this IUnityContainer container)
        {
            var baseType = typeof(T);
            container.RegisterInheritedTypes(baseType);
        }

        /// <summary>
        /// 注入baseType的子类对象
        /// </summary>
        public static void RegisterInheritedTypes(this IUnityContainer container, Type baseType)
        {
            var allTypes = baseType.Assembly.GetTypes();
            var baseInterfaces = baseType.GetInterfaces();
            foreach (var type in allTypes)
            {
                if (type.BaseType != null && type.BaseType.GenericEq(baseType))
                {
                    var typeInterface = type.GetInterfaces()
                        .FirstOrDefault(x => !baseInterfaces.Any(bi => bi.GenericEq(x)));
                    if (typeInterface == null)
                    {
                        continue;
                    }
                    container.RegisterType(typeInterface, type);
                }
            }
        }

        /// <summary>
        /// 注入实现接口interfaceType的子类对象
        /// </summary>
        /// <param name="interfaceType"></param>
        public static void RegisterInterfaceTypes(this IUnityContainer container, Type baseInterface)
        {
            var allTypes = baseInterface.Assembly.GetTypes();
            var types = allTypes.ToList().FindAll(type => type.GetInterfaces().ToList().Exists(x => baseInterface.GenericEq(x)) 
                && !type.Attributes.HasFlag(TypeAttributes.Abstract));

            foreach (var type in types)
            {
                var typeInterface = type.GetInterfaces()
                        .FirstOrDefault(x => !baseInterface.GenericEq(x));
                if (typeInterface == null)
                {
                    continue;
                }
                container.RegisterType(typeInterface, type);
            }
        }
    }
}
