using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace Cocon90.Lib.Util.Time
{
    /// <summary>
    /// 延时辅助执行类
    /// </summary>
    public class delayInvoker
    {
        /// <summary>
        /// 延时执行指定毫秒，后执行某事。（同一线程内，异步执行）
        /// </summary>
        /// <param name="delayMillisecond"></param>
        /// <param name="action"></param>
        public static void Invoke(int delayMillisecond, Action action)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (ss, ee) => { Thread.Sleep(delayMillisecond); };
            bw.RunWorkerCompleted += (ss, ee) => { action.Invoke(); };
            bw.RunWorkerAsync();
        }
    }

}
