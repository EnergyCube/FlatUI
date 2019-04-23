using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    [DefaultEvent("CheckedChanged")]
    public sealed class FlatToggle : Control
    {
        public delegate void CheckedChangedEventHandler(object sender);

        [Flags]
        public enum StyleOptions
        {
            Style1,
            Style2,
            Style3
        }

        private readonly Color _baseColorRed = Color.FromArgb(220, 85, 96);
        private readonly Color _bgColor = Color.FromArgb(84, 85, 86);
        private readonly Color _textColor = Color.FromArgb(243, 243, 243);
        private readonly Color _toggleColor = Color.FromArgb(45, 47, 49);

        private Color _baseColor = Helpers.FlatColor;
        private int _h;
        private int _w;
        public MouseState State = MouseState.None;

        public FlatToggle()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Size = new Size(44, Height + 1);
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 10);
            Size = new Size(76, 33);
        }

        [Category("Options")] public StyleOptions Options { get; set; }

        [Category("Options")] public bool Checked { get; set; }

        public event CheckedChangedEventHandler CheckedChanged;

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Width = 76;
            Height = 33;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            State = MouseState.Over;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            State = MouseState.Down;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            State = MouseState.None;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            State = MouseState.Over;
            Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Checked = !Checked;
            CheckedChanged?.Invoke(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            UpdateColors();

            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);
            _w = Width - 1;
            _h = Height - 1;

            GraphicsPath gp;
            var gp2 = new GraphicsPath();
            var Base = new Rectangle(0, 0, _w, _h);
            var toggle = new Rectangle(Convert.ToInt32(_w / 2), 0, 38, _h);

            var with9 = g;
            with9.SmoothingMode = SmoothingMode.HighQuality;
            with9.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with9.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with9.Clear(BackColor);

            switch (Options)
            {
                case StyleOptions.Style1:
                    //-- Style 1
                    //-- Base
                    gp = Helpers.RoundRec(Base, 6);
                    gp2 = Helpers.RoundRec(toggle, 6);
                    with9.FillPath(new SolidBrush(_bgColor), gp);
                    with9.FillPath(new SolidBrush(_toggleColor), gp2);

                    //-- Text
                    with9.DrawString("OFF", Font, new SolidBrush(_bgColor), new Rectangle(19, 1, _w, _h),
                        Helpers.CenterSf);

                    if (Checked)
                    {
                        //-- Base
                        gp = Helpers.RoundRec(Base, 6);
                        gp2 = Helpers.RoundRec(new Rectangle(Convert.ToInt32(_w / 2), 0, 38, _h), 6);
                        with9.FillPath(new SolidBrush(_toggleColor), gp);
                        with9.FillPath(new SolidBrush(_baseColor), gp2);

                        //-- Text
                        with9.DrawString("ON", Font, new SolidBrush(_baseColor), new Rectangle(8, 7, _w, _h),
                            Helpers.NearSf);
                    }

                    break;
                case StyleOptions.Style2:
                    //-- Style 2
                    //-- Base
                    gp = Helpers.RoundRec(Base, 6);
                    toggle = new Rectangle(4, 4, 36, _h - 8);
                    gp2 = Helpers.RoundRec(toggle, 4);
                    with9.FillPath(new SolidBrush(_baseColorRed), gp);
                    with9.FillPath(new SolidBrush(_toggleColor), gp2);

                    //-- Lines
                    with9.DrawLine(new Pen(_bgColor), 18, 20, 18, 12);
                    with9.DrawLine(new Pen(_bgColor), 22, 20, 22, 12);
                    with9.DrawLine(new Pen(_bgColor), 26, 20, 26, 12);

                    //-- Text
                    with9.DrawString("r", new Font("Marlett", 8), new SolidBrush(_textColor),
                        new Rectangle(19, 2, Width, Height), Helpers.CenterSf);

                    if (Checked)
                    {
                        gp = Helpers.RoundRec(Base, 6);
                        toggle = new Rectangle(Convert.ToInt32(_w / 2) - 2, 4, 36, _h - 8);
                        gp2 = Helpers.RoundRec(toggle, 4);
                        with9.FillPath(new SolidBrush(_baseColor), gp);
                        with9.FillPath(new SolidBrush(_toggleColor), gp2);

                        //-- Lines
                        with9.DrawLine(new Pen(_bgColor), Convert.ToInt32(_w / 2) + 12, 20,
                            Convert.ToInt32(_w / 2) + 12, 12);
                        with9.DrawLine(new Pen(_bgColor), Convert.ToInt32(_w / 2) + 16, 20,
                            Convert.ToInt32(_w / 2) + 16, 12);
                        with9.DrawLine(new Pen(_bgColor), Convert.ToInt32(_w / 2) + 20, 20,
                            Convert.ToInt32(_w / 2) + 20, 12);

                        //-- Text
                        with9.DrawString("ü", new Font("Wingdings", 14), new SolidBrush(_textColor),
                            new Rectangle(8, 7, Width, Height), Helpers.NearSf);
                    }

                    break;
                case StyleOptions.Style3:
                    //-- Style 3
                    //-- Base
                    gp = Helpers.RoundRec(Base, 16);
                    toggle = new Rectangle(_w - 28, 4, 22, _h - 8);
                    gp2.AddEllipse(toggle);
                    with9.FillPath(new SolidBrush(_toggleColor), gp);
                    with9.FillPath(new SolidBrush(_baseColorRed), gp2);

                    //-- Text
                    with9.DrawString("OFF", Font, new SolidBrush(_baseColorRed), new Rectangle(-12, 2, _w, _h),
                        Helpers.CenterSf);

                    if (Checked)
                    {
                        //-- Base
                        gp = Helpers.RoundRec(Base, 16);
                        toggle = new Rectangle(6, 4, 22, _h - 8);
                        gp2.Reset();
                        gp2.AddEllipse(toggle);
                        with9.FillPath(new SolidBrush(_toggleColor), gp);
                        with9.FillPath(new SolidBrush(_baseColor), gp2);

                        //-- Text
                        with9.DrawString("ON", Font, new SolidBrush(_baseColor), new Rectangle(12, 2, _w, _h),
                            Helpers.CenterSf);
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

            _baseColor = colors.Flat;
        }
    }
}