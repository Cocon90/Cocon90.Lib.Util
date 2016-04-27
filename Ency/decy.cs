using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cocon90.Lib.Util.Ency
{
    /// <summary>
    /// 解密辅助类
    /// </summary>
    public class decy
    {
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string des(string decryptString, string decryptKey8Letter)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey8Letter);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        /// <summary>
        /// 对整个文件进行解密
        /// </summary>
        /// <param name="sourceFilePath">解密的源加密文件路径</param>
        /// <param name="targetFilePath">解密后存放到哪里</param>
        /// <param name="isDeleteSoruceFile">解密之后是否删除源加密文件</param>
        /// <returns></returns>
        public static bool file(string sourceFilePath, string targetFilePath, bool isDeleteSoruceFile)
        {
            if (File.Exists(sourceFilePath))
            {
                string[] str = File.ReadAllLines(sourceFilePath, Encoding.Default);
                string[] res = new string[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    res[i] = des(str[i].ToString(), "chinapsu");
                }
                try
                {
                    File.WriteAllLines(targetFilePath, res, Encoding.Default);
                    if (isDeleteSoruceFile)
                    {
                        File.Delete(sourceFilePath);
                    }
                }
                catch { return false; }
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
