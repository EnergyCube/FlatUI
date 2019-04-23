using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatMini : Control
    {
        private MouseState _state = MouseState.None;

        public FlatMini()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            BackColor = Color.White;
            Size = new Size(18, 18);
            Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Font = new Font("Marlett", 12);
        }

        [Category("Colors")] public Color BaseColor { get; set; } = Color.FromArgb(45, 47, 49);

        [Category("Colors")] public Color TextColor { get; set; } = Color.FromArgb(243, 243, 243);

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _state = MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _state = MouseState.Down;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _state = MouseState.None;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _state = MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            var parent = FindForm();

            if (parent == null) return;

            switch (parent.WindowState)
            {
                case FormWindowState.Normal:
                    parent.WindowState = FormWindowState.Minimized;
                    break;
                case FormWindowState.Maximized:
                    parent.WindowState = FormWindowState.Minimized;
                    break;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Size = new Size(18, 18);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);

            var Base = new Rectangle(0, 0, Width, Height);

            var with5 = g;
            with5.SmoothingMode = SmoothingMode.HighQuality;
            with5.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with5.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with5.Clear(BackColor);

            //-- Base
            with5.FillRectangle(new SolidBrush(BaseColor), Base);

            //-- Minimize
            with5.DrawString("0", Font, new SolidBrush(TextColor), new Rectangle(2, 1, Width, Height),
                Helpers.CenterSf);

            //-- Hover/down
            switch (_state)
            {
                case MouseState.Over:
                    with5.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.White)), Base);
                    break;
                case MouseState.Down:
                    with5.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Black)), Base);
                    break;
            }

            base.OnPaint(e);
            g.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(b, 0, 0);
            b.Dispose();
        }
    }
}