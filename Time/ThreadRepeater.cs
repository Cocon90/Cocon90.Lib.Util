using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Cocon90.Lib.Util.Time
{
    /// <summary>
    /// 使用多线程循环重复执行某事。
    /// </summary>
    public class ThreadRepeater : Object
    {
        Action action = null;
        decimal intervalSecond = 60;
        decimal perCheckMillisecond = 10m;
        /// <summary>
        /// 构建一个自动多线程执行类，传入要执行的Action和间隔多长执行一次时间（秒）。
        /// </summary>
        /// <param name="action"></param>
        /// <param name="intervalSecond"></param>
        public ThreadRepeater(Action action, decimal intervalSecond, decimal perCheckMillisecond = 10m)
        {
            this.action = action;
            this.intervalSecond = intervalSecond;
            this.perCheckMillisecond = perCheckMillisecond;
            myThread = new Thread(new ThreadStart(Method));
            myThread.Name = "ThreadRepeater类对像中的线程";
        }
        Thread myThread = null;
        private void Method()
        {
            while (myThread.IsAlive)
            {
                if (action != null)
                { action(); }
                for (decimal i = 0; i < intervalSecond * 1000m / perCheckMillisecond; i++) //每多少秒检测一次
                {
                    if (!myThread.IsAlive)
                        break;
                    Thread.Sleep((int)perCheckMillisecond);
                }
            }
        }
        /// <summary>
        /// 开始执行
        /// </summary>
        public void Start()
        {
            if (myThread != null) { Stop(); }
            //自动处理线程
            myThread.Start();
        }
        /// <summary>
        /// 停止线程操作
        /// </summary>
        public void Stop()
        {
            if (myThread != null && myThread.IsAlive)
            {
                myThread.Abort();
                myThread.Join();
            }

        }

    }
}
