using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Cocon90.Lib.Util.Window
{
    /// <summary>
    ///开机自动运行的辅助类
    /// </summary>
    public class RunManager
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="executablePath">启动了应用程序的可执行文件的路径，包括可执行文件的名称</param>
        public RunManager(string executablePath)
        {
            this.ExecutablePath = executablePath;
        }

        /// <summary>
        /// 获得或设置启动了应用程序的可执行文件的路径，包括可执行文件的名称
        /// </summary>
        public string ExecutablePath { get; set; }
        string RunStartupKey = @"Software\Microsoft\Windows\CurrentVersion\Run";

        /// <summary>
        /// 获取或设置程序是否开机启动
        /// </summary>
        public bool RunAtStartUp
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(RunStartupKey);
                string retStr = key.GetValue(ExecutablePath, string.Empty).ToString();
                return !(retStr == string.Empty);
            }
            set
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(RunStartupKey, true);
                if (value)
                {
                    key.SetValue(ExecutablePath, ExecutablePath);
                }
                else
                {
                    key.DeleteValue(ExecutablePath);
                }
            }

        }

    }
}
