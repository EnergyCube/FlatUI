using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    [DefaultEvent("CheckedChanged")]
    public sealed class FlatRadioButton : Control
    {
        public delegate void CheckedChangedEventHandler(object sender);

        [Flags]
        public enum StyleOptions
        {
            Style1,
            Style2
        }

        private readonly Color _baseColor = Color.FromArgb(45, 47, 49);
        private readonly Color _textColor = Color.FromArgb(243, 243, 243);
        private Color _borderColor = Helpers.FlatColor;

        private bool _checked;
        private int _h;
        private MouseState _state = MouseState.None;
        private int _w;

        public FlatRadioButton()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
            Size = new Size(100, 22);
            BackColor = Color.FromArgb(60, 70, 73);
            Font = new Font("Segoe UI", 10);
        }

        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                InvalidateControls();
                CheckedChanged?.Invoke(this);
                Invalidate();
            }
        }

        [Category("Options")] public StyleOptions Options { get; set; }

        public event CheckedChangedEventHandler CheckedChanged;

        protected override void OnClick(EventArgs e)
        {
            if (!_checked)
                Checked = true;
            base.OnClick(e);
        }

        private void InvalidateControls()
        {
            if (!IsHandleCreated || !_checked)
                return;
            foreach (Control c in Parent.Controls)
            {
                if (!ReferenceEquals(c, this) && c is FlatRadioButton button)
                {
                    button.Checked = false;
                    Invalidate();
                }
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            InvalidateControls();
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
            var dot = new Rectangle(4, 6, _h - 12, _h - 12);

            var with10 = g;
            with10.SmoothingMode = SmoothingMode.HighQuality;
            with10.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with10.Clear(BackColor);

            switch (Options)
            {
                case StyleOptions.Style1:
                    //-- Base
                    with10.FillEllipse(new SolidBrush(_baseColor), Base);

                    switch (_state)
                    {
                        case MouseState.Over:
                            with10.DrawEllipse(new Pen(_borderColor), Base);
                            break;
                        case MouseState.Down:
                            with10.DrawEllipse(new Pen(_borderColor), Base);
                            break;
                    }

                    //-- If Checked 
                    if (Checked) with10.FillEllipse(new SolidBrush(_borderColor), dot);
                    break;
                case StyleOptions.Style2:
                    //-- Base
                    with10.FillEllipse(new SolidBrush(_baseColor), Base);

                    switch (_state)
                    {
                        case MouseState.Over:
                            //-- Base
                            with10.DrawEllipse(new Pen(_borderColor), Base);
                            with10.FillEllipse(new SolidBrush(Color.FromArgb(118, 213, 170)), Base);
                            break;
                        case MouseState.Down:
                            //-- Base
                            with10.DrawEllipse(new Pen(_borderColor), Base);
                            with10.FillEllipse(new SolidBrush(Color.FromArgb(118, 213, 170)), Base);
                            break;
                    }

                    //-- If Checked
                    if (Checked) with10.FillEllipse(new SolidBrush(_borderColor), dot);
                    break;
            }

            with10.DrawString(Text, Font, new SolidBrush(_textColor), new Rectangle(20, 2, _w, _h), Helpers.NearSf);

            base.OnPaint(e);
            g.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(b, 0, 0);
            b.Dispose();
        }

        private void UpdateColors()
        {
            var colors = Helpers.GetColors(this);

            _borderColor = colors.Flat;
        }
    }
}