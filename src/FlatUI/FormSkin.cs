using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FormSkin : ContainerControl
    {
        private const int MoveHeight = 50;

        private readonly Color _textColor = Color.FromArgb(234, 234, 234);
        private bool _cap;
        private int _h;
        private Point _mousePoint = new Point(0, 0);
        private int _w;

        public Color TextLight = Color.FromArgb(45, 47, 49);

        public FormSkin()
        {
            MouseDoubleClick += FormSkin_MouseDoubleClick;
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 12);
        }

        [Category("Colors")] public Color HeaderColor { get; set; } = Color.FromArgb(45, 47, 49);

        [Category("Colors")] public Color BaseColor { get; set; } = Color.FromArgb(60, 70, 73);

        [Category("Colors")] public Color BorderColor { get; set; } = Color.FromArgb(53, 58, 60);

        [Category("Colors")]
        public Color FlatColor
        {
            // get { return Helpers.FlatColor; }
            // set { Helpers.FlatColor = value; }
            get;
            set;
        } = Helpers.FlatColor;

        [Category("Options")] public bool HeaderMaximize { get; set; }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if ((e.Button == MouseButtons.Left) & new Rectangle(0, 0, Width, MoveHeight).Contains(e.Location))
            {
                _cap = true;
                _mousePoint = e.Location;
            }
        }

        private void FormSkin_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (HeaderMaximize)
                if ((e.Button == MouseButtons.Left) & new Rectangle(0, 0, Width, MoveHeight).Contains(e.Location))
                {
                    var parent = FindForm();

                    if (parent == null) return;

                    switch (parent.WindowState)
                    {
                        case FormWindowState.Normal:
                            parent.WindowState = FormWindowState.Maximized;
                            parent.Refresh();
                            break;
                        case FormWindowState.Maximized:
                            parent.WindowState = FormWindowState.Normal;
                            parent.Refresh();
                            break;
                    }
                }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _cap = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_cap)
                Parent.Location = new Point(
                    MousePosition.X - _mousePoint.X,
                    MousePosition.Y - _mousePoint.Y
                );
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (ParentForm != null)
            {
                ParentForm.FormBorderStyle = FormBorderStyle.None;
                ParentForm.AllowTransparency = false;
                ParentForm.TransparencyKey = Color.Fuchsia;

                var pparent = ParentForm.FindForm();

                if (pparent != null) pparent.StartPosition = FormStartPosition.CenterScreen;
            }

            Dock = DockStyle.Fill;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);
            _w = Width;
            _h = Height;

            var Base = new Rectangle(0, 0, _w, _h);
            var header = new Rectangle(0, 0, _w, 50);

            var with2 = g;
            with2.SmoothingMode = SmoothingMode.HighQuality;
            with2.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with2.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with2.Clear(BackColor);

            //-- Base
            with2.FillRectangle(new SolidBrush(BaseColor), Base);

            //-- Header
            with2.FillRectangle(new SolidBrush(HeaderColor), header);

            //-- Logo
            with2.FillRectangle(new SolidBrush(Color.FromArgb(243, 243, 243)), new Rectangle(8, 16, 4, 18));
            with2.FillRectangle(new SolidBrush(FlatColor), 16, 16, 4, 18);
            with2.DrawString(Text, Font, new SolidBrush(_textColor), new Rectangle(26, 15, _w, _h), Helpers.NearSf);

            //-- Border
            with2.DrawRectangle(new Pen(BorderColor), Base);

            base.OnPaint(e);
            g.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(b, 0, 0);
            b.Dispose();
        }
    }
}