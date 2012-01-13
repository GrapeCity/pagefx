//Ported from C to C# from mono libgdiplus

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Flash;

namespace System.Drawing.Drawing2D
{
    using FlashGraphicsPath = flash.display.GraphicsPath;

    public sealed class GraphicsPath : MarshalByRefObject, ICloneable, IDisposable
    {
        #region GpStatus
        private const int InvalidParameter = 2;
        private const int OutOfMemory = 3;
        #endregion

        static Exception GdipException(int status)
        {
            throw new InvalidOperationException();
        }

        // 1/4 is the FlatnessDefault as defined in GdiPlusEnums.h
        internal const float FlatnessDefault = 1.0f / 4.0f;
        internal const float DefaultTension = 0.5f;
        const FillMode DefaultFillMode = FillMode.Alternate;
        
        #region Data
        FillMode _fillMode;
        List<byte> _types = new List<byte>();
        List<PointF> _points = new List<PointF>();
        bool _startNewFig;	/* Flag to keep track if we need to start a new figure */
        FlashGraphicsPath _native;
        #endregion

        #region Constructors
        public GraphicsPath()
        {
        }

        public GraphicsPath(FillMode fillMode)
        {
            _fillMode = fillMode;
        }

        public GraphicsPath(Point[] pts, byte[] types)
            : this(pts, types, FillMode.Alternate)
        {
        }

        public GraphicsPath(PointF[] pts, byte[] types)
            : this(pts, types, FillMode.Alternate)
        {
        }

        public GraphicsPath(Point[] pts, byte[] types, FillMode fillMode)
        {
            if (pts == null)
                throw new ArgumentNullException("pts");
            int n = pts.Length;
            if (n != types.Length)
                throw new ArgumentException("Invalid parameter passed. Number of points and types must be same.");
            _fillMode = fillMode;
            for (int i = 0; i < n; ++i)
            {
                _points.Add(pts[i]);
                _types.Add(types[i]);
            }
        }

        public GraphicsPath(PointF[] pts, byte[] types, FillMode fillMode)
        {
            if (pts == null)
                throw new ArgumentNullException("pts");
            int n = pts.Length;
            if (n != types.Length)
                throw new ArgumentException("Invalid parameter passed. Number of points and types must be same.");
            _fillMode = fillMode;
            for (int i = 0; i < n; ++i)
            {
                _points.Add(pts[i]);
                _types.Add(types[i]);
            }
        }
        #endregion

        void OnDataChanged()
        {
            _native = null;
        }

        internal FlashGraphicsPath NativeObject
        {
            get
            {
                EnsureFlashPath();
                return _native;
            }
        }

        void EnsureFlashPath()
        {
            if (_native != null) return;
            var render = new FlashPathRender();
            PathRendering.Render(this, render, PathRenderFeatures.Quad);
            _native = render.path;
        }

