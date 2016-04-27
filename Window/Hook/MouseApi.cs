using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Cocon90.Lib.Util.Window.Hook
{
    /// <summary>
    /// 使用WindowAPI来操控鼠标，如果要移动鼠标，请使用WinForm中的Cursor.Position=new Point(X,Y);
    /// </summary>
    public class MouseApi
    {
        [DllImport("User32.dll")]
        static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);
        [Flags]
        enum MouseEventFlag : uint
        {
            Move = 0x001, LeftDown = 0x0002, LeftUP = 0x0004, RightDown = 0x0008,
            RightUp = 0x0010, MiddleDown = 0x0020, MiddleUP = 0x0040, Absolut = 0x8000, xDown = 0x0080, xUp = 0x0100, wheel = 0x0800, virtualDesk = 0x4000
        }
        public void LeftClick()
        {
            mouse_event(MouseEventFlag.LeftUP, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.LeftUP, 0, 0, 0, UIntPtr.Zero);
        }
        public void RightClick()
        {
            mouse_event(MouseEventFlag.RightUp, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.RightDown, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.RightUp, 0, 0, 0, UIntPtr.Zero);
        }
        public void MiddleClick()
        {
            mouse_event(MouseEventFlag.MiddleUP, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.MiddleDown, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.MiddleUP, 0, 0, 0, UIntPtr.Zero);
        }

    }
}
