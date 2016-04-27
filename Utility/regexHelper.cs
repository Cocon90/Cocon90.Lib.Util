using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cocon90.Lib.Util.Utility
{
    /// <summary>
    /// 正则表达式辅助类
    /// </summary>
    public class regexHelper
    {
        /// <summary>
        /// 正规则试验IP地址
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static bool checkIP(string IP)
        {
            string num = "(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)";
            return Regex.IsMatch(IP, ("^" + num + "\\." + num + "\\." + num + "\\." + num + "$"));
        }
        /// <summary>
        /// 判断一个字符串是否是NULL或者是空格组成的字符串或者是空字符串。
        /// </summary>
        /// <param name="oldString"></param>
        /// <returns></returns>
        public static bool isNullOrEmpty(string oldString)
        {
            return trimEmpty(oldString) == "";
        }
        /// <summary>
        /// 返回一个字符串对像的字符形式。如传入NULL则返回空字符串。
        /// </summary>
        /// <param name="oldString"></param>
        /// <returns></returns>
        public static string trimEmpty(string oldString)
        {
            return (oldString + "").Trim();
        }
    }
}
