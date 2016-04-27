using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Cocon90.Lib.Util.Window.API
{
    /// <summary>
    /// Window常用Api收集辅助
    /// </summary>
    public class apiHelper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint GetCurrentProcessId();
        #region 获取windows桌面背景
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern int SystemParametersInfo(int uAction, int uParam, StringBuilder lpvParam, int fuWinIni);
        private const int SPI_GETDESKWALLPAPER = 0x0073;
        #endregion
        /// <summary>
        /// 获取当前Windows的桌面壁纸
        /// </summary>
        /// <returns></returns>
        public static string GetWindowWallpaper()
        {
            //定义存储缓冲区大小
            StringBuilder s = new StringBuilder(3000);
            //获取Window 桌面背景图片地址，使用缓冲区
            SystemParametersInfo(SPI_GETDESKWALLPAPER, 3000, s, 0);
            //缓冲区中字符进行转换
            string wallpaper_path = s.ToString(); //系统桌面背景图片路径
            return wallpaper_path;
        }


    }
}
