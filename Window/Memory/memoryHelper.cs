using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Cocon90.Lib.Util.Window
{
    /// <summary>
    /// 内存助手
    /// </summary>
    public class memoryHelper
    {
        /// <summary>
        /// 设置操作系统实际划分给进程使用的内存容量,减少内存的使用率,在程序最小化时 立即释放内存
        /// </summary>
        /// <param name="process">当前运行程序的进程</param>
        /// <param name="minSize">分配最小内存</param>
        /// <param name="maxSize">分配最大内存</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 设置操作系统实际划分给进程使用的内存容量,减少内存的使用率,在程序最小化时 立即释放内存
        /// </summary>
        public static void FlushMemory()
        {
            GC.Collect(); GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
        }
    }
}
