using System;
using System.Drawing.Drawing2D;

namespace System.Drawing
{
    #region enum PathRenderFeatures
    /// <summary>
    /// Defines features of <see cref="IPathRender"/>.
    /// </summary>
    [Flags]
    public enum PathRenderFeatures
    {
        /// <summary>
        /// Path formmatter supports only line segments.
        /// </summary>
        None = 0,

        /// <summary>
        /// Path formatter supports quadratic bezier segments.
        /// </summary>
        Quad = 1,

        /// <summary>
        /// Path formatter supports cubic bezier segments.
        /// </summary>
        Cubic = 2,

        /// <summary>
        /// Path formatter supports close path command.
        /// </summary>
        Close = 4,

        All = Quad | Cubic | Close,
    }
    #endregion

    #region interface IPathRender
    /// <summary>
    /// Interface for objects that are capable of formatting graphics paths.
    /// </summary>
    public interface IPathRender
    {
        /// <summary>
        /// Makes (x,y) the current point.
        /// </summary>
        /// <param name="cur">Current point of the path.</param>
        /// <param name="pt">Next point of the path.</param>
        void Move(PointF cur, PointF pt);

        /// <summary>
        /// Draws a line from the current point to <i>pt</i> and make <i>pt</i> the current point.
        /// </summary>
        /// <param name="cur">Current point of the path.</param>
        /// <param name="pt">Next point of the path.</param>
        void Line(PointF cur, PointF pt);

        /// <summary>
        /// Draws a quadratic bezier curve from the current point to <i>pt</i> using
        /// the control point <i>c</i> and make <i>pt</i> the current point.
        /// </summary>
        /// <param name="cur">Current point of the path.</param>
        /// <param name="c"></param>
        /// <param name="pt">Next point of the path.</param>
        void Quad(PointF cur, PointF c, PointF pt);

        /// <summary>
        /// Draws a cubic bezier curve from the current point to <i>pt</i> using
        /// the control points <i>c1</i> and <i>c2</i> and make <i>pt</i> the current point.
        /// </summary>
        /// <param name="cur">Current point of the path.</param>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="pt">Next point of the path.</param>
        void Cubic(PointF cur, PointF c1, PointF c2, PointF pt);

        /// <summary>
        /// Closes the path by drawing a straight line to the last point which was argument to move.
        /// </summary>
        /// <param name="cur">Current point of the path.</param>
        void Close(PointF cur);
    }
    #endregion

    #region static class PathRendering
    public static class PathRendering
    {
        static void Close(IPathRender output, PathRenderFeatures features, PointF cur, PointF start)
        {
            if ((features & PathRenderFeatures.Close) != 0)
                output.Close(cur);
            else
                output.Line(cur, start);
        }

        #region QuadToCubic
        public static void QuadToCubic(IPathRender output, PointF cur, PointF c1, PointF pt)
        {
            float xctrl1 = c1.X + (cur.X - c1.X) / 3f;
            float yctrl1 = c1.Y + (cur.Y - c1.Y) / 3f;
            float xctrl2 = c1.X + (pt.X - c1.X) / 3f;
            float yctrl2 = c1.Y + (pt.Y - c1.Y) / 3f;
            output.Cubic(cur, new PointF(xctrl1, yctrl1), new PointF(xctrl2, yctrl2), pt);
        }
        #endregion

        #region CubicToQuad
        static bool Intersect(PointF p1, PointF p2, PointF p3, PointF p4, ref PointF res)
        {
            float dx1 = p2.X - p1.X;
            float dx2 = p3.X - p4.X;
            if ((dx1 == 0) && (dx2 == 0))
                return false;

            float m1 = (p2.Y - p1.Y) / dx1;
            float m2 = (p3.Y - p4.Y) / dx2;

            if (dx1 == 0)
            {
                // infinity
                res = new PointF(p1.X, m2 * (p1.X - p4.X) + p4.Y);
                return true;
            }
            else if (dx2 == 0)
            {
                // infinity
                res = new PointF(p4.X, m1 * (p4.X - p1.X) + p1.Y);
                return true;
            }

            float x = (-m2 * p4.X + p4.Y + m1 * p1.X - p1.Y) / (m1 - m2);
            float y = m1 * (x - p1.X) + p1.Y;
            res = new PointF(x, y);
            return true;
        }

        static PointF MidPoint(PointF a, PointF b)
        {
            return new PointF((a.X + b.X) / 2, (a.Y + b.Y) / 2);
        }

        static void Quadratify(IPathRender output, PointF a, PointF b, PointF c, PointF d, float resolutionSq)
        {
            // find intersection between bezier arms
            var s = PointF.Empty;
            Intersect(a, b, c, d, ref s);
            // find distance between the midpoints
            float dx = (a.X + d.X + s.X * 4 - (b.X + c.X) * 3) * 0.125f;
            float dy = (a.Y + d.Y + s.Y * 4 - (b.Y + c.Y) * 3) * 0.125f;
            // split curve if the quadratic isn't close enough
            if (dx * dx + dy * dy > resolutionSq)
            {
                var p01 = MidPoint(a, b);
                var p12 = MidPoint(b, c);
                var p23 = MidPoint(c, d);
                var p02 = MidPoint(p01, p12);
                var p13 = MidPoint(p12, p23);
                var p03 = MidPoint(p02, p13);
                // recursive call to subdivide curve
                Quadratify(output, a, p01, p02, p03, resolutionSq);
                Quadratify(output, p03, p13, p23, d, resolutionSq);
            }
            else
            {
                // end recursion by drawing quadratic bezier
                output.Quad(a, s, d);
            }
        }

