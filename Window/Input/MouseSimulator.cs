using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Cocon90.Lib.Util.Window.Input
{
    /// <summary>
    /// 鼠标模拟器
    /// </summary>
    public static class MouseSimulator
    {
        private const int MOUSEEVENTF_MOVE = 1;
        private const int MOUSEEVENTF_LEFTDOWN = 2;
        private const int MOUSEEVENTF_LEFTUP = 4;
        private const int MOUSEEVENTF_RIGHTDOWN = 8;
        private const int MOUSEEVENTF_RIGHTUP = 16;
        private const int MOUSEEVENTF_MIDDLEDOWN = 32;
        private const int MOUSEEVENTF_MIDDLEUP = 64;
        private const int MOUSEEVENTF_WHEEL = 2048;
        private const int MOUSEEVENTF_ABSOLUTE = 32768;
        public static Point Position
        {
            get
            {
                return new Point(Cursor.Position.X, Cursor.Position.Y);
            }
            set
            {
                Cursor.Position = value;
            }
        }
        public static int X
        {
            get
            {
                return Cursor.Position.X;
            }
            set
            {
                Cursor.Position = new Point(value, MouseSimulator.Y);
            }
        }
        public static int Y
        {
            get
            {
                return Cursor.Position.Y;
            }
            set
            {
                Cursor.Position = new Point(MouseSimulator.X, value);
            }
        }
        [DllImport("user32.dll")]
        private static extern int ShowCursor(bool show);
        [DllImport("user32.dll")]
        private static extern void mouse_event(int flags, int dX, int dY, int buttons, int extraInfo);
        public static void MouseDown(MouseButton button)
        {
            MouseSimulator.mouse_event((int)button, 0, 0, 0, 0);
        }
        public static void MouseDown(MouseButtons button)
        {
            if (button != MouseButtons.Left)
            {
                if (button != MouseButtons.Right)
                {
                    if (button == MouseButtons.Middle)
                    {
                        MouseSimulator.MouseDown(MouseButton.Middle);
                    }
                }
                else
                {
                    MouseSimulator.MouseDown(MouseButton.Right);
                }
            }
            else
            {
                MouseSimulator.MouseDown(MouseButton.Left);
            }
        }
        public static void MouseUp(MouseButton button)
        {
            MouseSimulator.mouse_event((int)((int)(button) * (int)(MouseButton.Left)), 0, 0, 0, 0);
        }
        public static void MouseUp(MouseButtons button)
        {
            if (button != MouseButtons.Left)
            {
                if (button != MouseButtons.Right)
                {
                    if (button == MouseButtons.Middle)
                    {
                        MouseSimulator.MouseUp(MouseButton.Middle);
                    }
                }
                else
                {
                    MouseSimulator.MouseUp(MouseButton.Right);
                }
            }
            else
            {
                MouseSimulator.MouseUp(MouseButton.Left);
            }
        }
        public static void Click(MouseButton button)
        {
            MouseSimulator.MouseDown(button);
            MouseSimulator.MouseUp(button);
        }
        public static void Click(MouseButtons button)
        {
            if (button != MouseButtons.Left)
            {
                if (button != MouseButtons.Right)
                {
                    if (button == MouseButtons.Middle)
                    {
                        MouseSimulator.Click(MouseButton.Middle);
                    }
                }
                else
                {
                    MouseSimulator.Click(MouseButton.Right);
                }
            }
            else
            {
                MouseSimulator.Click(MouseButton.Left);
            }
        }
        public static void DoubleClick(MouseButton button)
        {
            MouseSimulator.Click(button);
            MouseSimulator.Click(button);
        }
        public static void DoubleClick(MouseButtons button)
        {
            if (button != MouseButtons.Left)
            {
                if (button != MouseButtons.Right)
                {
                    if (button == MouseButtons.Middle)
                    {
                        MouseSimulator.DoubleClick(MouseButton.Middle);
                    }
                }
                else
                {
                    MouseSimulator.DoubleClick(MouseButton.Right);
                }
            }
            else
            {
                MouseSimulator.DoubleClick(MouseButton.Left);
            }
        }
        public static void MouseWheel(int delta)
        {
            MouseSimulator.mouse_event(2048, 0, 0, delta, 0);
        }
        public static void Show()
        {
            MouseSimulator.ShowCursor(true);
        }
        public static void Hide()
        {
            MouseSimulator.ShowCursor(false);
        }
    }
}
