using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
    public sealed class FlatTreeView : TreeView
    {
        private readonly Color _baseColor = Color.FromArgb(45, 47, 49);
        private readonly Color _lineColor = Color.FromArgb(25, 27, 29);
        private readonly TreeNodeStates _state;

        public FlatTreeView(TreeNodeStates state)
        {
            _state = state;
            SetStyle(
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer, true);
            DoubleBuffered = true;
            BackColor = _baseColor;
            ForeColor = Color.White;
            LineColor = _lineColor;
            DrawMode = TreeViewDrawMode.OwnerDrawAll;
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            try
            {
                var bounds = new Rectangle(e.Bounds.Location.X, e.Bounds.Location.Y, e.Bounds.Width, e.Bounds.Height);
                //e.Node.Nodes.Item.
                switch (_state)
                {
                    case TreeNodeStates.Default:
                        e.Graphics.FillRectangle(Brushes.Red, bounds);
                        e.Graphics.DrawString(e.Node.Text, new Font("Segoe UI", 8), Brushes.LimeGreen,
                            new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width, bounds.Height), Helpers.NearSf);
                        Invalidate();
                        break;
                    case TreeNodeStates.Checked:
                        e.Graphics.FillRectangle(Brushes.Green, bounds);
                        e.Graphics.DrawString(e.Node.Text, new Font("Segoe UI", 8), Brushes.Black,
                            new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width, bounds.Height), Helpers.NearSf);
                        Invalidate();
                        break;
                    case TreeNodeStates.Selected:
                        e.Graphics.FillRectangle(Brushes.Green, bounds);
                        e.Graphics.DrawString(e.Node.Text, new Font("Segoe UI", 8), Brushes.Black,
                            new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width, bounds.Height), Helpers.NearSf);
                        Invalidate();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            base.OnDrawNode(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var b = new Bitmap(Width, Height);
            var g = Graphics.FromImage(b);

            var Base = new Rectangle(0, 0, Width, Height);

            var with22 = g;
            with22.SmoothingMode = SmoothingMode.HighQuality;
            with22.PixelOffsetMode = PixelOffsetMode.HighQuality;
            with22.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            with22.Clear(BackColor);

            with22.FillRectangle(new SolidBrush(_baseColor), Base);
            with22.DrawString(Text, new Font("Segoe UI", 8), Brushes.Black,
                new Rectangle(Bounds.X + 2, Bounds.Y + 2, Bounds.Width, Bounds.Height), Helpers.NearSf);


            base.OnPaint(e);
            g.Dispose();
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImageUnscaled(b, 0, 0);
            b.Dispose();
        }
    }
}