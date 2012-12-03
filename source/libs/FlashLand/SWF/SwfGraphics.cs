using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using DataDynamics.PageFX.Common.Graphics;
using DataDynamics.PageFX.FlashLand.Swf.Tags;
using DataDynamics.PageFX.FlashLand.Swf.Tags.Shapes;

namespace DataDynamics.PageFX.FlashLand.Swf
{
    public sealed class SwfGraphics
    {
        internal SwfGraphics(SwfMovie swf)
        {
            _swf = swf;
        }
        private readonly SwfMovie _swf;

        #region DrawPath & FillPath
        private SwfTagDefineShape CreateShapeTag()
        {
            int ver = _swf.Version;
            if (ver <= 1) return new SwfTagDefineShape();
            if (ver == 2) return new SwfTagDefineShape2();
            return new SwfTagDefineShape3();
        }

        private void AddObject(ISwfDisplayObject obj)
        {
            var tag = obj as SwfTag;
            if (tag == null)
                throw new ArgumentException();
            obj.CharacterID = ++_swf.CID;
            _swf.Tags.Add(tag);
            _swf.PlaceObject(obj, null);
        }

        public void DrawPath(Pen pen, GraphicsPath path)
        {
            switch (pen.PenType)
            {
                case PenType.SolidColor:
                    {
                        var tag = CreateShapeTag();
                        tag.LineStyles.Add(pen.Color, pen.Width);
                        tag.Shape.AddPath(path, PathPainting.Stroke);
                        AddObject(tag);
                    }
                    break;

                default:
                    throw new NotSupportedException();
            }
        }

        private SwfFillStyle DefineFillStyle(Brush brush)
        {
            var sb = brush as SolidBrush;
            if (sb != null)
                return new SwfSolidFillStyle(sb.Color);

            var lg = brush as LinearGradientBrush;
            if (lg != null)
                return new SwfGradientFillStyle(lg);

            var tb = brush as TextureBrush;
            if (tb != null)
            {
                var image = tb.Image;
                ushort id = _swf.DefineImage(image);

                var wm = tb.WrapMode;
                var kind = SwfFillKind.RepeatingBitmap;
                if (wm == WrapMode.Clamp)
                    kind = SwfFillKind.ClippedBitmap;

                var fs = new SwfTextureFillStyle(id, kind);

                //TODO: Flip
                var m = new Matrix();
                //m.Translate(-image.Width / 2, -image.Height / 2);
                m.Scale(20, 20);
                fs.Matrix = m;
                return fs;
            }

            var pg = brush as PathGradientBrush;
            if (pg != null)
            {
                throw new NotImplementedException();
            }

            return null;
        }

        public void FillPath(Brush brush, GraphicsPath path)
        {
            var fs = DefineFillStyle(brush);
            var tag = CreateShapeTag();
            tag.FillStyles.Add(fs);
            tag.Shape.AddPath(path, PathPainting.Fill);
            AddObject(tag);
        }
        #endregion

        #region Draw Methods (System.Drawing.Graphics)
        #region DrawLine / DrawLines
        /// <summary>
        /// Draws a line connecting the two points specified by coordinate pairs.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the line.</param>
        /// <param name="x1">x-coordinate of the first point.</param>
        /// <param name="y1">y-coordinate of the first point.</param>
        /// <param name="x2">x-coordinate of the second point.</param>
        /// <param name="y2">y-coordinate of the second point.</param>
        /// <remarks>
        /// This method draws a line connecting the two points specified by the x1, y1, x2, and y2 parameters.
        /// </remarks>
        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
        {
            DrawLine(pen, x1, y1, x2, (float)y1);
        }

