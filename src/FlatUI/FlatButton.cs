using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatButton : Control
    {
        private int _h;

        private MouseState _state = MouseState.None;
        private int _w;

        public FlatButton()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            Size = new Size(106, 32);
            BackColor = Color.Transparent;
            Font = new Font("Segoe UI", 12);
            Cursor = Cursors.Hand;
        }

        [Category("Colors")] public Color BaseColor { get; set; } = Helpers.FlatColor;

        [Category("Colors")] public Color TextColor { get; set; } = Color.FromArgb(243, 243, 243);

        [Category("Options")] public bool Rounded { get; set; }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _state = MouseState.Down;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _state = MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _state = MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _state = MouseState.None;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            UpdateColors();

            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);
            _w = Width - 1;
            _h = Height - 1;

            GraphicsPath gp;
            var Base = new Rectangle(0, 0, _w, _h);

            var with8 = g;
            with8.SmoothingMode = SmoothingMode.HighQuality;
            with8.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with8.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with8.Clear(BackColor);

            switch (_state)
            {
                case MouseState.None:
                    if (Rounded)
                    {
                        //-- Base
                        gp = Helpers.RoundRec(Base, 6);
                        with8.FillPath(new SolidBrush(BaseColor), gp);

                        //-- Text
                        with8.DrawString(Text, Font, new SolidBrush(TextColor), Base, Helpers.CenterSf);
                    }
                    else
                    {
                        //-- Base
                        with8.FillRectangle(new SolidBrush(BaseColor), Base);

                        //-- Text
                        with8.DrawString(Text, Font, new SolidBrush(TextColor), Base, Helpers.CenterSf);
                    }

                    break;
                case MouseState.Over:
                    if (Rounded)
                    {
                        //-- Base
                        gp = Helpers.RoundRec(Base, 6);
                        with8.FillPath(new SolidBrush(BaseColor), gp);
                        with8.FillPath(new SolidBrush(Color.FromArgb(20, Color.White)), gp);

                        //-- Text
                        with8.DrawString(Text, Font, new SolidBrush(TextColor), Base, Helpers.CenterSf);
                    }
                    else
                    {
                        //-- Base
                        with8.FillRectangle(new SolidBrush(BaseColor), Base);
                        with8.FillRectangle(new SolidBrush(Color.FromArgb(20, Color.White)), Base);

                        //-- Text
                        with8.DrawString(Text, Font, new SolidBrush(TextColor), Base, Helpers.CenterSf);
                    }

                    break;
                case MouseState.Down:
                    if (Rounded)
                    {
                        //-- Base
                        gp = Helpers.RoundRec(Base, 6);
                        with8.FillPath(new SolidBrush(BaseColor), gp);
                        with8.FillPath(new SolidBrush(Color.FromArgb(20, Color.Black)), gp);

                        //-- Text
                        with8.DrawString(Text, Font, new SolidBrush(TextColor), Base, Helpers.CenterSf);
                    }
                    else
                    {
                        //-- Base
                        with8.FillRectangle(new SolidBrush(BaseColor), Base);
                        with8.FillRectangle(new SolidBrush(Color.FromArgb(20, Color.Black)), Base);

                        //-- Text
                        with8.DrawString(Text, Font, new SolidBrush(TextColor), Base, Helpers.CenterSf);
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
            BaseColor = colors.Flat;
        }
    }
}