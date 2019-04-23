using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    [DefaultEvent("TextChanged")]
    public sealed class FlatTextBox : Control
    {
        private readonly Color _baseColor = Color.FromArgb(45, 47, 49);
        private readonly TextBox _tb;
        private int _h;

        private int _maxLength = 32767;

        private bool _multiline;

        private bool _readOnly;

        private HorizontalAlignment _textAlign = HorizontalAlignment.Left;

        private bool _useSystemPasswordChar;
        private int _w;
        public MouseState State = MouseState.None;

        public FlatTextBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);

            DoubleBuffered = true;
            BackColor = Color.Transparent;

            _tb = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Text = Text,
                BackColor = _baseColor,
                ForeColor = TextColor,
                MaxLength = _maxLength,
                Multiline = _multiline,
                ReadOnly = _readOnly,
                UseSystemPasswordChar = _useSystemPasswordChar,
                BorderStyle = BorderStyle.None,
                Location = new Point(5, 5),
                Width = Width - 10,
                Cursor = Cursors.IBeam
            };


            if (_multiline)
                _tb.Height = Height - 11;
            else
                Height = _tb.Height + 11;

            _tb.TextChanged += OnBaseTextChanged;
            _tb.KeyDown += OnBaseKeyDown;
        }

        [Category("Options")]
        public HorizontalAlignment TextAlign
        {
            get => _textAlign;
            set
            {
                _textAlign = value;
                if (_tb != null) _tb.TextAlign = value;
            }
        }

        [Category("Options")]
        public int MaxLength
        {
            get => _maxLength;
            set
            {
                _maxLength = value;
                if (_tb != null) _tb.MaxLength = value;
            }
        }

        [Category("Options")]
        public bool ReadOnly
        {
            get => _readOnly;
            set
            {
                _readOnly = value;
                if (_tb != null) _tb.ReadOnly = value;
            }
        }

        [Category("Options")]
        public bool UseSystemPasswordChar
        {
            get => _useSystemPasswordChar;
            set
            {
                _useSystemPasswordChar = value;
                if (_tb != null) _tb.UseSystemPasswordChar = value;
            }
        }

        [Category("Options")]
        public bool Multiline
        {
            get => _multiline;
            set
            {
                _multiline = value;
                if (_tb != null)
                {
                    _tb.Multiline = value;

                    if (value)
                        _tb.Height = Height - 11;
                    else
                        Height = _tb.Height + 11;
                }
            }
        }

        [Category("Options")] public bool FocusOnHover { get; set; }

        [Category("Options")]
        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                if (_tb != null) _tb.Text = value;
            }
        }

        [Category("Options")]
        public override Font Font
        {
            get => base.Font;
            set
            {
                base.Font = value;
                if (_tb != null)
                {
                    _tb.Font = value;
                    _tb.Location = new Point(3, 5);
                    _tb.Width = Width - 6;

                    if (!_multiline) Height = _tb.Height + 11;
                }
            }
        }

        [Category("Colors")] public Color TextColor { get; set; } = Color.FromArgb(192, 192, 192);

        public override Color ForeColor
        {
            get => TextColor;
            set => TextColor = value;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!Controls.Contains(_tb)) Controls.Add(_tb);
        }

        private void OnBaseTextChanged(object s, EventArgs e)
        {
            Text = _tb.Text;
        }

        private void OnBaseKeyDown(object s, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                _tb.SelectAll();
                e.SuppressKeyPress = true;
            }

            if (e.Control && e.KeyCode == Keys.C)
            {
                _tb.Copy();
                e.SuppressKeyPress = true;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            _tb.Location = new Point(5, 5);
            _tb.Width = Width - 10;

            if (_multiline)
                _tb.Height = Height - 11;
            else
                Height = _tb.Height + 11;

            base.OnResize(e);
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
            _tb.Focus();
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            State = MouseState.Over;
            if (FocusOnHover) _tb.Focus();
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            State = MouseState.None;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);
            _w = Width - 1;
            _h = Height - 1;

            var Base = new Rectangle(0, 0, _w, _h);

            var with12 = g;
            with12.SmoothingMode = SmoothingMode.HighQuality;
            with12.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with12.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with12.Clear(BackColor);

            //-- Colors
            _tb.BackColor = _baseColor;
            _tb.ForeColor = TextColor;

            //-- Base
            with12.FillRectangle(new SolidBrush(_baseColor), Base);

            base.OnPaint(e);
            g.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(b, 0, 0);
            b.Dispose();
        }
    }
}