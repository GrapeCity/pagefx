using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DataDynamics
{
    public static class KnownPathes
    {
        #region RoundRect
        public static GraphicsPath RoundRect(RectangleF rect, float rx, float ry)
        {
            return RoundRect(rect.X, rect.Y, rect.Width, rect.Height, rx, ry);
        }

        public static GraphicsPath RoundRect(float x, float y, float width, float height, float rx, float ry)
        {
            var path = new GraphicsPath();
            AddRoundRect(path, x, y, width, height, rx, ry);
            return path;
        }

        public static void AddRoundRect(GraphicsPath path, float x, float y, float width, float height, float rx,
                                        float ry)
        {
            float b = 0.4477f;
            var gp = new GPBuilder(path);
            gp.MoveTo(x + rx, y);
            gp.LineTo(x + width - rx, y);
            gp.BezierTo(x + width - rx * b, y,
                        x + width, y + ry * b,
                        x + width, y + ry);
            gp.LineTo(x + width, y + height - ry);
            gp.BezierTo(x + width, y + height - ry * b,
                        x + width - rx * b, y + height,
                        x + width - rx, y + height);
            gp.LineTo(x + rx, y + height);
            gp.BezierTo(x + rx * b, y + height,
                        x, y + height - ry * b,
                        x, y + height - ry);
            gp.LineTo(x, y + ry);
            gp.BezierTo(x, y + ry * b,
                        x + rx * b, y,
                        x + rx, y);
            gp.Close();
        }
        #endregion

        #region Star
        public static GraphicsPath Star(float x, float y, float r, int n, float startAngle)
        {
            float phi = 360 / n;
            float phi2 = phi / 2;
            float r2 = (float)(r * Math.Sin(Angle.ToRadians(90 - phi)) / Math.Sin(Angle.ToRadians(90 + phi2)));
            return Star(x, y, r, r2, n, startAngle);
        }

        public static GraphicsPath Star(float x, float y, float r, float r2, int n, float startAngle)
        {
            float phi = 360 / n;
            float phi2 = phi / 2;
            int n2 = 2 * n;
            var points = new PointF[n2];
            float angle = startAngle;
            for (int i = 0; i < n2; ++i)
            {
                float cr = (i & 1) != 0 ? r2 : r;
                float a = Angle.ToRadians(angle);
                points[i].X = x + cr * (float)Math.Cos(a);
                points[i].Y = y + cr * (float)Math.Sin(a);
                angle += phi2;
            }
            var res = new GraphicsPath(FillMode.Winding);
            res.AddPolygon(points);
            return res;
        }
        #endregion
    }
}