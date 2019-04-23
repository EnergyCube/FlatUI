using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatAlertBox : Control
    {
        [Flags]
        public enum StyleKind
        {
            Success,
            Error,
            Info
        }

        private readonly Color _errorColor = Color.FromArgb(87, 71, 71);
        private readonly Color _errorText = Color.FromArgb(254, 142, 122);
        private readonly Color _infoColor = Color.FromArgb(70, 91, 94);
        private readonly Color _infoText = Color.FromArgb(97, 185, 186);

        private readonly Color _successColor = Color.FromArgb(60, 85, 79);
        private readonly Color _successText = Color.FromArgb(35, 169, 110);
        private int _h;
        private MouseState _state = MouseState.None;
        private string _text;

        /// <summary>
        ///     How to use: FlatAlertBox.ShowControl(Kind, String, Interval)
        /// </summary>
        /// <remarks></remarks>
        private int _w;

        private Timer _withEventsFieldT;

        public FlatAlertBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            BackColor = Color.FromArgb(60, 70, 73);
            Size = new Size(576, 42);
            Location = new Point(10, 61);
            Font = new Font("Segoe UI", 10);
            Cursor = Cursors.Hand;
        }

        private Timer T
        {
            get => _withEventsFieldT;
            set
            {
                if (_withEventsFieldT != null) _withEventsFieldT.Tick -= T_Tick;
                _withEventsFieldT = value;
                if (_withEventsFieldT != null) _withEventsFieldT.Tick += T_Tick;
            }
        }

        [Category("Options")] public StyleKind Kind { get; set; }

        [Category("Options")]
        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                if (_text != null) _text = value;
            }
        }

        [Category("Options")]
        public new bool Visible
        {
            get => !base.Visible;
            set => base.Visible = value;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Height = 42;
        }

        public void ShowControl(StyleKind kind, string str, int interval)
        {
            Kind = kind;
            Text = str;
            Visible = true;
            T = new Timer {Interval = interval, Enabled = true};
        }

        private void T_Tick(object sender, EventArgs e)
        {
            Visible = false;
            T.Enabled = false;
            T.Dispose();
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Visible = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);
            _w = Width - 1;
            _h = Height - 1;

            var Base = new Rectangle(0, 0, _w, _h);

            var with14 = g;
            with14.SmoothingMode = SmoothingMode.HighQuality;
            with14.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with14.Clear(BackColor);

            switch (Kind)
            {
                case StyleKind.Success:
                    //-- Base
                    with14.FillRectangle(new SolidBrush(_successColor), Base);

                    //-- Ellipse
                    with14.FillEllipse(new SolidBrush(_successText), new Rectangle(8, 9, 24, 24));
                    with14.FillEllipse(new SolidBrush(_successColor), new Rectangle(10, 11, 20, 20));

                    //-- Checked Sign
                    with14.DrawString("ü", new Font("Wingdings", 22), new SolidBrush(_successText),
                        new Rectangle(7, 7, _w, _h), Helpers.NearSf);
                    with14.DrawString(Text, Font, new SolidBrush(_successText), new Rectangle(48, 12, _w, _h),
                        Helpers.NearSf);

                    //-- X button
                    with14.FillEllipse(new SolidBrush(Color.FromArgb(35, Color.Black)),
                        new Rectangle(_w - 30, _h - 29, 17, 17));
                    with14.DrawString("r", new Font("Marlett", 8), new SolidBrush(_successColor),
                        new Rectangle(_w - 28, 16, _w, _h), Helpers.NearSf);

                    switch (_state)
                    {
                        // -- Mouse Over
                        case MouseState.Over:
                            with14.DrawString("r", new Font("Marlett", 8),
                                new SolidBrush(Color.FromArgb(25, Color.White)), new Rectangle(_w - 28, 16, _w, _h),
                                Helpers.NearSf);
                            break;
                    }

                    break;
                case StyleKind.Error:
                    //-- Base
                    with14.FillRectangle(new SolidBrush(_errorColor), Base);

                    //-- Ellipse
                    with14.FillEllipse(new SolidBrush(_errorText), new Rectangle(8, 9, 24, 24));
                    with14.FillEllipse(new SolidBrush(_errorColor), new Rectangle(10, 11, 20, 20));

                    //-- X Sign
                    with14.DrawString("r", new Font("Marlett", 16), new SolidBrush(_errorText),
                        new Rectangle(6, 11, _w, _h), Helpers.NearSf);
                    with14.DrawString(Text, Font, new SolidBrush(_errorText), new Rectangle(48, 12, _w, _h),
                        Helpers.NearSf);

                    //-- X button
                    with14.FillEllipse(new SolidBrush(Color.FromArgb(35, Color.Black)),
                        new Rectangle(_w - 32, _h - 29, 17, 17));
                    with14.DrawString("r", new Font("Marlett", 8), new SolidBrush(_errorColor),
                        new Rectangle(_w - 30, 17, _w, _h), Helpers.NearSf);

                    switch (_state)
                    {
                        case MouseState.Over:
                            // -- Mouse Over
                            with14.DrawString("r", new Font("Marlett", 8),
                                new SolidBrush(Color.FromArgb(25, Color.White)), new Rectangle(_w - 30, 15, _w, _h),
                                Helpers.NearSf);
                            break;
                    }

                    break;
                case StyleKind.Info:
                    //-- Base
                    with14.FillRectangle(new SolidBrush(_infoColor), Base);

                    //-- Ellipse
                    with14.FillEllipse(new SolidBrush(_infoText), new Rectangle(8, 9, 24, 24));
                    with14.FillEllipse(new SolidBrush(_infoColor), new Rectangle(10, 11, 20, 20));

                    //-- Info Sign
                    with14.DrawString("¡", new Font("Segoe UI", 20, FontStyle.Bold), new SolidBrush(_infoText),
                        new Rectangle(12, -4, _w, _h), Helpers.NearSf);
                    with14.DrawString(Text, Font, new SolidBrush(_infoText), new Rectangle(48, 12, _w, _h),
                        Helpers.NearSf);

                    //-- X button
                    with14.FillEllipse(new SolidBrush(Color.FromArgb(35, Color.Black)),
                        new Rectangle(_w - 32, _h - 29, 17, 17));
                    with14.DrawString("r", new Font("Marlett", 8), new SolidBrush(_infoColor),
                        new Rectangle(_w - 30, 17, _w, _h), Helpers.NearSf);

                    switch (_state)
                    {
                        case MouseState.Over:
                            // -- Mouse Over
                            with14.DrawString("r", new Font("Marlett", 8),
                                new SolidBrush(Color.FromArgb(25, Color.White)), new Rectangle(_w - 30, 17, _w, _h),
                                Helpers.NearSf);
                            break;
                    }

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