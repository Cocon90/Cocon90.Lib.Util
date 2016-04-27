using System;
using System.Runtime.CompilerServices;
using System.Timers;
namespace Cocon90.Lib.Util.Controls.Msg
{
	internal class AnimationManager
	{
		public delegate void AnimationUpdatedHandler();
		public delegate void AnimationFinishedHandler();
		private const float _animationDecreaseFactor = 0.07f;
		private Timer _animationTimer;
		private int _animationUpdateInterval;
		private float _animationFactor;
		private bool _running;
        public event AnimationManager.AnimationUpdatedHandler AnimationUpdated;
        public event AnimationManager.AnimationFinishedHandler AnimationFinished;
		public bool Running
		{
			get
			{
				return this._running;
			}
		}
		public float AnimationFactor
		{
			get
			{
				return this._animationFactor;
			}
		}
		public AnimationManager(int animationUpdateInterval)
		{
			this._animationUpdateInterval = animationUpdateInterval;
			this._animationTimer = new Timer((double)this._animationUpdateInterval);
			this._animationTimer.Elapsed += new ElapsedEventHandler(this.AnimationTimerElapsed);
			this._animationTimer.AutoReset = false;
			this._animationFactor = 1f;
		}
		public void Start()
		{
			this._animationFactor = 1f;
			this._animationTimer.Start();
			this._animationTimer.Interval = (double)this._animationUpdateInterval;
			this._running = true;
		}
		public void Stop()
		{
			this._animationTimer.Stop();
			this._animationFactor = 1f;
			this._running = false;
		}
		private void AnimationTimerElapsed(object sender, ElapsedEventArgs e)
		{
			this._animationFactor -= 0.07f;
			if (this._animationFactor > 0f)
			{
				this._animationTimer.Interval = (double)this._animationUpdateInterval;
				this.NotifyUpdateListeners();
				return;
			}
			this.Stop();
			this.NotifyFinishedListeners();
		}
		private void NotifyUpdateListeners()
		{
			if (this.AnimationUpdated != null)
			{
				this.AnimationUpdated();
			}
		}
		private void NotifyFinishedListeners()
		{
			if (this.AnimationFinished != null)
			{
				this.AnimationFinished();
			}
		}
	}
}
