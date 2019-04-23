using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatListBox : Control
    {
        private readonly Color _baseColor = Color.FromArgb(45, 47, 49);
        private ListBox _withEventsFieldListBx = new ListBox();

        public FlatListBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;

            ListBx.DrawMode = DrawMode.OwnerDrawFixed;
            ListBx.ScrollAlwaysVisible = false;
            ListBx.HorizontalScrollbar = false;
            ListBx.BorderStyle = BorderStyle.None;
            ListBx.BackColor = _baseColor;
            ListBx.ForeColor = Color.White;
            ListBx.Location = new Point(3, 3);
            ListBx.Font = new Font("Segoe UI", 8);
            ListBx.ItemHeight = 20;
            ListBx.Items.Clear();
            ListBx.IntegralHeight = false;

            Size = new Size(131, 101);
            BackColor = _baseColor;
        }

        private ListBox ListBx
        {
            get => _withEventsFieldListBx;
            set
            {
                if (_withEventsFieldListBx != null) _withEventsFieldListBx.DrawItem -= Drawitem;

                _withEventsFieldListBx = value;

                if (_withEventsFieldListBx != null) _withEventsFieldListBx.DrawItem += Drawitem;
            }
        }

        [Category("Options")] public ListBox.ObjectCollection Items => ListBx.Items;

        [Category("Colors")] public Color SelectedColor { get; set; } = Helpers.FlatColor;

        public string SelectedItem => ListBx.SelectedItem.ToString();
        public int SelectedIndex => ListBx.SelectedIndex;

        public void Clear()
        {
            ListBx.Items.Clear();
        }

        public void ClearSelected()
        {
            for (var i = ListBx.SelectedItems.Count - 1; i >= 0; i += -1) ListBx.Items.Remove(ListBx.SelectedItems[i]);
        }

        public void Drawitem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            e.DrawBackground();
            e.DrawFocusRectangle();

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            //-- if selected
            if (e.State.ToString().IndexOf("Selected,", StringComparison.Ordinal) >= 0)
            {
                //-- Base
                e.Graphics.FillRectangle(new SolidBrush(SelectedColor),
                    new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

                //-- Text
                e.Graphics.DrawString(" " + ListBx.Items[e.Index], new Font("Segoe UI", 8), Brushes.White, e.Bounds.X,
                    e.Bounds.Y + 2);
            }
            else
            {
                //-- Base
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(51, 53, 55)),
                    new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));

                //-- Text 
                e.Graphics.DrawString(" " + ListBx.Items[e.Index], new Font("Segoe UI", 8), Brushes.White, e.Bounds.X,
                    e.Bounds.Y + 2);
            }

            e.Graphics.Dispose();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (!Controls.Contains(ListBx)) Controls.Add(ListBx);
        }

        public void AddRange(object[] items)
        {
            ListBx.Items.Remove("");
            ListBx.Items.AddRange(items);
        }

        public void AddItem(object item)
        {
            ListBx.Items.Remove("");
            ListBx.Items.Add(item);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            UpdateColors();

            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);

            var Base = new Rectangle(0, 0, Width, Height);

            var with19 = g;
            with19.SmoothingMode = SmoothingMode.HighQuality;
            with19.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with19.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with19.Clear(BackColor);

            //-- Size
            ListBx.Size = new Size(Width - 6, Height - 2);

            //-- Base
            with19.FillRectangle(new SolidBrush(_baseColor), Base);

            base.OnPaint(e);
            g.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(b, 0, 0);
            b.Dispose();
        }

        private void UpdateColors()
        {
            var colors = Helpers.GetColors(this);
            SelectedColor = colors.Flat;
        }
    }
}