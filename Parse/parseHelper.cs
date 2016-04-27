using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Parse
{
    /// <summary>
    /// 数据类型安全转换
    /// </summary>
    public class parseHelper
    {
        /// <summary>
        /// 默认失败则返回-1
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int toInt(string num, int defaultValue = -1)
        {
            int temp = defaultValue;
            if (int.TryParse(num, out temp)) return temp;
            else return defaultValue;
        }
        /// <summary>
        /// 默认失败则返回-1
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static long toLong(string num, long defaultValue = -1l)
        {
            long temp = defaultValue;
            if (long.TryParse(num, out temp)) return temp;
            else return defaultValue;
        }
        /// <summary>
        ///默认失败则返回-1.0000d
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static double toDouble(string num, double defaultValue = -1.0000d)
        {
            double temp = defaultValue;
            if (double.TryParse(num, out temp)) return temp;
            else return defaultValue;
        }
        /// <summary>
        /// 默认失败则返回-1.000f
        /// </summary>
        /// <param name="floatString"></param>
        /// <returns></returns>
        public static float toFloat(string floatString, float defaultValue = -1.000f)
        {
            float temp = defaultValue;
            if (float.TryParse(floatString, out temp)) return temp;
            else return defaultValue;
        }
        /// <summary>
        /// 默认转换失败则返回Null
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? toDateTime(string date, DateTime? defaultValue = null)
        {
            DateTime temp;
            if (DateTime.TryParse(date, out temp)) return temp;
            return defaultValue;
        }
        /// <summary>
        /// 默认转换失败时反回此该起的100年前
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime toDateTimeLow(string date)
        {
            DateTime temp;
            if (DateTime.TryParse(date, out temp)) return temp;
            return DateTime.Now.AddYears(-100);
        }
        /// <summary>
        ///  默认转换失败时反回此该起的100年后
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime toDateTimeHigh(string date)
        {
            DateTime temp;
            if (DateTime.TryParse(date, out temp)) return temp;
            return DateTime.Now.AddYears(+100);
        }
        /// <summary>
        /// 对Bool类型进行转换，boolString为True的语句是："on","1","true","yes" 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool toBool(string boolString)
        {
            boolString = boolString + "";
            if (boolString.ToLower() == "on" || boolString.ToLower() == "1" || boolString.ToLower() == "true" || boolString.ToLower() == "yes") return true;
            else return false;
        }
        /// <summary>
        /// 将一个数字字符串，转为指定长度的数据字符串，比如：199转为4位长度为：0199,如果是12345转为2位，则为12
        /// </summary>
        /// <param name="num"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string toFixLength(string num, int length)
        {
            if (length - num.Length > 0)
                return new String('0', length - num.Length) + num;
            else
            {
                return num.Substring(0, length);
            }
        }
        /// <summary>
        /// 将一个字符串，转为指定长度的数据字符串，可以在前面加指定的字符
        /// </summary>
        /// <param name="num"></param>
        /// <param name="length"></param>
        /// <param name="padChar"></param>
        /// <returns></returns>
        public static string toFixLength(string num, int length, char padChar)
        {
            if (length - num.Length > 0)
                return new String(padChar, length - num.Length) + num;
            else
            {
                return num.Substring(0, length);
            }
        }
        /// <summary>
        /// 判断一个数字字符串是否是标准数字(包括正负int，double，decimal)。
        /// </summary>
        public static bool isNumber(string num)
        {
            Decimal dec;
            return Decimal.TryParse(num, out dec);
        }
        /// <summary>
        /// 将itemModel中的属性值替换sourceString中出现的“${属性名(区分大小写)}”的字符串。替换完成后返回。
        /// </summary>
        public static string toReplace(string sourceString, object itemModel, string datetimeFormat = "yyyy年MM月dd日")
        {
            sourceString = sourceString + "";//防止为 Null
            var props = itemModel.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.CanRead)//如果属性可读
                {
                    if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    {
                        var value = prop.GetValue(itemModel, null);
                        if (value is DateTime)
                        {
                            sourceString = sourceString.Replace("${" + prop.Name + "}", ((DateTime)value).ToString(datetimeFormat));
                        }
                        else if (value == null)
                        {
                            sourceString = sourceString.Replace("${" + prop.Name + "}", "");
                        }
                        else
                        {
                            sourceString = sourceString.Replace("${" + prop.Name + "}", ((DateTime?)value).Value.ToString(datetimeFormat));
                        }

                    }
                    else
                    {
                        sourceString = sourceString.Replace("${" + prop.Name + "}", prop.GetValue(itemModel, null) + "");
                    }

                }
            }
            return sourceString;
        }

        /// <summary>
        /// 将Html中的如#000这样的颜色转为Color对像。
        /// </summary>
        public static Color toColor(string htmlColor)
        {
            return ColorTranslator.FromHtml(htmlColor);
        }
        /// <summary>
        /// 将Color对像转为Html中的如#000这样的颜色。
        /// </summary>
        public static string toColor(Color color)
        {
            return ColorTranslator.ToHtml(color);
        }
        /// <summary>
        /// 将0-99999内的数字转为对应的汉字。
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string numberToChinese(int num)
        {
            string numStr = "0123456789";
            string chineseStr = "零一二三四五六七八九";
            if (num >= 0 && num <= 9) { return chineseStr[numStr.IndexOf(num.ToString()[0])].ToString(); }
            else if (num >= 10 && num <= 19) { if (num == 10) { return "十"; } return "十" + chineseStr[numStr.IndexOf(num.ToString()[1])].ToString(); }
            else if (num >= 20 && num <= 99)
            {
                if (num % 10 == 0) { return chineseStr[numStr.IndexOf(num.ToString()[0])].ToString() + "十"; }
                else return chineseStr[numStr.IndexOf(num.ToString()[0])].ToString() + "十" + chineseStr[numStr.IndexOf(num.ToString()[1])].ToString();
            }
            else if (num >= 100 && num <= 999)
            {
                if (num % 100 == 0) { return chineseStr[numStr.IndexOf(num.ToString()[0])].ToString() + "百"; }
                else return chineseStr[numStr.IndexOf(num.ToString()[0])].ToString() + "百" +
                  ((int.Parse(num.ToString().Substring(1).ToString()).ToString().Length != (num.ToString().Length - 1)) ?
                     "零" + numberToChinese(int.Parse(num.ToString().Substring(1)))
                    : numberToChinese(int.Parse(num.ToString().Substring(1))));
            }
            else if (num >= 1000 && num <= 9999)
            {
                if (num % 1000 == 0) { return chineseStr[numStr.IndexOf(num.ToString()[0])].ToString() + "千"; }
                else return
                     chineseStr[numStr.IndexOf(num.ToString()[0])].ToString() + "千" +
                  ((int.Parse(num.ToString().Substring(1).ToString()).ToString().Length != (num.ToString().Length - 1)) ?
                     "零" + numberToChinese(int.Parse(num.ToString().Substring(1)))
                    : numberToChinese(int.Parse(num.ToString().Substring(1))));
            }
            else if (num >= 10000 && num <= 99999)
            {
                if (num % 1000 == 0) { return chineseStr[numStr.IndexOf(num.ToString()[0])].ToString() + "万"; }
                else return
                     chineseStr[numStr.IndexOf(num.ToString()[0])].ToString() + "万" +
                  ((int.Parse(num.ToString().Substring(1).ToString()).ToString().Length != (num.ToString().Length - 1)) ?
                     "零" + numberToChinese(int.Parse(num.ToString().Substring(1)))
                    : numberToChinese(int.Parse(num.ToString().Substring(1))));
            }
            return num.ToString();
        }
    }
}
