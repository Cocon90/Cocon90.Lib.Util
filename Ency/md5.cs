using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cocon90.Lib.Util.Ency
{
    /// <summary>
    /// MD5签名
    /// </summary>
    public sealed class md5
    {
        /// <summary>
        /// 用MD5加密字符串
        /// </summary>
        public static string Ency(string sourceString)
        {
            StringBuilder sb = new StringBuilder(32);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.UTF8.GetBytes(sourceString));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }

    }

}
