using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatColorPalette : Control
    {
        private int _h;
        private int _w;

        public FlatColorPalette()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            BackColor = Color.FromArgb(60, 70, 73);
            Size = new Size(160, 80);
            Font = new Font("Segoe UI", 12);
        }

        [Category("Colors")] public Color Red { get; set; } = Color.FromArgb(220, 85, 96);

        [Category("Colors")] public Color Cyan { get; set; } = Color.FromArgb(10, 154, 157);

        [Category("Colors")] public Color Blue { get; set; } = Color.FromArgb(0, 128, 255);

        [Category("Colors")] public Color LimeGreen { get; set; } = Color.FromArgb(35, 168, 109);

        [Category("Colors")] public Color Orange { get; set; } = Color.FromArgb(253, 181, 63);

        [Category("Colors")] public Color Purple { get; set; } = Color.FromArgb(155, 88, 181);

        [Category("Colors")] public Color Black { get; set; } = Color.FromArgb(45, 47, 49);

        [Category("Colors")] public Color Gray { get; set; } = Color.FromArgb(63, 70, 73);

        [Category("Colors")] public Color White { get; set; } = Color.FromArgb(243, 243, 243);

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Width = 180;
            Height = 80;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);
            _w = Width - 1;
            _h = Height - 1;

            var with6 = g;
            with6.SmoothingMode = SmoothingMode.HighQuality;
            with6.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with6.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with6.Clear(BackColor);

            //-- Colors 
            with6.FillRectangle(new SolidBrush(Red), new Rectangle(0, 0, 20, 40));
            with6.FillRectangle(new SolidBrush(Cyan), new Rectangle(20, 0, 20, 40));
            with6.FillRectangle(new SolidBrush(Blue), new Rectangle(40, 0, 20, 40));
            with6.FillRectangle(new SolidBrush(LimeGreen), new Rectangle(60, 0, 20, 40));
            with6.FillRectangle(new SolidBrush(Orange), new Rectangle(80, 0, 20, 40));
            with6.FillRectangle(new SolidBrush(Purple), new Rectangle(100, 0, 20, 40));
            with6.FillRectangle(new SolidBrush(Black), new Rectangle(120, 0, 20, 40));
            with6.FillRectangle(new SolidBrush(Gray), new Rectangle(140, 0, 20, 40));
            with6.FillRectangle(new SolidBrush(White), new Rectangle(160, 0, 20, 40));

            //-- Text
            with6.DrawString("Color Palette", Font, new SolidBrush(White), new Rectangle(0, 22, _w, _h),
                Helpers.CenterSf);

            base.OnPaint(e);
            g.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(b, 0, 0);
            b.Dispose();
        }
    }
}