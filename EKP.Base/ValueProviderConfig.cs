using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EKP.Base
{
    /// <summary>
    /// 名    称：ControllerModelValueProviderFactory
    /// 作    者：胡政
    /// 创建时间：2015-08-28
    /// 联系方式：13436053642
    /// 描    述：自定义Controller模型绑定提供器
    /// </summary>
    public class ValueProviderConfig
    {
        /// <summary>
        /// 注册自定义模型绑定器
        /// </summary>
        public static void RegisterValueProvider()
        {
            System.Web.Mvc.ValueProviderFactories.Factories.Insert(0, new ControllerModelValueProviderFactory());
        }
    }

    public class ControllerModelValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new ValueProvider(controllerContext.HttpContext.Request);
        }

        private class ValueProvider : IValueProvider
        {
            private readonly HttpRequestBase request;

            public ValueProvider(HttpRequestBase request)
            {
                this.request = request;
            }

            public bool ContainsPrefix(string prefix)
            {
                string key = string.Empty;
                key = GetKey(request, prefix);
                return !string.IsNullOrEmpty(key);
            }

            public ValueProviderResult GetValue(string key)
            {
                if (!ContainsPrefix(key))
                    return null;
                string formKey = GetKey(request, key);
                string value = request[formKey];
                return new ValueProviderResult(value, value, CultureInfo.CurrentCulture);
            }

            private string GetKey(HttpRequestBase request, string prefix)
            {
                return request.Params.AllKeys.FirstOrDefault(k =>
                    !string.IsNullOrEmpty(k) &&
                    k.Split('.').Count() > 1 && 
                    k.Split('.').ToList().Contains(prefix));
            }
        }
    }
}