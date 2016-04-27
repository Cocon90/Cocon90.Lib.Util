using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Cocon90.Lib.Web.Request
{
    /// <summary>
    /// 以表单文件的形式提交
    /// </summary>
    public class postFileHelper
    {

        /// <summary>
        /// 以表单的形式提交文件，可以额外传送一对键值对。stringKey/stringContent
        /// </summary>
        /// <param name="url">提交到的接收地址</param>
        /// <param name="stringKey">额外传送的字符串内容的键</param>
        /// <param name="stringContent">额外传送的字符串内容的值</param>
        /// <param name="fileKey">上传文件的键</param>
        /// <param name="filePath">文件全路径</param>
        /// <returns></returns>
        public static string PostFile(string url, string stringKey, string stringContent, string fileKey, string filePath)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

            //请求 
            WebRequest req = WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "multipart/form-data; boundary=" + boundary;

            //组织表单数据 
            StringBuilder sb = new StringBuilder();
            sb.Append("--" + boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"" + stringKey + "\"");
            sb.Append("\r\n\r\n");
            sb.Append(stringContent);
            sb.Append("\r\n");


            sb.Append("--" + boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"" + fileKey + "\"; filename=\"" + Path.GetFileName(filePath) + "\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: application/octet-stream");
            sb.Append("\r\n\r\n");

            string head = sb.ToString();
            byte[] form_data = Encoding.UTF8.GetBytes(head);
            //结尾 
            byte[] foot_data = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            //文件 
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //post总长度 
            long length = form_data.Length + fileStream.Length + foot_data.Length;
            req.ContentLength = length;

            Stream requestStream = req.GetRequestStream();
            //发送表单参数 
            requestStream.Write(form_data, 0, form_data.Length);
            //文件内容 
            byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                requestStream.Write(buffer, 0, bytesRead);
            //结尾 
            requestStream.Write(foot_data, 0, foot_data.Length);
            requestStream.Close();

            //响应 
            WebResponse pos = req.GetResponse();
            StreamReader sr = new StreamReader(pos.GetResponseStream(), Encoding.UTF8);
            string html = sr.ReadToEnd().Trim();
            sr.Close();
            if (pos != null)
            {
                pos.Close();
                pos = null;
            }
            if (req != null)
            {
                req = null;
            }
            return html;
        }
    }
}
