using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Window.Regedit
{
    /// <summary>
    /// 注册表 辅助操作类
    /// </summary>
    public class regeditHelper
    {
        /// <summary>
        /// 取得路径path下（如：Software\\MyCompany\\MySoft ）指定Key 的字符串值。没有时，返回DefaultValue
        /// </summary>
        public static string GetLocalMachineValueByPath(string path, string key, string defaultValue)
        {
            try
            {
                var regedit = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(path);
                var value = regedit.GetValue(key, defaultValue) + "";
                regedit.Close();
                return value;
            }
            catch (Exception ex) { return defaultValue; }
        }
        /// <summary>
        /// 设置路径path下（如：Software\\MyCompany\\MySoft ）指定Key 的字符串值 ，成功则返回True，如果有异常，则返回False
        /// </summary>
        public static bool SetLocalMachineValueByPath(string path, string key, string value)
        {
            try
            {
                var regedit = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(path);
                regedit.SetValue(key, value, Microsoft.Win32.RegistryValueKind.String);
                regedit.Close();
                return true;
            }
            catch (Exception ex) { return false; }
        }


    }
}
