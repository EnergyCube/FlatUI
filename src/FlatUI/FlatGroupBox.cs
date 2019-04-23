using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatGroupBox : ContainerControl
    {
        private int _h;

        private Color _textColor = Helpers.FlatColor;
        private int _w;

        public FlatGroupBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Size = new Size(240, 180);
            Font = new Font("Segoe ui", 10);
        }

        private bool ShowTextInternal { get; set; } = true;

        [Category("Colors")] public Color BaseColor { get; set; } = Color.FromArgb(60, 70, 73);

        public bool ShowText
        {
            get => ShowTextInternal;
            set => ShowTextInternal = value;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            UpdateColors();

            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);
            _w = Width - 1;
            _h = Height - 1;

            var Base = new Rectangle(8, 8, _w - 16, _h - 16);

            var with7 = g;
            with7.SmoothingMode = SmoothingMode.HighQuality;
            with7.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with7.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with7.Clear(BackColor);

            //-- Base
            var gp = Helpers.RoundRec(Base, 8);
            with7.FillPath(new SolidBrush(BaseColor), gp);

            //-- Arrows
            var gp2 = Helpers.DrawArrow(28, 2, false);
            with7.FillPath(new SolidBrush(BaseColor), gp2);

            var gp3 = Helpers.DrawArrow(28, 8, true);
            with7.FillPath(new SolidBrush(Color.FromArgb(60, 70, 73)), gp3);

            //-- if ShowText
            if (ShowText)
                with7.DrawString(Text, Font, new SolidBrush(_textColor), new Rectangle(16, 16, _w, _h), Helpers.NearSf);

            base.OnPaint(e);
            g.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(b, 0, 0);
            b.Dispose();
        }

        private void UpdateColors()
        {
            var colors = Helpers.GetColors(this);
            _textColor = colors.Flat;
        }
    }
}