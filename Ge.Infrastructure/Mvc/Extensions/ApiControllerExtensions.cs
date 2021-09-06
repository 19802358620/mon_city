using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace Ge.Infrastructure.Mvc.Extensions
{
    /// <summary>
    /// 名    称：ApiControllerExtensions
    /// 作    者：胡政
    /// 创建时间：2015-09-02
    /// 联系方式：13436053642
    /// 描    述：ApiController控制器扩展方法
    /// </summary>
    public static class ApiControllerExtensions
    {
        /// <summary>
        /// 获取第一条验证失败的信息
        /// </summary>
        public static ModelError FirstError(this System.Web.Http.ApiController apiController)
        {
            var error = new ModelError(string.Empty);
            foreach (var item in apiController.ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    error = item.Errors.First();
                }
            }
            return error;
        }
    }
}

