using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ge.Infrastructure.WebService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    /// <summary>
    /// web接口访问
    /// </summary>
    public class WebServiceRespose
    {
        #region Static Field
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        #endregion

        #region  public Method

        /// <summary>
        /// 创建并发送POST请求
        /// </summary>
        /// <param name="param">POST请求所需参数</param>
        /// <returns></returns>
        public HttpWebResponse Post(WebServiceResposeParam param)
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (param.Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(param.Url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(param.Url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            request.KeepAlive = true;

            if (!string.IsNullOrEmpty(param.UserAgentt))
            {
                request.UserAgent = param.UserAgentt;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (param.Timeout.HasValue)
            {
                request.Timeout = param.Timeout.Value;
            }

            //如果需要POST querystring形式的数据 （类似querystring格式）
            if (param.Parameters != null)
            {
                var data = param.RequestEncoding.GetBytes(param.Parameters);
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
            }

            //如果需要POST JSON数据
            if (!string.IsNullOrEmpty(param.PostJson))
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    //string json = "{\"user\":\"test\"," +
                    //              "\"password\":\"bla\"}";

                    streamWriter.Write(param.PostJson);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }


            if (!string.IsNullOrEmpty(param.Referer))
            {
                request.Referer = param.Referer;
            }

            var response = request.GetResponse() as HttpWebResponse;

            if (response !=null && request.CookieContainer != null)
            {
                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
            }

            return response;
        }

        /// <summary>
        /// 创建并发送GET请求
        /// </summary>
        /// <param name="param">GET请求所需参数</param>
        /// <returns></returns>
        public HttpWebResponse Get(WebServiceResposeParam param)
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (param.Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(param.Url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(param.Url) as HttpWebRequest;
            }
            request.Method = "GET";
            request.ContentType = "application/json;charset=UTF-8";
            request.KeepAlive = true;

            if (!string.IsNullOrEmpty(param.UserAgentt))
            {
                request.UserAgent = param.UserAgentt;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (param.Timeout.HasValue)
            {
                request.Timeout = param.Timeout.Value;
            }

            //如果需要POST querystring形式的数据 （类似querystring格式）
            if (param.Parameters != null)
            {
                var data = param.RequestEncoding.GetBytes(param.Parameters);
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
            }

            if (!string.IsNullOrEmpty(param.Referer))
            {
                request.Referer = param.Referer;
            }

            var response = request.GetResponse() as HttpWebResponse;

            if (response != null && request.CookieContainer != null)
            {
                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
            }

            return response;
        }

        /// <summary>
        /// 将response转换成文本
        /// </summary>
        /// <param name="response"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetStream(HttpWebResponse response, Encoding encoding)
        {
            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    switch (response.ContentEncoding.ToLower())
                    {
                        case "gzip":
                            {
                                string result = Decompress(response.GetResponseStream(), encoding);
                                response.Close();
                                return result;
                            }
                        default:
                            {
                                using (var sr = new StreamReader(response.GetResponseStream(), encoding))
                                {
                                    string result = sr.ReadToEnd();
                                    sr.Close();
                                    sr.Dispose();
                                    response.Close();
                                    return result;
                                }
                            }
                    }
                }
                else
                {
                    response.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return "";
        }

        #endregion

        #region  private Method

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }


        private static string Decompress(Stream stream, Encoding encoding)
        {
            var buffer = new byte[100];
            //int length = 0;

            using (var gz = new GZipStream(stream, CompressionMode.Decompress))
            {
                //GZipStream gzip = new GZipStream(res.GetResponseStream(), CompressionMode.Decompress);
                using (var reader = new StreamReader(gz, encoding))
                {
                    return reader.ReadToEnd();
                }
                /*
                using (MemoryStream msTemp = new MemoryStream())
                {
                    //解压时直接使用Read方法读取内容，不能调用GZipStream实例的Length等属性，否则会出错：System.NotSupportedException: 不支持此操作；
                    while ((length = gz.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        msTemp.Write(buffer, 0, length);
                    }

                    return encoding.GetString(msTemp.ToArray());
                }
                 * */
            }
        }

        #endregion
    }

    /// <summary>
    /// 请求页面需要的参数
    /// </summary>
    public class WebServiceResposeParam
    {
        private string postJson = "{}";
        private Encoding requestEncoding = Encoding.UTF8;

        /// <summary>
        /// 请求的URL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 需要post的json参数
        /// </summary>
        public string PostJson{
            get { return postJson; }
            set { postJson = value; }
        }
        /// <summary>
        /// 随同请求POST的参数名称及参数值字典
        /// </summary>
        public string Parameters{ get; set; }
        /// <summary>
        /// 请求的超时时间
        /// </summary>
        public int? Timeout{ get; set; }
        /// <summary>
        /// 请求的客户端浏览器信息，可以为空
        /// </summary>
        public string UserAgentt{ get; set; }
        /// <summary>
        /// 发送HTTP请求时所用的编码
        /// </summary>
        public Encoding RequestEncoding
        {
            get { return requestEncoding; }
            set { requestEncoding = value; }
        }
        /// <summary>
        /// 随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空
        /// </summary>
        public string Referer { get; set; }
    }
}