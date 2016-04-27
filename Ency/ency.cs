using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web;
namespace Cocon90.Lib.Util.Ency
{
    /// <summary>
    /// 加密辅助类
    /// </summary>
    public class ency
    {
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="sourceString">待加密的字符串</param>
        /// <param name="encyKeyUse8Letter">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string des(string sourceString, string encyKeyUse8Letters)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encyKeyUse8Letters.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(sourceString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return sourceString;
            }
        }
        /// <summary>
        /// 对整个文件执行加密
        /// </summary>
        /// <param name="sourceFilePath">源文件路径</param>
        /// <param name="targetFilePath">目标文件存放路径</param>
        /// <param name="isDeleteSoruceFile">加密后删源文件否</param>
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
