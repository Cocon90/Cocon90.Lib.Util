using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.GZip
{
    /// <summary>
    /// GZIP压缩辅助类
    /// </summary>
    public class gzipHelper
    {
        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="strUncompressed">未压缩的字符串</param>
        /// <returns>压缩的字符串</returns>
        public static string StringCompress(string strUncompressed)
        {
            byte[] bytData = System.Text.Encoding.Unicode.GetBytes(strUncompressed);
            MemoryStream ms = new MemoryStream();
            Stream s = new GZipStream(ms, CompressionMode.Compress);
            s.Write(bytData, 0, bytData.Length);
            s.Close();
            byte[] dataCompressed = (byte[])ms.ToArray();
            return System.Convert.ToBase64String(dataCompressed, 0, dataCompressed.Length);
        }
        /// <summary>
        /// 解压缩字符串
        /// </summary>
        /// <param name="strCompressed">压缩的字符串</param>
        /// <returns>未压缩的字符串</returns>
        public static string StringDeCompress(string strCompressed)
        {
            System.Text.StringBuilder strUncompressed = new System.Text.StringBuilder();
            int totalLength = 0;
            byte[] bInput = System.Convert.FromBase64String(strCompressed); ;
            byte[] dataWrite = new byte[4096];
            Stream s = new GZipStream(new MemoryStream(bInput), CompressionMode.Decompress);
            while (true)
            {
                int size = s.Read(dataWrite, 0, dataWrite.Length);
                if (size > 0)
                {
                    totalLength += size;
                    strUncompressed.Append(System.Text.Encoding.Unicode.GetString(dataWrite, 0, size));
                }
                else
                {
                    break;
                }
            }
            s.Close();
            return strUncompressed.ToString();
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="iFile">压缩前文件路径</param>
        /// <param name="oFile">压缩后文件路径</param>
        public static void CompressFile(string iFile, string oFile)
        {
            //判断文件是否存在
            if (File.Exists(iFile) == false)
            {
                throw new FileNotFoundException("文件未找到!");
            }
            //创建文件流
            byte[] buffer = null;
            FileStream iStream = null;
            FileStream oStream = null;
            GZipStream cStream = null;
            try
            {
                //把文件写进数组
                iStream = new FileStream(iFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                buffer = new byte[iStream.Length];
                int num = iStream.Read(buffer, 0, buffer.Length);
                if (num != buffer.Length)
                {
                    throw new ApplicationException("压缩文件异常!");
                }
                //创建文件输出流并输出
                oStream = new FileStream(oFile, FileMode.OpenOrCreate, FileAccess.Write);
                cStream = new GZipStream(oStream, CompressionMode.Compress, true);
                cStream.Write(buffer, 0, buffer.Length);
            }
            finally
            {
                //关闭流对象
                if (iStream != null) iStream.Close();
                if (cStream != null) cStream.Close();
                if (oStream != null) oStream.Close();
            }
        }
        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="iFile">压缩前文件路径</param>
        /// <param name="oFile">压缩后文件路径</param>
        public static void DecompressFile(string iFile, string oFile)
        {
            //判断文件是否存在
            if (File.Exists(iFile) == false)
            {
                throw new FileNotFoundException("文件未找到!");
            }
            //创建文件流
            FileStream iStream = null;
            FileStream oStream = null;
            GZipStream dStream = null;
            byte[] qBuffer = new byte[4];
            try
            {
                //把压缩文件写入数组
                iStream = new FileStream(iFile, FileMode.Open);
                dStream = new GZipStream(iStream, CompressionMode.Decompress, true);
                int position = (int)iStream.Length - 4;
                iStream.Position = position;
                iStream.Read(qBuffer, 0, 4);
                iStream.Position = 0;
                int num = BitConverter.ToInt32(qBuffer, 0);
                byte[] buffer = new byte[num + 100];
                int offset = 0, total = 0;
                while (true)
                {
                    int bytesRead = dStream.Read(buffer, offset, 100);
                    if (bytesRead == 0) break;
                    offset += bytesRead;
                    total += bytesRead;
                }
                //创建输出流并输出
                oStream = new FileStream(oFile, FileMode.Create);
                oStream.Write(buffer, 0, total);
                oStream.Flush();
            }
            finally
            {
                //关闭流对象
                if (iStream != null) iStream.Close();
                if (dStream != null) dStream.Close();
                if (oStream != null) oStream.Close();
            }
        }
    }
}

