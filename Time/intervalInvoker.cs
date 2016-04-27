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
    /// 用来重复执行某事
    /// </summary>
    public class intervalInvoker
    {
        /// <summary>
        /// 委托，表示要执行的事件。传出当前执行次数。
        /// </summary>
        /// <param name="currentCount"></param>
        public delegate void DoWork(int currentCount);
        /// <summary>
        /// 重复每隔interval毫秒执行一次action委托，执行count次后，执行finishAction委托。isActionStart指定是否本方法刚调用就执行一次。
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="count"></param>
        /// <param name="isActionStart"></param>
        /// <param name="action"></param>
        /// <param name="finishAction"></param>
        public static void Invoke(int interval, int count, bool isActionStart, DoWork action, Action finishAction = null)
        {
            if (action == null || count <= 0) return;
            int HasCount = 0;
            if (isActionStart) { HasCount++; action(HasCount); if (HasCount >= count) { if (finishAction != null) { finishAction(); } return; } }
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (ss, ee) => { Thread.Sleep(interval); };
            bw.RunWorkerCompleted += (ss, ee) => { HasCount++; action(HasCount); if (HasCount >= count) { if (finishAction != null) { finishAction(); } return; } else { bw.RunWorkerAsync(); } };
            bw.RunWorkerAsync();
        }

    }
    ///// <summary>
    ///// 用来重复执行某事
    ///// </summary>
    //public class IntervalInvoker
    //{
    //    private static Timer commontimer = null;
    //    /// <summary>
    //    ///             获取或设置 每次执行后间隔多长时间再执行（调用Invoke方法前配置此属性才生效）
    //    /// </summary>
    //    public int Interval
    //    {
    //        get;
    //        set;
    //    }
    //    /// <summary>
    //    /// 获取或设置 总执行次数
    //    /// </summary>
    //    public int Count
    //    {
    //        get;
    //        set;
    //    }
    //    public int CurrentCount
    //    {
    //        get;
    //        set;
    //    }
    //    /// <summary>
    //    /// 创建一个新实例，如果想无限次数执行下去，请把count参数传为int.MaxValue
    //    /// </summary>
    //    /// <param name="interval"></param>
    //    /// <returns></returns>
    //    public static IntervalInvoker CreateInstence(int interval, int count)
    //    {
    //        IntervalInvoker.commontimer = new Timer((double)interval);
    //        return new IntervalInvoker(interval, count);
    //    }
    //    /// <summary>
    //    /// 定义多少毫秒的延时，以及执行次数
    //    /// </summary>
    //    /// <param name="interval"></param>
    //    private IntervalInvoker(int interval, int count)
    //    {
    //        this.Interval = interval;
    //        this.Count = count;
    //    }
    //    /// <summary>
    //    /// 间隔Interval毫秒后开始执行某事，第二参指定是否在调用Invoke的瞬间就执行一次action（默认为false）
    //    /// </summary>
    //    /// <param name="action"></param>
    //    public void Invoke(Action action, bool isActionStart = false, Action finishAction = null)
    //    {
    //        this.CurrentCount = 0;
    //        if (isActionStart)
    //        {
    //            action();
    //            this.CurrentCount++;
    //        }
    //        Timer th = new Timer((double)this.Interval);
    //        th.Elapsed += delegate(object s, ElapsedEventArgs e)
    //        {
    //            this.CurrentCount++;
    //            if (this.Count <= this.CurrentCount)
    //            {
    //                if (this.Count != 2147483647)
    //                {
    //                    th.Stop();
    //                    if (finishAction != null)
    //                    {
    //                        finishAction();
    //                    }
    //                }
    //            }
    //            action();
    //        };
    //        th.Start();
    //    }
    //    /// <summary>
    //    /// 间隔Interval毫秒后开始执行某事，第二参指定是否在调用Invoke的瞬间就执行一次action（默认为false）
    //    /// </summary>
    //    /// <param name="action"></param>
    //    public void InvokeByOneTimer(Action action, bool isActionStart = false, Action finishAction = null)
    //    {
    //        this.CurrentCount = 0;
    //        if (isActionStart)
    //        {
    //            action();
    //            this.CurrentCount++;
    //        }
    //        IntervalInvoker.commontimer.Elapsed += delegate(object s, ElapsedEventArgs e)
    //        {
    //            this.CurrentCount++;
    //            if (this.Count <= this.CurrentCount)
    //            {
    //                if (this.Count != 2147483647)
    //                {
    //                    IntervalInvoker.commontimer.Stop();
    //                    if (finishAction != null)
    //                    {
    //                        finishAction();
    //                    }
    //                }
    //            }
    //            action();
    //        };
    //        IntervalInvoker.commontimer.Start();
    //    }
    //}
}
