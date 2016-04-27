using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cocon90.Lib.Util.WndProc
{
    /// <summary>
    ///  在Form对像的protected override void WndProc(ref Message m)方法中，加入的方法，用来起到某些特殊作用。
    /// </summary>
    public class wndProcHelper
    {
        /// <summary>
        /// 当用户点击了关闭窗口时，自动执行最小化操作。除非结束进程才可关闭。
        /// </summary>
        /// <param name="from">要最小化的窗口</param>
        /// <param name="m">传入override void WndProc(ref Message m)中的m</param>
        /// <param name="action">最小化后要执行的其它操作。</param>
        public static void SetFormMinimizedOnClose(Form from, ref Message m, Action action = null)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;
            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_CLOSE)
            {
                // 屏蔽传入的消息事件 
                from.WindowState = FormWindowState.Minimized;
                if (action != null) { action(); }
                return;
            }
        }
        /// <summary>
        /// 当用户点击了关闭窗口或最小化窗口时，自动执行窗体的Hide方法。除非结束进程才可关闭
        /// </summary>
        /// <param name="from">要Hide的窗口</param>
        /// <param name="m">传入override void WndProc(ref Message m)中的m</param>
        /// <param name="action">最小化后要执行的其它操作。</param>
        public static void SetFormHideOnMinimizedOrClose(Form form, ref Message m, Action action = null)
        {

            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;
            const int SC_MINIMIZE = 0xF020;
            if (m.Msg == WM_SYSCOMMAND && ((int)m.WParam == SC_MINIMIZE || (int)m.WParam == SC_CLOSE))
            {
                //最小化到系统栏 
                if (action != null) { action(); }
                form.Hide();
                return;
            }
        }

    }
}
