using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    [DefaultEvent("CheckedChanged")]
    public sealed class FlatCheckBox : Control
    {
        public delegate void CheckedChangedEventHandler(object sender);

        [Flags]
        public enum StyleOptions
        {
            Style1,
            Style2
        }

        private readonly Color _textColor = Color.FromArgb(243, 243, 243);
        private bool _checked;
        private int _h;
        private MouseState _state = MouseState.None;
        private int _w;

        public FlatCheckBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            BackColor = Color.FromArgb(60, 70, 73);
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 10);
            Size = new Size(112, 22);
        }

        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                Invalidate();
            }
        }

        [Category("Options")] public StyleOptions Style { get; set; }

        [Category("Colors")] public Color BaseColor { get; set; } = Color.FromArgb(45, 47, 49);

        [Category("Colors")] public Color BorderColor { get; set; } = Helpers.FlatColor;

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        public event CheckedChangedEventHandler CheckedChanged;

        protected override void OnClick(EventArgs e)
        {
            _checked = !_checked;
            CheckedChanged?.Invoke(this);
            base.OnClick(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 22;
        }

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

            var Base = new Rectangle(0, 2, Height - 5, Height - 5);

            var with11 = g;
            with11.SmoothingMode = SmoothingMode.HighQuality;
            with11.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with11.Clear(BackColor);

            switch (Style)
            {
                case StyleOptions.Style1:
                    //-- Style 1
                    //-- Base
                    with11.FillRectangle(new SolidBrush(BaseColor), Base);

                    switch (_state)
                    {
                        case MouseState.Over:
                            //-- Base
                            with11.DrawRectangle(new Pen(BorderColor), Base);
                            break;
                        case MouseState.Down:
                            //-- Base
                            with11.DrawRectangle(new Pen(BorderColor), Base);
                            break;
                    }

                    //-- If Checked
                    if (Checked)
                        with11.DrawString("ü", new Font("Wingdings", 18), new SolidBrush(BorderColor),
                            new Rectangle(5, 7, _h - 9, _h - 9), Helpers.CenterSf);

                    //-- If Enabled
                    if (Enabled == false)
                    {
                        with11.FillRectangle(new SolidBrush(Color.FromArgb(54, 58, 61)), Base);
                        with11.DrawString(Text, Font, new SolidBrush(Color.FromArgb(140, 142, 143)),
                            new Rectangle(20, 2, _w, _h), Helpers.NearSf);
                    }

                    //-- Text
                    with11.DrawString(Text, Font, new SolidBrush(_textColor), new Rectangle(20, 2, _w, _h),
                        Helpers.NearSf);
                    break;
                case StyleOptions.Style2:
                    //-- Style 2
                    //-- Base
                    with11.FillRectangle(new SolidBrush(BaseColor), Base);

                    switch (_state)
                    {
                        case MouseState.Over:
                            //-- Base
                            with11.DrawRectangle(new Pen(BorderColor), Base);
                            with11.FillRectangle(new SolidBrush(Color.FromArgb(118, 213, 170)), Base);
                            break;
                        case MouseState.Down:
                            //-- Base
                            with11.DrawRectangle(new Pen(BorderColor), Base);
                            with11.FillRectangle(new SolidBrush(Color.FromArgb(118, 213, 170)), Base);
                            break;
                    }

                    //-- If Checked
                    if (Checked)
                        with11.DrawString("ü", new Font("Wingdings", 18), new SolidBrush(BorderColor),
                            new Rectangle(5, 7, _h - 9, _h - 9), Helpers.CenterSf);

                    //-- If Enabled
                    if (Enabled == false)
                    {
                        with11.FillRectangle(new SolidBrush(Color.FromArgb(54, 58, 61)), Base);
                        with11.DrawString(Text, Font, new SolidBrush(Color.FromArgb(48, 119, 91)),
                            new Rectangle(20, 2, _w, _h), Helpers.NearSf);
                    }

                    //-- Text
                    with11.DrawString(Text, Font, new SolidBrush(_textColor), new Rectangle(20, 2, _w, _h),
                        Helpers.NearSf);
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
            BorderColor = colors.Flat;
        }
    }
}