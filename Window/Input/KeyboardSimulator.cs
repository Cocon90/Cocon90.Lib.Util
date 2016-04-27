
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Cocon90.Lib.Util.Window.Input
{
    /// <summary>
    /// 键盘模拟器
    /// </summary>
	public static class KeyboardSimulator
	{
		private const int KEYEVENTF_EXTENDEDKEY = 1;
		private const int KEYEVENTF_KEYUP = 2;
		[DllImport("user32.dll")]
		private static extern void keybd_event(byte key, byte scan, int flags, int extraInfo);
		public static void KeyDown(Keys key)
		{
			KeyboardSimulator.keybd_event(KeyboardSimulator.ParseKey(key), 0, 0, 0);
		}
		public static void KeyUp(Keys key)
		{
			KeyboardSimulator.keybd_event(KeyboardSimulator.ParseKey(key), 0, 2, 0);
		}
		public static void KeyPress(Keys key)
		{
			KeyboardSimulator.KeyDown(key);
			KeyboardSimulator.KeyUp(key);
		}
		public static void SimulateStandardShortcut(StandardShortcut shortcut)
		{
			switch (shortcut)
			{
			case StandardShortcut.Copy:
				KeyboardSimulator.KeyDown(Keys.Control);
				KeyboardSimulator.KeyPress(Keys.C);
				KeyboardSimulator.KeyUp(Keys.Control);
				break;
			case StandardShortcut.Cut:
				KeyboardSimulator.KeyDown(Keys.Control);
				KeyboardSimulator.KeyPress(Keys.X);
				KeyboardSimulator.KeyUp(Keys.Control);
				break;
			case StandardShortcut.Paste:
				KeyboardSimulator.KeyDown(Keys.Control);
				KeyboardSimulator.KeyPress(Keys.V);
				KeyboardSimulator.KeyUp(Keys.Control);
				break;
			case StandardShortcut.SelectAll:
				KeyboardSimulator.KeyDown(Keys.Control);
				KeyboardSimulator.KeyPress(Keys.A);
				KeyboardSimulator.KeyUp(Keys.Control);
				break;
			case StandardShortcut.Save:
				KeyboardSimulator.KeyDown(Keys.Control);
				KeyboardSimulator.KeyPress(Keys.S);
				KeyboardSimulator.KeyUp(Keys.Control);
				break;
			case StandardShortcut.Open:
				KeyboardSimulator.KeyDown(Keys.Control);
				KeyboardSimulator.KeyPress(Keys.O);
				KeyboardSimulator.KeyUp(Keys.Control);
				break;
			case StandardShortcut.New:
				KeyboardSimulator.KeyDown(Keys.Control);
				KeyboardSimulator.KeyPress(Keys.N);
				KeyboardSimulator.KeyUp(Keys.Control);
				break;
			case StandardShortcut.Close:
				KeyboardSimulator.KeyDown(Keys.Alt);
				KeyboardSimulator.KeyPress(Keys.F4);
				KeyboardSimulator.KeyUp(Keys.Alt);
				break;
			case StandardShortcut.Print:
				KeyboardSimulator.KeyDown(Keys.Control);
				KeyboardSimulator.KeyPress(Keys.P);
				KeyboardSimulator.KeyUp(Keys.Control);
				break;
			}
		}
		private static byte ParseKey(Keys key)
		{
			byte result;
			if (key != Keys.Shift)
			{
				if (key != Keys.Control)
				{
					if (key != Keys.Alt)
					{
						result = (byte)key;
					}
					else
					{
						result = 18;
					}
				}
				else
				{
					result = 17;
				}
			}
			else
			{
				result = 16;
			}
			return result;
		}
	}
}
