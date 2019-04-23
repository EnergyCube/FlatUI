using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatNumeric : Control
    {
        private bool _bool;
        private int _h;
        private long _max;
        private long _min;
        private long _value;
        private int _w;
        private int _x;
        private int _y;
        public MouseState State = MouseState.None;

        public FlatNumeric()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 10);
            BackColor = Color.FromArgb(60, 70, 73);
            ForeColor = Color.White;
            _min = 0;
            _max = 9999999;
        }

        public long Value
        {
            get => _value;
            set
            {
                if ((value <= _max) & (value >= _min))
                    _value = value;
                Invalidate();
            }
        }

        public long Maximum
        {
            get => _max;
            set
            {
                if (value > _min)
                    _max = value;
                if (_value > _max)
                    _value = _max;
                Invalidate();
            }
        }

        public long Minimum
        {
            get => _min;
            set
            {
                if (value < _max)
                    _min = value;
                if (_value < _min)
                    _value = Minimum;
                Invalidate();
            }
        }

        [Category("Colors")] public Color BaseColor { get; set; } = Color.FromArgb(45, 47, 49);

        [Category("Colors")] public Color ButtonColor { get; set; } = Helpers.FlatColor;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            _x = e.Location.X;
            _y = e.Location.Y;
            Invalidate();
            Cursor = e.X < Width - 23 ? Cursors.IBeam : Cursors.Hand;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_x > Width - 21 && _x < Width - 3)
            {
                if (_y < 15)
                {
                    if (Value + 1 <= _max)
                        _value += 1;
                }
                else
                {
                    if (Value - 1 >= _min)
                        _value -= 1;
                }
            }
            else
            {
                _bool = !_bool;
                Focus();
            }

            Invalidate();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            try
            {
                if (_bool)
                    _value = Convert.ToInt64(_value + e.KeyChar.ToString());
                if (_value > _max)
                    _value = _max;
                Invalidate();
            }
            catch
            {
                // ignored
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Back) Value = 0;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 30;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            UpdateColors();

            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);
            _w = Width;
            _h = Height;

            var Base = new Rectangle(0, 0, _w, _h);

            var with18 = g;
            with18.SmoothingMode = SmoothingMode.HighQuality;
            with18.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with18.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with18.Clear(BackColor);

            //-- Base
            with18.FillRectangle(new SolidBrush(BaseColor), Base);
            with18.FillRectangle(new SolidBrush(ButtonColor), new Rectangle(Width - 24, 0, 24, _h));

            //-- Add
            with18.DrawString("+", new Font("Segoe UI", 12), Brushes.White, new Point(Width - 12, 8), Helpers.CenterSf);
            //-- Subtract
            with18.DrawString("-", new Font("Segoe UI", 10, FontStyle.Bold), Brushes.White, new Point(Width - 12, 22),
                Helpers.CenterSf);

            //-- Text
            with18.DrawString(Value.ToString(), Font, Brushes.White, new Rectangle(5, 1, _w, _h),
                new StringFormat {LineAlignment = StringAlignment.Center});

            base.OnPaint(e);
            g.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(b, 0, 0);
            b.Dispose();
        }

        private void UpdateColors()
        {
            var colors = Helpers.GetColors(this);
            ButtonColor = colors.Flat;
        }
    }
}