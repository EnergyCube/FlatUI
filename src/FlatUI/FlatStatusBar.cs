using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatStatusBar : Control
    {
        private int _h;
        private int _w;

        public FlatStatusBar()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 8);
            ForeColor = Color.White;
            Size = new Size(Width, 20);
        }

        [Category("Colors")] public Color BaseColor { get; set; } = Color.FromArgb(45, 47, 49);

        [Category("Colors")] public Color TextColor { get; set; } = Color.White;

        [Category("Colors")] public Color RectColor { get; set; } = Helpers.FlatColor;

        public bool ShowTimeDate { get; set; }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            Dock = DockStyle.Bottom;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        public string GetTimeDate()
        {
            return DateTime.Now.Date + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            UpdateColors();

            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);
            _w = Width;
            _h = Height;

            var Base = new Rectangle(0, 0, _w, _h);

            var with21 = g;
            with21.SmoothingMode = SmoothingMode.HighQuality;
            with21.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with21.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with21.Clear(BaseColor);

            //-- Base
            with21.FillRectangle(new SolidBrush(BaseColor), Base);

            //-- Text
            with21.DrawString(Text, Font, Brushes.White, new Rectangle(10, 4, _w, _h), Helpers.NearSf);

            //-- Rectangle
            with21.FillRectangle(new SolidBrush(RectColor), new Rectangle(4, 4, 4, 14));

            //-- TimeDate
            if (ShowTimeDate)
                with21.DrawString(GetTimeDate(), Font, new SolidBrush(TextColor), new Rectangle(-4, 2, _w, _h),
                    new StringFormat
                    {
                        Alignment = StringAlignment.Far,
                        LineAlignment = StringAlignment.Center
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
            RectColor = colors.Flat;
        }
    }
}