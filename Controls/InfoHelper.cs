using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocon90.Lib.Util.Controls
{
    public class InfoHelper
    {
        public static void Warn(string text)
        {
            System.Windows.Forms.MessageBox.Show(text, "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
        }

        public static bool Confirm(string text)
        {
            return System.Windows.Forms.MessageBox.Show(text, "确认", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK;
        }

        public static void Error(string text)
        {
            System.Windows.Forms.MessageBox.Show(text, "出错", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        }

        public static void Info(string text)
        {
            System.Windows.Forms.MessageBox.Show(text, "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }
    }
}