        public static void CubicToQuad(IPathRender output, PointF cur, PointF c1, PointF c2, PointF pt, float resolution)
        {
            Quadratify(output, cur, c1, c2, pt, resolution * resolution);
        }
        #endregion

        #region CubicToLine
        class ControlSet
        {
            public PointF point0;
            public PointF point1;
            public PointF point2;
            public PointF point3;

            public ControlSet(PointF p0, PointF p1, PointF p2, PointF p3)
            {
                point0 = p0;
                point1 = p1;
                point2 = p2;
                point3 = p3;
            }

            public double CalcBreadth(double resolution)
            {
                double f0 = point0.X;
                double f4 = point0.Y;
                double f1 = point1.X;
                double f5 = point1.Y;
                double f2 = point2.X;
                double f6 = point2.Y;
                double f3 = point3.X;
                double f7 = point3.Y;
                if ((Math.Abs(f0 - f3) < resolution) &&
                    (Math.Abs(f4 - f7) < resolution))
                {
                    double f8 = Math.Abs(f1 - f0) + Math.Abs(f5 - f4);
                    double f10 = Math.Abs(f2 - f0) + Math.Abs(f6 - f4);
                    return Math.Max(f10, f8);
                }
                else
                {
                    double d0 = f4 - f7;
                    double d1 = f3 - f0;
                    double f12 = Math.Sqrt(d0 * d0 + d1 * d1);
                    double d2 = f3 * f4 - f0 * f7;
                    double f9 = Math.Abs((d0 * f2 + d1 * f6) - d2) / f12;
                    double f11 = Math.Abs((d0 * f1 + d1 * f5) - d2) / f12;
                    return Math.Max(f9, f11);
                }
            }

            public ControlSet Bisect()
            {
                var p0 = MidPoint(point0, point1);
                var p1 = MidPoint(point1, point2);
                var p2 = MidPoint(point2, point3);
                var p3 = MidPoint(p0, p1);
                var p4 = MidPoint(p1, p2);
                var p5 = MidPoint(p3, p4);
                var controlset = new ControlSet(p5, p4, p2, point3);
                point1 = p0;
                point2 = p3;
                point3 = p5;
                return controlset;
            }

            public PointF getPoint()
            {
                return point3;
            }
        }

        public static void CubicToLine(IPathRender output, PointF cur, PointF c1, PointF c2, PointF pt,
                                       double resolution)
        {
            int k = 0;
            int l = 0;
            var tempSet = new ControlSet[64];
            var controlSet = new ControlSet[64];
            tempSet[l++] = new ControlSet(cur, c1, c2, pt);
            while (l > 0)
            {
                var control1 = tempSet[--l];
                double b = control1.CalcBreadth(resolution);
                if (b > resolution)
                {
                    var control3 = control1.Bisect();
                    tempSet[l++] = control1;
                    tempSet[l++] = control3;
                }
                else
                {
                    controlSet[k++] = control1;
                }
            }
            while (k > 0)
            {
                var control2 = controlSet[--k];
                var p = control2.getPoint();
                output.Line(cur, p);
                cur = p;
            }
        }
        #endregion

        #region Cubic
        static void Cubic(IPathRender output, PathRenderFeatures features, PointF cur, PointF c1, PointF c2,
                          PointF pt, float resolution)
        {
            if ((features & PathRenderFeatures.Cubic) != 0)
            {
                output.Cubic(cur, c1, c2, pt);
                return;
            }

            if ((features & PathRenderFeatures.Quad) != 0)
            {
                CubicToQuad(output, cur, c1, c2, pt, resolution);
            }
            else
            {
                CubicToLine(output, cur, c1, c2, pt, 0.025);
            }
        }
        #endregion

        #region RenderCore
        public static void Render(GraphicsPath path, IPathRender render, PathRenderFeatures features, float resolution)
        {
            int n = path.PointCount;
            if (n > 0)
            {
                var bezier = new PointF[3];
                int bezierIdx = 0;
                var points = path.PathPoints;
                var types = path.PathTypes;
                PathPointType type;
                PathPointType pointType;
                var start = PointF.Empty;
                var cur = PointF.Empty;
                var pt = PointF.Empty;
                for (int i = 0; i < n; ++i)
                {
                    type = (PathPointType)types[i];
                    pointType = type & PathPointType.PathTypeMask;
                    pt = points[i];
                    if (type == PathPointType.Start)
                    {
                        render.Move(cur, pt);
                        start = cur = pt;
                    }
                    else if (type == PathPointType.CloseSubpath)
                    {
                        Close(render, features, cur, start);
                        cur = start;
                    }
                    else if (type == PathPointType.PathMarker)
                    {
                    }
                    else if (pointType == PathPointType.Line)
                    {
                        if (i == 0)
                        {
                            render.Move(cur, pt);
                            start = pt;
                        }
                        else
                        {
                            render.Line(cur, pt);
                        }
                        cur = pt;
                        if ((type & PathPointType.CloseSubpath) != 0)
                        {
                            Close(render, features, cur, start);
                            cur = start;
                        }
                    }
                    else if (pointType == PathPointType.Bezier
                             || pointType == PathPointType.Bezier3)
                    {
                        bezier[bezierIdx++] = pt;
                        if (bezierIdx == 3)
                        {
                            Cubic(render, features, cur, bezier[0], bezier[1], pt, resolution);
                            cur = pt;
                            bezierIdx = 0;
                            if ((type & PathPointType.CloseSubpath) != 0)
                            {
                                Close(render, features, cur, start);
                                cur = start;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        public static readonly float DefaultResolution = 0.5f / 20;

        public static void Render(GraphicsPath path, IPathRender render, PathRenderFeatures features)
        {
            Render(path, render, features, DefaultResolution);
        }
    }
    #endregion
}