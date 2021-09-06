using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Ge.Infrastructure.RegularExpressions;

namespace Ge.Infrastructure.Utilities
{
    /// <summary>
    /// 对指定格式的Ip段的辅助操作方法
    /// </summary>
    public class IpsHelper
    {
        /// <summary>
        /// 检查Ip段格式是否正确，如果不正确则返回错误信息
        /// </summary>
        /// <returns></returns>
        public static bool IsRightIps(string ipStr, out string error)
        {
            error = null;
            if (ipStr == null) return true;
            ipStr = ipStr.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(" ", string.Empty);
            if (string.IsNullOrEmpty(ipStr)) return true;

            var ips = ipStr.Split(',').ToList();
            foreach (var ipItem in ips)
            {
                var ipStartEnd = ipItem.Split('-');
                if (ipStartEnd.Length != 1 && ipStartEnd.Length != 2)
                {
                    error = string.Format("\"{0}\"处格式有误", ipItem[0]);
                    return false;
                }

                if (!new Regex(RegularDictionary.Ip).IsMatch(ipStartEnd[0]))
                {
                    error = string.Format("\"{0}\"不是有效的Ip", ipStartEnd[0]);
                    return false;
                }

                if (ipStartEnd.Length == 2 && !new Regex(RegularDictionary.Ip).IsMatch(ipStartEnd[1]))
                {
                    error = string.Format("\"{0}\"不是有效的Ip", ipStartEnd[1]);
                    return false;
                }

                if (ipStartEnd.Length == 2 && IpToInt(ipStartEnd[0]) > IpToInt(ipStartEnd[1]))
                {
                    error = string.Format("起始Ip\"{0}\"应小于等于终止Ip\"{1}\"", ipStartEnd[0], ipStartEnd[1]);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Ip地址格式转换为int型
        /// </summary>
        public static long IpToInt(string ip)
        {
            byte[] ipBytes = System.Net.IPAddress.Parse(ip).GetAddressBytes();
            double num = 0;
            if (!string.IsNullOrEmpty(ip))
            {
                for (int i = ipBytes.Length - 1; i >= 0; i--)
                {
                    num += ((ipBytes[i] % 256) * Math.Pow(256, (3 - i)));
                }
            }

            return (long)num;
        }

        /// <summary>
        /// 检查ip是否在指定的ip段内
        /// </summary>
        public static bool IsInIps(string ip, string ipStr)
        {
            ipStr = ipStr.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(" ", string.Empty);
            var ips = ipStr.Split(',').ToList();
            foreach (var ipItem in ips)
            {
                var ipStartEnd = ipItem.Split('-');
                if (ipStartEnd.Length == 2 && IpToInt(ipStartEnd[0]) <= IpToInt(ip) &&
                    IpToInt(ipStartEnd[1]) >= IpToInt(ip))
                {
                    return true;
                }
                else if (ipStartEnd.Length == 1 && IpToInt(ipStartEnd[0]) == IpToInt(ip))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 检查两个ip段是否冲突
        /// </summary>
        public static bool IpsConflict(string ipStr1, string ipStr2)
        {
            if (string.IsNullOrEmpty(ipStr1) || string.IsNullOrEmpty(ipStr2)) return false;

            ipStr1 = ipStr1.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(" ", string.Empty);
            ipStr2 = ipStr2.Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(" ", string.Empty);
            var ips1 = ipStr1.Split(',').ToList();
            var ips2 = ipStr2.Split(',').ToList();
            foreach (var ipItem1 in ips1)
            {
                var ipStartEnd1 = ipItem1.Split('-');
                foreach (var ipItem2 in ips2)
                {
                    var ipStartEnd2 = ipItem2.Split('-');
                    if (ipStartEnd1.Length == 1 && IsInIps(ipStartEnd1[0], ipStr2))
                    {
                        return true;
                    }
                    else if (ipStartEnd2.Length == 1 && IsInIps(ipStartEnd2[0], ipStr1))
                    {
                        return true;
                    }
                    else if ((ipStartEnd1.Length == 2 && ipStartEnd2.Length == 2) &&
                             (Math.Max(IpToInt(ipStartEnd1[0]), IpToInt(ipStartEnd1[1])) >=
                              Math.Min(IpToInt(ipStartEnd2[0]), IpToInt(ipStartEnd2[1]))
                              && Math.Min(IpToInt(ipStartEnd1[0]), IpToInt(ipStartEnd1[1])) <=
                              Math.Max(IpToInt(ipStartEnd2[0]), IpToInt(ipStartEnd2[1]))))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// IP地区
        /// </summary>
        public class RequestIpArea
        {
            public string Country { get; set; }
            public string Province { get; set; }
            public string City { get; set; }
            public string ISP { get; set; }
        }
        ///// <summary>
        ///// 静态IP库
        ///// </summary>
        //public static QQWry ipLocation =
        //    new QQWry(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["QQwry"]));

        /// <summary>
        /// IP 转地区
        /// </summary>
        /// <param name="ip"></param>
        public static RequestIpArea IpToArea(string ip)
        {
            var ipl = new QQWry(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["QQwry"])).SearchIPLocation(ip);

            var requestIpArea = new RequestIpArea
            {
                ISP = ipl.ISP,
                Country = ipl.Area,
                Province = "",
                City = "",
            };

            if (ipl.Area.Contains("省") || ipl.Area.Contains("市") || ipl.Area.Contains("县"))
            {
                if (new[]
                {
                    "香港", "广西", "内蒙", "澳门",
                    "新疆", "西藏", "宁夏"
                }.Contains(ipl.Area.Substring(0, 2)))
                {
                    requestIpArea.Country = "中国";
                    requestIpArea.Province = ipl.Area.Substring(0, 2) == "内蒙" ? "内蒙古" : ipl.Area.Substring(0, 2);

                    var fix = ipl.Area.Substring(0, 2) == "内蒙" ? 3 : 2;
                    var pcc = ipl.Area.Substring(fix).Split(new[] {"市", "区", "县", "自治"}, StringSplitOptions.RemoveEmptyEntries);
                    if (pcc.Length >= 1)
                    {
                        requestIpArea.City = pcc[0];
                    }
                }
                else
                {
                    var pcc = ipl.Area.Split(new[] { "省", "市", "区", "县", "自治" }, StringSplitOptions.RemoveEmptyEntries);
                    if (pcc.Length >= 1)
                    {
                        if (new[]
                        {
                            "北京", "广东", "山东", "江苏",
                            "河南", "上海", "河北", "浙江",
                            "香港", "陕西", "湖南", "重庆",
                            "福建", "天津", "云南", "四川",
                            "广西", "安徽", "海南", "江西",
                            "湖北", "山西", "辽宁", "台湾",
                            "黑龙江", "内蒙古", "澳门", "贵州",
                            "甘肃", "青海", "新疆", "西藏", "吉林", "宁夏"
                        }.Contains(pcc[0]))
                        {
                            //第一字符串包含 省份
                            requestIpArea.Country = "中国";
                            requestIpArea.Province = pcc[0];
                            if (pcc.Length > 1)
                            {
                                requestIpArea.City = pcc[1];
                            }
                            else if (new[] { "北京", "天津", "重庆", "上海" }.Contains(pcc[0]))
                            {
                                requestIpArea.Province = pcc[0];
                                requestIpArea.City = pcc[0];
                            }
                        }
                        else
                        {
                            requestIpArea.Province = pcc[0];
                        }
                    }
                }
            }

            return requestIpArea;
        }
    }
}