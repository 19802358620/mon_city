using System.Text;
using System.Security.Cryptography;

namespace Ge.Infrastructure.Utilities
{
    /// <summary>
    /// Md5加密
    /// </summary>
    public sealed class Md5Encrypt
    {
        private static MD5 md5 = MD5.Create();
        public static string HashString(string sourceString)
        {
            return HashString(Encoding.UTF8, sourceString);
        }

        public static string HashString(Encoding encode, string sourceString)
        {
            byte[] source = md5.ComputeHash(encode.GetBytes(sourceString));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                sBuilder.Append(source[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static string Md5EncryptPassword(string sourceString)
        {
            return HashString(sourceString);
        }

        public static string Md5EncryptToken(string sourceString)
        {
            MD5CryptoServiceProvider md5csp = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(sourceString);
            byte[] result = md5csp.ComputeHash(bytes);
            string returnString = "";
            foreach (byte b in result)
            {
                returnString += b.ToString("X");
            }
            return returnString;
        }
    }
}