        /// <summary>
        /// Draws a line connecting the two points specified by coordinate pairs.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the line.</param>
        /// <param name="x1">Pen object that determines the color, width, and style of the line.</param>
        /// <param name="y1">y-coordinate of the first point.</param>
        /// <param name="x2">x-coordinate of the second point.</param>
        /// <param name="y2">y-coordinate of the second point.</param>
        /// <remarks>
        /// This method draws a line connecting the two points specified by the x1, y1, x2, and y2 parameters.
        /// </remarks>
        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            using (var path = new GraphicsPath())
            {
                path.AddLine(x1, y1, x2, y2);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a line connecting two Point structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the line.</param>
        /// <param name="pt1">Point structure that represents the first point to connect.</param>
        /// <param name="pt2">Point structure that represents the second point to connect.</param>
        public void DrawLine(Pen pen, Point pt1, Point pt2)
        {
            DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        /// <summary>
        /// Draws a line connecting two PointF structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the line.</param>
        /// <param name="pt1">PointF structure that represents the first point to connect.</param>
        /// <param name="pt2">PointF structure that represents the second point to connect.</param>
        public void DrawLine(Pen pen, PointF pt1, PointF pt2)
        {
            DrawLine(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        /// <summary>
        /// Draws a series of line segments that connect an array of Point structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the line segments.</param>
        /// <param name="points">Array of Point structures that represent the points to connect.</param>
        /// <remarks>
        /// This method draws a series of lines connecting an array of ending points. The first two points in the array specify the first line. 
        /// Each additional point specifies the end of a line segment whose starting point is the ending point of the previous line segment.
        /// </remarks>
        public void DrawLines(Pen pen, Point[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddLines(points);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a series of line segments that connect an array of PointF structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the line segments.</param>
        /// <param name="points">Array of PointF structures that represent the points to connect.</param>
        /// <remarks>
        /// This method draws a series of lines connecting an array of ending points. The first two points in the array specify the first line. 
        /// Each additional point specifies the end of a line segment whose starting point is the ending point of the previous line segment.
        /// </remarks>
        public void DrawLines(Pen pen, PointF[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddLines(points);
                DrawPath(pen, path);
            }
        }
        #endregion

        #region DrawPolygon
        /// <summary>
        /// Draws a polygon defined by an array of Point structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the polygon.</param>
        /// <param name="points">Array of Point structures that represent the vertices of the polygon.</param>
        /// <remarks>
        /// Every pair of two consecutive points in the array specify a side of the polygon. 
        /// In addition, if the last point and the first point of the array do not coincide, 
        /// they specify the last side of the polygon.
        /// </remarks>
        public void DrawPolygon(Pen pen, Point[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPolygon(points);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a polygon defined by an array of PointF structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the polygon.</param>
        /// <param name="points">Array of PointF structures that represent the vertices of the polygon.</param>
        /// <remarks>
        /// Every pair of two consecutive points in the array specify a side of the polygon. 
        /// In addition, if the last point and the first point of the array do not coincide, 
        /// they specify the last side of the polygon.
        /// </remarks>
        public void DrawPolygon(Pen pen, PointF[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPolygon(points);
                DrawPath(pen, path);
            }
        }
        #endregion

        #region DrawRectangle / DrawRectangles
        /// <summary>
        /// Draws a rectangle specified by a coordinate pair, a width, and a height.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the rectangle.</param>
        /// <param name="x">x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">Width of the rectangle to draw.</param>
        /// <param name="height">Height of the rectangle to draw.</param>
        public void DrawRectangle(Pen pen, int x, int y, int width, int height)
        {
            DrawRectangle(pen, new RectangleF(x, y, width, height));
        }

        /// <summary>
        /// Draws a rectangle specified by a coordinate pair, a width, and a height.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the rectangle.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">The width of the rectangle to draw.</param>
        /// <param name="height">The height of the rectangle to draw.</param>
        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            DrawRectangle(pen, new RectangleF(x, y, width, height));
        }

        /// <summary>
        /// Draws a rectangle specified by a Rectangle structure.
        /// </summary>
        /// <param name="pen">A Pen object that determines the color, width, and style of the rectangle.</param>
        /// <param name="rect">A Rectangle structure that represents the rectangle to draw.</param>
        public void DrawRectangle(Pen pen, Rectangle rect)
        {
            DrawRectangle(pen, new RectangleF(rect.X, rect.Y, rect.Width, rect.Height));
        }

        /// <summary>
        /// Draws a rectangle specified by a RectangleF structure.
        /// </summary>
        /// <param name="pen">A Pen object that determines the color, width, and style of the rectangle.</param>
        /// <param name="rect">A RectangleF structure that represents the rectangle to draw.</param>
        public void DrawRectangle(Pen pen, RectangleF rect)
        {
            using (var path = new GraphicsPath())
            {
                path.AddRectangle(rect);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a series of rectangles specified by Rectangle structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the outlines of the rectangles.</param>
        /// <param name="rects">Array of Rectangle structures that represent the rectangles to draw.</param>
        public void DrawRectangles(Pen pen, Rectangle[] rects)
        {
            using (var path = new GraphicsPath())
            {
                path.AddRectangles(rects);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a series of rectangles specified by RectangleF structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the outlines of the rectangles.</param>
        /// <param name="rects">Array of RectangleF structures that represent the rectangles to draw.</param>
        public void DrawRectangles(Pen pen, RectangleF[] rects)
        {
            using (var path = new GraphicsPath())
            {
                path.AddRectangles(rects);
                DrawPath(pen, path);
            }
        }
        #endregion

        #region DrawRoundRectangle
        /// <summary>
        /// Draws a round rectangle specified by a coordinate pair, a width, and a height.
        /// </summary>
        /// <param name="x">x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">Width of the rectangle to draw.</param>
        /// <param name="height">Height of the rectangle to draw.</param>
        /// <param name="rx">x component of coner radius.</param>
        /// <param name="ry">y component of coner radius.</param>
        public void DrawRoundRectangle(Pen pen, int x, int y, int width, int height, int rx, int ry)
        {
            DrawRoundRectangle(pen, x, y, width, height, rx, (float)ry);
        }

        /// <summary>
        /// Draws a round rectangle specified by a coordinate pair, a width, and a height.
        /// </summary>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">The width of the rectangle to draw.</param>
        /// <param name="height">The height of the rectangle to draw.</param>
        /// <param name="rx">x component of coner radius.</param>
        /// <param name="ry">y component of coner radius.</param>
        public void DrawRoundRectangle(Pen pen, float x, float y, float width, float height, float rx, float ry)
        {
            using (var path = KnownPathes.RoundRect(x, y, width, height, rx, ry))
                DrawPath(pen, path);
        }

        /// <summary>
        /// Draws a round rectangle specified by a System.Drawing.Rectangle structure.
        /// </summary>
        /// <param name="rect">A <see cref="Rectangle"/> structure that represents the rectangle to draw.</param>
        /// <param name="rx">x component of coner radius.</param>
        /// <param name="ry">y component of coner radius.</param>
        public void DrawRoundRectangle(Pen pen, Rectangle rect, int rx, int ry)
        {
            DrawRoundRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height, rx, ry);
        }

        /// <summary>
        /// Draws a round rectangle specified by a System.Drawing.RectangleF structure.
        /// </summary>
        /// <param name="rect">A <see cref="RectangleF"/> structure that represents the rectangle to draw.</param>
        /// <param name="rx">x component of coner radius.</param>
        /// <param name="ry">y component of coner radius.</param>
        public void DrawRoundRectangle(Pen pen, RectangleF rect, float rx, float ry)
        {
            DrawRoundRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height, rx, ry);
        }
        #endregion

        #region DrawBezier / DrawBeziers
        /// <summary>
        /// Draws a Bezier spline defined by four Point structures.
        /// </summary>
        /// <param name="pen">Pen structure that determines the color, width, and style of the curve.</param>
        /// <param name="pt1">Point structure that represents the starting point of the curve.</param>
        /// <param name="pt2">Point structure that represents the first control point for the curve.</param>
        /// <param name="pt3">Point structure that represents the second control point for the curve.</param>
        /// <param name="pt4">Point structure that represents the ending point of the curve.</param>
        /// <remarks>
        /// The Bezier curve is drawn from the first point to the fourth point. 
        /// The second and third points are control points that determine the shape of the curve.
        /// </remarks>
        public void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4)
        {
            using (var path = new GraphicsPath())
            {
                path.AddBezier(pt1, pt2, pt3, pt4);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a Bezier spline defined by four PointF structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the curve.</param>
        /// <param name="pt1">PointF structure that represents the starting point of the curve.</param>
        /// <param name="pt2">PointF structure that represents the first control point for the curve.</param>
        /// <param name="pt3">PointF structure that represents the second control point for the curve.</param>
        /// <param name="pt4">PointF structure that represents the ending point of the curve.</param>
        /// <remarks>
        /// The Bezier curve is drawn from the first point to the fourth point. 
        /// The second and third points are control points that determine the shape of the curve.
        /// </remarks>
        public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4)
        {
            using (var path = new GraphicsPath())
            {
                path.AddBezier(pt1, pt2, pt3, pt4);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a Bezier spline defined by four ordered pairs of coordinates that represent points.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the curve.</param>
        /// <param name="x1">x-coordinate of the starting point of the curve.</param>
        /// <param name="y1">y-coordinate of the starting point of the curve.</param>
        /// <param name="x2">x-coordinate of the first control point of the curve.</param>
        /// <param name="y2">y-coordinate of the first control point of the curve.</param>
        /// <param name="x3">x-coordinate of the second control point of the curve.</param>
        /// <param name="y3">y-coordinate of the second control point of the curve.</param>
        /// <param name="x4">x-coordinate of the ending point of the curve.</param>
        /// <param name="y4">y-coordinate of the ending point of the curve.</param>
        /// <remarks>
        /// The Bezier curve is drawn from the first point to the fourth point. 
        /// The second and third points are control points that determine the shape of the curve.
        /// </remarks>
        public void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            using (var path = new GraphicsPath())
            {
                path.AddBezier(x1, y1, x2, y2, x3, y3, x4, y4);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a series of Bezier splines from an array of Point structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the curve.</param>
        /// <param name="points">Array of Point structures that represent the points that determine the curve.</param>
        /// <remarks>
        /// The first Bezier spline is drawn from the first point to the fourth point in the point array. The second and third points are control points 
        /// that determine the shape of the curve. Each subsequent curve needs exactly three more points: two more control points and an ending point. 
        /// The ending point of the previous curve is used as the starting point for each additional curve.
        /// </remarks>
        public void DrawBeziers(Pen pen, Point[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddBeziers(points);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a series of Bezier splines from an array of PointF structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the curve.</param>
        /// <param name="points">Array of PointF structures that represent the points that determine the curve.</param>
        /// <remarks>
        /// The first Bezier curve is drawn from the first point to the fourth point in the point array. 
        /// The second and third points are control points that determine the shape of the curve. 
        /// Each subsequent curve needs exactly three more points: two more control points and an ending point. 
        /// The ending point of the previous curve is used as the starting point for each additional curve.
        /// </remarks>
        public void DrawBeziers(Pen pen, PointF[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddBeziers(points);
                DrawPath(pen, path);
            }
        }
        #endregion

        #region DrawArc
        /// <summary>
        /// Draws an arc representing a portion of an ellipse specified by a pair of coordinates, a width, and a height.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the arc.</param>
        /// <param name="x">x-coordinate of the upper-left corner of the rectangle that defines the ellipse.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the rectangle that defines the ellipse.</param>
        /// <param name="width">Width of the rectangle that defines the ellipse.</param>
        /// <param name="height">Height of the rectangle that defines the ellipse.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the starting point of the arc.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the startAngle parameter to ending point of the arc.</param>
        /// <remarks>
        /// This method draws an arc that is a portion of the perimeter of an ellipse. The ellipse is defined by the boundaries of a rectangle. 
        /// The arc is the portion of the perimeter of the ellipse between the startAngle parameter and the startAngle + sweepAngle parameters.
        /// </remarks>
        public void DrawArc(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            using (var path = new GraphicsPath())
            {
                path.AddArc(x, y, width, height, startAngle, sweepAngle);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws an arc representing a portion of an ellipse specified by a pair of coordinates, a width, and a height.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the arc.</param>
        /// <param name="x">x-coordinate of the upper-left corner of the rectangle that defines the ellipse.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the rectangle that defines the ellipse.</param>
        /// <param name="width">Width of the rectangle that defines the ellipse.</param>
        /// <param name="height">Height of the rectangle that defines the ellipse.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the starting point of the arc.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the startAngle parameter to ending point of the arc.</param>
        /// <remarks>
        /// This method draws an arc that is a portion of the perimeter of an ellipse. The ellipse is defined by the boundaries of a rectangle. 
        /// The arc is the portion of the perimeter of the ellipse between the startAngle parameter and the startAngle + sweepAngle parameters.
        /// </remarks>
        public void DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            using (var path = new GraphicsPath())
            {
                path.AddArc(x, y, width, height, startAngle, sweepAngle);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws an arc representing a portion of an ellipse specified by a Rectangle structure.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the arc.</param>
        /// <param name="rect">RectangleF structure that defines the boundaries of the ellipse.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the starting point of the arc.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the startAngle parameter to ending point of the arc.</param>
        /// <remarks>
        /// This method draws an arc that is a portion of the perimeter of an ellipse. The ellipse is defined by the boundaries of a rectangle. 
        /// The arc is the portion of the perimeter of the ellipse between the startAngle parameter and the startAngle + sweepAngle parameters.
        /// </remarks>
        public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            DrawArc(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        /// <summary>
        /// Draws an arc representing a portion of an ellipse specified by a RectangleF structure.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the arc.</param>
        /// <param name="rect">RectangleF structure that defines the boundaries of the ellipse.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the starting point of the arc.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the startAngle parameter to ending point of the arc.</param>
        public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            DrawArc(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }
        #endregion

        #region DrawClosedCurve
        /// <summary>
        /// Draws a closed cardinal spline defined by an array of Point structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and height of the curve.</param>
        /// <param name="points">Array of Point structures that define the spline.</param>
        /// <remarks>
        /// This method draws a closed cardinal spline that passes through each point in the array. 
        /// If the last point does not match the first point, an additional curve segment is added from the last point to the first point to close the figure.
        /// The array of points must contain at least four Point structures.
        /// This method uses a default tension of 0.5.
        /// </remarks>
        public void DrawClosedCurve(Pen pen, Point[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a closed cardinal spline defined by an array of PointF structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and height of the curve.</param>
        /// <param name="points">Array of PointF structures that define the spline.</param>
        /// <remarks>
        /// This method draws a closed cardinal spline that passes through each point in the array. 
        /// If the last point does not match the first point, an additional curve segment is added from the last point to the first point to close it.
        /// The array of points must contain at least four PointF structures.
        /// This method uses a default tension of 0.5.
        /// </remarks>
        public void DrawClosedCurve(Pen pen, PointF[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a closed cardinal spline defined by an array of Point structures using a specified tension.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and height of the curve.</param>
        /// <param name="points">Array of Point structures that define the spline.</param>
        /// <param name="tension">Pattern greater than or equal to 0.0F that specifies the tension of the curve.</param>
        /// <param name="fillmode">Member of the WmfFillMode enumeration that determines how the curve is filled. This parameter is required but ignored.</param>
        /// <remarks>
        /// This method draws a closed cardinal spline that passes through each point in the array. 
        /// If the last point does not match the first point, an additional curve segment is added from the last point to the first point to close it.
        /// The array of points must contain at least four Point structures.
        /// The tension parameter determines the shape of the spline. 
        /// If the value of the tension parameter is 0.0F, this method draws straight line segments to connect the points. 
        /// Usually, the tension parameter is less than or equal to 1.0F. Values over 1.0F produce unusual results.
        /// </remarks>
        public void DrawClosedCurve(Pen pen, Point[] points, float tension, FillMode fillmode)
        {
            using (var path = new GraphicsPath(fillmode))
            {
                path.AddClosedCurve(points, tension);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a closed cardinal spline defined by an array of PointF structures using a specified tension.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and height of the curve.</param>
        /// <param name="points">Array of PointF structures that define the spline.</param>
        /// <param name="tension">Pattern greater than or equal to 0.0F that specifies the tension of the curve.</param>
        /// <param name="fillmode">Member of the WmfFillMode enumeration that determines how the curve is filled. This parameter is required but is ignored.</param>
        /// <remarks>
        /// This method draws a closed cardinal spline that passes through each point in the array. 
        /// If the last point does not match the first point, an additional curve segment is added from the last point to the first point to close it.
        /// The array of points must contain at least four PointF structures.
        /// The tension parameter determines the shape of the spline. 
        /// If the value of the tension parameter is 0.0F, this method draws straight line segments to connect the points. 
        /// Usually, the tension parameter is less than or equal to 1.0F. Values over 1.0F produce unusual results.
        /// </remarks>
        public void DrawClosedCurve(Pen pen, PointF[] points, float tension, FillMode fillmode)
        {
            using (var path = new GraphicsPath(fillmode))
            {
                path.AddClosedCurve(points, tension);
                DrawPath(pen, path);
            }
        }
        #endregion

        #region DrawCurve
        /// <summary>
        /// Draws a cardinal spline through a specified array of Point structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and height of the curve.</param>
        /// <param name="points">Array of Point structures that define the spline.</param>
        /// <remarks>
        /// This method draws a cardinal spline that passes through each point in the array.
        /// The array of points must contain at least four Point structures.
        /// This method uses a default tension of 0.5.
        /// </remarks>
        public void DrawCurve(Pen pen, Point[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddCurve(points);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a cardinal spline through a specified array of PointF structures.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and height of the curve.</param>
        /// <param name="points">Array of PointF structures that define the spline.</param>
        /// <remarks>
        /// This method draws a cardinal spline that passes through each point in the array.
        /// The array of points must contain at least four PointF structures.
        /// This method uses a default tension of 0.5.
        /// </remarks>
        public void DrawCurve(Pen pen, PointF[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddCurve(points);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a cardinal spline through a specified array of Point structures using a specified tension.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and height of the curve.</param>
        /// <param name="points">Array of Point structures that define the spline.</param>
        /// <param name="tension">Pattern greater than or equal to 0.0F that specifies the tension of the curve.</param>
        /// <remarks>
        /// This method draws a cardinal spline that passes through each point in the array.
        /// The array of points must contain at least four Point structures.
        /// The tension parameter determines the shape of the spline. 
        /// If the value of the tension parameter is 0.0F, this method draws straight line segments to connect the points. 
        /// Usually, the tension parameter is less than or equal to 1.0F. Values over 1.0F produce unusual results.
        /// </remarks>
        public void DrawCurve(Pen pen, Point[] points, float tension)
        {
            using (var path = new GraphicsPath())
            {
                path.AddCurve(points, tension);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a cardinal spline through a specified array of PointF structures using a specified tension.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and height of the curve.</param>
        /// <param name="points">Array of PointF structures that define the spline.</param>
        /// <param name="tension">Pattern greater than or equal to 0.0F that specifies the tension of the curve.</param>
        /// <remarks>
        /// This method draws a cardinal spline that passes through each point in the array.
        /// The array of points must contain at least four PointF structures.
        /// The tension parameter determines the shape of the spline. 
        /// If the value of the tension parameter is 0.0F, this method draws straight line segments to connect the points. 
        /// Usually, the tension parameter is less than or equal to 1.0F. Values over 1.0F produce unusual results.
        /// </remarks>
        public void DrawCurve(Pen pen, PointF[] points, float tension)
        {
            using (var path = new GraphicsPath())
            {
                path.AddCurve(points, tension);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a cardinal spline through a specified array of PointF structures. The drawing begins offset from the beginning of the array.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and height of the curve.</param>
        /// <param name="points">Array of PointF structures that define the spline.</param>
        /// <param name="offset">Offset from the first element in the array of the points parameter to the starting point in the curve.</param>
        /// <param name="numberOfSegments">Number of segments after the starting point to include in the curve.</param>
        /// <remarks>
        /// This method draws a cardinal spline that passes through each point in the array.
        /// The array of points must contain at least four PointF structures.
        /// The value of the offset parameter specifies the number of elements to skip in the array. The first element after the skipped elements represents the starting point of the curve.
        /// The value of the numberOfSegments parameter specifies the number of segments, after the starting point, to draw in the curve. The value of the numberOfSegments parameter must be at least 1. The value of the offset parameter plus the value of the numberOfSegments parameter must be less than the number of elements in the array of the points parameter.
        /// This method uses a default tension of 0.5.
        /// </remarks>
        public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments)
        {
            DrawCurve(pen, points, offset, numberOfSegments, 0.5f);
        }

        /// <summary>
        /// Draws a cardinal spline through a specified array of Point structures using a specified tension.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and height of the curve.</param>
        /// <param name="points">Array of Point structures that define the spline.</param>
        /// <param name="offset">Offset from the first element in the array of the points parameter to the starting point in the curve.</param>
        /// <param name="numberOfSegments">Number of segments after the starting point to include in the curve.</param>
        /// <param name="tension">Pattern greater than or equal to 0.0F that specifies the tension of the curve.</param>
        /// <remarks>
        /// This method draws a cardinal spline that passes through each point in the array.
        /// The array of points must contain at least four Point structures.
        /// The value of the offset parameter specifies the number of elements to skip in the array. The first element after the skipped elements represents the starting point of the curve.
        /// The value of the numberOfSegments parameter specifies the number of segments, after the starting point, to draw in the curve. The value of the numberOfSegments parameter must be at least 1. The value of the offset parameter plus the value of the numberOfSegments parameter must be less than the number of elements in the array of the points parameter.
        /// The tension parameter determines the shape of the spline. If the value of the tension parameter is 0.0F, this method draws straight line segments to connect the points. Usually, the tension parameter is less than or equal to 1.0F. Values over 1.0F produce unusual results.
        /// </remarks>
        public void DrawCurve(Pen pen, Point[] points, int offset, int numberOfSegments, float tension)
        {
            using (var path = new GraphicsPath())
            {
                path.AddCurve(points, offset, numberOfSegments, tension);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a cardinal spline through a specified array of PointF structures using a specified tension.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and height of the curve.</param>
        /// <param name="points">Array of PointF structures that define the spline.</param>
        /// <param name="offset">Offset from the first element in the array of the points parameter to the starting point in the curve.</param>
        /// <param name="numberOfSegments">Number of segments after the starting point to include in the curve.</param>
        /// <param name="tension">Pattern greater than or equal to 0.0F that specifies the tension of the curve.</param>
        /// <remarks>
        /// This method draws a cardinal spline that passes through each point in the array.
        /// The array of points must contain at least four PointF structures.
        /// The value of the offset parameter specifies the number of elements to skip in the array. The first element after the skipped elements represents the starting point of the curve.
        /// The value of the numberOfSegments parameter specifies the number of segments, after the starting point, to draw in the curve. The value of the numberOfSegments parameter must be at least 1. The value of the offset parameter plus the value of the numberOfSegments parameter must be less than the number of elements in the array of the points parameter.
        /// The tension parameter determines the shape of the spline. If the value of the tension parameter is 0.0F, this method draws straight line segments to connect the points. Usually, the tension parameter is less than or equal to 1.0F. Values over 1.0F produce unusual results.
        /// </remarks>
        public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments, float tension)
        {
            using (var path = new GraphicsPath())
            {
                path.AddCurve(points, offset, numberOfSegments, tension);
                DrawPath(pen, path);
            }
        }
        #endregion

        #region DrawEllipse
        /// <summary>
        /// Draws an ellipse defined by a bounding rectangle specified by a pair of coordinates, a height, and a width.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the ellipse.</param>
        /// <param name="x">x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
        /// <param name="width">Width of the bounding rectangle that defines the ellipse.</param>
        /// <param name="height">Height of the bounding rectangle that defines the ellipse.</param>
        /// <remarks>
        /// This method draws an ellipse that is defined by the bounding rectangle described by the x, y, width, and height parameters.
        /// </remarks>
        public void DrawEllipse(Pen pen, int x, int y, int width, int height)
        {
            DrawEllipse(pen, x, y, width, (float)height);
        }

        /// <summary>
        /// Draws an ellipse defined by a bounding rectangle specified by a pair of coordinates, a height, and a width.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the ellipse.</param>
        /// <param name="x">x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
        /// <param name="width">Width of the bounding rectangle that defines the ellipse.</param>
        /// <param name="height">Height of the bounding rectangle that defines the ellipse.</param>
        /// <remarks>
        /// This method draws an ellipse that is defined by the bounding rectangle described by the x, y, width, and height parameters.
        /// </remarks>
        public void DrawEllipse(Pen pen, float x, float y, float width, float height)
        {
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(x, y, width, height);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws an ellipse specified by a bounding Rectangle structure.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the ellipse.</param>
        /// <param name="rect">Rectangle structure that defines the boundaries of the ellipse.</param>
        /// <remarks>
        /// This method draws an ellipse that is defined by the bounding rectangle specified by the rect parameter.
        /// </remarks>
        public void DrawEllipse(Pen pen, Rectangle rect)
        {
            DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Draws an ellipse specified by a bounding RectangleF structure.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the ellipse.</param>
        /// <param name="rect">RectangleF structure that defines the boundaries of the ellipse.</param>
        /// <remarks>
        /// This method draws an ellipse that is defined by the bounding rectangle specified by the rect parameter.
        /// </remarks>
        public void DrawEllipse(Pen pen, RectangleF rect)
        {
            DrawEllipse(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }
        #endregion

        #region DrawPie
        /// <summary>
        /// Draws a pie shape defined by an ellipse specified by a coordinate pair, a width, and a height and two radial lines.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the pie shape.</param>
        /// <param name="x">x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
        /// <param name="width">Width of the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
        /// <param name="height">Height of the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
        /// <param name="startAngle">Angle measured in degrees clockwise from the x-axis to the first side of the pie shape.</param>
        /// <param name="sweepAngle">Angle measured in degrees clockwise from the startAngle parameter to the second side of the pie shape.</param>
        /// <remarks>
        /// This method draws a pie shape defined by an arc of an ellipse and the two radial lines that intersect with the endpoints of the arc. The ellipse is defined by the bounding rectangle described by the x, y, width, and height parameters. The pie shape consists of the two radial lines defined by the startAngle and sweepAngle parameters, and the arc between the intersections of those radial lines with the ellipse.
        /// If the sweepAngle parameter is greater than 360 degrees or less than -360 degrees, it is treated as if it were 360 degrees or -360 degrees, respectively.
        /// </remarks>
        public void DrawPie(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPie(x, y, width, height, startAngle, sweepAngle);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a pie shape defined by an ellipse specified by a coordinate pair, a width, and a height and two radial lines.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the pie shape.</param>
        /// <param name="x">x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
        /// <param name="width">Width of the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
        /// <param name="height">Height of the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
        /// <param name="startAngle">Angle measured in degrees clockwise from the x-axis to the first side of the pie shape.</param>
        /// <param name="sweepAngle">Angle measured in degrees clockwise from the startAngle parameter to the second side of the pie shape.</param>
        /// <remarks>
        /// This method draws a pie shape defined by an arc of an ellipse and the two radial lines that intersect with the endpoints of the arc. The ellipse is defined by the bounding rectangle described by the x, y, width, and height parameters. The pie shape consists of the two radial lines defined by the startAngle and sweepAngle parameters, and the arc between the intersections of those radial lines with the ellipse.
        /// If the sweepAngle parameter is greater than 360 degrees or less than -360 degrees, it is treated as if it were 360 degrees or -360 degrees, respectively.
        /// </remarks>
        public void DrawPie(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPie(x, y, width, height, startAngle, sweepAngle);
                DrawPath(pen, path);
            }
        }

        /// <summary>
        /// Draws a pie shape defined by an ellipse specified by a Rectangle structure and two radial lines.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the pie shape.</param>
        /// <param name="rect">Rectangle structure that represents the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
        /// <param name="startAngle">Angle measured in degrees clockwise from the x-axis to the first side of the pie shape.</param>
        /// <param name="sweepAngle">Angle measured in degrees clockwise from the startAngle parameter to the second side of the pie shape.</param>
        /// <remarks>
        /// This method draws a pie shape defined by an arc of an ellipse and the two radial lines that intersect with the endpoints of the arc. The ellipse is defined by the bounding rectangle. The pie shape consists of the two radial lines defined by the startAngle and sweepAngle parameters, and the arc between the intersections of those radial lines with the ellipse.
        /// If the sweepAngle parameter is greater than 360 degrees or less than -360 degrees, it is treated as if it were 360 degrees or -360 degrees, respectively.
        /// </remarks>
        public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            DrawPie(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        /// <summary>
        /// Draws a pie shape defined by an ellipse specified by a RectangleF structure and two radial lines.
        /// </summary>
        /// <param name="pen">Pen object that determines the color, width, and style of the pie shape.</param>
        /// <param name="rect">RectangleF structure that represents the bounding rectangle that defines the ellipse from which the pie shape comes.</param>
        /// <param name="startAngle">Angle measured in degrees clockwise from the x-axis to the first side of the pie shape.</param>
        /// <param name="sweepAngle">Angle measured in degrees clockwise from the startAngle parameter to the second side of the pie shape.</param>
        /// <remarks>
        /// This method draws a pie shape defined by an arc of an ellipse and the two radial lines that intersect with the endpoints of the arc. The ellipse is defined by the bounding rectangle. The pie shape consists of the two radial lines defined by the startAngle and sweepAngle parameters, and the arc between the intersections of those radial lines with the ellipse.
        /// If the sweepAngle parameter is greater than 360 degrees or less than -360 degrees, it is treated as if it were 360 degrees or -360 degrees, respectively.
        /// </remarks>
        public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            DrawPie(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }
        #endregion
        #endregion

        #region Fill Methods  (System.Drawing.Graphics)
        #region FillRectangle / FillRectangles
        /// <summary>
        /// Fills the interior of a rectangle specified by a Rectangle structure.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="rect">Rectangle structure that represents the rectangle to fill.</param>
        public void FillRectangle(Brush brush, Rectangle rect)
        {
            FillRectangle(brush, new RectangleF(rect.X, rect.Y, rect.Width, rect.Height));
        }

        /// <summary>
        /// Fills the interior of a rectangle specified by a RectangleF structure.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="rect">RectangleF structure that represents the rectangle to fill.</param>
        public void FillRectangle(Brush brush, RectangleF rect)
        {
            using (var path = new GraphicsPath())
            {
                path.AddRectangle(rect);
                FillPath(brush, path);
            }
        }

        /// <summary>
        /// Fills the interior of a rectangle specified by a pair of coordinates, a width, and a height.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="x">x-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="width">Width of the rectangle to fill.</param>
        /// <param name="height">Height of the rectangle to fill.</param>
        /// <remarks>
        /// This method fills the interior of the rectangle defined by the x, y, width, and height parameters.
        /// </remarks>
        public void FillRectangle(Brush brush, int x, int y, int width, int height)
        {
            FillRectangle(brush, new RectangleF(x, y, width, height));
        }

        /// <summary>
        /// Fills the interior of a rectangle specified by a pair of coordinates, a width, and a height.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="x">x-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="width">Width of the rectangle to fill.</param>
        /// <param name="height">Height of the rectangle to fill.</param>
        /// <remarks>
        /// This method fills the interior of the rectangle defined by the x, y, width, and height parameters.
        /// </remarks>
        public void FillRectangle(Brush brush, float x, float y, float width, float height)
        {
            FillRectangle(brush, new RectangleF(x, y, width, height));
        }

        /// <summary>
        /// Fills the interiors of a series of rectangles specified by Rectangle structures.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="rects">Array of Rectangle structures that represent the rectangles to fill.</param>
        public void FillRectangles(Brush brush, Rectangle[] rects)
        {
            using (var path = new GraphicsPath())
            {
                path.AddRectangles(rects);
                FillPath(brush, path);
            }
        }

        /// <summary>
        /// Fills the interiors of a series of rectangles specified by RectangleF structures.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="rects">Array of RectangleF structures that represent the rectangles to fill.</param>
        public void FillRectangles(Brush brush, RectangleF[] rects)
        {
            using (var path = new GraphicsPath())
            {
                path.AddRectangles(rects);
                FillPath(brush, path);
            }
        }
        #endregion

        #region FillPolygon
        /// <summary>
        /// Fills the interior of a polygon defined by an array of points specified by Point structures.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="points">Array of Point structures that represent the vertices of the polygon to fill.</param>
        /// <remarks>
        /// Every two consecutive points in the array specify a side of the polygon. In addition, if the last point and the first point do not coincide, they specify the closing side of the polygon.
        /// </remarks>
        public void FillPolygon(Brush brush, Point[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPolygon(points);
                FillPath(brush, path);
            }
        }

        /// <summary>
        /// Fills the interior of a polygon defined by an array of points specified by PointF structures.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="points">Array of PointF structures that represent the vertices of the polygon to fill.</param>
        /// <remarks>
        /// Every two consecutive points in the array specify a side of the polygon. In addition, if the last point and the first point do not coincide, they specify the closing side of the polygon.
        /// </remarks>
        public void FillPolygon(Brush brush, PointF[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPolygon(points);
                FillPath(brush, path);
            }
        }

        /// <summary>
        /// Fills the interior of a polygon defined by an array of points specified by Point structures using the specified fill mode.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="points">Array of Point structures that represent the vertices of the polygon to fill.</param>
        /// <param name="fillMode">Member of the WmfFillMode enumeration that determines the style of the fill.</param>
        /// <remarks>
        /// Every two consecutive points in the array specify a side of the polygon. In addition, if the last point and the first point do not coincide, they specify the closing side of the polygon.
        /// </remarks>
        public void FillPolygon(Brush brush, Point[] points, FillMode fillMode)
        {
            using (var path = new GraphicsPath(fillMode))
            {
                path.AddPolygon(points);
                FillPath(brush, path);
            }
        }

        /// <summary>
        /// Fills the interior of a polygon defined by an array of points specified by PointF structures using the specified fill mode.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="points">Array of PointF structures that represent the vertices of the polygon to fill.</param>
        /// <param name="fillMode">Member of the WmfFillMode enumeration that determines the style of the fill.</param>
        /// <remarks>
        /// Every two consecutive points in the array specify a side of the polygon. In addition, if the last point and the first point do not coincide, they specify the closing side of the polygon.
        /// </remarks>
        public void FillPolygon(Brush brush, PointF[] points, FillMode fillMode)
        {
            using (var path = new GraphicsPath(fillMode))
            {
                path.AddPolygon(points);
                FillPath(brush, path);
            }
        }
        #endregion

        #region FillClosedCurve
        /// <summary>
        /// Fills the interior a closed cardinal spline curve defined by an array of Point structures.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="points">Array of Point structures that define the spline.</param>
        /// <remarks>
        /// This method fills the interior of a closed cardinal spline that passes through each point in the array. If the last point does not match the first point, an additional curve segment is added from the last point to the first point to close it.
        /// The array of points must contain at least four Point structures.
        /// This method uses a default tension of 0.5.
        /// </remarks>
        public void FillClosedCurve(Brush brush, Point[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points);
                FillPath(brush, path);
            }
        }

        /// <summary>
        /// Fills the interior a closed cardinal spline curve defined by an array of PointF structures.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="points">Array of PointF structures that define the spline.</param>
        /// <remarks>
        /// This method fills the interior of a closed cardinal spline that passes through each point in the array. If the last point does not match the first point, an additional curve segment is added from the last point to the first point to close it.
        /// The array of points must contain at least four PointF structures.
        /// This method uses a default tension of 0.5.
        /// </remarks>
        public void FillClosedCurve(Brush brush, PointF[] points)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points);
                FillPath(brush, path);
            }
        }

        /// <summary>
        /// Fills the interior a closed cardinal spline curve defined by an array of Point structures using the specified fill mode.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="points">Array of Point structures that define the spline.</param>
        /// <param name="fillmode">Member of the WmfFillMode enumeration that determines how the curve is filled.</param>
        /// <remarks>
        /// This method fills the interior of a closed cardinal spline that passes through each point in the array. If the last point does not match the first point, an additional curve segment is added from the last point to the first point to close it.
        /// The array of points must contain at least four Point structures.
        /// This method uses a default tension of 0.5.
        /// </remarks>
        public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode)
        {
            using (var path = new GraphicsPath(fillmode))
            {
                path.AddClosedCurve(points);
                FillPath(brush, path);
            }
        }

        /// <summary>
        /// Fills the interior a closed cardinal spline curve defined by an array of PointF structures using the specified fill mode.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="points">Array of PointF structures that define the spline.</param>
        /// <param name="fillmode">Member of the WmfFillMode enumeration that determines how the curve is filled.</param>
        /// <remarks>
        /// This method fills the interior of a closed cardinal spline that passes through each point in the array. If the last point does not match the first point, an additional curve segment is added from the last point to the first point to close it.
        /// The array of points must contain at least four PointF structures.
        /// This method uses a default tension of 0.5.
        /// </remarks>
        public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode)
        {
            using (var path = new GraphicsPath(fillmode))
            {
                path.AddClosedCurve(points);
                FillPath(brush, path);
            }
        }

        /// <summary>
        /// Fills the interior a closed cardinal spline curve defined by an array of Point structures using the specified fill mode and tension.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="points">Array of Point structures that define the spline.</param>
        /// <param name="fillmode">Member of the WmfFillMode enumeration that determines how the curve is filled.</param>
        /// <param name="tension">Pattern greater than or equal to 0.0F that specifies the tension of the curve.</param>
        /// <remarks>
        /// This method fills the interior of a closed cardinal spline that passes through each point in the array. If the last point does not match the first point, an additional curve segment is added from the last point to the first point to close it.
        /// The array of points must contain at least four Point structures.
        /// The tension parameter determines the shape of the spline. If the value of the tension parameter is 0.0F, this method draws straight line segments to connect the points. Usually, the tension parameter is less than or equal to 1.0F. Values over 1.0F produce unusual results.
        /// </remarks>
        public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode, float tension)
        {
            using (var path = new GraphicsPath())
            {
                path.AddClosedCurve(points, tension);
                FillPath(brush, path);
            }
        }

        /// <summary>
        /// Fills the interior a closed cardinal spline curve defined by an array of PointF structures using the specified fill mode and tension.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="points">Array of PointF structures that define the spline.</param>
        /// <param name="fillmode">Member of the WmfFillMode enumeration that determines how the curve is filled.</param>
        /// <param name="tension">Pattern greater than or equal to 0.0F that specifies the tension of the curve.</param>
        /// <remarks>
        /// This method fills the interior of a closed cardinal spline that passes through each point in the array. If the last point does not match the first point, an additional curve segment is added from the last point to the first point to close it.
        /// The array of points must contain at least four PointF structures.
        /// The tension parameter determines the shape of the spline. If the value of the tension parameter is 0.0F, this method draws straight line segments to connect the points. Usually, the tension parameter is less than or equal to 1.0F. Values over 1.0F produce unusual results.
        /// </remarks>
        public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode, float tension)
        {
            using (var path = new GraphicsPath(fillmode))
            {
                path.AddClosedCurve(points, tension);
                FillPath(brush, path);
            }
        }
        #endregion

        #region FillEllipse
        /// <summary>
        /// Fills the interior of an ellipse defined by a bounding rectangle specified by a Rectangle structure.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="rect">Rectangle structure that represents the bounding rectangle that defines the ellipse.</param>
        /// <remarks>
        /// This method fills the interior of an ellipse with a Brush object. The ellipse is defined by the bounding rectangle represented by the rect parameter.
        /// </remarks>
        public void FillEllipse(Brush brush, Rectangle rect)
        {
            FillEllipse(brush, rect.X, rect.Y, rect.Width, (float)rect.Height);
        }

        /// <summary>
        /// Fills the interior of an ellipse defined by a bounding rectangle specified by a RectangleF structure.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="rect">RectangleF structure that represents the bounding rectangle that defines the ellipse.</param>
        /// <remarks>
        /// This method fills the interior of an ellipse with a Brush object. The ellipse is defined by the bounding rectangle represented by the rect parameter.
        /// </remarks>
        public void FillEllipse(Brush brush, RectangleF rect)
        {
            FillEllipse(brush, rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Fills the interior of an ellipse defined by a bounding rectangle specified by a pair of coordinates, a width, and a height.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="x">x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
        /// <param name="width">Width of the bounding rectangle that defines the ellipse.</param>
        /// <param name="height">Height of the bounding rectangle that defines the ellipse.</param>
        /// <remarks>
        /// This method fills the interior of an ellipse with a Brush object. The ellipse is defined by the bounding rectangle represented by the x, y, width, and height parameters.
        /// </remarks>
        public void FillEllipse(Brush brush, int x, int y, int width, int height)
        {
            FillEllipse(brush, x, y, width, (float)height);
        }

        /// <summary>
        /// Fills the interior of an ellipse defined by a bounding rectangle specified by a pair of coordinates, a width, and a height.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="x">x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse.</param>
        /// <param name="width">Width of the bounding rectangle that defines the ellipse.</param>
        /// <param name="height">Height of the bounding rectangle that defines the ellipse.</param>
        /// <remarks>
        /// This method fills the interior of an ellipse with a Brush object. The ellipse is defined by the bounding rectangle represented by the x, y, width, and height parameters.
        /// </remarks>
        public void FillEllipse(Brush brush, float x, float y, float width, float height)
        {
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(x, y, width, height);
                FillPath(brush, path);
            }
        }
        #endregion

        #region FillPie
        /// <summary>
        /// Fills the interior of a pie section defined by an ellipse specified by a Rectangle structure and two radial lines.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="rect">Rectangle structure that represents the bounding rectangle that defines the ellipse from which the pie section comes.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the startAngle parameter to the second side of the pie section.</param>
        /// <remarks>
        /// This method fills the interior of a pie section defined by an arc of an ellipse and the two radial lines that intersect with the endpoints of the arc. The ellipse is defined by the bounding rectangle. The pie section consists of the two radial lines defined by the startAngle and sweepAngle parameters and the arc between the intersections of those radial lines with the ellipse.
        /// If the sweepAngle parameter is greater than 360 degrees or less than -360 degrees, it is treated as if it were 360 degrees or -360 degrees, respectively.
        /// </remarks>
        public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle)
        {
            FillPie(brush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        /// <summary>
        /// Fills the interior of a pie section defined by an ellipse specified by a RectangleF structure and two radial lines.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="rect">RectangleF structure that represents the bounding rectangle that defines the ellipse from which the pie section comes.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the startAngle parameter to the second side of the pie section.</param>
        /// <remarks>
        /// This method fills the interior of a pie section defined by an arc of an ellipse and the two radial lines that intersect with the endpoints of the arc. The ellipse is defined by the bounding rectangle. The pie section consists of the two radial lines defined by the startAngle and sweepAngle parameters and the arc between the intersections of those radial lines with the ellipse.
        /// If the sweepAngle parameter is greater than 360 degrees or less than -360 degrees, it is treated as if it were 360 degrees or -360 degrees, respectively.
        /// </remarks>
        public void FillPie(Brush brush, RectangleF rect, float startAngle, float sweepAngle)
        {
            FillPie(brush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        /// <summary>
        /// Fills the interior of a pie section defined by an ellipse specified by a pair of coordinates, a width, and a height and two radial lines.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="x">x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie section comes.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie section comes.</param>
        /// <param name="width">Width of the bounding rectangle that defines the ellipse from which the pie section comes.</param>
        /// <param name="height">Height of the bounding rectangle that defines the ellipse from which the pie section comes.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the startAngle parameter to the second side of the pie section.</param>
        /// <remarks>
        /// This method fills the interior of a pie section defined by an arc of an ellipse and the two radial lines that intersect with the endpoints of the arc. The ellipse is defined by the bounding rectangle. The pie section consists of the two radial lines defined by the startAngle and sweepAngle parameters and the arc between the intersections of those radial lines with the ellipse.
        /// If the sweepAngle parameter is greater than 360 degrees or less than -360 degrees, it is treated as if it were 360 degrees or -360 degrees, respectively.
        /// </remarks>
        public void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            FillPie(brush, x, y, width, height, startAngle, (float)sweepAngle);
        }

        /// <summary>
        /// Fills the interior of a pie section defined by an ellipse specified by a pair of coordinates, a width, and a height and two radial lines.
        /// </summary>
        /// <param name="brush">Brush object that determines the characteristics of the fill.</param>
        /// <param name="x">x-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie section comes.</param>
        /// <param name="y">y-coordinate of the upper-left corner of the bounding rectangle that defines the ellipse from which the pie section comes.</param>
        /// <param name="width">Width of the bounding rectangle that defines the ellipse from which the pie section comes.</param>
        /// <param name="height">Height of the bounding rectangle that defines the ellipse from which the pie section comes.</param>
        /// <param name="startAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section.</param>
        /// <param name="sweepAngle">Angle in degrees measured clockwise from the startAngle parameter to the second side of the pie section.</param>
        /// <remarks>
        /// This method fills the interior of a pie section defined by an arc of an ellipse and the two radial lines that intersect with the endpoints of the arc. The ellipse is defined by the bounding rectangle. The pie section consists of the two radial lines defined by the startAngle and sweepAngle parameters and the arc between the intersections of those radial lines with the ellipse.
        /// If the sweepAngle parameter is greater than 360 degrees or less than -360 degrees, it is treated as if it were 360 degrees or -360 degrees, respectively.
        /// </remarks>
        public void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            using (var path = new GraphicsPath())
            {
                path.AddPie(x, y, width, height, startAngle, sweepAngle);
                FillPath(brush, path);
            }
        }
        #endregion
        #endregion

        public void DrawImage(Image image, float x, float y)
        {
            using (var tb = new TextureBrush(image))
            {
                FillRectangle(tb, x, y, image.Width, image.Height);
            }
        }
    }
}