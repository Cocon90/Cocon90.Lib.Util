using Cocon90.Lib.Util.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Server
{
    /// <summary>
    /// 定时执行服务，需要子类实现 
    /// </summary>
    public abstract class BaseIntervalServer : IBaseServer
    {
        /// <summary>
        /// 获取或设置 间隔多称时间执行一次DoSomeThing方法。
        /// </summary>
        public long IntervalMisseconds { get; set; }
        /// <summary>
        /// 间隔多少毫秒执行一次
        /// </summary>
        /// <param name="intervalMisseconds"></param>
        public BaseIntervalServer(long intervalMisseconds = 1000)
        {
            this.IntervalMisseconds = intervalMisseconds;
        }
        /// <summary>
        /// 间隔多少毫秒执行一次，和检测的最小单位
        /// </summary>
        public BaseIntervalServer(long intervalMisseconds, long perSleepMissecond)
        {
            this.IntervalMisseconds = intervalMisseconds;
            this.PerSleepMissecond = perSleepMissecond;
        }
        /// <summary>
        /// 多长时间检测一次是否停止。
        /// </summary>
        public long PerSleepMissecond { get { return 10; } set { perSleepMissecond = value; } }
        long perSleepMissecond = 10;
        /// <summary>
        /// 指示当前服务是否正在运行
        /// </summary>
        public bool IsRunning { get; set; }
        /// <summary>
        /// 指示当前服务的开始执行日期。
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 指示当前服务调用了多少次DoSomeThing方法。
        /// </summary>
        public long Count { get; set; }
        /// <summary>
        /// 启动服务。
        /// </summary>
        public virtual void Start()
        {
            this.StartTime = DateTime.Now;
            this.IsRunning = true;
            bgWorker.runAsync((run) =>
            {
                while (this.IsRunning)
                {
                    DoSomeThing(StartTime, Count);
                    Count++;
                    if (this.IsRunning)
                    {
                        for (int i = 0; i < this.IntervalMisseconds / PerSleepMissecond; i++)
                        {
                            if (this.IsRunning)
                            {
                                System.Threading.Thread.Sleep((int)PerSleepMissecond);
                            }
                            else break;
                        }
                        if (!this.IsRunning) break;
                    }
                    else { break; }
                }

            }, fin =>
            {
                StartTime = null;
                this.Count = 0;
            });
        }
        /// <summary>
        /// 要不断调用的方法。
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="count"></param>
        public abstract void DoSomeThing(DateTime? startTime, long count);
        /// <summary>
        /// 停止服务
        /// </summary>
        public virtual void Stop()
        {
            this.Count = 0;
            this.IsRunning = false;
            this.StartTime = null;
        }
    }
}
