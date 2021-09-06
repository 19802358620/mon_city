using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.WebApi.HttpRequestMessage
{
    /// <summary>
    /// 判断是否是ajax请求
    /// </summary>
    public static class HttpRequestExtention
    {
        public static bool IsAjaxRequest(this System.Net.Http.HttpRequestMessage request)
        {
            var headers = request.Headers;

            return !(!headers.Contains("X-Requested-With") || headers.GetValues("X-Requested-With").FirstOrDefault() != "XMLHttpRequest");
        }
    }
}
