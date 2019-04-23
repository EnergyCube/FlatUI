using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FlatUI
{
    public static class Helpers
    {
        public static Color FlatColor = Color.FromArgb(35, 168, 109);

        public static readonly StringFormat NearSf = new StringFormat
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        };

        public static readonly StringFormat CenterSf = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        public static GraphicsPath RoundRec(Rectangle rectangle, int curve)
        {
            var p = new GraphicsPath();
            var arcRectangleWidth = curve * 2;
            p.AddArc(new Rectangle(rectangle.X, rectangle.Y, arcRectangleWidth, arcRectangleWidth), -180, 90);
            p.AddArc(
                new Rectangle(rectangle.Width - arcRectangleWidth + rectangle.X, rectangle.Y, arcRectangleWidth,
                    arcRectangleWidth), -90, 90);
            p.AddArc(
                new Rectangle(rectangle.Width - arcRectangleWidth + rectangle.X,
                    rectangle.Height - arcRectangleWidth + rectangle.Y, arcRectangleWidth, arcRectangleWidth), 0, 90);
            p.AddArc(
                new Rectangle(rectangle.X, rectangle.Height - arcRectangleWidth + rectangle.Y, arcRectangleWidth,
                    arcRectangleWidth), 90, 90);
            p.AddLine(new Point(rectangle.X, rectangle.Height - arcRectangleWidth + rectangle.Y),
                new Point(rectangle.X, curve + rectangle.Y));
            return p;
        }

        public static GraphicsPath RoundRect(float x, float y, float w, float h, double r = 0.3,
            bool tl = true, bool tr = true, bool br = true, bool bl = true)
        {
            var d = Math.Min(w, h) * (float) r;
            var xw = x + w;
            var yh = y + h;
            var functionReturnValue = new GraphicsPath();

            var with1 = functionReturnValue;
            if (tl)
                with1.AddArc(x, y, d, d, 180, 90);
            else
                with1.AddLine(x, y, x, y);
            if (tr)
                with1.AddArc(xw - d, y, d, d, 270, 90);
            else
                with1.AddLine(xw, y, xw, y);
            if (br)
                with1.AddArc(xw - d, yh - d, d, d, 0, 90);
            else
                with1.AddLine(xw, yh, xw, yh);
            if (bl)
                with1.AddArc(x, yh - d, d, d, 90, 90);
            else
                with1.AddLine(x, yh, x, yh);

            with1.CloseFigure();
            return functionReturnValue;
        }

        //-- Credit: AeonHack
        public static GraphicsPath DrawArrow(int x, int y, bool flip)
        {
            var gp = new GraphicsPath();

            const int w = 12;
            const int h = 6;

            if (flip)
            {
                gp.AddLine(x + 1, y, x + w + 1, y);
                gp.AddLine(x + w, y, x + h, y + h - 1);
            }
            else
            {
                gp.AddLine(x, y + h, x + w, y + h);
                gp.AddLine(x + w, y + h, x + h, y);
            }

            gp.CloseFigure();
            return gp;
        }

        /// <summary>
        ///     Get the colorscheme of a Control from a parent FormSkin.
        /// </summary>
        /// <param name="control">Control</param>
        /// <returns>Colors</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static FlatColors GetColors(Control control)
        {
            if (control == null)
                throw new ArgumentNullException();

            var colors = new FlatColors();

            while (control != null && control.GetType() != typeof(FormSkin)) control = control.Parent;

            if (control != null)
            {
                var skin = (FormSkin) control;
                colors.Flat = skin.FlatColor;
            }

            return colors;
        }
    }
}