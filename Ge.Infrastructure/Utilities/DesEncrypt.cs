using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Ge.Infrastructure.Utilities
{
    /// <summary>
    /// DES加密、解密
    /// </summary>
    public class DesEncrypt
    {
        /// <summary>
        /// 对字符串进行DES加密
        /// </summary>
        /// <param name="sourceString">待加密的字符串</param>
        /// <returns>加密后的BASE64编码的字符串</returns>
        public static string Encrypt(string sourceString, string key = "56422589")
        {

            byte[] btKey = Encoding.UTF8.GetBytes(key);

            byte[] btIV = Encoding.UTF8.GetBytes(key);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            {

                byte[] inData = Encoding.UTF8.GetBytes(sourceString);

                try
                {

                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                    {

                        cs.Write(inData, 0, inData.Length);

                        cs.FlushFinalBlock();

                    }



                    return Convert.ToBase64String(ms.ToArray());

                }

                catch
                {

                    throw;

                }

            }

        } 

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位</param>
        /// <returns>已解密的字符串</returns>
        public static string Decrypt(string pToDecrypt, string key = "56422589")

        {

            //转义特殊字符

            pToDecrypt = pToDecrypt.Replace("-", "+");

            pToDecrypt = pToDecrypt.Replace("_", "/");

            pToDecrypt = pToDecrypt.Replace("~", "=");

            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);

            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())

            {

                des.Key = ASCIIEncoding.ASCII.GetBytes(key);

                des.IV = ASCIIEncoding.ASCII.GetBytes(key);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))

                {

                    cs.Write(inputByteArray, 0, inputByteArray.Length);

                    cs.FlushFinalBlock();

                    cs.Close();

                }

                string str = Encoding.UTF8.GetString(ms.ToArray());

                ms.Close();

                return str;

            }

        }
    }
}
