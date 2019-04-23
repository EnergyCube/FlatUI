using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatStickyButton : Control
    {
        private int _h;
        private MouseState _state = MouseState.None;
        private int _w;

        public FlatStickyButton()
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

        private Rectangle Rect => new Rectangle(Left, Top, Width, Height);

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

        private bool[] GetConnectedSides()
        {
            var Bool = new[] {false, false, false, false};

            foreach (Control b in Parent.Controls)
            {
                if (!(b is FlatStickyButton)) continue;

                if (ReferenceEquals(b, this) || !Rect.IntersectsWith(Rect))
                    continue;

                var a = Math.Atan2(Left - b.Left, Top - b.Top) * 2 / Math.PI;

                if (Math.Abs(a / 1 - a) < 0.1)
                    Bool[(int) a + 1] = true;
            }

            return Bool;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            UpdateColors();

            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);
            _w = Width;
            _h = Height;

            GraphicsPath gp;

            var gcs = GetConnectedSides();
            // dynamic RoundedBase = Helpers.RoundRect(0, 0, W, H, ???, !(GCS(2) | GCS(1)), !(GCS(1) | GCS(0)), !(GCS(3) | GCS(0)), !(GCS(3) | GCS(2)));
            var roundedBase = Helpers.RoundRect(0, 0, _w, _h, 0.3, !(gcs[2] || gcs[1]), !(gcs[1] || gcs[0]),
                !(gcs[3] || gcs[0]), !(gcs[3] || gcs[2]));
            var Base = new Rectangle(0, 0, _w, _h);

            var with17 = g;
            with17.SmoothingMode = SmoothingMode.HighQuality;
            with17.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with17.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with17.Clear(BackColor);

            switch (_state)
            {
                case MouseState.None:
                    if (Rounded)
                    {
                        //-- Base
                        gp = roundedBase;
                        with17.FillPath(new SolidBrush(BaseColor), gp);

                        //-- Text
                        with17.DrawString(Text, Font, new SolidBrush(TextColor), Base, Helpers.CenterSf);
                    }
                    else
                    {
                        //-- Base
                        with17.FillRectangle(new SolidBrush(BaseColor), Base);

                        //-- Text
                        with17.DrawString(Text, Font, new SolidBrush(TextColor), Base, Helpers.CenterSf);
                    }

                    break;
                case MouseState.Over:
                    if (Rounded)
                    {
                        //-- Base
                        gp = roundedBase;
                        with17.FillPath(new SolidBrush(BaseColor), gp);
                        with17.FillPath(new SolidBrush(Color.FromArgb(20, Color.White)), gp);

                        //-- Text
                        with17.DrawString(Text, Font, new SolidBrush(TextColor), Base, Helpers.CenterSf);
                    }
                    else
                    {
                        //-- Base
                        with17.FillRectangle(new SolidBrush(BaseColor), Base);
                        with17.FillRectangle(new SolidBrush(Color.FromArgb(20, Color.White)), Base);

                        //-- Text
                        with17.DrawString(Text, Font, new SolidBrush(TextColor), Base, Helpers.CenterSf);
                    }

                    break;
                case MouseState.Down:
                    if (Rounded)
                    {
                        //-- Base
                        gp = roundedBase;
                        with17.FillPath(new SolidBrush(BaseColor), gp);
                        with17.FillPath(new SolidBrush(Color.FromArgb(20, Color.Black)), gp);

                        //-- Text
                        with17.DrawString(Text, Font, new SolidBrush(TextColor), Base, Helpers.CenterSf);
                    }
                    else
                    {
                        //-- Base
                        with17.FillRectangle(new SolidBrush(BaseColor), Base);
                        with17.FillRectangle(new SolidBrush(Color.FromArgb(20, Color.Black)), Base);

                        //-- Text
                        with17.DrawString(Text, Font, new SolidBrush(TextColor), Base, Helpers.CenterSf);
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