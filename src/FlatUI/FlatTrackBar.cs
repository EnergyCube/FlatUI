using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    [DefaultEvent("Scroll")]
    public sealed class FlatTrackBar : Control
    {
        public delegate void ScrollEventHandler(object sender);

        [Flags]
        public enum ElementStyle
        {
            Slider,
            Knob
        }

        private readonly Color _baseColor = Color.FromArgb(45, 47, 49);
        private readonly Color _sliderColor = Color.FromArgb(25, 27, 29);
        private bool _bool;
        private int _h;
        private Rectangle _knob;

        private int _maximum = 10;

        private int _minimum;
        private Rectangle _track;
        private int _val;

        private int _value;
        private int _w;

        public FlatTrackBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);

            DoubleBuffered = true;
            Height = 18;
            BackColor = Color.FromArgb(60, 70, 73);
        }

        public ElementStyle Style { get; set; }

        [Category("Colors")] public Color TrackColor { get; set; } = Helpers.FlatColor;

        [Category("Colors")] public Color HatchColor { get; set; } = Color.FromArgb(23, 148, 92);

        public int Minimum
        {
            get => _minimum;

            set
            {
                if (value < 0) return;

                _minimum = value;

                if (value > _value)
                    _value = value;

                if (value > _maximum)
                    _maximum = value;

                Invalidate();
            }
        }

        public int Maximum
        {
            get => _maximum;
            set
            {
                if (value < 0) return;

                _maximum = value;

                if (value < _value)
                    _value = value;

                if (value < _minimum)
                    _minimum = value;

                Invalidate();
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                if (value == _value)
                    return;

                if (value > _maximum || value < _minimum) return;

                _value = value;
                Invalidate();
                Scroll?.Invoke(this);
            }
        }

        public bool ShowValue { get; set; }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                _val = Convert.ToInt32((_value - _minimum) / (float) (_maximum - _minimum) * (Width - 11));
                _track = new Rectangle(_val, 0, 10, 20);

                _bool = _track.Contains(e.Location);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_bool && e.X > -1 && e.X < Width + 1)
                Value = _minimum + Convert.ToInt32((_maximum - _minimum) * (e.X / (float) Width));
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _bool = false;
        }

        public event ScrollEventHandler Scroll;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Subtract when Value == 0:
                    return;
                case Keys.Subtract:
                    Value -= 1;
                    break;
                case Keys.Add when Value == _maximum:
                    return;
                case Keys.Add:
                    Value += 1;
                    break;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 23;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            UpdateColors();

            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);
            _w = Width - 1;
            _h = Height - 1;

            var Base = new Rectangle(1, 6, _w - 2, 8);
            var gp = new GraphicsPath();
            var gp2 = new GraphicsPath();

            var with20 = g;
            with20.SmoothingMode = SmoothingMode.HighQuality;
            with20.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with20.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with20.Clear(BackColor);

            //-- Value
            _val = Convert.ToInt32((_value - _minimum) / (float) (_maximum - _minimum) * (_w - 10));
            _track = new Rectangle(_val, 0, 10, 20);
            _knob = new Rectangle(_val, 4, 11, 14);

            //-- Base
            gp.AddRectangle(Base);
            with20.SetClip(gp);
            with20.FillRectangle(new SolidBrush(_baseColor), new Rectangle(0, 7, _w, 8));
            with20.FillRectangle(new SolidBrush(TrackColor), new Rectangle(0, 7, _track.X + _track.Width, 8));
            with20.ResetClip();

            //-- Hatch Brush
            var hb = new HatchBrush(HatchStyle.Plaid, HatchColor, TrackColor);
            with20.FillRectangle(hb, new Rectangle(-10, 7, _track.X + _track.Width, 8));

            //-- Slider/Knob
            switch (Style)
            {
                case ElementStyle.Slider:
                    gp2.AddRectangle(_track);
                    with20.FillPath(new SolidBrush(_sliderColor), gp2);
                    break;
                case ElementStyle.Knob:
                    gp2.AddEllipse(_knob);
                    with20.FillPath(new SolidBrush(_sliderColor), gp2);
                    break;
            }

            //-- Show the value 
            if (ShowValue)
                with20.DrawString(Value.ToString(), new Font("Segoe UI", 8), Brushes.White, new Rectangle(1, 6, _w, _h),
                    new StringFormat
                    {
                        Alignment = StringAlignment.Far,
                        LineAlignment = StringAlignment.Far
                    });

            base.OnPaint(e);
            g.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(b, 0, 0);
            b.Dispose();
        }

        private void UpdateColors()
        {
            var colors = Helpers.GetColors(this);

            TrackColor = colors.Flat;
        }
    }
}