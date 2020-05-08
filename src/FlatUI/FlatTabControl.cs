using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatTabControl : TabControl
    {
        private readonly Color _bgColor = Color.FromArgb(60, 70, 73);

        public FlatTabControl()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            BackColor = Color.FromArgb(60, 70, 73);

            Font = new Font("Segoe UI", 10);
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(120, 40);
        }

        [Category("Colors")] public Color BaseColor { get; set; } = Color.FromArgb(45, 47, 49);

        [Category("Colors")] public Color ActiveColor { get; set; } = Helpers.FlatColor;

        protected override void CreateHandle()
        {
            base.CreateHandle();
            Alignment = TabAlignment.Top;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            UpdateColors();

            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);

            var with13 = g;
            with13.SmoothingMode = SmoothingMode.HighQuality;
            with13.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with13.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with13.Clear(BaseColor);

            for (var i = 0; i <= TabCount - 1; i++)
            {
                var Base = new Rectangle(new Point(GetTabRect(i).Location.X + 2, GetTabRect(i).Location.Y),
                    new Size(GetTabRect(i).Width, GetTabRect(i).Height));
                var baseSize = new Rectangle(Base.Location, new Size(Base.Width, Base.Height));

                if (i == SelectedIndex)
                {
                    //-- Base
                    with13.FillRectangle(new SolidBrush(BaseColor), baseSize);

                    //-- Gradiant
                    //.fill
                    with13.FillRectangle(new SolidBrush(ActiveColor), baseSize);

                    //-- ImageList
                    if (ImageList != null)
                        try
                        {
                            if (ImageList.Images[TabPages[i].ImageIndex] != null)
                            {
                                //-- Image
                                with13.DrawImage(ImageList.Images[TabPages[i].ImageIndex],
                                    new Point(baseSize.Location.X + 8, baseSize.Location.Y + 6));
                                //-- Text
                                with13.DrawString("      " + TabPages[i].Text, Font, Brushes.White, baseSize,
                                    Helpers.CenterSf);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    else
                        with13.DrawString(TabPages[i].Text, Font, Brushes.White, baseSize, Helpers.CenterSf);
                }
                else
                {
                    //-- Base
                    with13.FillRectangle(new SolidBrush(BaseColor), baseSize);

                    //-- ImageList
                    if (ImageList != null)
                        try
                        {
                            if (ImageList.Images[TabPages[i].ImageIndex] != null)
                            {
                                //-- Image
                                with13.DrawImage(ImageList.Images[TabPages[i].ImageIndex],
                                    new Point(baseSize.Location.X + 8, baseSize.Location.Y + 6));
                                //-- Text
                                with13.DrawString("      " + TabPages[i].Text, Font, new SolidBrush(Color.White),
                                    baseSize, new StringFormat
                                    {
                                        LineAlignment = StringAlignment.Center,
                                        Alignment = StringAlignment.Center
                                    });
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    else
                        with13.DrawString(TabPages[i].Text, Font, new SolidBrush(Color.White), baseSize,
                            new StringFormat
                            {
                                LineAlignment = StringAlignment.Center,
                                Alignment = StringAlignment.Center
                            });
                }
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
            ActiveColor = colors.Flat;
        }
    }
}