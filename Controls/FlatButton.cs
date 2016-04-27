using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cocon90.Lib.Util.Controls
{
    public class FlatButton : Button
    {
        public FlatButton()
        {
            base.FlatStyle = FlatStyle.Flat;
            base.Cursor = Cursors.Hand;
            base.FlatAppearance.BorderColor = Color.Red;
            base.FlatAppearance.MouseDownBackColor = Color.FromArgb(255,192,0,0);
            base.FlatAppearance.MouseOverBackColor = Color.Yellow;
        }
    }
}
