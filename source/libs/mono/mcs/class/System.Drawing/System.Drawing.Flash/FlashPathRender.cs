using flash.display;

namespace System.Drawing.Flash
{
    class FlashPathRender : IPathRender
    {
        internal GraphicsPath path = new GraphicsPath();

        public void Move(PointF cur, PointF pt)
        {
            path.moveTo(pt.X, pt.Y);
        }

        public void Line(PointF cur, PointF pt)
        {
            path.lineTo(pt.X, pt.Y);
        }

        public void Quad(PointF cur, PointF c, PointF pt)
        {
            path.curveTo(c.X, c.Y, pt.X, pt.Y);
        }

        public void Cubic(PointF cur, PointF c1, PointF c2, PointF pt)
        {
            throw new NotImplementedException();
        }

        public void Close(PointF cur)
        {
            path.lineTo(cur.X, cur.Y);
        }
    }
}