using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text;

namespace DataDynamics.PageFX.Common.Graphics
{
    public sealed class SvgPathRender : IPathRender
    {
        readonly StringBuilder str = new StringBuilder();
        bool space;

        static string ToString(IConvertible v)
        {
            return v.ToString(NumberFormatInfo.InvariantInfo);
        }

        #region IPathRender Members
        public void Move(PointF cur, PointF pt)
        {
            if (space) str.Append(' ');
            str.AppendFormat("M {0} {1}", ToString(pt.X), ToString(pt.Y));
            space = true;
        }

        public void Line(PointF cur, PointF pt)
        {
            if (space) str.Append(' ');
            str.AppendFormat("L {0} {1}", ToString(pt.X), ToString(pt.Y));
            space = true;
        }

        public void Quad(PointF cur, PointF c, PointF pt)
        {
            if (space) str.Append(' ');
            str.AppendFormat("Q {0} {1} {2} {3}",
                             ToString(c.X), ToString(c.Y),
                             ToString(pt.X), ToString(pt.Y));
            space = true;
        }

        public void Cubic(PointF cur, PointF c1, PointF c2, PointF pt)
        {
            if (space) str.Append(' ');
            str.AppendFormat("C {0} {1} {2} {3} {4} {5}",
                             ToString(c1.X), ToString(c1.Y),
                             ToString(c2.X), ToString(c2.Y),
                             ToString(pt.X), ToString(pt.Y));
            space = true;
        }

        public void Close(PointF cur)
        {
            if (space) str.Append(' ');
            str.Append('Z');
            space = true;
        }
        #endregion

        public override string ToString()
        {
            return str.ToString();
        }

        public static string ToString(GraphicsPath path)
        {
            var output = new SvgPathRender();
            PathRendering.Render(path, output, PathRenderFeatures.All, 0.5f / 20);
            return output.ToString();
        }
    }
}