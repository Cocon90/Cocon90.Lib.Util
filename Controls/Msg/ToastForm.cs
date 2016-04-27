using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;
namespace Cocon90.Lib.Util.Controls.Msg
{
	public class ToastForm : Form
	{
		private delegate void AnimationCallback();
		private delegate void CloseTimerElapsedCallback(object sender, ElapsedEventArgs e);
		public enum GWL
		{
			ExStyle = -20
		}
		public enum WS_EX
		{
			Transparent = 32,
			Layered = 524288
		}
		public enum LWA
		{
			ColorKey = 1,
			Alpha
		}
		private const int margin = 35;
		private const float _maxOpacity = 0.9f;
		private InvisibleLabel _textLabel;
		private string _text;
		private System.Timers.Timer _closeTimer;
		private AnimationManager _fadeManager;
		private bool _fadingIn;
		private bool _fadingOut;
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 128;
				return createParams;
			}
		}
        
		public ToastForm()
		{
			base.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.DimGray;
            //this.BackColor = Color.Red;
            this.Opacity = 0.0;
            this.TopMost = true;
            this.ShowInTaskbar = false;
			this._closeTimer = new System.Timers.Timer();
			this._closeTimer.AutoReset = false;
			this._closeTimer.Elapsed += new ElapsedEventHandler(this.CloseTimerElapsed);
			this._fadeManager = new AnimationManager(40);
			this._fadeManager.AnimationFinished += new AnimationManager.AnimationFinishedHandler(this.AnimationFinished);
			this._fadeManager.AnimationUpdated += new AnimationManager.AnimationUpdatedHandler(this.AnimationUpdated);

		}
		public void Show(string text, int timeOut, Screen screen)
		{
			this.Show(text, screen);
			this._closeTimer.Interval = (double)timeOut;
			this._closeTimer.Start();
		}
		public void Show(string text, Screen screen)
		{
			if (base.Visible && !this._fadingOut && text == this._text)
			{
				return;
			}
			this._text = text;
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			this._textLabel = new InvisibleLabel(text, new PointF(35f, 35f), InvisibleLabel.AnchorPoint.UpperLeft, stringFormat);
			this._textLabel.FontSize = 12f;
			if (this._fadingOut || !base.Visible)
			{
				this._fadingIn = true;
				this._fadingOut = false;
				this._fadeManager.Start();
				base.Opacity = 0.0;
				base.Show();
			}
			base.Invalidate();
			this.MoveToScreen(screen);
		}
		public new void Hide()
		{
			this.StartFadeOut();
		}
		private void MoveToScreen(Screen screen)
		{
			Rectangle bounds = screen.Bounds;
			int x = bounds.Left + (bounds.Width - base.ClientSize.Width) / 2;
			int y = bounds.Top + (bounds.Height - base.ClientSize.Height) / 2;
			base.Location = new Point(x, y);
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			SizeF size = this._textLabel.Bounds.Size;
			SolidBrush brush = new SolidBrush(Color.WhiteSmoke);
			this._textLabel.Draw(e.Graphics, brush);
			if (!size.Equals(this._textLabel.Bounds.Size))
			{
				base.ClientSize = new Size((int)this._textLabel.Bounds.Width + 70, (int)this._textLabel.Bounds.Height + 70);
				Rectangle bounds = Screen.FromControl(this).Bounds;
				int x = bounds.Left + (bounds.Width - base.ClientSize.Width) / 2;
				int y = bounds.Top + (bounds.Height - base.ClientSize.Height) / 2;
				base.Location = new Point(x, y);
				base.Invalidate();
			}
		}
		private void AnimationUpdated()
		{
			if (base.InvokeRequired)
			{
				ToastForm.AnimationCallback method = new ToastForm.AnimationCallback(this.AnimationUpdated);
				base.Invoke(method, new object[0]);
				return;
			}
			if (this._fadingIn)
			{
				base.Opacity = (double)(0.9f * (1f - this._fadeManager.AnimationFactor));
			}
			else
			{
				if (this._fadingOut)
				{
					base.Opacity = (double)(0.9f * this._fadeManager.AnimationFactor);
				}
			}
			base.Invalidate();
		}
		private void AnimationFinished()
		{
			if (this._fadingIn)
			{
				this._fadingIn = false;
				return;
			}
			if (this._fadingOut)
			{
				if (base.InvokeRequired)
				{
					ToastForm.AnimationCallback method = new ToastForm.AnimationCallback(this.AnimationFinished);
					base.Invoke(method, new object[0]);
					return;
				}
				base.Hide();
				this._fadingOut = false;
				this._text = null;
				base.Invalidate();
			}
		}
		private void CloseTimerElapsed(object sender, ElapsedEventArgs e)
		{
			if (base.InvokeRequired)
			{
				ToastForm.CloseTimerElapsedCallback method = new ToastForm.CloseTimerElapsedCallback(this.CloseTimerElapsed);
				base.Invoke(method, new object[]
				{
					sender,
					e
				});
				return;
			}
			this.StartFadeOut();
		}
		private void StartFadeOut()
		{
			this._fadingIn = false;
			this._fadingOut = true;
			this._fadeManager.Start();
		}
		[DllImport("user32.dll")]
		public static extern int GetWindowLong(IntPtr hWnd, ToastForm.GWL nIndex);
		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, ToastForm.GWL nIndex, int dwNewLong);
		[DllImport("user32.dll")]
		public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte alpha, ToastForm.LWA dwFlags);
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
            //int num = Toast.GetWindowLong(base.Handle, Toast.GWL.ExStyle);
            //num = (num | 524288 | 32);
            //Toast.SetWindowLong(base.Handle, Toast.GWL.ExStyle, num);
            //Toast.SetLayeredWindowAttributes(base.Handle, 0, 128, Toast.LWA.Alpha);
		}

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PopupTopmost
            // 
            this.ClientSize = new System.Drawing.Size(116, 12);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PopupTopmost";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }
	}
}
