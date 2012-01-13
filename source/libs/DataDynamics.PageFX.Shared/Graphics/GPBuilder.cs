using System.Drawing;
using System.Drawing.Drawing2D;

namespace DataDynamics
{
    public class GPBuilder
    {
        #region Constructors
        public GPBuilder()
        {
            Path = new GraphicsPath();
        }

        public GPBuilder(GraphicsPath path)
        {
            Path = path;
        }

        public GPBuilder(float x, float y)
            : this()
        {
            MoveTo(x, y);
        }
        #endregion

        #region Public Members
        public float X
        {
            get { return Point.X; }
            set { Point.X = value; }
        }

        public float Y
        {
            get { return Point.Y; }
            set { Point.Y = value; }
        }

        public void MoveTo(float x, float y)
        {
            Point.X = x;
            Point.Y = y;
        }

        public void LineTo(float x, float y)
        {
            Path.AddLine(X, Y, x, y);
            MoveTo(x, y);
        }

        public void LineRel(float dx, float dy)
        {
            LineTo(X + dx, Y + dy);
        }

        public void HLineTo(float x)
        {
            LineTo(x, Y);
        }

        public void VLineTo(float y)
        {
            LineTo(X, y);
        }

        public void HLineRel(float dx)
        {
            LineTo(X + dx, Y);
        }

        public void VLineRel(float dy)
        {
            LineTo(X, Y + dy);
        }

        public void ArcTo(float rx, float ry, float startAngle, float sweepAngle)
        {
            var rect = RectangleF.FromLTRB(X - rx, Y - ry, X + rx, Y + ry);
            Path.AddArc(rect, startAngle, sweepAngle);
        }

        public void ArcTo(PointF r, float startAngle, float sweepAngle)
        {
            ArcTo(r.X, r.Y, startAngle, sweepAngle);
        }

        public void BezierTo(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            Path.AddBezier(X, Y, x1, y1, x2, y2, x3, y3);
            MoveTo(x3, y3);
        }

        public void BezierTo(float x2, float y2, float x3, float y3)
        {
            BezierTo(X, Y, x2, y2, x3, y3);
        }

        public void Close()
        {
            Path.CloseFigure();
        }
        #endregion

        #region Member Variables
        public PointF Point = new PointF(0, 0);
        public GraphicsPath Path;
        #endregion
    }
}