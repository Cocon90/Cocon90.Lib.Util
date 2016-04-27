using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cocon90.Lib.Util.Controls.Msg
{
    /// <summary>
    /// 平常消息提示
    /// </summary>
    public class Toast
    {
        internal static ToastForm toastForm = new ToastForm();
        /// <summary>
        /// 消息显示 输入消息内容，显示时间，屏幕
        /// </summary>
        public static void Show(string text, int timeOut)
        {
            toastForm.BackColor = Color.DimGray;
            toastForm.Show(text, timeOut, Screen.FromControl(toastForm));
        }
        /// <summary>
        /// 消息显示 输入消息内容，显示时间，屏幕
        /// </summary>
        public static void Show(string text, int timeOut, Color backColor)
        {
            toastForm.BackColor = backColor;
            toastForm.Show(text, timeOut, Screen.FromControl(toastForm));
        }
        /// <summary>
        /// 消息显示 输入消息内容，屏幕
        /// </summary>
        public static void Show(string text)
        {
            toastForm.BackColor = Color.DimGray;
            toastForm.Show(text, Screen.FromControl(toastForm));
        }
        /// <summary>
        /// 消息显示 输入消息内容，屏幕
        /// </summary>
        public static void Show(string text, Color backColor)
        {
            toastForm.BackColor = backColor;
            toastForm.Show(text, Screen.FromControl(toastForm));
        }
        /// <summary>
        /// 关闭消息显示
        /// </summary>
        public static void Hide()
        {
            toastForm.Hide();
        }
    }
}
