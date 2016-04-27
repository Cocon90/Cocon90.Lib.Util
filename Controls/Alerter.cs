using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cocon90.Lib.Util.Time;

namespace Cocon90.Lib.Util.Controls
{
    /// <summary>
    /// 表示一个消息提示控件
    /// </summary>
    public partial class Alerter : TextBox
    {
        /// <summary>
        /// 表示一个消息提示控件
        /// </summary>
        public Alerter()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 显示消息 自动在前面加上“消息提示：”
        /// </summary>
        /// <param name="info"></param>
        /// <param name="isWarn"></param>
        public void info(string infoMsg, bool isWarn = false)
        {
            info(infoMsg, "消息提示", isWarn);
        }
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="isWarn"></param>
        public void info(string info, string title, bool isWarn = false)
        {
            this.Text = (title + "" == "") ? info : (title + "：" + info);
            var bk = this.BackColor;
            if (isWarn)
            {
                intervalInvoker.Invoke(80, 12, false, (count) =>
                {
                    if (this.BackColor == Color.Red)
                    { this.BackColor = bk; this.ForeColor = Color.Red; }
                    else { this.BackColor = Color.Red; this.ForeColor = Color.Yellow; }
                });
            }
            else
            {
                intervalInvoker.Invoke(80, 12, false, (count) =>
                {
                    if (this.BackColor == Color.Yellow)
                    { this.BackColor = bk; this.ForeColor = Color.Green; }
                    else { this.BackColor = Color.Yellow; this.ForeColor = Color.Black; }
                });

            }
            this.ForeColor = bk;
        }
        /// <summary>
        /// 在指定控件显示指定信息！
        /// </summary>
        /// <param name="control"></param>
        /// <param name="info"></param>
        public static void infoControl(Control control)
        {
            infoControl(control, "");
        }
        /// <summary>
        /// 在指定控件显示指定信息！
        /// </summary>
        /// <param name="control"></param>
        /// <param name="info"></param>
        public static void infoControl(Control control, Control senderControl)
        {
            infoControl(control, "", senderControl);
        }

        /// <summary>
        /// 在指定控件显示指定信息！
        /// </summary>
        /// <param name="control"></param>
        /// <param name="info"></param>
        public static void infoControl(Control control, string info)
        {
            infoControl(control, info, true, null);
        }
        /// <summary>
        /// 在指定控件显示指定信息！
        /// </summary>
        public static void infoControl(Control control, string info, Control senderControl)
        {
            infoControl(control, info, true, senderControl);
        }
        /// <summary>
        /// 在指定控件显示指定信息！
        /// </summary>
        public static void infoControl(Control control, string info, bool isWarn, Control senderControl)
        {
            var srcTxt = control.Text;
            control.Text = "";
            var fc = control.ForeColor;
            var bc = control.BackColor;
            if (senderControl != null)
            {
                senderControl.Enabled = false;
            }
            intervalInvoker.Invoke(100, 12, false, (count) =>
            {
                if (control.ForeColor == fc)
                {
                    if (isWarn)
                    {
                        control.ForeColor = Color.Yellow;
                        control.BackColor = Color.Red;
                    }
                    else
                    {
                        control.ForeColor = Color.Red;
                        control.BackColor = Color.Yellow;
                    }
                    if ((info + "").Trim() != "") { control.Text = info; }
                }
                else
                {
                    control.ForeColor = fc;
                    control.BackColor = bc;
                    control.Text = "";

                }
            }, () =>
            {
                control.Text = srcTxt;
                if (senderControl != null)
                {
                    senderControl.Enabled = true;
                }
            });
        }
        /// <summary>
        /// 在指定控件显示指定信息！
        /// </summary>
        public static void infoControl(Control control, string info, bool isWarn)
        {
            infoControl(control, info, isWarn, null);
        }
    }
}
