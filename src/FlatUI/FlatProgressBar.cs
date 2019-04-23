using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatProgressBar : Control
    {
        private readonly Color _baseColor = Color.FromArgb(45, 47, 49);
        private int _h;
        private int _maximum = 100;
        private int _value;
        private int _w;

        public FlatProgressBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            BackColor = Color.FromArgb(60, 70, 73);
            Height = 42;
        }

        [Category("Control")]
        public int Maximum
        {
            get => _maximum;
            set
            {
                if (value < _value)
                    _value = value;
                _maximum = value;
                Invalidate();
            }
        }

        [Category("Control")]
        public int Value
        {
            get => _value;
            set
            {
                if (value > _maximum)
                {
                    value = _maximum;
                    Invalidate();
                }

                _value = value;
                Invalidate();
            }
        }

        public bool Pattern { get; set; } = true;

        public bool ShowBalloon { get; set; } = true;

        public bool PercentSign { get; set; }

        [Category("Colors")] public Color ProgressColor { get; set; } = Helpers.FlatColor;

        [Category("Colors")] public Color DarkerProgress { get; set; } = Color.FromArgb(23, 148, 92);

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 42;
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            Height = 42;
        }

        public void Increment(int amount)
        {
            Value += amount;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            UpdateColors();

            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);
            _w = Width - 1;
            _h = Height - 1;

            var Base = new Rectangle(0, 24, _w, _h);
            var gp = new GraphicsPath();

            var with15 = g;
            with15.SmoothingMode = SmoothingMode.HighQuality;
            with15.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with15.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with15.Clear(BackColor);

            //-- Progress Value
            //int iValue = Convert.ToInt32(((float)_Value) / ((float)(_Maximum * Width)));
            var percent = _value / (float) _maximum;
            var iValue = (int) (percent * Width);

            switch (Value)
            {
                case 0:
                    //-- Base
                    with15.FillRectangle(new SolidBrush(_baseColor), Base);
                    //--Progress
                    with15.FillRectangle(new SolidBrush(ProgressColor), new Rectangle(0, 24, iValue - 1, _h - 1));
                    break;
                case 100:
                    //-- Base
                    with15.FillRectangle(new SolidBrush(_baseColor), Base);
                    //--Progress
                    with15.FillRectangle(new SolidBrush(ProgressColor), new Rectangle(0, 24, iValue - 1, _h - 1));
                    break;
                default:
                    //-- Base
                    with15.FillRectangle(new SolidBrush(_baseColor), Base);

                    //--Progress
                    gp.AddRectangle(new Rectangle(0, 24, iValue - 1, _h - 1));
                    with15.FillPath(new SolidBrush(ProgressColor), gp);

                    if (Pattern)
                    {
                        //-- Hatch Brush
                        var hb = new HatchBrush(HatchStyle.Plaid, DarkerProgress, ProgressColor);
                        with15.FillRectangle(hb, new Rectangle(0, 24, iValue - 1, _h - 1));
                    }

                    if (ShowBalloon)
                    {
                        //-- Balloon
                        var balloon = new Rectangle(iValue - 18, 0, 34, 16);
                        var gp2 = Helpers.RoundRec(balloon, 4);
                        with15.FillPath(new SolidBrush(_baseColor), gp2);

                        //-- Arrow
                        var gp3 = Helpers.DrawArrow(iValue - 9, 16, true);
                        with15.FillPath(new SolidBrush(_baseColor), gp3);

                        //-- Value > You can add "%" > value & "%"
                        var text = PercentSign ? Value + "%" : Value.ToString();
                        var wOffset = PercentSign ? iValue - 15 : iValue - 11;
                        with15.DrawString(text, new Font("Segoe UI", 10), new SolidBrush(ProgressColor),
                            new Rectangle(wOffset, -2, _w, _h), Helpers.NearSf);
                    }

                    break;
            }

            base.OnPaint(e);

            g.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(b, 0, 0);
            b.Dispose();
        }

        private void UpdateColors()
        {
            var colors = Helpers.GetColors(this);

            ProgressColor = colors.Flat;
        }
    }
}