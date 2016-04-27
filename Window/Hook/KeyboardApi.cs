using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;

namespace Cocon90.Lib.Util.Window.Hook
{
    /// <summary>
    /// 键盘操作辅助类（使用键盘钩子）
    /// </summary>  
    public class KeyboardApi
    {
        /// <summary>
        /// 键盘事件的委托
        /// </summary>
        /// <param name="keyEvent"></param>
        /// <param name="key"></param>
        public delegate void KeyboardEventHandler(KeyboardEvents keyEvent, System.Windows.Forms.Keys key);
        /// <summary>
        /// 键盘事件
        /// </summary>
        public event KeyboardEventHandler KeyboardEvent;
        enum HookType
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14,
            WH_MSGFILTER = -1,
        }
        private delegate IntPtr HookProc(int code, int wParam, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc hook, IntPtr instance, int threadID);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CallNextHookEx(IntPtr hookHandle, int code, int wParam, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern bool UnhookWindowsHookEx(IntPtr hookHandle);

        private IntPtr instance;
        private IntPtr hookHandle;
        private int threadID;
        private HookProc hookProcEx;
        /// <summary>
        /// 实倒化一个键盘钩子
        /// </summary>
        public KeyboardApi()
        {
            this.instance = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]); this.threadID = 0;
            hookHandle = IntPtr.Zero;
            hookProcEx = hookProc;
            //SetHook();
        }
        /// <summary>
        /// 安装钩子 返回是否安装成功
        /// </summary>
        /// <returns></returns>
        public bool SetHook()
        {
            this.hookHandle = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, hookProcEx, this.instance, this.threadID);
            return ((int)hookHandle != 0);
        }
        /// <summary>
        /// 卸载钩子 返回是否卸载成功
        /// </summary>
        /// <returns></returns>
        public bool UnHook()
        {
            return KeyboardApi.UnhookWindowsHookEx(this.hookHandle);
        }
        private IntPtr hookProc(int code, int wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                KeyboardEvents kEvent = (KeyboardEvents)wParam;
                if (kEvent != KeyboardEvents.KeyDown &&
                     kEvent != KeyboardEvents.KeyUp &&
                     kEvent != KeyboardEvents.SystemKeyDown &&
                     kEvent != KeyboardEvents.SystemKeyUp)
                {
                    return CallNextHookEx(this.hookHandle, (int)HookType.WH_KEYBOARD_LL, wParam, lParam);
                }
                KeyboardHookStruct MyKey = new KeyboardHookStruct();
                Type t = MyKey.GetType(); MyKey = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, t);
                Keys keyData = (Keys)MyKey.vkCode;
                KeyboardEvent(kEvent, keyData);
            }
            return CallNextHookEx(this.hookHandle, (int)HookType.WH_KEYBOARD_LL, wParam, lParam);
        }
        /// <summary>
        /// 键盘按键动作枚举
        /// </summary>
        public enum KeyboardEvents
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SystemKeyDown = 0x0104,
            SystemKeyUp = 0x0105
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

    }

}

