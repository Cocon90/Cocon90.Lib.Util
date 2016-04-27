using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Cocon90.Lib.Util.Animate
{
    /// <summary>
    /// 动画类型
    /// </summary>
    public enum AnimateType
    {
        /// <summary>
        /// 从左到右展示控件
        /// </summary>
        AW_HOR_POSITIVE = 0x00000001,
        /// <summary>
        /// 从右到左展示控件
        /// </summary>
        AW_HOR_NEGATIVE = 0x00000002,
        /// <summary>
        /// 从上到下展示控件
        /// </summary>
        AW_VER_POSITIVE = 0x00000004,
        /// <summary>
        /// 从下到上展示控件
        /// </summary>
        AW_VER_NEGATIVE = 0x00000008,
        /// <summary>
        /// 从中间展示控件
        /// </summary>
        AW_CENTER = 0x00000010,
        /// <summary>
        /// 隐藏控件
        /// </summary>
        AW_HIDE = 0x00010000,
        /// <summary>
        /// 激活控件
        /// </summary>
        AW_ACTIVATE = 0x00020000,
        /// <summary>
        /// 使用滑动样式展示控件
        /// </summary>
        AW_SLIDE = 0x00040000,
        /// <summary>
        /// 使用谈出样式展示控件
        /// </summary>
        AW_BLEND = 0x00080000
    }
    /// <summary>
    /// 让控件执行动画的类
    /// </summary>
    public class animateHelper
    {

        public const Int32 AW_HOR_POSITIVE = 0x00000001; // 从左到右打开窗口
        public const Int32 AW_HOR_NEGATIVE = 0x00000002; // 从右到左打开窗口
        public const Int32 AW_VER_POSITIVE = 0x00000004; // 从上到下打开窗口
        public const Int32 AW_VER_NEGATIVE = 0x00000008; // 从下到上打开窗口
        public const Int32 AW_CENTER = 0x00000010; //若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展。
        public const Int32 AW_HIDE = 0x00010000; //隐藏窗口，缺省则显示窗口。
        public const Int32 AW_ACTIVATE = 0x00020000; //激活窗口。在使用了AW_HIDE标志后不要使用这个标志。
        public const Int32 AW_SLIDE = 0x00040000; //使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略。
        public const Int32 AW_BLEND = 0x00080000; //使用淡出效果。只有当hWnd为顶层窗口的时候才可以使用此标志。

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool AnimateWindow(
        IntPtr hwnd, // handle to window 
        int dwTime, // duration of animation 
        int dwFlags // animation type 
        );
        /// <summary>
        /// 执行控件的打开动画
        /// </summary>
        /// <param name="form"></param>
        public static void SetOpenAnimateMode(System.Windows.Forms.Control targetControl, AnimateType animateType = AnimateType.AW_ACTIVATE)
        {
            try
            {
                AnimateWindow(targetControl.Handle, Convert.ToInt32(500), (int)animateType);

            }
            catch
            {
            }
        }
        /// <summary>
        /// 执行控件的隐藏动画
        /// </summary>
        /// <param name="form"></param>
        public static void SetCloseAnimateMode(System.Windows.Forms.Control targetControl, AnimateType animateType = AnimateType.AW_HIDE)
        {
            try
            {
                AnimateWindow(targetControl.Handle, Convert.ToInt32(500), (int)(AnimateType.AW_HIDE | animateType));
            }
            catch
            {
            }
        }
    }
}
