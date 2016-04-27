using System;
using System.Drawing;
namespace Cocon90.Lib.Util.Controls.Msg
{
    internal class InvisibleLabel
    {
        public enum AnchorPoint
        {
            UpperLeft,
            UpperRight,
            LowerRight,
            LowerLeft,
            Centre
        }
        protected RectangleF _bounds;
        protected PointF _position;
        protected InvisibleLabel.AnchorPoint _anchor;
        protected string _label;
        protected StringFormat _formatting;
        private Font _font = new Font(FontFamily.GenericMonospace, 12f);
        public RectangleF Bounds
        {
            get
            {
                return this._bounds;
            }
        }
        public float FontSize
        {
            get
            {
                return this._font.Size;
            }
            set
            {
                //this._font = new Font(FontFamily.GenericMonospace, value);
                this._font = new Font("ËÎÌו", value, FontStyle.Bold);
                this._bounds = RectangleF.Empty;
            }
        }
        public InvisibleLabel(string label, PointF position, InvisibleLabel.AnchorPoint anchor)
        {
            this._label = label;
            this._position = position;
            this._anchor = anchor;
            this._bounds = RectangleF.Empty;
            this._formatting = new StringFormat();
        }
        public InvisibleLabel(string label, PointF position, InvisibleLabel.AnchorPoint anchor, StringFormat formatting)
            : this(label, position, anchor)
        {
            this._formatting = formatting;
        }
        public void Draw(Graphics g)
        {
            this.Draw(g, Brushes.Black);
        }
        public void Draw(Graphics g, Brush brush)
        {
            if (this._bounds == RectangleF.Empty)
            {
                this.CreateBounds(g);
            }
            g.DrawString(this._label, this._font, brush, this._bounds, this._formatting);
        }
        public bool Contains(PointF p)
        {
            return this._bounds != RectangleF.Empty && this._bounds.Contains(p);
        }
        private void CreateBounds(Graphics g)
        {
            PointF pointF = PointF.Empty;
            SizeF size = g.MeasureString(this._label, this._font);
            switch (this._anchor)
            {
                case InvisibleLabel.AnchorPoint.UpperLeft:
                    pointF = this._position;
                    break;
                case InvisibleLabel.AnchorPoint.UpperRight:
                    pointF = new PointF(this._position.X - size.Width, this._position.Y);
                    break;
                case InvisibleLabel.AnchorPoint.LowerRight:
                    pointF = new PointF(this._position.X - size.Width, this._position.Y - size.Height);
                    break;
                case InvisibleLabel.AnchorPoint.LowerLeft:
                    pointF = new PointF(this._position.X, this._position.Y - size.Height);
                    break;
                case InvisibleLabel.AnchorPoint.Centre:
                    pointF = new PointF(this._position.X - size.Width / 2f, this._position.Y - size.Height / 2f);
                    break;
            }
            if (pointF != PointF.Empty)
            {
                this._bounds = new RectangleF(pointF, size);
            }
        }
    }
}
