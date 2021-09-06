using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ge.Infrastructure.RegularExpressions
{
    /// <summary>
    /// 正则表达式字典
    /// </summary>
    public class RegularDictionary
    {
        public static string Moble { get { return @"^1[3456789]\d{9}$"; } }//手机号码
        public static string Telephone { get { return @"^(0\\d{2,3}-?\\d{7,8}(-\\d{3,5}){0,1})|(((13[0-9])|(15([0-3]|[5-9]))|(18[0-9])|(17[0-9])|(14[0-9]))\\d{8})$"; } }//固定电话
        public static string IdentityCard { get { return @"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$"; } } //身份证
        public static string Email { get { return @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"; } }//邮箱账户
        public static string PosInt { get { return @"^[1-9]\d*$"; } }//正整数
        public static string Ip { get { return @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$"; } }//Ip
        public static string LetterNumUnderLine { get { return @"^[0-9a-z_]*$"; } }//字母、数字、下划线组成
    }
}
