using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Cocon90.Lib.Util.Time
{
    /// <summary>
    /// 线程池， 允许指定数量的线程在池中运行。
    /// </summary>
    public class threadPoolHelper
    {
        /// <summary>
        /// 设置线程池最大请求数目
        /// </summary>
        /// <param name="maxThreadCount"></param>
        /// <returns></returns>
        public static bool SetPoolMaxCount(int maxThreadCount)
        {
            if (maxThreadCount >= 0)
            {
                ThreadPool.SetMaxThreads(maxThreadCount, maxThreadCount);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置线程池中工作的线程数
        /// </summary>
        /// <param name="minThreadCount"></param>
        /// <returns></returns>
        public static bool SetPoolMinCount(int minThreadCount)
        {
            if (minThreadCount >= 0)
            {
                ThreadPool.SetMinThreads(minThreadCount, minThreadCount);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 取得线程池最大请求数目
        /// </summary>
        /// <returns></returns>
        public static int GetPoolMaxCount()
        {
            int temp = 0, asyncTemp = 0;
            ThreadPool.GetMaxThreads(out temp, out asyncTemp);
            return temp;
        }
        /// <summary>
        /// 取得线程池中工作的线程数
        /// </summary>
        /// <returns></returns>
        public static int GetPoolMinCount()
        {
            int temp = 0, asyncTemp = 0;
            ThreadPool.GetMinThreads(out temp, out asyncTemp);
            return temp;
        }
        /// <summary>
        /// 取得线程池中可用的线程数
        /// </summary>
        /// <returns></returns>
        public static int GetAvailableCount()
        {
            int temp = 0, asyncTemp = 0;
            ThreadPool.GetAvailableThreads(out temp, out asyncTemp);
            return temp;
        }
        /// <summary>
        /// 将处理任务加入到线程池中执行。
        /// </summary>
        /// <param name="waitCallback"></param>
        /// <param name="objArgs"></param>
        public static void DoInvoke(WaitCallback waitCallback, object objArgs)
        {
            ThreadPool.QueueUserWorkItem(waitCallback, objArgs);
        }
    }
}
