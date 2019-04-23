using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatComboBox : ComboBox
    {
        private readonly Color _baseColor = Color.FromArgb(25, 27, 29);
        private readonly Color _bgColor = Color.FromArgb(45, 47, 49);
        private int _h;
        private int _w;

        public MouseState State = MouseState.None;

        public FlatComboBox()
        {
            DrawItem += DrawItem_;
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);

            DoubleBuffered = true;

            DrawMode = DrawMode.OwnerDrawFixed;
            BackColor = Color.FromArgb(45, 45, 48);
            ForeColor = Color.White;
            DropDownStyle = ComboBoxStyle.DropDownList;
            Cursor = Cursors.Hand;
            StartIndex = 0;
            ItemHeight = 18;
            Font = new Font("Segoe UI", 8, FontStyle.Regular);
        }

        [Category("Colors")] public Color HoverColor { get; set; } = Color.FromArgb(35, 168, 109);

        private int StartIndex
        {
            set
            {
                try
                {
                    SelectedIndex = value;
                }
                catch
                {
                    // ignored
                }

                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            State = MouseState.Down;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            State = MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            State = MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            State = MouseState.None;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Invalidate();
            Cursor = e.X < Width - 41 ? Cursors.IBeam : Cursors.Hand;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            Invalidate();
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Invalidate();
        }

        public void DrawItem_(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            e.DrawBackground();
            e.DrawFocusRectangle();

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            e.Graphics.FillRectangle(
                (e.State & DrawItemState.Selected) == DrawItemState.Selected
                    ? new SolidBrush(HoverColor)
                    : new SolidBrush(_baseColor), e.Bounds);

            //-- Text
            e.Graphics.DrawString(GetItemText(Items[e.Index]), new Font("Segoe UI", 8), Brushes.White,
                new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height));

            e.Graphics.Dispose();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 18;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);
            _w = Width;
            _h = Height;

            var Base = new Rectangle(0, 0, _w, _h);
            var button = new Rectangle(Convert.ToInt32(_w - 40), 0, _w, _h);
            var gp = new GraphicsPath();

            var with16 = g;
            with16.Clear(Color.FromArgb(45, 45, 48));
            with16.SmoothingMode = SmoothingMode.HighQuality;
            with16.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with16.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            //-- Base
            with16.FillRectangle(new SolidBrush(_bgColor), Base);

            //-- Button
            gp.Reset();
            gp.AddRectangle(button);
            with16.SetClip(gp);
            with16.FillRectangle(new SolidBrush(_baseColor), button);
            with16.ResetClip();

            //-- Lines
            with16.DrawLine(Pens.White, _w - 10, 6, _w - 30, 6);
            with16.DrawLine(Pens.White, _w - 10, 12, _w - 30, 12);
            with16.DrawLine(Pens.White, _w - 10, 18, _w - 30, 18);

            //-- Text
            with16.DrawString(Text, Font, Brushes.White, new Point(4, 6), Helpers.NearSf);

            g.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(b, 0, 0);
            b.Dispose();
        }
    }
}