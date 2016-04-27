using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Ini
{
    /// <summary>
    /// INI文件操作工具
    /// </summary>
    public class IniHelper
    {
        public IniHelper(string iniFilePath)
        {
            this.IniFilePath = iniFilePath;
            try
            {
                string dir = iniFilePath.Substring(0, iniFilePath.LastIndexOf('\\'));
                if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
            }
            catch { }
        }
        public string IniFilePath { get; set; }
        // 声明INI文件的写操作函数 WritePrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        // 声明INI文件的读操作函数 GetPrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 写入ini文件
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Write(string section, string key, string value)
        {
            // section=配置节，key=键名，value=键值，path=路径
            WritePrivateProfileString(section, key, value, IniFilePath);
        }
        /// <summary>
        /// 从INI中读取
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Read(string section, string key)
        {
            // 每次从ini中读取多少字节
            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);
            // section=配置节，key=键名，temp=上面，path=路径
            GetPrivateProfileString(section, key, "", temp, 255, IniFilePath);
            return temp.ToString();
        }

    }
}
