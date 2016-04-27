 
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Cocon90.Lib.Util.Window.Hook.Common
{
    /// <summary>
    /// 鼠标钩子
    /// </summary>
	public class MouseHook : GlobalHook
	{
		private enum MouseEventType
		{
			None,
			MouseDown,
			MouseUp,
			DoubleClick,
			MouseWheel,
			MouseMove
		}
		public event MouseEventHandler MouseDown;
		public event MouseEventHandler MouseUp;
		public event MouseEventHandler MouseMove;
		public event MouseEventHandler MouseWheel;
		public event EventHandler Click;
		public event EventHandler DoubleClick;
        /// <summary>
        /// 鼠标钩子
        /// </summary>
		public MouseHook()
		{
			this._hookType = 14;
		}
		protected override int HookCallbackProcedure(int nCode, int wParam, IntPtr lParam)
		{
			if (nCode > -1 && (this.MouseDown != null || this.MouseUp != null || this.MouseMove != null))
			{
				GlobalHook.MouseLLHookStruct mouseLLHookStruct = (GlobalHook.MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(GlobalHook.MouseLLHookStruct));
				MouseButtons button = this.GetButton(wParam);
				MouseHook.MouseEventType mouseEventType = this.GetEventType(wParam);
				MouseEventArgs e = new MouseEventArgs(button, (mouseEventType == MouseHook.MouseEventType.DoubleClick) ? 2 : 1, mouseLLHookStruct.pt.x, mouseLLHookStruct.pt.y, (int)((mouseEventType == MouseHook.MouseEventType.MouseWheel) ? ((short)(mouseLLHookStruct.mouseData >> 16 & 65535)) : 0));
				if (button == MouseButtons.Right && mouseLLHookStruct.flags != 0)
				{
					mouseEventType = MouseHook.MouseEventType.None;
				}
				switch (mouseEventType)
				{
				case MouseHook.MouseEventType.MouseDown:
					if (this.MouseDown != null)
					{
						this.MouseDown(this, e);
					}
					break;
				case MouseHook.MouseEventType.MouseUp:
					if (this.Click != null)
					{
						this.Click(this, new EventArgs());
					}
					if (this.MouseUp != null)
					{
						this.MouseUp(this, e);
					}
					break;
				case MouseHook.MouseEventType.DoubleClick:
					if (this.DoubleClick != null)
					{
						this.DoubleClick(this, new EventArgs());
					}
					break;
				case MouseHook.MouseEventType.MouseWheel:
					if (this.MouseWheel != null)
					{
						this.MouseWheel(this, e);
					}
					break;
				case MouseHook.MouseEventType.MouseMove:
					if (this.MouseMove != null)
					{
						this.MouseMove(this, e);
					}
					break;
				}
			}
			return GlobalHook.CallNextHookEx(this._handleToHook, nCode, wParam, lParam);
		}
		private MouseButtons GetButton(int wParam)
		{
			MouseButtons result;
			switch (wParam)
			{
			case 513:
			case 514:
			case 515:
				result = MouseButtons.Left;
				break;
			case 516:
			case 517:
			case 518:
				result = MouseButtons.Right;
				break;
			case 519:
			case 520:
			case 521:
				result = MouseButtons.Middle;
				break;
			default:
				result = MouseButtons.None;
				break;
			}
			return result;
		}
		private MouseHook.MouseEventType GetEventType(int wParam)
		{
			MouseHook.MouseEventType result;
			switch (wParam)
			{
			case 512:
				result = MouseHook.MouseEventType.MouseMove;
				break;
			case 513:
			case 516:
			case 519:
				result = MouseHook.MouseEventType.MouseDown;
				break;
			case 514:
			case 517:
			case 520:
				result = MouseHook.MouseEventType.MouseUp;
				break;
			case 515:
			case 518:
			case 521:
				result = MouseHook.MouseEventType.DoubleClick;
				break;
			case 522:
				result = MouseHook.MouseEventType.MouseWheel;
				break;
			default:
				result = MouseHook.MouseEventType.None;
				break;
			}
			return result;
		}
	}
}
