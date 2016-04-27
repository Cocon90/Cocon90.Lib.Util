using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Cocon90.Lib.Util.Window.Hook.Common
{
    /// <summary>
    /// 键盘钩子
    /// </summary>
    public class KeyboardHook : GlobalHook
    {
        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;
        public event KeyPressEventHandler KeyPress;
        /// <summary>
        /// 键盘钩子
        /// </summary>
        public KeyboardHook()
        {
            this._hookType = 13;
        }
        protected override int HookCallbackProcedure(int nCode, int wParam, IntPtr lParam)
        {
            bool flag = false;
            if (nCode > -1 && (this.KeyDown != null || this.KeyUp != null || this.KeyPress != null))
            {
                GlobalHook.KeyboardHookStruct keyboardHookStruct = (GlobalHook.KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(GlobalHook.KeyboardHookStruct));
                bool flag2 = (GlobalHook.GetKeyState(162) & 128) != 0 || (GlobalHook.GetKeyState(3) & 128) != 0;
                bool flag3 = (GlobalHook.GetKeyState(160) & 128) != 0 || (GlobalHook.GetKeyState(161) & 128) != 0;
                bool flag4 = (GlobalHook.GetKeyState(164) & 128) != 0 || (GlobalHook.GetKeyState(165) & 128) != 0;
                bool flag5 = GlobalHook.GetKeyState(20) != 0;
                KeyEventArgs keyEventArgs = new KeyEventArgs((Keys)(keyboardHookStruct.vkCode | (flag2 ? 131072 : 0) | (flag3 ? 65536 : 0) | (flag4 ? 262144 : 0)));
                switch (wParam)
                {
                    case 256:
                    case 260:
                        if (this.KeyDown != null)
                        {
                            this.KeyDown(this, keyEventArgs);
                            flag = (flag || keyEventArgs.Handled);
                        }
                        break;
                    case 257:
                    case 261:
                        if (this.KeyUp != null)
                        {
                            this.KeyUp(this, keyEventArgs);
                            flag = (flag || keyEventArgs.Handled);
                        }
                        break;
                }
                if (wParam == 256 && !flag && !keyEventArgs.SuppressKeyPress && this.KeyPress != null)
                {
                    byte[] array = new byte[256];
                    byte[] array2 = new byte[2];
                    GlobalHook.GetKeyboardState(array);
                    if (GlobalHook.ToAscii(keyboardHookStruct.vkCode, keyboardHookStruct.scanCode, array, array2, keyboardHookStruct.flags) == 1)
                    {
                        char c = (char)array2[0];
                        if ((flag5 ^ flag3) && char.IsLetter(c))
                        {
                            c = char.ToUpper(c);
                        }
                        KeyPressEventArgs e = new KeyPressEventArgs(c);
                        this.KeyPress(this, e);
                        flag = (flag || keyEventArgs.Handled);
                    }
                }
            }
            int result;
            if (flag)
            {
                result = 1;
            }
            else
            {
                result = GlobalHook.CallNextHookEx(this._handleToHook, nCode, wParam, lParam);
            }
            return result;
        }
    }
}
