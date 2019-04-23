using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatMax : Control
    {
        private MouseState _state = MouseState.None;

        public FlatMax()
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

            var parentForm = FindForm();

            if (parentForm == null) return;

            switch (parentForm.WindowState)
            {
                case FormWindowState.Maximized:
                    parentForm.WindowState = FormWindowState.Normal;
                    break;
                case FormWindowState.Normal:
                    parentForm.WindowState = FormWindowState.Maximized;
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

            var with4 = g;
            with4.SmoothingMode = SmoothingMode.HighQuality;
            with4.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with4.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with4.Clear(BackColor);

            //-- Base
            with4.FillRectangle(new SolidBrush(BaseColor), Base);

            var parent = FindForm();

            if (parent == null) return;

            switch (parent.WindowState)
            {
                //-- Maximize
                case FormWindowState.Maximized:
                    with4.DrawString("1", Font, new SolidBrush(TextColor), new Rectangle(1, 1, Width, Height),
                        Helpers.CenterSf);
                    break;
                case FormWindowState.Normal:
                    with4.DrawString("2", Font, new SolidBrush(TextColor), new Rectangle(1, 1, Width, Height),
                        Helpers.CenterSf);
                    break;
            }

            //-- Hover/down
            switch (_state)
            {
                case MouseState.Over:
                    with4.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.White)), Base);
                    break;
                case MouseState.Down:
                    with4.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Black)), Base);
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