using System.Web.Mvc;

namespace Ge.Infrastructure.Ioc
{
    public class Ioc
    {
        public static T GetService<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }
    }
}
