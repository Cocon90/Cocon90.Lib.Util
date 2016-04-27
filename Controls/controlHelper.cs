using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cocon90.Lib.Util.Controls
{
    /// <summary>
    /// 控件常用功能辅助类
    /// </summary>
    public class controlHelper
    {
        /// <summary>
        /// 给控件  绑定获取焦点时的变色事件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="alertColor"></param>
        public static void BindFocusEventColor(Control control, Color alertColor)
        {
            if (control == null) return;
            Color color = control.BackColor;
            control.GotFocus += (s, e) => { color = control.BackColor; control.BackColor = alertColor; };
            control.LostFocus += (s, e) => { control.BackColor = color; };
        }
        /// <summary>
        ///给控件的子控件 绑定获取焦点时的变色事件
        /// </summary>
        /// <param name="groupControl"></param>
        /// <param name="alertColor"></param>
        public static void BindFocusChildrenEventColor(Control groupControl, Color alertColor)
        {
            if (groupControl == null) return;
            foreach (Control control in groupControl.Controls)
            {
                BindFocusEventColor(control, alertColor);
            }
        }
        /// <summary>
        /// 在指定控件显示指定信息！内部调用了Alerter.infoControl()方法。
        /// </summary>
        /// <param name="control"></param>
        /// <param name="info"></param>
        public static void infoControl(Control control, string info = null)
        {
            Alerter.infoControl(control, info);
        }
        /// <summary>
        /// 为控件绑带支持鼠标拖动事件。
        /// </summary>
        /// <param name="control"></param>
        public static void BindMouseDragEvent(Control control)
        {
            int ox = 0, oy = 0;
            control.MouseDown += (ss, ee) =>
            {
                ox = ee.X;
                oy = ee.Y;
            };
            control.MouseMove += (ss, ee) =>
            {
                if (ee.Button == MouseButtons.Left)
                {
                    control.Location = new Point(control.Location.X + ee.X - ox, control.Location.Y + ee.Y - oy);
                }
            };
        }
        /// <summary>
        /// 为控件绑带支持鼠标拖动事件。拖动control的时候，moveTarget会执行移动。
        /// </summary>
        /// <param name="control"></param>
        public static void BindMouseDragEvent(Control control, Control moveTarget)
        {
            int ox = 0, oy = 0;
            control.MouseDown += (ss, ee) =>
            {
                ox = ee.X;
                oy = ee.Y;
            };
            control.MouseMove += (ss, ee) =>
            {
                if (ee.Button == MouseButtons.Left)
                {
                    moveTarget.Location = new Point(moveTarget.Location.X + ee.X - ox, moveTarget.Location.Y + ee.Y - oy);
                }
            };
        }
    }
}
