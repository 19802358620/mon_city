using System.Linq;
using System.Web.Mvc;
using Ge.Infrastructure.Model;

namespace Ge.Infrastructure.Mvc.Extensions
{
    /// <summary>
    /// 名    称：ControllerExtensions
    /// 作    者：胡政
    /// 创建时间：2015-08-22
    /// 联系方式：13436053642
    /// 描    述：Controller控制器扩展方法
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// 获取第一条验证失败的信息
        /// </summary>
        public static ModelError FirstError(this System.Web.Mvc.Controller controller)
        {
            var error = new ModelError(string.Empty);
            foreach (var item in controller.ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    error = item.Errors.First();
                }
            }
            return error;
        }

        /// <summary>
        /// 自定义模型验证器
        /// </summary>
        public static ModelValidate ModelValidate(this System.Web.Mvc.Controller controller, params object[] models)
        {
            return new ModelValidate(models);
        }
    }
}
