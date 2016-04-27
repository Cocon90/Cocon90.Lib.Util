using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Cocon90.Lib.Util.Request
{
    /// <summary>
    /// 请求辅助功能
    /// </summary>
    public class requestHelper
    {
        /// <summary>
        /// 开始向此URL发请求（自动将使用异常读取的方式），请求完成后调用readComplted（2参数）委托
        /// </summary>
        /// <param name="url"></param>
        /// <param name="readComplted"></param>
        public static void OpenRead(string url, OpenReadCompletedEventHandler readComplted)
        {
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += readComplted;
            wc.OpenReadAsync(new Uri(url));
        }
        /// <summary>
        /// 异步上传文件到到指定的Url页面。其中filePath为本地文件全路径，uploadFinish表示发送成功时执行的委托
        /// </summary>
        public static void PostFile(string filePath, string url, UploadFileCompletedEventHandler uploadFinish)
        {
            WebClient wc = new WebClient();
            wc.QueryString.Add("date", DateTime.Now.Millisecond + "");
            if (uploadFinish != null)
                wc.UploadFileCompleted += uploadFinish;
            wc.UploadFileAsync(new Uri(url), filePath);
        }

        CookieContainer cookie = new CookieContainer();
        /// <summary>
        /// 获取或设置当前的请求的Cookie容器。
        /// </summary>
        public CookieContainer Cookie { get { return cookie; } set { cookie = value; } }
        private string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            var bys = Encoding.UTF8.GetBytes(postDataStr);
            myRequestStream.Write(bys, 0, bys.Length);
            myRequestStream.Flush();
            myRequestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        public string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

    }
}
