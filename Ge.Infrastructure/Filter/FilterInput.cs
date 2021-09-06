using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ge.Infrastructure.Filter
{
    /// <summary>
    /// 过滤输入字符是否包含数据库关键字
    /// </summary>
    public class FilterInput
    {
        /// <summary>
        /// 过滤关键字符
        /// </summary>
        public static bool CheckValid(string sInput)
        {
            if (String.IsNullOrWhiteSpace(sInput))
                return true;
            string pattern = @"select|insert|delete|from|iframe|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec|netlocalgroup administrators";
            Match m = Regex.Match(sInput, pattern, RegexOptions.IgnoreCase);
            if (m.Success)
            {
                return false;
            }
            return true;
        }
    }
}