        #region Clone
        public GraphicsPath Clone()
        {
            var clone = new GraphicsPath();
            int n = _types.Count;
            for (int i = 0; i < n; i++)
            {
                clone._types.Add(_types[i]);
                clone._points.Add(_points[i]);
            }
            clone._fillMode = _fillMode;
            clone._startNewFig = _startNewFig;
            return clone;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _points = null;
            _types = null;
            _native = null;
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Properties
        public FillMode FillMode
        {
            get
            {
                return _fillMode;
            }
            set
            {
                if ((value < FillMode.Alternate) || (value > FillMode.Winding))
                    throw new InvalidEnumArgumentException("FillMode");
                _fillMode = value;
            }
        }

        public PathData PathData
        {
            get
            {
                return new PathData
                                {
                                    Points = _points.ToArray(),
                                    Types = _types.ToArray()
                                };
            }
        }

        public PointF[] PathPoints
        {
            get { return _points.ToArray(); }
        }

        public byte[] PathTypes
        {
            get { return _types.ToArray(); }
        }

        public int PointCount
        {
            get { return _points.Count; }
        }
        #endregion

        #region Utils
        /* return true if the specified path has (at least one) curves, false otherwise */
        internal bool HasCurve
        {
            get
            {
                int n = _points.Count;
                for (int i = 0; i < n; i++)
                {
                    if (((PathPointType)_types[i] & PathPointType.PathTypeMask) == PathPointType.Bezier)
                        return true;
                }
                return false;
            }
        }

        public PointF GetLastPoint()
        {
            int n = PointCount;
            if (n == 0)
                throw new InvalidOperationException();
            return _points[n - 1];
        }
        #endregion

        #region append methods
        internal void append(float x, float y, PathPointType type, bool compress)
        {
            PathPointType t = type;

            int n = PointCount;
            /* in some case we're allowed to compress identical points */
            if (compress && (n > 0))
            {
                /* points (X, Y) must be identical */
                PointF lastPoint = _points[n - 1];
                if ((lastPoint.X == x) && (lastPoint.Y == y))
                {
                    /* types need not be identical but must handle closed subpaths */
                    var last_type = (PathPointType)_types[n - 1];
                    if ((last_type & PathPointType.CloseSubpath) != PathPointType.CloseSubpath)
                        return;
                }
            }

            if (_startNewFig)
                t = (byte)PathPointType.Start;
                /* if we closed a subpath, then start new figure and append */
            else if (n > 0)
            {
                type = (PathPointType)_types[n - 1];
                if ((type & PathPointType.CloseSubpath) != 0)
                    t = PathPointType.Start;
            }

            _points.Add(new PointF(x, y));
            _types.Add((byte)t);

            _startNewFig = false;

            OnDataChanged();
        }

        internal void append_point(PointF pt, PathPointType type, bool compress)
        {
            append(pt.X, pt.Y, type, compress);
        }

        internal void append_bezier(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            append(x1, y1, PathPointType.Bezier3, false);
            append(x2, y2, PathPointType.Bezier3, false);
            append(x3, y3, PathPointType.Bezier3, false);
        }

        internal void append_bezier(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            append((float)x1, (float)y1, PathPointType.Bezier3, false);
            append((float)x2, (float)y2, PathPointType.Bezier3, false);
            append((float)x3, (float)y3, PathPointType.Bezier3, false);
        }

        internal void append_curve(PointF[] points, PointF[] tangents, int offset, int length, bool close)
        {
            int n = PointCount;
            int i;
            PathPointType ptype = ((close) || (n == 0)) ? PathPointType.Start : PathPointType.Line;

            append_point(points[offset], ptype, true);
            for (i = offset; i < offset + length; i++)
            {
                int j = i + 1;

                double x1 = points[i].X + tangents[i].X;
                double y1 = points[i].Y + tangents[i].Y;

                double x2 = points[j].X - tangents[j].X;
                double y2 = points[j].Y - tangents[j].Y;

                double x3 = points[j].X;
                double y3 = points[j].Y;

                append_bezier((float)x1, (float)y1, (float)x2, (float)y2, (float)x3, (float)y3);
            }

            if (close)
            {
                /* complete (close) the curve using the first point */
                double x1 = points[i].X + tangents[i].X;
                double y1 = points[i].Y + tangents[i].Y;

                double x2 = points[0].X - tangents[0].X;
                double y2 = points[0].Y - tangents[0].Y;

                double x3 = points[0].X;
                double y3 = points[0].Y;

                append_bezier((float)x1, (float)y1, (float)x2, (float)y2, (float)x3, (float)y3);
                CloseFigure();
            }
        }

        internal void append_arc(bool start, float x, float y, float width, float height, float startAngle, float endAngle)
        {
            double delta, bcp;
            double sin_alpha, sin_beta, cos_alpha, cos_beta;

            double rx = width / 2;
            double ry = height / 2;

            /* center */
            double cx = x + rx;
            double cy = y + ry;

            /* angles in radians */
            double alpha = startAngle * Math.PI / 180;
            double beta = endAngle * Math.PI / 180;

            /* adjust angles for ellipses */
            alpha = Math.Atan2(rx * Math.Sin(alpha), ry * Math.Cos(alpha));
            beta = Math.Atan2(rx * Math.Sin(beta), ry * Math.Cos(beta));

            if (Math.Abs(beta - alpha) > Math.PI)
            {
                if (beta > alpha)
                    beta -= 2 * Math.PI;
                else
                    alpha -= 2 * Math.PI;
            }

            delta = beta - alpha;
            // http://www.stillhq.com/ctpfaq/2001/comp.text.pdf-faq-2001-04.txt (section 2.13)
            bcp = 4.0 / 3 * (1 - Math.Cos(delta / 2)) / Math.Sin(delta / 2);

            sin_alpha = Math.Sin(alpha);
            sin_beta = Math.Sin(beta);
            cos_alpha = Math.Cos(alpha);
            cos_beta = Math.Cos(beta);

            /* move to the starting point if we're not continuing a curve */
            if (start)
            {
                /* starting point */
                double sx = cx + rx * cos_alpha;
                double sy = cy + ry * sin_alpha;
                append((float)sx, (float)sy, PathPointType.Line, false);
            }

            append_bezier(
                cx + rx * (cos_alpha - bcp * sin_alpha),
                cy + ry * (sin_alpha + bcp * cos_alpha),
                cx + rx * (cos_beta + bcp * sin_beta),
                cy + ry * (sin_beta - bcp * cos_beta),
                cx + rx * cos_beta,
                cy + ry * sin_beta);
        }

        internal void append_arcs(float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            if (Math.Abs(sweepAngle) >= 360)
            {
                AddEllipse(x, y, width, height);
                return;
            }

            float endAngle = startAngle + sweepAngle;
            int increment = (endAngle < startAngle) ? -90 : 90;

            float drawn = 0;
            bool enough = false;

            /* i is the number of sub-arcs drawn, each sub-arc can be at most 90 degrees.*/
            /* there can be no more then 4 subarcs, ie. 90 + 90 + 90 + (something less than 90) */
            for (int i = 0; i < 4; i++)
            {
                float current = startAngle + drawn;

                if (enough) return;

                float additional = endAngle - current;
                if (Math.Abs(additional) > 90)
                {
                    additional = increment;
                }
                else
                {
                    /* a near zero value will introduce bad artefact in the drawing (#78999) */
                    if (FloatHelper.NearZero(additional))
                        return;

                    enough = true;
                }

                append_arc((i == 0), /* only move to the starting pt in the 1st iteration */
                           x, y, width, height, /* bounding rectangle */
                           current, current + additional);
                drawn += additional;
            }
        }
        #endregion
        
        #region AddArc
        public void AddArc(Rectangle rect, float start_angle, float sweep_angle)
        {
            AddArc((float)rect.X, rect.Y, rect.Width, rect.Height, start_angle, sweep_angle);
        }

        public void AddArc(RectangleF rect, float start_angle, float sweep_angle)
        {
            AddArc(rect.X, rect.Y, rect.Width, rect.Height, start_angle, sweep_angle);
        }

        public void AddArc(int x, int y, int width, int height, float startAngle, float sweepAngle)
        {
            AddArc((float)x, y, width, height, startAngle, sweepAngle);
        }

        public void AddArc(float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            /* draw the arcs */
            append_arcs(x, y, width, height, startAngle, sweepAngle);
        }
        #endregion

        #region AddBezier
        public void AddBezier(Point pt1, Point pt2, Point pt3, Point pt4)
        {
            AddBezier((float)pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
        }

        public void AddBezier(PointF pt1, PointF pt2, PointF pt3, PointF pt4)
        {
            AddBezier(pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
        }

        public void AddBezier(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
        {
            AddBezier((float)x1, y1, x2, y2, x3, y3, x4, y4);
        }

        public void AddBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            append(x1, y1, PathPointType.Line, true);
            append_bezier(x2, y2, x3, y3, x4, y4);
        }
        #endregion

        #region AddBeziers
#if NET_2_0
        public void AddBeziers(params Point[] pts)
#else
                public void AddBeziers (Point [] pts)
#endif
        {
            if (pts == null)
                throw new ArgumentNullException("pts");

            int n = pts.Length;
            /* first bezier requires 4 points, other 3 more points */
            if ((n < 4) || ((n % 3) != 1))
                throw GdipException(InvalidParameter); //

            append_point(pts[0], PathPointType.Line, true);

            for (int i = 1; i < n; i++)
                append_point(pts[i], PathPointType.Bezier3, false);
        }

        public void AddBeziers(PointF[] pts)
        {
            if (pts == null)
                throw new ArgumentNullException("pts");
            
            int n = pts.Length;
            /* first bezier requires 4 points, other 3 more points */
            if ((n < 4) || ((n % 3) != 1))
                throw GdipException(InvalidParameter); //

            append_point(pts[0], PathPointType.Line, true);

            for (int i = 1; i < n; i++)
                append_point(pts[i], PathPointType.Bezier3, false);
        }
        #endregion

        #region AddEllipse
        /* constant for make_ellipse */
        private const double C1 = 0.552285;

        public void AddEllipse(float x, float y, float width, float height)
        {
            double rx = width / 2;
            double ry = height / 2;
            double cx = x + rx;
            double cy = y + ry;

            /* origin */
            append((float)(cx + rx), (float)cy, PathPointType.Start, false);

            /* quadrant I */
            append_bezier(
                          cx + rx, cy - C1 * ry,
                          cx + C1 * rx, cy - ry,
                          cx, cy - ry);

            /* quadrant II */
            append_bezier(
                          cx - C1 * rx, cy - ry,
                          cx - rx, cy - C1 * ry,
                          cx - rx, cy);

            /* quadrant III */
            append_bezier(
                          cx - rx, cy + C1 * ry,
                          cx - C1 * rx, cy + ry,
                          cx, cy + ry);

            /* quadrant IV */
            append_bezier(
                          cx + C1 * rx, cy + ry,
                          cx + rx, cy + C1 * ry,
                          cx + rx, cy);

            /* close the path */
            CloseFigure();
        }

        public void AddEllipse(int x, int y, int width, int height)
        {
            AddEllipse((float)x, y, width, height);
        }

        public void AddEllipse(RectangleF r)
        {
            AddEllipse(r.X, r.Y, r.Width, r.Height);
        }

        public void AddEllipse(Rectangle r)
        {
            AddEllipse((float)r.X, r.Y, r.Width, r.Height);
        }
        #endregion

        #region AddLine, AddLines
        public void AddLine(Point a, Point b)
        {
            AddLine((float)a.X, a.Y, b.X, b.Y);
        }

        public void AddLine(PointF a, PointF b)
        {
            AddLine(a.X, a.Y, b.X, b.Y);
        }

        public void AddLine(int x1, int y1, int x2, int y2)
        {
            AddLine((float)x1, y1, x2, y2);
        }

        public void AddLine(float x1, float y1, float x2, float y2)
        {
            /* only the first point can be compressed (i.e. removed if identical to previous) */
            append(x1, y1, PathPointType.Line, true);
            append(x2, y2, PathPointType.Line, false);
        }
        
        public void AddLines(Point[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            int n = points.Length;
            if (n == 0)
                throw new ArgumentException("points");
            /* only the first point can be compressed (i.e. removed if identical to previous) */
            for (int i = 0; i < n; i++)
            {
                var pt = points[i];
                append(pt.X, pt.Y, PathPointType.Line, (i == 0));
            }
        }

        public void AddLines(PointF[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            int n = points.Length;
            if (n == 0)
                throw new ArgumentException("points");
            /* only the first point can be compressed (i.e. removed if identical to previous) */
            for (int i = 0; i < n; i++)
            {
                var pt = points[i];
                append(pt.X, pt.Y, PathPointType.Line, (i == 0));
            }
        }
        #endregion

        #region AddPie
        public void AddPie(Rectangle rect, float startAngle, float sweepAngle)
        {
            AddPie(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void AddPie(RectangleF rect, float startAngle, float sweepAngle)
        {
            AddPie(rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void AddPie(int x, int y, int width, int height, float startAngle, float sweepAngle)
        {
            AddPie((float)x, y, width, height, startAngle, sweepAngle);
        }

        public void AddPie(float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            double rx = width / 2;
            double ry = height / 2;

            /* center */
            double cx = x + rx;
            double cy = y + ry;

            /* angles in radians */
            double alpha = startAngle * Math.PI / 180;

            /* adjust angle for ellipses */
            alpha = Math.Atan2(rx * Math.Sin(alpha), ry * Math.Cos(alpha));

            double sin_alpha = Math.Sin(alpha);
            double cos_alpha = Math.Cos(alpha);

            /* move to center */
            append((float)cx, (float)cy, PathPointType.Start, false);

            /* draw pie edge */
            if (Math.Abs(sweepAngle) < 360)
                append(
                    (float)(cx + rx * cos_alpha),
                    (float)(cy + ry * sin_alpha),
                    PathPointType.Line, false);

            /* draw the arcs */
            append_arcs(x, y, width, height, startAngle, sweepAngle);

            /* draw pie edge */
            if (Math.Abs(sweepAngle) < 360)
                append((float)cx, (float)cy, PathPointType.Line, false);

            /* close the path */
            CloseFigure();
        }
        #endregion

        #region AddPolygon
        public void AddPolygon(Point[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            int n = points.Length;
            if (n < 3)
                throw GdipException(InvalidParameter);

            /* note: polygon points are never compressed (i.e. removed if identical) */

            append_point(points[0], PathPointType.Start, false);

            for (int i = 1; i < n; i++)
                append_point(points[i], PathPointType.Line, false);

            /*
         * Add a line from the last point back to the first point if
         * they're not the same
         */
            if (points[0].X != points[n - 1].X && points[0].Y != points[n - 1].Y)
                append_point(points[0], PathPointType.Line, false);

            /* close the path */
            CloseFigure();
        }

        public void AddPolygon(PointF[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            int n = points.Length;
            if (n < 3)
                throw GdipException(InvalidParameter);

            /* note: polygon points are never compressed (i.e. removed if identical) */

            append_point(points[0], PathPointType.Start, false);

            for (int i = 1; i < n; i++)
                append_point(points[i], PathPointType.Line, false);

            /*
         * Add a line from the last point back to the first point if
         * they're not the same
         */
            if (points[0].X != points[n - 1].X && points[0].Y != points[n - 1].Y)
                append_point(points[0], PathPointType.Line, false);

            /* close the path */
            CloseFigure();
        }
        #endregion

        #region AddRectangle, AddRectangles
        public void AddRectangle(Rectangle rect)
        {
            AddRectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void AddRectangle(RectangleF rect)
        {
            AddRectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        internal void AddRectangle(float x, float y, float width, float height)
        {
            if ((width == 0) || (height == 0))
                return;

            append(x, y, PathPointType.Start, false);
            append(x + width, y, PathPointType.Line, false);
            append(x + width, y + height, PathPointType.Line, false);
            append(x, y + height, PathPointType.Line | PathPointType.CloseSubpath, false);
        }

        public void AddRectangles(Rectangle[] rects)
        {
            if (rects == null)
                throw new ArgumentNullException("rects");
            int n = rects.Length;
            if (n == 0)
                throw new ArgumentException("rects");
            for (int i = 0; i < n; i++)
                AddRectangle(rects[i]);
        }

        public void AddRectangles(RectangleF[] rects)
        {
            if (rects == null)
                throw new ArgumentNullException("rects");
            int n = rects.Length;
            if (n == 0)
                throw new ArgumentException("rects");
            for (int i = 0; i < n; i++)
                AddRectangle(rects[i]);
        }
        #endregion

        #region AddPath
        /*
         * Return the correct point type when adding a new shape to the path.
         */
        PathPointType GetFirstPointType()
        {
            /* check for a new figure flag or an empty path */
            int n = PointCount;
            if (_startNewFig || (n == 0))
                return PathPointType.Start;

            /* check if the previous point is a closure */
            byte type = _types[n - 1];
            return ((PathPointType)type & PathPointType.CloseSubpath) != 0
                       ? PathPointType.Start
                       : PathPointType.Line;
        }

        public void AddPath(GraphicsPath addingPath, bool connect)
        {
            if (addingPath == null)
                throw new ArgumentNullException("addingPath");

            int length = addingPath.PointCount;
            if (length < 1) return;

            /* We can connect only open figures. If first figure is closed
	        * it can't be connected.
	        */
            PathPointType first = connect ? GetFirstPointType() : PathPointType.Start;

            append_point(addingPath._points[0], first, false);

            for (int i = 1; i < length; i++)
                append_point(addingPath._points[i], (PathPointType)addingPath._types[i], false);
        }
        #endregion

        #region AddClosedCurve
        public void AddClosedCurve(Point[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            AddClosedCurve(points, DefaultTension);
        }

        public void AddClosedCurve(PointF[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            AddClosedCurve(points, DefaultTension);
        }

        public void AddClosedCurve(Point[] points, float tension)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            AddClosedCurve(to_float_points(points), tension);
        }

        public void AddClosedCurve(PointF[] points, float tension)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            int count = points.Length;
            if (count < 3)
                throw GdipException(InvalidParameter);

            var tangents = CurveMath.CalcClosedCurveTangents(CurveMath.CURVE_MIN_TERMS, points, tension);

            append_curve(points, tangents, 0, count - 1, true);

            /* close the path */
            CloseFigure();
        }
        #endregion

        #region AddCurve
        static PointF[] to_float_points(Point[] points)
        {
            int n = points.Length;
            var pt = new PointF[n];
            for (int i = 0; i < n; ++i)
                pt[i] = points[i];
            return pt;
        }

        public void AddCurve(Point[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            AddCurve(points, DefaultTension);
        }

        public void AddCurve(PointF[] points)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            AddCurve(points, DefaultTension);
        }

        public void AddCurve(Point[] points, float tension)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            AddCurve(to_float_points(points), tension);
        }

        public void AddCurve(PointF[] points, float tension)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            int count = points.Length;
            /* special case, here we support a curve with 2 points */
            if (count < 2)
                throw GdipException(InvalidParameter);

            var tangents = CurveMath.CalcOpenCurveTangents(CurveMath.CURVE_MIN_TERMS, points, tension);

            append_curve(points, tangents, 0, count - 1, false);
        }

        public void AddCurve(Point[] points, int offset, int numberOfSegments, float tension)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            AddCurve(to_float_points(points), offset, numberOfSegments, tension);
        }

        public void AddCurve(PointF[] points, int offset, int numberOfSegments, float tension)
        {
            if (points == null)
                throw new ArgumentNullException("points");

            if (numberOfSegments < 1)
                throw GdipException(InvalidParameter);

            int count = points.Length;

            /* we need 3 points for the first curve, 2 more for each curves */
            /* and it's possible to use a point prior to the offset (to calculate) */
            if ((offset == 0) && (numberOfSegments == 1) && (count < 3))
                throw GdipException(InvalidParameter);

            if (numberOfSegments >= count - offset)
                throw GdipException(InvalidParameter);

            var tangents = CurveMath.CalcOpenCurveTangents(CurveMath.CURVE_MIN_TERMS, points, tension);

            append_curve(points, tangents, offset, numberOfSegments, false);
        }
        #endregion

        #region Reset
        public void Reset()
        {
            _fillMode = DefaultFillMode;
            _startNewFig = false;
            _types.Clear();
            _points.Clear();
            OnDataChanged();
        }
        #endregion

        #region Reverse
        public void Reverse()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Transform
        public void Transform(Matrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");

            int count = PointCount;
            if (count == 0)
                return; /* GdipTransformMatrixPoints would fail */

            /* avoid allocation/free/calculation for null/identity matrix */
            if (Matrix.IsEmpty(matrix))
                return;

            var arr = _points.ToArray();
            matrix.TransformPoints(arr);
            _points.Clear();
            _points.AddRange(arr);
            OnDataChanged();
        }
        #endregion

        #region AddString
#if NOT_PFX
        [MonoTODO("The StringFormat parameter is ignored when using libgdiplus.")]
        public void AddString(string s, FontFamily family, int style, float emSize, Point origin, StringFormat format)
        {
            Rectangle layout = new Rectangle();
            layout.X = origin.X;
            layout.Y = origin.Y;
            AddString(s, family, style, emSize, layout, format);
        }

        [MonoTODO("The StringFormat parameter is ignored when using libgdiplus.")]
        public void AddString(string s, FontFamily family, int style, float emSize, PointF origin, StringFormat format)
        {
            RectangleF layout = new RectangleF();
            layout.X = origin.X;
            layout.Y = origin.Y;
            AddString(s, family, style, emSize, layout, format);
        }

        [MonoTODO("The layoutRect and StringFormat parameters are ignored when using libgdiplus.")]
        public void AddString(string s, FontFamily family, int style, float emSize, Rectangle layoutRect, StringFormat format)
        {
            if (family == null)
                throw new ArgumentException("family");

            IntPtr sformat = (format == null) ? IntPtr.Zero : format.NativeObject;
            // note: the NullReferenceException on s.Length is the expected (MS) exception
            Status status = GDIPlus.GdipAddPathStringI(nativePath, s, s.Length, family.NativeObject, style, emSize, ref layoutRect, sformat);
            GDIPlus.CheckStatus(status);
        }

        [MonoTODO("The layoutRect and StringFormat parameters are ignored when using libgdiplus.")]
        public void AddString(string s, FontFamily family, int style, float emSize, RectangleF layoutRect, StringFormat format)
        {
            if (family == null)
                throw new ArgumentException("family");

            IntPtr sformat = (format == null) ? IntPtr.Zero : format.NativeObject;
            // note: the NullReferenceException on s.Length is the expected (MS) exception
            Status status = GDIPlus.GdipAddPathString(nativePath, s, s.Length, family.NativeObject, style, emSize, ref layoutRect, sformat);
            GDIPlus.CheckStatus(status);
        }
#endif
        #endregion

        #region ClearMarkers, SetMarkers
        public void ClearMarkers()
        {
            int n = PointCount;
            /* shortcut to avoid allocations */
            if (n == 0) return;

            var cleared = new List<byte>();

            for (int i = 0; i < n; i++)
            {
                var current = (PathPointType)_types[i];

                /* take out the marker if there is one */
                if ((current & PathPointType.PathMarker) != 0)
                    current &= ~PathPointType.PathMarker;

                cleared.Add((byte)current);
            }

            /* replace the existing with the cleared array */
            _types = cleared;
            OnDataChanged();
        }

        public void SetMarkers()
        {
            int n = PointCount;
            if (n == 0) return;
            _types[n - 1] |= (byte)PathPointType.PathMarker;
        }
        #endregion

        #region CloseAllFigures, CloseFigure
        public void StartFigure()
        {
            _startNewFig = true;
        }

        public void CloseFigure()
        {
            int n = PointCount;
            if (n > 0)
            {
                _types[n - 1] |= (byte)PathPointType.CloseSubpath;
                OnDataChanged();
            }
            _startNewFig = true;
        }

        public void CloseAllFigures()
        {
            int n = PointCount;
            /* first point is not closed */
            if (n <= 1) return;

            var oldTypes = _types;
            _types = new List<byte>();

            int index = 0;
            byte lastType = oldTypes[index];
            index++;

            for (index = 1; index < n; index++)
            {
                byte currentType = oldTypes[index];
                /* we dont close on the first point */
                if ((currentType == (byte)PathPointType.Start) && (index > 1))
                {
                    lastType |= (byte)PathPointType.CloseSubpath;
                    _types.Add(lastType);
                }
                else
                    _types.Add(lastType);

                lastType = currentType;
            }

            /* close at the end */
            lastType |= (byte)PathPointType.CloseSubpath;
            _types.Add(lastType);

            _startNewFig = true;

            OnDataChanged();
        }
        #endregion

        #region Flatten
        public void Flatten()
        {
            Flatten(null, FlatnessDefault);
        }

        public void Flatten(Matrix matrix)
        {
            Flatten(matrix, FlatnessDefault);
        }

        public void Flatten(Matrix matrix, float flatness)
        {
            /* apply matrix before flattening (as there's less points at this stage) */
            if (matrix != null)
            {
                Transform(matrix);
            }

            /* if no bezier are present then the path doesn't need to be flattened */
            if (!HasCurve) return;

            var flat_points = new List<PointF>();
            var flat_types = new List<byte>();

            /* Iterate the current path and replace each bezier with multiple lines */
            int n = PointCount;
            for (int i = 0; i < n; i++)
            {
                PointF point = _points[i];
                byte type = _types[i];

                /* PathPointType.Bezier3 has the same value as PathPointType.Bezier */
                if (((PathPointType)type & PathPointType.Bezier) == PathPointType.Bezier)
                {
                    if (!gdip_convert_bezier_to_lines(this, i, Math.Abs(flatness), flat_points, flat_types))
                    {
                        /* uho, too much recursion - do not pass go, do not collect 200$ */
                        PointF pt = PointF.Empty;

                        /* free the the partial flat */
                        flat_points.Clear();
                        flat_types.Clear();

                        /* mimic MS behaviour when recursion becomes a problem */
                        /* note: it's not really an empty rectangle as the last point isn't closing */

                        type = (byte)PathPointType.Start;
                        flat_points.Add(pt);
                        flat_types.Add(type);

                        type = (byte)PathPointType.Line;
                        flat_points.Add(pt);
                        flat_types.Add(type);

                        flat_points.Add(pt);
                        flat_types.Add(type);

                        flat_points.Add(pt);
                        flat_types.Add(type);
                        break;
                    }

                    /* beziers have 4 points: the previous one, the current and the next two */
                    i += 2;
                }
                else
                {
                    /* no change required, just copy the point */
                    flat_points.Add(point);
                    flat_types.Add(type);
                }
            }

            /* transfer new path informations */
            _points = flat_points;
            _types = flat_types;
            OnDataChanged();

            /* note: no error code is given for excessive recursion */
        }

        static bool gdip_convert_bezier_to_lines(GraphicsPath path, int index, float flatness,
            ICollection<PointF> flat_points, ICollection<byte> flat_types)
        {
            if ((index <= 0) || (index + 2 >= path.PointCount))
                return false; /* bad path data */

            PointF start = path._points[index - 1];
            PointF first = path._points[index];
            PointF second = path._points[index + 1];
            PointF end = path._points[index + 2];

            /* we can't add points directly to the original list as we could end up with too much recursion */
            var points = new List<PointF>();
            if (!CurveMath.nr_curve_flatten(start.X, start.Y, first.X, first.Y, second.X, second.Y, end.X, end.Y, flatness, 0, points))
            {
                /* curved path is too complex (i.e. would result in too many points) to render as a polygon */
                return false;
            }

            /* recursion was within limits, append the result to the original supplied list */
            int n = points.Count;
            if (n > 0)
            {
                flat_points.Add(points[0]);
                flat_types.Add((byte)PathPointType.Line);
            }

            /* always PathPointType.Line */
            for (int i = 1; i < n; i++)
            {
                flat_points.Add(points[i]);
                flat_types.Add((byte)PathPointType.Line);
            }

            return true;
        }
        #endregion

        #region GetBounds
        public RectangleF GetBounds()
        {
            return GetBounds(null, null);
        }

        public RectangleF GetBounds(Matrix matrix)
        {
            return GetBounds(matrix, null);
        }

        public RectangleF GetBounds(Matrix matrix, Pen pen)
        {
            int n = PointCount;
            if (n < 1)
            {
                /* special case #1 - Empty */
                return RectangleF.Empty;
            }

            var workpath = Clone();


            /* We don't need a very precise flat value to get the bounds (GDI+ isn't, big time) -
	 * however flattening helps by removing curves, making the rest of the algorithm a 
	 * lot simpler.
	 */

            /* note: only the matrix is applied if no curves are present in the path */
            workpath.Flatten(matrix, 25.0f);
            int i;
            PointF pt;

            pt = workpath._points[0];
            RectangleF bounds = new RectangleF();
            bounds.X = pt.X; /* keep minimum X here */
            bounds.Y = pt.Y; /* keep minimum Y here */
            if (workpath.PointCount == 1)
            {
                /* special case #2 - Only one element */
                bounds.Width = 0.0f;
                bounds.Height = 0.0f;
                return bounds;
            }

            bounds.Width = pt.X; /* keep maximum X here */
            bounds.Height = pt.Y; /* keep maximum Y here */

            for (i = 1; i < workpath.PointCount; i++)
            {
                pt = workpath._points[i];
                if (pt.X < bounds.X)
                    bounds.X = pt.X;
                if (pt.Y < bounds.Y)
                    bounds.Y = pt.Y;
                if (pt.X > bounds.Width)
                    bounds.Width = pt.X;
                if (pt.Y > bounds.Height)
                    bounds.Height = pt.Y;
            }

            /* convert maximum values (width/height) as length */
            bounds.Width -= bounds.X;
            bounds.Height -= bounds.Y;

            if (pen != null)
            {
                /* in calculation the pen's width is at least 1.0 */
                float width = (pen.Width < 1.0f) ? 1.0f : pen.Width;
                float halfw = (width / 2);

                bounds.X -= halfw;
                bounds.Y -= halfw;
                bounds.Width += width;
                bounds.Height += width;
            }

            return bounds;
        }
        #endregion

        #region IsOutlineVisible
        public bool IsOutlineVisible(Point point, Pen pen)
        {
            return IsOutlineVisible(point.X, point.Y, pen, null);
        }

        public bool IsOutlineVisible(PointF point, Pen pen)
        {
            return IsOutlineVisible(point.X, point.Y, pen, null);
        }

        public bool IsOutlineVisible(int x, int y, Pen pen)
        {
            return IsOutlineVisible(x, y, pen, null);
        }

        public bool IsOutlineVisible(float x, float y, Pen pen)
        {
            return IsOutlineVisible(x, y, pen, null);
        }

        public bool IsOutlineVisible(Point pt, Pen pen, Graphics graphics)
        {
            return IsOutlineVisible(pt.X, pt.Y, pen, graphics);
        }

        public bool IsOutlineVisible(PointF pt, Pen pen, Graphics graphics)
        {
            return IsOutlineVisible(pt.X, pt.Y, pen, graphics);
        }

        public bool IsOutlineVisible(int x, int y, Pen pen, Graphics graphics)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            //bool result;
            //IntPtr g = (graphics == null) ? IntPtr.Zero : graphics.nativeObject;
            //Status s = GDIPlus.GdipIsOutlineVisiblePathPointI(nativePath, x, y, pen.nativeObject, g, out result);
            //GDIPlus.CheckStatus(s);

            //return result;
            throw new NotImplementedException();
        }

        public bool IsOutlineVisible(float x, float y, Pen pen, Graphics graphics)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            //bool result;
            //IntPtr g = (graphics == null) ? IntPtr.Zero : graphics.nativeObject;

            //Status s = GDIPlus.GdipIsOutlineVisiblePathPoint(nativePath, x, y, pen.nativeObject, g, out result);
            //GDIPlus.CheckStatus(s);

            //return result;
            throw new NotImplementedException();
        }
        #endregion

        #region IsVisible
        public bool IsVisible(Point point)
        {
            return IsVisible(point.X, point.Y, null);
        }

        public bool IsVisible(PointF point)
        {
            return IsVisible(point.X, point.Y, null);
        }

        public bool IsVisible(int x, int y)
        {
            return IsVisible(x, y, null);
        }

        public bool IsVisible(float x, float y)
        {
            return IsVisible(x, y, null);
        }

        public bool IsVisible(Point pt, Graphics graphics)
        {
            return IsVisible(pt.X, pt.Y, graphics);
        }

        public bool IsVisible(PointF pt, Graphics graphics)
        {
            return IsVisible(pt.X, pt.Y, graphics);
        }

        public bool IsVisible(int x, int y, Graphics graphics)
        {
            //bool retval;
            //IntPtr g = (graphics == null) ? IntPtr.Zero : graphics.nativeObject;
            //Status s = GDIPlus.GdipIsVisiblePathPointI(nativePath, x, y, g, out retval);
            //GDIPlus.CheckStatus(s);
            //return retval;
            throw new NotImplementedException();
        }

        public bool IsVisible(float x, float y, Graphics graphics)
        {
            //bool retval;
            //IntPtr g = (graphics == null) ? IntPtr.Zero : graphics.nativeObject;
            //Status s = GDIPlus.GdipIsVisiblePathPoint(nativePath, x, y, g, out retval);
            //GDIPlus.CheckStatus(s);
            //return retval;
            throw new NotImplementedException();
        }
        #endregion

        #region Prepare
        void Prepare(Matrix matrix, float flatness)
        {
            /* convert any curve into lines */
            if (HasCurve)
            {
                /* this will apply the matrix too (before flattening) */
                Flatten(matrix, flatness);
                return;
            }

            if (!Matrix.IsEmpty(matrix))
            {
                /* no curve, but we still have a matrix to apply... */
                Transform(matrix);
                return;
            }

            /* no preparation required */
        }
        #endregion

        #region Warp
        [MonoTODO("GdipWarpPath isn't implemented in libgdiplus")]
        public void Warp(PointF[] destPoints, RectangleF srcRect)
        {
            Warp(destPoints, srcRect, null, WarpMode.Perspective, FlatnessDefault);
        }

        [MonoTODO("GdipWarpPath isn't implemented in libgdiplus")]
        public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix)
        {
            Warp(destPoints, srcRect, matrix, WarpMode.Perspective, FlatnessDefault);
        }

        [MonoTODO("GdipWarpPath isn't implemented in libgdiplus")]
        public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix, WarpMode warpMode)
        {
            Warp(destPoints, srcRect, matrix, warpMode, FlatnessDefault);
        }

        [MonoTODO("GdipWarpPath isn't implemented in libgdiplus")]
        public void Warp(PointF[] destPoints, RectangleF srcRect, Matrix matrix, WarpMode warpMode, float flatness)
        {
            if (destPoints == null)
                throw new ArgumentNullException("destPoints");

            int count = destPoints.Length;
            if (count < 1)
                throw GdipException(InvalidParameter);

            /* quick out */
            int n = PointCount;
            if (n == 0) return;

            /* an invalid warp mode resets the current path */
            /* a path with a single point will reset it too */
            if (((warpMode != WarpMode.Perspective) && (warpMode != WarpMode.Bilinear)) || (n == 1))
            {
                Reset();
                return;
            }

            Prepare(matrix, flatness);
            
            /* TODO */

            throw new NotImplementedException();
        }
        #endregion

        #region Widen
        [MonoTODO("GdipWidenPath isn't implemented in libgdiplus")]
        public void Widen(Pen pen)
        {
            Widen(pen, null, FlatnessDefault);
        }

        [MonoTODO("GdipWidenPath isn't implemented in libgdiplus")]
        public void Widen(Pen pen, Matrix matrix)
        {
            Widen(pen, matrix, FlatnessDefault);
        }

        [MonoTODO("GdipWidenPath isn't implemented in libgdiplus")]
        public void Widen(Pen pen, Matrix matrix, float flatness)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");

            int n = PointCount;
#if NET_2_0
            if (n == 0)
                return;
#endif
            /* (0) is deal within System.Drawing */
            /* (1) for compatibility with MS GDI+ (reported as FDBK49685) */
            if (n <= 1)
                throw GdipException(OutOfMemory);

            Prepare(matrix, flatness);
            
            /* TODO inner path (same number of points as the prepared path) */

            /* TODO outer path (twice the number of points as the prepared path) */

            throw new NotImplementedException();
        }
        #endregion
    }
}
