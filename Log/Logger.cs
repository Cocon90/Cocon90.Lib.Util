using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Log
{
    /// <summary>
    /// 日志记录工具
    /// </summary>
    public class Logger
    {
        public Logger(string LogDirectory)
        {

            this.LogDirectory = LogDirectory;
        }
        public string LogDirectory { get; set; }
        private object obj = new object();
        /// <summary>
        /// 写到日志
        /// </summary>
        /// <param name="msg"></param>
        public void addToLog(string msg)
        {
            lock (obj)
            {
                string pth = LogDirectory;
                if (!System.IO.Directory.Exists(pth)) System.IO.Directory.CreateDirectory(pth);
                string log = String.Format("{0}{1}.txt", pth, DateTime.Now.ToString("yyyy年MM月dd日"));
                FileStream fs;
                msg = msg ?? "";
                lock (msg)
                {
                    lock (log)
                    {
                        if (File.Exists(log))
                        {
                            fs = new FileStream(log, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                            lock (fs)
                            {
                                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                                sw.WriteLine(String.Format("{0}:{1}", DateTime.Now.ToString("【yyyy-MM-dd HH:mm:ss:fff】"), msg));
                                sw.Close();
                                fs.Close();
                            }
                        }
                        else
                        {
                            fs = new FileStream(log, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                            lock (fs)
                            {
                                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                                sw.WriteLine(String.Format("{0}:{1}", DateTime.Now.ToString("【yyyy-MM-dd HH:mm:ss:fff】"), msg));
                                sw.Close();
                                fs.Close();
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 写到日志
        /// </summary>
        /// <param name="msg"></param>
        private void addToLogHasBreak(string msg)
        {
            lock (obj)
            {
               
                string pth = LogDirectory;
                if (!System.IO.Directory.Exists(pth)) System.IO.Directory.CreateDirectory(pth);
                string log = String.Format("{0}{1}.txt", pth, DateTime.Now.ToString("yyyy年MM月dd日"));
                FileStream fs;
                msg = msg ?? "";
                lock (msg)
                {
                    lock (log)
                    {
                        if (File.Exists(log))
                        {
                            fs = new FileStream(log, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                            lock (fs)
                            {
                                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                                sw.WriteLine(String.Format("\r\n\r\n\r\n{0}:{1}\r\n\r\n\r\n", DateTime.Now.ToString("【yyyy-MM-dd HH:mm:ss:fff】"), msg));
                                sw.Close();
                                fs.Close();
                            }
                        }
                        else
                        {
                            fs = new FileStream(log, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                            lock (fs)
                            {
                                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                                sw.WriteLine(String.Format("\r\n\r\n\r\n{0}:{1}\r\n\r\n\r\n", DateTime.Now.ToString("【yyyy-MM-dd HH:mm:ss:fff】"), msg));
                                sw.Close();
                                fs.Close();
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 将一个异常添加到日志记录
        /// </summary>
        /// <param name="exception"></param>
        public void addToLog(Exception exception)
        {
            if (exception == null) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("====================EXCEPTION====================");
            sb.AppendLine("【异常信息】:" + exception.Message);
            sb.AppendLine("【错误源】:" + exception.Source);
            sb.AppendLine("【产生异常的方法】:" + (exception.TargetSite != null ? exception.TargetSite.Name : "无"));
            sb.AppendLine("【堆栈调用】:" + exception.StackTrace);
            sb.AppendLine("【异常】:" + exception + "");
            sb.AppendLine("=================================================");
            if (exception.InnerException != null)
            {
                sb.AppendLine("====================INNER EXCEPTION====================");
                sb.AppendLine("【异常信息】:" + exception.InnerException.Message);
                sb.AppendLine("【错误源】:" + exception.InnerException.Source);
                sb.AppendLine("【产生异常的方法】:" + (exception.InnerException.TargetSite != null ? exception.InnerException.TargetSite.Name : "无"));
                sb.AppendLine("【堆栈调用】:" + exception.InnerException.StackTrace);
                sb.AppendLine(("【异常】:" + exception.InnerException) ?? "");
                sb.AppendLine("=================================================");
            }
            addToLogHasBreak(sb.ToString());
        }
        /// <summary>
        /// 将一个异常添加到日志记录
        /// </summary>
        /// <param name="exception"></param>
        public void addToLog(Exception exception, string msg)
        {
            if (exception == null) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(msg);
            sb.AppendLine("====================EXCEPTION====================");
            sb.AppendLine("【异常信息】:" + exception.Message);
            sb.AppendLine("【错误源】:" + exception.Source);
            sb.AppendLine("【产生异常的方法】:" + (exception.TargetSite != null ? exception.TargetSite.Name : "无"));
            sb.AppendLine("【堆栈调用】:" + exception.StackTrace);
            sb.AppendLine("【异常】:" + exception + "");
            sb.AppendLine("=================================================");
            if (exception.InnerException != null)
            {
                sb.AppendLine("====================INNER EXCEPTION====================");
                sb.AppendLine("【异常信息】:" + exception.InnerException.Message);
                sb.AppendLine("【错误源】:" + exception.InnerException.Source);
                sb.AppendLine("【产生异常的方法】:" + (exception.InnerException.TargetSite != null ? exception.InnerException.TargetSite.Name : "无"));
                sb.AppendLine("【堆栈调用】:" + exception.InnerException.StackTrace);
                sb.AppendLine(("【异常】:" + exception.InnerException) ?? "");
                sb.AppendLine("=================================================");
            }
            addToLogHasBreak(sb.ToString());
        }
    }
}
