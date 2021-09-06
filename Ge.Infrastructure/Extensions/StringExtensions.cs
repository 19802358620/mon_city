using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ge.Infrastructure.RegularExpressions;

namespace Ge.Infrastructure.Extensions
{
    /// <summary>
    /// 名    称：DataTableExtensions
    /// 作    者：胡政
    /// 创建时间：2015-08-22
    /// 联系方式：13436053642
    /// 描    述：字符串扩展方法
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsMobile(this string str)
        {
            return IsMatch(str, RegularDictionary.Moble, false, true);
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmail(this string str)
        {
            return IsMatch(str, RegularDictionary.Email, false, true);
        }

        /// <summary>
        /// 验证Int类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPosInt(this string str)
        {
            return IsMatch(str, RegularDictionary.PosInt, false, true);
        }

        /// <summary>
        /// 转化为int类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int? ToInt(this object obj)
        {
            if (obj == null || obj.ToString() == string.Empty) return null;

            return Convert.ToInt32(obj);
        }

        /// <summary>  
        /// 验证字符串是否匹配正则表达式描述的规则  
        /// </summary>  
        /// <param name="inputStr">待验证的字符串</param>  
        /// <param name="patternStr">正则表达式字符串</param>  
        /// <param name="ifIgnoreCase">匹配时是否不区分大小写</param>  
        /// <param name="ifValidateWhiteSpace">是否验证空白字符串</param>  
        /// <returns>是否匹配</returns>  
        private static bool IsMatch(string inputStr, string patternStr, bool ifIgnoreCase, bool ifValidateWhiteSpace)
        {
            if (!ifValidateWhiteSpace && string.IsNullOrWhiteSpace(inputStr))
                return false;//如果不要求验证空白字符串而此时传入的待验证字符串为空白字符串，则不匹配  
            Regex regex = null;
            if (ifIgnoreCase)
                regex = new Regex(patternStr, RegexOptions.IgnoreCase);//指定不区分大小写的匹配  
            else
                regex = new Regex(patternStr);
            return regex.IsMatch(inputStr);
        }

        /// <summary>
        /// 过滤SQL关键词
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Filter(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            string output = value;
            string pattern = @"select|insert|delete|from|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec|netlocalgroup administrators|script|frame";
            Match m = Regex.Match(value, pattern, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                foreach (Group g in m.Groups)
                {
                    output = output.Replace(g.Value, "");
                }
            }
            else
            {
                output = output.Replace("'", "''");
            }
            return output;
        }

        /// <summary>
        /// 和String的Format功能相同，但是扩展了一些新功能
        /// </summary>
        /// <param name="str"></param>
        public static string Format2(this string str, params object[] parameters)
        {
            return string.Format(str, parameters);
        }

        /// <summary>
        /// 用oldStr替换掉原来的字符串中的newStr，只替换一次
        /// </summary>
        /// <returns></returns>
        public static string RelpaceFirst(this string str, string oldStr, string newStr)
        {
            var r = new Regex(oldStr);
            str = r.Replace(str, newStr, 1);

            return str;
        }

        /// <summary>
        /// 字符串是否是以指定字符串数组中的某个字符串开始的，如果是，返回该字符串，否者返回null
        /// </summary>
        public static string StartsWith(this string str, List<string> arrs)
        {
            foreach (var s in arrs)
            {
                if (str.StartsWith(s))
                    return s;
            }

            return null;
        }
    }
}
