using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cocon90.Lib.Util.Ency
{
    /// <summary>
    /// MD5签名与校验，服务端要发给客户端一段重要信息前，先将信息进行签名，然后将签名后的结果一起发送。客户端接收到之后，进行校验。如果是原来的信息，则校验成功，返回True。如果信息被修改过，则校验失败，返回False
    /// </summary>
    public sealed class md5Sign
    {
        /// <summary>
        /// 用MD5签名字符串
        /// </summary>
        /// <param name="sourceString">需要签名的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>签名结果</returns>
        public static string Sign(string sourceString, string key)
        {
            StringBuilder sb = new StringBuilder(32);

            sourceString = sourceString + key;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(sourceString));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }
        public static string Sign(string sourceString)
        {
            return Sign(sourceString, "chinapsu");
        }
        /// <summary>
        /// 进行校验。如果是原来的信息，则校验成功，返回True。如果信息被修改过，则校验失败，返回False
        /// </summary>
        /// <param name="sourceString">需要签名的字符串</param>
        /// <param name="sign">签名结果</param>
        /// <param name="key">密钥</param>
        /// <returns>验证结果</returns>
        public static bool Verify(string sourceString, string sign, string key)
        {
            string mysign = Sign(sourceString, key);
            if (mysign == sign)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool Verify(string sourceString, string sign)
        {
            return Verify(sourceString, sign, "chinapsu");
        }
    }

}
