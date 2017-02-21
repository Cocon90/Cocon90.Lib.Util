﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class DatetimeExtension
    {
        /// <summary>
        /// 取得该日期的日初
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0);
        }
        /// <summary>
        /// 取得该日期的日末，精确到最后一毫秒
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0).AddDays(1).AddMilliseconds(-1);
        }
        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(double timeStamp)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(timeStamp);
            return time;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            //double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            var intResult = (time - startTime).TotalSeconds;
            //long t = (time.Ticks - startTime.Ticks) / 10000;            //除10000调整为13位
            return (long)intResult;
        }

    }
}
