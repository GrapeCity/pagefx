using System.Drawing.Drawing2D;
using System.Drawing.Flash;
using System.Drawing.Imaging;
using System.Drawing.Text;
using flash.display;
using GraphicsPath=System.Drawing.Drawing2D.GraphicsPath;
using Matrix=System.Drawing.Drawing2D.Matrix;

namespace System.Drawing
{
    using FlashBitmap = flash.display.Bitmap;

    //TODO: Clipping
    //TODO: Rest DrawImage methods
    //TODO: Clear
    //TODO: More tests
    //TODO: MeasureString
    //TODO: Implement Transform via Containers

    public sealed class Graphics : MarshalByRefObject, IDisposable
    {
        internal readonly DisplayObjectContainer _container;

        #region Inner Types
#if NOT_PFX
        public delegate bool EnumerateMetafileProc(EmfPlusRecordType recordType,
                                                   int flags,
                                                   int dataSize,
                                                   IntPtr data,
                                                   PlayRecordCallback callbackData);
#endif

        public delegate bool DrawImageAbort(IntPtr callbackData);
        #endregion

        public Graphics(DisplayObjectContainer container)
        {
            _container = container;
        }

        #region Flags
        [Flags]
        enum FLAGS
        {
            NoTextContainer = 1
        }
        FLAGS _flags;

        public bool NoTextContainer
        {
            get { return (_flags & FLAGS.NoTextContainer) != 0; }
            set 
            {
                if (value) _flags |= FLAGS.NoTextContainer;
                else _flags &= ~FLAGS.NoTextContainer;
            }
        }
        #endregion

        #region systemDpiX, systemDpiY
        internal static float systemDpiX
        {
            get
            {
                if (defDpiX == 0)
                {
                    defDpiX = (float)flash.system.Capabilities.screenDPI;
                }
                return defDpiX;
            }
        }
        static float defDpiX;

        static internal float systemDpiY
        {
            get
            {
                if (defDpiY == 0)
                {
                    defDpiY = (float)flash.system.Capabilities.screenDPI;
                }
                return defDpiY;
            }
        }
        static float defDpiY;
        #endregion

        #region BeginContainer, EndContainer
        public GraphicsContainer BeginContainer()
        {
            return new GraphicsContainer(Save());
        }

        [MonoTODO("The rectangles and unit parameters aren't supported in libgdiplus")]
        public GraphicsContainer BeginContainer(Rectangle dstrect, Rectangle srcrect, GraphicsUnit unit)
        {
            return BeginContainer();
        }

        [MonoTODO("The rectangles and unit parameters aren't supported in libgdiplus")]
        public GraphicsContainer BeginContainer(RectangleF dstrect, RectangleF srcrect, GraphicsUnit unit)
        {
            return BeginContainer();
        }

        public void EndContainer(GraphicsContainer container)
        {
#if NET_2_0
            if (container == null)
                throw new ArgumentNullException("container");
#endif
            container.StateObject.RestoreState(this);
        }
        #endregion

        #region Clear
        public void Clear(Color color)
        {
            DisplayHelper.RemoveAllChildren(_container);

            var sprite = _container as Sprite;
            if (sprite != null)
                sprite.graphics.clear();

            //TODO: How to use color
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
        }
        #endregion

        #region Transform
        public void MultiplyTransform(Matrix matrix)
        {
            MultiplyTransform(matrix, MatrixOrder.Prepend);
        }

        public void MultiplyTransform(Matrix matrix, MatrixOrder order)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");
            _transform.Multiply(matrix, order);
        }

        public void ResetTransform()
        {
            _transform.Reset();
        }

        public void RotateTransform(float angle)
        {
            RotateTransform(angle, MatrixOrder.Prepend);
        }

        public void RotateTransform(float angle, MatrixOrder order)
        {
            _transform.Rotate(angle, order);
        }

        public void ScaleTransform(float sx, float sy)
        {
            ScaleTransform(sx, sy, MatrixOrder.Prepend);
        }

        public void ScaleTransform(float sx, float sy, MatrixOrder order)
        {
            _transform.Scale(sx, sy, order);
        }

        public void TranslateTransform(float dx, float dy)
        {
            TranslateTransform(dx, dy, MatrixOrder.Prepend);
        }

        public void TranslateTransform(float dx, float dy, MatrixOrder order)
        {
            _transform.Translate(dx, dy, order);
        }

        public Matrix Transform
        {
            get
            {
                return _transform.Clone();
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _transform = value.Clone();
            }
        }
        Matrix _transform = new Matrix();
        #endregion

        #region Clip
        public Region Clip
        {
            get
            {
                return null;
            }
            set
            {
                //SetClip(value, CombineMode.Replace);
            }
        }

        public RectangleF ClipBounds
        {
            get
            {
                return RectangleF.Empty;
            }
        }

        public bool IsClipEmpty
        {
            get
            {
                return false;
            }
        }

        public bool IsVisibleClipEmpty
        {
            get
            {
                return true;
            }
        }

        public RectangleF VisibleClipBounds
        {
            get
            {
                return RectangleF.Empty;
            }
        }

        public void ResetClip()
        {
            throw new NotImplementedException();
        }

        public void ExcludeClip(Rectangle rect)
        {
            SetClip(rect, CombineMode.Exclude);
        }

        public void ExcludeClip(Region region)
        {
            SetClip(region, CombineMode.Exclude);
        }

        public void IntersectClip(Region region)
        {
            SetClip(region, CombineMode.Intersect);
        }

        public void IntersectClip(RectangleF rect)
        {
            SetClip(rect, CombineMode.Intersect);
        }

        public void IntersectClip(Rectangle rect)
        {
            SetClip(rect, CombineMode.Intersect);
        }

        public void SetClip(RectangleF rect)
        {
            SetClip(rect, CombineMode.Replace);
        }

        public void SetClip(GraphicsPath path)
        {
            SetClip(path, CombineMode.Replace);
        }

        public void SetClip(Rectangle rect)
        {
            SetClip(rect, CombineMode.Replace);
        }

        public void SetClip(Graphics g)
        {
            SetClip(g, CombineMode.Replace);
        }

        public void SetClip(Graphics g, CombineMode combineMode)
        {
            if (g == null)
                throw new ArgumentNullException("g");
            throw new NotImplementedException();
        }

        public void SetClip(Rectangle rect, CombineMode combineMode)
        {
            SetClip((RectangleF)rect, combineMode);
        }

        public void SetClip(RectangleF rect, CombineMode combineMode)
        {
            var mask = CreateMask(rect);
            SetMask(mask);
        }

        public void SetClip(Region region, CombineMode combineMode)
        {
            if (region == null)
                throw new ArgumentNullException("region");
            throw new NotImplementedException();
        }

        public void SetClip(GraphicsPath path, CombineMode combineMode)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            var mask = CreateMask(path);
            SetMask(mask);
        }

        public void TranslateClip(int dx, int dy)
        {
            throw new NotImplementedException();
        }

        public void TranslateClip(float dx, float dy)
        {
            throw new NotImplementedException();
        }

        DisplayObjectContainer _clipContainer;

        internal DisplayObject ClipMask
        {
            get 
            {
                if (_clipContainer == null) return null;
                return _clipContainer.mask;
            }
        }

        internal void SetMask(DisplayObject mask)
        {
            if (ClipMask == mask) return;
            if (mask != null)
            {
                _clipContainer = new Sprite();
                _clipContainer.mask = mask;
                _clipContainer.addChild(mask);
                _container.addChild(_clipContainer);
            }
            else
            {
                _clipContainer = null;
            }
        }

        static void Detach(DisplayObject obj)
        {
            if (obj == null) return;
            var parent = obj.parent;
            if (parent == null) return;
            parent.removeChild(obj);
        }

        const uint mask_color = 0xff0000;

        static DisplayObject CreateMask(GraphicsPath path)
        {
            var mask = new Shape();
            var g = mask.graphics;
            g.beginFill(mask_color);
            FlashDrawingAdapter.DrawPath(mask.graphics, path);
            g.endFill();
            return mask;
        }

        static DisplayObject CreateMask(RectangleF rect)
        {
            var mask = new Shape();
            var g = mask.graphics;
            g.beginFill(mask_color);
            g.drawRect(rect.X, rect.Y, rect.Width, rect.Height);
            g.endFill();
            return mask;
        }
        #endregion

        #region BeginShape, EndShape
        bool IsIdentityTransform
        {
            get 
            {
                if (_transform == null) return true;
                return _transform.IsIdentity;
            }
        }

        internal static flash.display.Graphics GetGraphics(DisplayObject obj)
        {
            var shape = obj as Shape;
            if (shape != null)
                return shape.graphics;
            var sprite = obj as Sprite;
            if (sprite != null)
                return sprite.graphics;
            throw new InvalidOperationException();
        }

        void BeginObject(DisplayObject obj, Brush brush, Pen pen)
        {
            var g = GetGraphics(obj);

            TransformObject(obj);

            if (brush != null)
                FlashDrawingAdapter.SetBrush(g, brush);
            if (pen != null)
                FlashDrawingAdapter.SetPen(g, pen);
        }

        internal Shape BeginShape(Brush brush, Pen pen)
        {
            var shape = new Shape();
            BeginObject(shape, brush, pen);
            return shape;
        }

        internal Sprite BeginSprite(Brush brush, Pen pen)
        {
            var sprite = new Sprite();
            BeginObject(sprite, brush, pen);
            return sprite;
        }

        internal void EndObject(DisplayObject obj, bool endFill)
        {
            if (endFill)
            {
                var g = GetGraphics(obj);
                g.endFill();
            }
            AddChild(obj, false);
        }

        void TransformObject(DisplayObject shape)
        {
            if (!IsIdentityTransform)
                shape.transform.matrix = Transform.native;
        }

        internal void AddChild(DisplayObject child, bool transform)
        {
            if (transform)
                TransformObject(child);
            if (_clipContainer != null)
                _clipContainer.addChild(child);
            else
                _container.addChild(child);
        }
        #endregion

        #region DrawPath, FillPath
        public void DrawPath(Pen pen, GraphicsPath path)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (path == null)
                throw new ArgumentNullException("path");
            DrawPathCore(pen, path);
        }

        public void FillPath(Brush brush, GraphicsPath path)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (path == null)
                throw new ArgumentNullException("path");
            FillPathCore(brush, path);
        }

        public void DrawPath(Brush brush, Pen pen, GraphicsPath path)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (path == null)
                throw new ArgumentNullException("path");
            DrawPathCore(brush, pen, path);
        }

        void DrawPathCore(Brush brush, Pen pen, GraphicsPath path)
        {
            var shape = BeginShape(brush, pen);
            FlashDrawingAdapter.DrawPath(shape.graphics, path);
            EndObject(shape, brush != null);
        }

        void DrawPathCore(Pen pen, GraphicsPath path)
        {
            DrawPathCore(null, pen, path);
        }

        void FillPathCore(Brush brush, GraphicsPath path)
        {
            DrawPathCore(brush, null, path);
        }
        #endregion

        #region DrawLine / DrawLines
        void DrawLineCore(Pen pen, float x1, float y1, float x2, float y2)
        {
            var shape = BeginShape(null, pen);
            var g = shape.graphics;
            g.moveTo(x1, y1);
            g.lineTo(x2, y2);
            EndObject(shape, false);
            
            //var path = new GraphicsPath();
            //path.AddLine(x1, y1, x2, y2);
            //DrawPathCore(pen, path);
        }

        public void DrawLine(Pen pen, PointF pt1, PointF pt2)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawLineCore(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        public void DrawLine(Pen pen, Point pt1, Point pt2)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawLineCore(pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
        }

        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawLineCore(pen, x1, y1, x2, y2);
        }

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawLineCore(pen, x1, y1, x2, y2);
        }

        public void DrawLines(Pen pen, PointF[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            int n = points.Length;
            if (n == 0) return;
            var path = new GraphicsPath();
            path.AddLines(points);
            DrawPathCore(pen, path);
        }

        public void DrawLines(Pen pen, Point[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            if (points.Length == 0)
                return;
            var path = new GraphicsPath();
            path.AddLines(points);
            DrawPathCore(pen, path);
        }
        #endregion

        #region DrawRectangle, DrawRectangles
        void DrawRectangleCore(Pen pen, RectangleF rect)
        {
            if (rect.IsEmpty) return;
            //var path = new GraphicsPath();
            //path.AddRectangle(rect);
            //DrawPathCore(pen, path);

            var shape = BeginShape(null, pen);
            shape.graphics.drawRect(rect.X, rect.Y, rect.Width, rect.Height);
            EndObject(shape, false);
        }

        public void DrawRectangle(Pen pen, Rectangle rect)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawRectangleCore(pen, rect);
        }

        public void DrawRectangle(Pen pen, RectangleF rect)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawRectangleCore(pen, rect);
        }

        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawRectangleCore(pen, new RectangleF(x, y, width, height));
        }

        public void DrawRectangle(Pen pen, int x, int y, int width, int height)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawRectangleCore(pen, new RectangleF(x, y, width, height));
        }

        public void DrawRectangles(Pen pen, RectangleF[] rects)
        {
            if (pen == null)
                throw new ArgumentNullException("image");
            if (rects == null)
                throw new ArgumentNullException("rects");
            if (rects.Length == 0) return;
            var path = new GraphicsPath();
            path.AddRectangles(rects);
            DrawPathCore(pen, path);
        }

        public void DrawRectangles(Pen pen, Rectangle[] rects)
        {
            if (pen == null)
                throw new ArgumentNullException("image");
            if (rects == null)
                throw new ArgumentNullException("rects");
            if (rects.Length == 0) return;
            var path = new GraphicsPath();
            path.AddRectangles(rects);
            DrawPathCore(pen, path);
        }
        #endregion

        #region FillRectangle, FillRectangles
        void FillRectangleCore(Brush brush, RectangleF rect)
        {
            if (rect.IsEmpty) return;
            //var path = new GraphicsPath();
            //path.AddRectangle(rect);
            //FillPathCore(brush, path);

            var shape = BeginShape(brush, null);
            shape.graphics.drawRect(rect.X, rect.Y, rect.Width, rect.Height);
            EndObject(shape, true);
        }

        public void FillRectangle(Brush brush, RectangleF rect)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            FillRectangleCore(brush, rect);
        }

        public void FillRectangle(Brush brush, Rectangle rect)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            FillRectangleCore(brush, rect);
        }

        public void FillRectangle(Brush brush, int x, int y, int width, int height)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            FillRectangleCore(brush, new RectangleF(x, y, width, height));
        }

        public void FillRectangle(Brush brush, float x, float y, float width, float height)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            FillRectangleCore(brush, new RectangleF(x, y, width, height));
        }

        public void FillRectangles(Brush brush, Rectangle[] rects)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (rects == null)
                throw new ArgumentNullException("rects");
            if (rects.Length == 0) return;
            var path = new GraphicsPath();
            path.AddRectangles(rects);
            FillPathCore(brush, path);
        }

        public void FillRectangles(Brush brush, RectangleF[] rects)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (rects == null)
                throw new ArgumentNullException("rects");
            if (rects.Length == 0) return;
            var path = new GraphicsPath();
            path.AddRectangles(rects);
            FillPathCore(brush, path);
        }
        #endregion

        #region DrawArc
        public void DrawArc(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            DrawArc(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }
        
        public void DrawArc(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            DrawArc(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }
        
        public void DrawArc(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawArcCore(pen, x, y, width, height, startAngle, sweepAngle);
        }

        // Microsoft documentation states that the signature for this member should be
        // public void DrawArc( Pen pen,  int x,  int y,  int width,  int height,   int startAngle,
        // int sweepAngle. However, GdipDrawArcI uses also float for the startAngle and sweepAngle params
        public void DrawArc(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawArcCore(pen, x, y, width, height, startAngle, sweepAngle);
        }

        void DrawArcCore(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            var path = new GraphicsPath();
            path.AddArc(x, y, width, height, startAngle, sweepAngle);
            DrawPathCore(pen, path);
        }
        #endregion

        #region DrawBezier, DrawBeziers
        void DrawBezierCore(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            var path = new GraphicsPath();
            path.AddBezier(x1, y1, x2, y2, x3, y3, x4, y4);
            DrawPathCore(pen, path);
        }

        public void DrawBezier(Pen pen, PointF pt1, PointF pt2, PointF pt3, PointF pt4)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawBezierCore(pen, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
        }

        public void DrawBezier(Pen pen, Point pt1, Point pt2, Point pt3, Point pt4)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawBezierCore(pen, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
        }

        public void DrawBezier(Pen pen, float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawBezierCore(pen, x1, y1, x2, y2, x3, y3, x4, y4);
        }

        public void DrawBeziers(Pen pen, Point[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");

            int length = points.Length;
            if (length < 4) return;

            var path = new GraphicsPath();
            path.AddBeziers(points);
            DrawPathCore(pen, path);
        }

        public void DrawBeziers(Pen pen, PointF[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");

            int length = points.Length;
            if (length < 4) return;

            var path = new GraphicsPath();
            path.AddBeziers(points);
            DrawPathCore(pen, path);
        }
        #endregion

        #region DrawClosedCurve, FillClosedCurve
        public void DrawClosedCurve(Pen pen, PointF[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddClosedCurve(points);
            DrawPathCore(pen, path);
        }

        public void DrawClosedCurve(Pen pen, Point[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddClosedCurve(points);
            DrawPathCore(pen, path);
        }

        // according to MSDN fillmode "is required but ignored" which makes _some_ sense since the unmanaged 
        // GDI+ call doesn't support it (issue spotted using Gendarme's AvoidUnusedParametersRule)
        public void DrawClosedCurve(Pen pen, Point[] points, float tension, FillMode fillmode)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath(fillmode);
            path.AddClosedCurve(points, tension);
            DrawPathCore(pen, path);
        }

        // according to MSDN fillmode "is required but ignored" which makes _some_ sense since the unmanaged 
        // GDI+ call doesn't support it (issue spotted using Gendarme's AvoidUnusedParametersRule)
        public void DrawClosedCurve(Pen pen, PointF[] points, float tension, FillMode fillmode)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath(fillmode);
            path.AddClosedCurve(points, tension);
            DrawPathCore(pen, path);
        }
        
        public void FillClosedCurve(Brush brush, PointF[] points)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddClosedCurve(points);
            FillPathCore(brush, path);
        }

        public void FillClosedCurve(Brush brush, Point[] points)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddClosedCurve(points);
            FillPathCore(brush, path);
        }
        
        public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (points == null)
                throw new ArgumentNullException("points");
            FillClosedCurve(brush, points, fillmode, GraphicsPath.DefaultTension);
        }

        public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (points == null)
                throw new ArgumentNullException("points");
            FillClosedCurve(brush, points, fillmode, GraphicsPath.DefaultTension);
        }

        public void FillClosedCurve(Brush brush, PointF[] points, FillMode fillmode, float tension)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath(fillmode);
            path.AddClosedCurve(points, tension);
            FillPathCore(brush, path);
        }

        public void FillClosedCurve(Brush brush, Point[] points, FillMode fillmode, float tension)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath(fillmode);
            path.AddClosedCurve(points, tension);
            FillPathCore(brush, path);
        }
        #endregion

        #region DrawCurve
        public void DrawCurve(Pen pen, Point[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddCurve(points);
            DrawPathCore(pen, path);
        }

        public void DrawCurve(Pen pen, PointF[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddCurve(points);
            DrawPathCore(pen, path);
        }

        public void DrawCurve(Pen pen, PointF[] points, float tension)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddCurve(points, tension);
            DrawPathCore(pen, path);
        }

        public void DrawCurve(Pen pen, Point[] points, float tension)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddCurve(points, tension);
            DrawPathCore(pen, path);
        }

        public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            DrawCurve(pen, points, offset, numberOfSegments, GraphicsPath.DefaultTension);
        }

        public void DrawCurve(Pen pen, Point[] points, int offset, int numberOfSegments, float tension)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddCurve(points, offset, numberOfSegments, tension);
            DrawPathCore(pen, path);
        }

        public void DrawCurve(Pen pen, PointF[] points, int offset, int numberOfSegments, float tension)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddCurve(points, offset, numberOfSegments, tension);
            DrawPathCore(pen, path);
        }
        #endregion

        #region DrawEllipse, FillEllipse
        public void DrawEllipse(Pen pen, Rectangle rect)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawEllipseCore(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawEllipse(Pen pen, RectangleF rect)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawEllipseCore(pen, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawEllipse(Pen pen, int x, int y, int width, int height)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawEllipseCore(pen, x, y, width, height);
        }

        public void DrawEllipse(Pen pen, float x, float y, float width, float height)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawEllipseCore(pen, x, y, width, height);
        }

        void DrawEllipseCore(Pen pen, float x, float y, float width, float height)
        {
            var shape = BeginShape(null, pen);
            shape.graphics.drawEllipse(x, y, width, height);
            EndObject(shape, false);

            //var path = new GraphicsPath();
            //path.AddEllipse(x, y, width, height);
            //DrawPathCore(pen, path);
        }
        
        public void FillEllipse(Brush brush, Rectangle rect)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            FillEllipseCore(brush, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void FillEllipse(Brush brush, RectangleF rect)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            FillEllipseCore(brush, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void FillEllipse(Brush brush, float x, float y, float width, float height)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            FillEllipseCore(brush, x, y, width, height);
        }

        public void FillEllipse(Brush brush, int x, int y, int width, int height)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            FillEllipseCore(brush, x, y, width, height);
        }

        void FillEllipseCore(Brush brush, float x, float y, float width, float height)
        {
            var shape = BeginShape(brush, null);
            shape.graphics.drawEllipse(x, y, width, height);
            EndObject(shape, false);

            //var path = new GraphicsPath();
            //path.AddEllipse(x, y, width, height);
            //FillPathCore(brush, path);
        }
        #endregion

        #region DrawPie, FillPie
        public void DrawPie(Pen pen, Rectangle rect, float startAngle, float sweepAngle)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawPie(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void DrawPie(Pen pen, RectangleF rect, float startAngle, float sweepAngle)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            DrawPie(pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public void DrawPie(Pen pen, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            var path = new GraphicsPath();
            path.AddPie(x, y, width, height, startAngle, sweepAngle);
            DrawPathCore(pen, path);
        }

        // Microsoft documentation states that the signature for this member should be
        // public void DrawPie(Pen pen, int x,  int y,  int width,   int height,   int startAngle
        // int sweepAngle. However, GdipDrawPieI uses also float for the startAngle and sweepAngle params
        public void DrawPie(Pen pen, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            var path = new GraphicsPath();
            path.AddPie(x, y, width, height, startAngle, sweepAngle);
            DrawPathCore(pen, path);
        }
        
        public void FillPie(Brush brush, Rectangle rect, float startAngle, float sweepAngle)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            var path = new GraphicsPath();
            path.AddPie(rect, startAngle, sweepAngle);
            FillPathCore(brush, path);
        }

        public void FillPie(Brush brush, int x, int y, int width, int height, int startAngle, int sweepAngle)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            var path = new GraphicsPath();
            path.AddPie(x, y, width, height, startAngle, sweepAngle);
            FillPathCore(brush, path);
        }

        public void FillPie(Brush brush, float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            var path = new GraphicsPath();
            path.AddPie(x, y, width, height, startAngle, sweepAngle);
            FillPathCore(brush, path);
        }
        #endregion

        #region DrawPolygon, FillPolygon
        public void DrawPolygon(Pen pen, Point[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddPolygon(points);
            DrawPathCore(pen, path);
        }

        public void DrawPolygon(Pen pen, PointF[] points)
        {
            if (pen == null)
                throw new ArgumentNullException("pen");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddPolygon(points);
            DrawPathCore(pen, path);
        }
        
        public void FillPolygon(Brush brush, PointF[] points)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddPolygon(points);
            FillPathCore(brush, path);
        }

        public void FillPolygon(Brush brush, Point[] points)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath();
            path.AddPolygon(points);
            FillPathCore(brush, path);
        }

        public void FillPolygon(Brush brush, Point[] points, FillMode fillMode)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (points == null)
                throw new ArgumentNullException("points");
            GraphicsPath path = new GraphicsPath(fillMode);
            path.AddPolygon(points);
            FillPathCore(brush, path);
        }

        public void FillPolygon(Brush brush, PointF[] points, FillMode fillMode)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (points == null)
                throw new ArgumentNullException("points");
            var path = new GraphicsPath(fillMode);
            path.AddPolygon(points);
            FillPathCore(brush, path);
        }
        #endregion

        #region FillRegion
        public void FillRegion(Brush brush, Region region)
        {
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (region == null)
                throw new ArgumentNullException("region");
            throw new NotImplementedException();
        }
        #endregion

        #region DrawIcon, DrawIconUnstretched
        public void DrawIcon(Icon icon, Rectangle targetRect)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            //DrawImage(icon.GetInternalBitmap(), targetRect);
            throw new NotImplementedException();
        }

        public void DrawIcon(Icon icon, int x, int y)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            //DrawImage(icon.GetInternalBitmap(), x, y);
            throw new NotImplementedException();
        }

        public void DrawIconUnstretched(Icon icon, Rectangle targetRect)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            //DrawImageUnscaled(icon.GetInternalBitmap(), targetRect);
            throw new NotImplementedException();
        }
        #endregion

        #region DrawImage
        static void CheckImage(Image image)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (image.Data == null)
                throw new InvalidOperationException("No bitmap data");
            if (!image.IsDataLoaded)
                throw new InvalidOperationException("Bitmap data is not loaded");
        }

        FlashBitmap BeginBitmap(Image image, Matrix matrix)
        {
            var data = image.Data;
            
            //TODO: Should we clone bitmap data?
            data = data.clone();

            var bmp = new FlashBitmap(data); 

            if (!IsIdentityTransform)
            {
                var m = _transform.Clone();
                if (matrix != null)
                    m.Multiply(matrix);
                matrix = m;
            }

            if (matrix != null)
                bmp.transform.matrix = matrix.native;

            return bmp;
        }

        #region DrawImage(x, y)
        static Matrix CreateScaleMatrix(float scaleX, float scaleY)
        {
            Matrix mat = null;
            if (!(FloatHelper.NearOne(scaleX) && FloatHelper.NearOne(scaleY)))
                mat = new Matrix(scaleX, 0, 0, scaleY, 0, 0);
            return mat;
        }

        FlashBitmap AddImage(Image image, float x, float y, Matrix matrix)
        {
            CheckImage(image);
            var bmp = BeginBitmap(image, matrix);
            bmp.x = x;
            bmp.y = y;
            AddChild(bmp, false);
            return bmp;
        }

        FlashBitmap AddImage(Image image, float x, float y, float scaleX, float scaleY)
        {
            Matrix mat = CreateScaleMatrix(scaleX, scaleY);
            return AddImage(image, x, y, mat);
        }

        public void DrawImage(Image image, float x, float y)
        {
            AddImage(image, x, y, null);
        }

        public void DrawImage(Image image, int x, int y)
        {
            DrawImage(image, (float)x, y);
        }

        public void DrawImage(Image image, PointF point)
        {
            DrawImage(image, point.X, point.Y);
        }

        public void DrawImage(Image image, Point point)
        {
            DrawImage(image, (float)point.X, point.Y);
        }
        #endregion

        #region DrawImage(x, y, width, height)
        public void DrawImage(Image image, float x, float y, float width, float height)
        {
            if (width <= 0 || height <= 0) return;
            CheckImage(image);
            float scaleX = width / image.Width;
            float scaleY = height / image.Height;
            AddImage(image, x, y, scaleX, scaleY);
        }

        public void DrawImage(Image image, int x, int y, int width, int height)
        {
            DrawImage(image, (float)x, y, width, height);
        }

        public void DrawImage(Image image, RectangleF rect)
        {
            DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawImage(Image image, Rectangle rect)
        {
            DrawImage(image, (float)rect.X, rect.Y, rect.Width, rect.Height);
        }
        #endregion

        #region DrawImage(destRect, srcRect)
        static int Clamp(int v, int min, int max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }

        static float OneToPixels(float dpi, GraphicsUnit srcUnit)
        {
            switch (srcUnit)
            {
                //Specifies the world coordinate system unit as the unit of measure.
                case GraphicsUnit.World:
                case GraphicsUnit.Pixel:
                    return 1;

                    //Specifies the unit of measure of the display device.
                    //Typically pixels for video displays, and 1/100 inch for printers.
                case GraphicsUnit.Display:
                    return dpi / 100;

                //Specifies a printer's point (1/72 inch) as the unit of measure
                case GraphicsUnit.Point:
                    return dpi / 72;
                    
                case GraphicsUnit.Inch:
                    return dpi;

                //Specifies the document unit (1/300 inch) as the unit of measure.
                case GraphicsUnit.Document:
                    return dpi / 300;

                //Specifies the millimeter as the unit of measure.
                case GraphicsUnit.Millimeter:
                    return dpi / 25.4f;

                default:
                    throw new ArgumentOutOfRangeException("srcUnit");
            }
        }

        static int ToImageSpace(float value, float dpi, GraphicsUnit srcUnit)
        {
            return (int)(OneToPixels(dpi, srcUnit) * value);
        }

        static int ToImageSpaceX(Image image, float x, GraphicsUnit srcUnit)
        {
            return ToImageSpace(x, image.HorizontalResolution, srcUnit);
        }

        static int ToImageSpaceY(Image image, float y, GraphicsUnit srcUnit)
        {
            return ToImageSpace(y, image.VerticalResolution, srcUnit);
        }

        static Rectangle ToImageSpace(Image image, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            int x = ToImageSpaceX(image, srcRect.X, srcUnit);
            int y = ToImageSpaceY(image, srcRect.Y, srcUnit);
            int w = ToImageSpaceX(image, srcRect.Width, srcUnit);
            int h = ToImageSpaceY(image, srcRect.Height, srcUnit);
            int iw = image.Width;
            int ih = image.Height;
            x = Clamp(x, 0, iw);
            y = Clamp(y, 0, ih);
            w = Clamp(w, 0, iw);
            h = Clamp(h, 0, ih);
            return new Rectangle(x, y, w, h);
        }

        public void DrawImagePortion(Image image, float x, float y, float scaleX, float scaleY, Rectangle srcRect)
        {
            var bmp = AddImage(image, x, y, scaleX, scaleY);
            bmp.scrollRect = new flash.geom.Rectangle(srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height);
        }

        public void DrawImage(Image image, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            CheckImage(image);
            var sr = ToImageSpace(image, srcRect, srcUnit);
            if (sr.Width <= 0 || sr.Height <= 0) return;
            int iw = image.Width;
            int ih = image.Height;
            if (sr.X >= iw || sr.Y >= ih) return;
            if (sr.X <= 0 && sr.Y <= 0 && sr.Width >= iw && sr.Height >= ih)
            {
                DrawImage(image, destRect);
                return;
            }
            float scaleX = destRect.Width / srcRect.Width;
            float scaleY = destRect.Height / srcRect.Height;
            DrawImagePortion(image, destRect.X, destRect.Y, scaleX, scaleY, sr);
        }

        public void DrawImage(Image image, float x, float y, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            CheckImage(image);
            var sr = ToImageSpace(image, srcRect, srcUnit);
            if (sr.Width <= 0 || sr.Height <= 0) return;
            int iw = image.Width;
            int ih = image.Height;
            if (sr.X >= iw || sr.Y >= ih) return;
            if (sr.X <= 0 && sr.Y <= 0 && sr.Width >= iw && sr.Height >= ih)
            {
                DrawImage(image, x, y);
                return;
            }
            DrawImagePortion(image, x, y, 1, 1, sr);
        }

        public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit)
        {
            DrawImage(image, (RectangleF)destRect, srcRect, srcUnit);
        }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit)
        {
            DrawImage(image, destRect, new RectangleF(srcX, srcY, srcWidth, srcHeight), srcUnit);
        }

        public void DrawImage(Image image, int x, int y, Rectangle srcRect, GraphicsUnit srcUnit)
        {
            DrawImage(image, (float)x, y, srcRect, srcUnit);
        }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit)
        {
            DrawImage(image, destRect, (float)srcX, srcY, srcWidth, srcHeight, srcUnit);
        }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs)
        {
            DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit);
        }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr)
        {
            DrawImage(image, destRect, (float)srcX, srcY, srcWidth, srcHeight, srcUnit);
        }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback)
        {
            DrawImage(image, destRect, (float)srcX, srcY, srcWidth, srcHeight, srcUnit);
        }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback)
        {
            DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit);
        }

        public void DrawImage(Image image, Rectangle destRect, float srcX, float srcY, float srcWidth, float srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData)
        {
            DrawImage(image, destRect, srcX, srcY, srcWidth, srcHeight, srcUnit);
        }

        public void DrawImage(Image image, Rectangle destRect, int srcX, int srcY, int srcWidth, int srcHeight, GraphicsUnit srcUnit, ImageAttributes imageAttrs, DrawImageAbort callback, IntPtr callbackData)
        {
            DrawImage(image, destRect, (float)srcX, srcY, srcWidth, srcHeight, srcUnit);
        }
        #endregion

        #region DrawImage(destPoints)
        public void DrawImage(Image image, Point[] destPoints)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (destPoints == null)
                throw new ArgumentNullException("destPoints");
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF[] destPoints)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (destPoints == null)
                throw new ArgumentNullException("destPoints");
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (destPoints == null)
                throw new ArgumentNullException("destPoints");
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (destPoints == null)
                throw new ArgumentNullException("destPoints");
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit,
                                ImageAttributes imageAttr)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (destPoints == null)
                throw new ArgumentNullException("destPoints");
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit,
                                ImageAttributes imageAttr)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (destPoints == null)
                throw new ArgumentNullException("destPoints");
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (destPoints == null)
                throw new ArgumentNullException("destPoints");
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (destPoints == null)
                throw new ArgumentNullException("destPoints");
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback, int callbackData)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (destPoints == null)
                throw new ArgumentNullException("destPoints");
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, ImageAttributes imageAttr, DrawImageAbort callback, int callbackData)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        #region DrawImageUnscaled
        public void DrawImageUnscaled(Image image, Point point)
        {
            DrawImageUnscaled(image, point.X, point.Y);
        }

        public void DrawImageUnscaled(Image image, Rectangle rect)
        {
            DrawImageUnscaled(image, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public void DrawImageUnscaled(Image image, int x, int y)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            DrawImage(image, x, y, image.Width, image.Height);
        }

        public void DrawImageUnscaled(Image image, int x, int y, int width, int height)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            // avoid creating an empty, or negative w/h, bitmap...
            if ((width <= 0) || (height <= 0))
                return;

            //TODO:
            //using (Image tmpImg = new Bitmap(width, height))
            //{
            //    using (Graphics g = FromImage(tmpImg))
            //    {
            //        g.DrawImage(image, 0, 0, image.Width, image.Height);
            //        DrawImage(tmpImg, x, y, width, height);
            //    }
            //}
            DrawImage(image, (float)x, y, width, height);
        }

#if NET_2_0
        public void DrawImageUnscaledAndClipped(Image image, Rectangle rect)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            int width = (image.Width > rect.Width) ? rect.Width : image.Width;
            int height = (image.Height > rect.Height) ? rect.Height : image.Height;

            DrawImageUnscaled(image, rect.X, rect.Y, width, height);
        }
#endif
        #endregion

        #region DrawString
        public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle)
        {
            DrawString(s, font, brush, layoutRectangle, null);
        }

        public void DrawString(string s, Font font, Brush brush, PointF point)
        {
            DrawString(s, font, brush, new RectangleF(point.X, point.Y, 0, 0), null);
        }

        public void DrawString(string s, Font font, Brush brush, PointF point, StringFormat format)
        {
            DrawString(s, font, brush, new RectangleF(point.X, point.Y, 0, 0), format);
        }

        public void DrawString(string s, Font font, Brush brush, float x, float y)
        {
            DrawString(s, font, brush, new RectangleF(x, y, 0, 0), null);
        }

        public void DrawString(string s, Font font, Brush brush, float x, float y, StringFormat format)
        {
            DrawString(s, font, brush, new RectangleF(x, y, 0, 0), format);
        }

        TextRenderer _textRenderer;

        public void DrawString(string s, Font font, Brush brush, RectangleF layoutRectangle, StringFormat format)
        {
            if (font == null)
                throw new ArgumentNullException("font");
            if (brush == null)
                throw new ArgumentNullException("brush");
            if (string.IsNullOrEmpty(s))
                return;
            if (_textRenderer == null)
                _textRenderer = new TextRenderer();
            _textRenderer.DrawString(this, s, font, brush, layoutRectangle, format);
        }
        #endregion

        #region Flush
        public void Flush()
        {
            Flush(FlushIntention.Flush);
        }
        
        public void Flush(FlushIntention intention)
        {
            //if (nativeObject == IntPtr.Zero)
            //{
            //    return;
            //}

            //Status status = GDIPlus.GdipFlush(nativeObject, intention);
            //GDIPlus.CheckStatus(status);
            //if (GDIPlus.UseCarbonDrawable && context.ctx != IntPtr.Zero)
            //    Carbon.CGContextSynchronize(context.ctx);
        }
        #endregion

        #region FromImage
        public static Graphics FromImage(Image image)
        {
            IntPtr graphics;

            if (image == null)
                throw new ArgumentNullException("image");

            if ((image.PixelFormat & PixelFormat.Indexed) != 0)
                throw new Exception(Locale.GetText("Cannot create Graphics from an indexed bitmap."));

            //Status status = GDIPlus.GdipGetImageGraphicsContext(image.nativeObject, out graphics);
            //GDIPlus.CheckStatus(status);
            //Graphics result = new Graphics(graphics);

            //if (GDIPlus.RunningOnUnix())
            //{
            //    Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            //    GDIPlus.GdipSetVisibleClip_linux(result.NativeObject, ref rect);
            //}

            //return result;


            throw new NotImplementedException();
        }
        #endregion

        #region GetNearestColor
        public Color GetNearestColor(Color color)
        {
            //int argb;

            //Status status = GDIPlus.GdipGetNearestColor(nativeObject, out argb);
            //GDIPlus.CheckStatus(status);

            //return Color.FromArgb(argb);
            throw new NotImplementedException();
        }
        #endregion

        #region IsVisible
        public bool IsVisible(Point point)
        {
            throw new NotImplementedException();
        }

        public bool IsVisible(RectangleF rect)
        {
            throw new NotImplementedException();
        }

        public bool IsVisible(PointF point)
        {
            throw new NotImplementedException();
        }

        public bool IsVisible(Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public bool IsVisible(float x, float y)
        {
            return IsVisible(new PointF(x, y));
        }

        public bool IsVisible(int x, int y)
        {
            return IsVisible(new Point(x, y));
        }

        public bool IsVisible(float x, float y, float width, float height)
        {
            return IsVisible(new RectangleF(x, y, width, height));
        }


        public bool IsVisible(int x, int y, int width, int height)
        {
            return IsVisible(new Rectangle(x, y, width, height));
        }
        #endregion

        #region MeasureCharacterRanges
        public Region[] MeasureCharacterRanges(string text, Font font, RectangleF layoutRect, StringFormat stringFormat)
        {
            //if ((text == null) || (text.Length == 0))
            //    return new Region[0];

            //if (font == null)
            //    throw new ArgumentNullException("font");

            //if (stringFormat == null)
            //    throw new ArgumentException("stringFormat");

            //int regcount = stringFormat.GetMeasurableCharacterRangeCount();
            //if (regcount == 0)
            //    return new Region[0];

            //IntPtr[] native_regions = new IntPtr[regcount];
            //Region[] regions = new Region[regcount];

            //for (int i = 0; i < regcount; i++)
            //{
            //    regions[i] = new Region();
            //    native_regions[i] = regions[i].NativeObject;
            //}

            //Status status = GDIPlus.GdipMeasureCharacterRanges(nativeObject, text, text.Length,
            //    font.NativeObject, ref layoutRect, stringFormat.NativeObject, regcount, out native_regions[0]);
            //GDIPlus.CheckStatus(status);
            
            //return regions;

            throw new NotImplementedException();
        }
        #endregion

        #region MeasureString
        //private unsafe SizeF GdipMeasureString(IntPtr graphics, string text, Font font, ref RectangleF layoutRect,
        //    IntPtr stringFormat)
        //{
        //    if ((text == null) || (text.Length == 0))
        //        return SizeF.Empty;

        //    if (font == null)
        //        throw new ArgumentNullException("font");

        //    RectangleF boundingBox = new RectangleF();

        //    Status status = GDIPlus.GdipMeasureString(nativeObject, text, text.Length, font.NativeObject,
        //        ref layoutRect, stringFormat, out boundingBox, null, null);
        //    GDIPlus.CheckStatus(status);

        //    return new SizeF(boundingBox.Width, boundingBox.Height);
        //}

        public SizeF MeasureString(string text, Font font)
        {
            return MeasureString(text, font, SizeF.Empty);
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea)
        {
            //RectangleF rect = new RectangleF(0, 0, layoutArea.Width, layoutArea.Height);
            //return GdipMeasureString(nativeObject, text, font, ref rect, IntPtr.Zero);
            throw new NotImplementedException();
        }

        public SizeF MeasureString(string text, Font font, int width)
        {
            //RectangleF rect = new RectangleF(0, 0, width, Int32.MaxValue);
            //return GdipMeasureString(nativeObject, text, font, ref rect, IntPtr.Zero);
            throw new NotImplementedException();
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat)
        {
            //RectangleF rect = new RectangleF(0, 0, layoutArea.Width, layoutArea.Height);
            //IntPtr format = (stringFormat == null) ? IntPtr.Zero : stringFormat.NativeObject;
            //return GdipMeasureString(nativeObject, text, font, ref rect, format);
            throw new NotImplementedException();
        }

        public SizeF MeasureString(string text, Font font, int width, StringFormat format)
        {
            //RectangleF rect = new RectangleF(0, 0, width, Int32.MaxValue);
            //IntPtr stringFormat = (format == null) ? IntPtr.Zero : format.NativeObject;
            //return GdipMeasureString(nativeObject, text, font, ref rect, stringFormat);
            throw new NotImplementedException();
        }

        public SizeF MeasureString(string text, Font font, PointF origin, StringFormat stringFormat)
        {
            //RectangleF rect = new RectangleF(origin.X, origin.Y, 0, 0);
            //IntPtr format = (stringFormat == null) ? IntPtr.Zero : stringFormat.NativeObject;
            //return GdipMeasureString(nativeObject, text, font, ref rect, format);
            throw new NotImplementedException();
        }

        public SizeF MeasureString(string text, Font font, SizeF layoutArea, StringFormat stringFormat,
            out int charactersFitted, out int linesFilled)
        {
            charactersFitted = 0;
            linesFilled = 0;

            if ((text == null) || (text.Length == 0))
                return SizeF.Empty;

            if (font == null)
                throw new ArgumentNullException("font");

            RectangleF boundingBox = new RectangleF();
            RectangleF rect = new RectangleF(0, 0, layoutArea.Width, layoutArea.Height);

            //IntPtr format = (stringFormat == null) ? IntPtr.Zero : stringFormat.NativeObject;
            //unsafe
            //{
            //    fixed (int* pc = &charactersFitted, pl = &linesFilled)
            //    {
            //        Status status = GDIPlus.GdipMeasureString(nativeObject, text, text.Length,
            //        font.NativeObject, ref rect, format, out boundingBox, pc, pl);
            //        GDIPlus.CheckStatus(status);
            //    }
            //}
            //return new SizeF(boundingBox.Width, boundingBox.Height);
            throw new NotImplementedException();
        }
        #endregion

        #region Restore, Save
        public void Restore(GraphicsState state)
        {
            if (state == null) throw new ArgumentNullException("state");
            state.RestoreState(this);
        }

        public GraphicsState Save()
        {
            return new GraphicsState(this, false);
        }
        #endregion

        #region TransformPoints
        public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, PointF[] pts)
        {
            if (pts == null)
                throw new ArgumentNullException("pts");

            //IntPtr ptrPt = GDIPlus.FromPointToUnManagedMemory(pts);

            //Status status = GDIPlus.GdipTransformPoints(nativeObject, destSpace, srcSpace, ptrPt, pts.Length);
            //GDIPlus.CheckStatus(status);

            //GDIPlus.FromUnManagedMemoryToPoint(ptrPt, pts);
            throw new NotImplementedException();
        }


        public void TransformPoints(CoordinateSpace destSpace, CoordinateSpace srcSpace, Point[] pts)
        {
            if (pts == null)
                throw new ArgumentNullException("pts");

            //IntPtr ptrPt = GDIPlus.FromPointToUnManagedMemoryI(pts);

            //Status status = GDIPlus.GdipTransformPointsI(nativeObject, destSpace, srcSpace, ptrPt, pts.Length);
            //GDIPlus.CheckStatus(status);

            //GDIPlus.FromUnManagedMemoryToPointI(ptrPt, pts);
            throw new NotImplementedException();
        }
        #endregion

        #region Properties
        public CompositingMode CompositingMode
        {
            get { return _mode; }
            set { _mode = value; }
        }
        CompositingMode _mode;

        public CompositingQuality CompositingQuality
        {
            get { return _compositingQuality; }
            set { _compositingQuality = value; }
        }
        CompositingQuality _compositingQuality;

        public float DpiX
        {
            get
            {
                return 72;
            }
        }

        public float DpiY
        {
            get
            {
                return 72;
            }
        }

        public InterpolationMode InterpolationMode
        {
            get
            {
                //InterpolationMode imode = InterpolationMode.Invalid;
                //Status status = GDIPlus.GdipGetInterpolationMode(nativeObject, out imode);
                //GDIPlus.CheckStatus(status);
                //return imode;
                return _interpolationMode;
            }
            set
            {
                //Status status = GDIPlus.GdipSetInterpolationMode(nativeObject, value);
                //GDIPlus.CheckStatus(status);
                _interpolationMode = value;
            }
        }
        InterpolationMode _interpolationMode;

        public float PageScale
        {
            get
            {
                //float scale;
                //Status status = GDIPlus.GdipGetPageScale(nativeObject, out scale);
                //GDIPlus.CheckStatus(status);
                //return scale;
                return 1;
            }
            set
            {
                //Status status = GDIPlus.GdipSetPageScale(nativeObject, value);
                //GDIPlus.CheckStatus(status);
            }
        }

        public GraphicsUnit PageUnit
        {
            get
            {
                //GraphicsUnit unit;
                //Status status = GDIPlus.GdipGetPageUnit(nativeObject, out unit);
                //GDIPlus.CheckStatus(status);
                //return unit;
                return _pageUnit;
            }
            set
            {
                //Status status = GDIPlus.GdipSetPageUnit(nativeObject, value);
                //GDIPlus.CheckStatus(status);
                _pageUnit = value;
            }
        }
        GraphicsUnit _pageUnit;

        [MonoTODO("This property does not do anything when used with libgdiplus.")]
        public PixelOffsetMode PixelOffsetMode
        {
            get
            {
                //PixelOffsetMode pixelOffset = PixelOffsetMode.Invalid;
                //Status status = GDIPlus.GdipGetPixelOffsetMode(nativeObject, out pixelOffset);
                //GDIPlus.CheckStatus(status);
                //return pixelOffset;
                return _pixelOffsetMode;
            }
            set
            {
                //Status status = GDIPlus.GdipSetPixelOffsetMode(nativeObject, value);
                //GDIPlus.CheckStatus(status);
                _pixelOffsetMode = value;
            }
        }
        PixelOffsetMode _pixelOffsetMode;

        public Point RenderingOrigin
        {
            get
            {
                //int x, y;
                //Status status = GDIPlus.GdipGetRenderingOrigin(nativeObject, out x, out y);
                //GDIPlus.CheckStatus(status);
                //return new Point(x, y);
                return _renderingOrigin;
            }
            set
            {
                //Status status = GDIPlus.GdipSetRenderingOrigin(nativeObject, value.X, value.Y);
                //GDIPlus.CheckStatus(status);
                _renderingOrigin = value;
            }
        }
        Point _renderingOrigin;

        public SmoothingMode SmoothingMode
        {
            get
            {
                //SmoothingMode mode = SmoothingMode.Invalid;
                //Status status = GDIPlus.GdipGetSmoothingMode(nativeObject, out mode);
                //GDIPlus.CheckStatus(status);
                //return mode;
                return _smoothingMode;
            }

            set
            {
                //Status status = GDIPlus.GdipSetSmoothingMode(nativeObject, value);
                //GDIPlus.CheckStatus(status);
                _smoothingMode = value;
            }
        }
        SmoothingMode _smoothingMode;

        [MonoTODO("This property does not do anything when used with libgdiplus.")]
        public int TextContrast
        {
            get
            {
                //int contrast;
                //Status status = GDIPlus.GdipGetTextContrast(nativeObject, out contrast);
                //GDIPlus.CheckStatus(status);
                //return contrast;
                return _textContrast;
            }
            set
            {
                //Status status = GDIPlus.GdipSetTextContrast(nativeObject, value);
                //GDIPlus.CheckStatus(status);
                _textContrast = value;
            }
        }
        int _textContrast;

        public TextRenderingHint TextRenderingHint
        {
            get
            {
                //TextRenderingHint hint;
                //Status status = GDIPlus.GdipGetTextRenderingHint(nativeObject, out hint);
                //GDIPlus.CheckStatus(status);
                //return hint;
                return _textRenderingHint;
            }
            set
            {
                //Status status = GDIPlus.GdipSetTextRenderingHint(nativeObject, value);
                //GDIPlus.CheckStatus(status);
                _textRenderingHint = value;
            }
        }
        TextRenderingHint _textRenderingHint;
        #endregion

        #region NOT_PFX
        #region NOT_PFX: EnumerateMetafile
#if NOT_PFX
        private const string MetafileEnumeration = "Metafiles enumeration, for both WMF and EMF formats, isn't supported.";

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Point destPoint, EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, PointF destPoint, EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, PointF destPoint, EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Point destPoint, EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Point destPoint, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, PointF destPoint, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit srcUnit, EnumerateMetafileProc callback, IntPtr callbackData)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Point[] destPoints, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Rectangle destRect, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, Point destPoint, Rectangle srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, RectangleF destRect, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, PointF[] destPoints, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }

        [MonoTODO(MetafileEnumeration)]
        public void EnumerateMetafile(Metafile metafile, PointF destPoint, RectangleF srcRect, GraphicsUnit unit, EnumerateMetafileProc callback, IntPtr callbackData, ImageAttributes imageAttr)
        {
            throw new NotImplementedException();
        }
#endif
        #endregion

        #region NOT_PFX: AddMetafileComment
#if NOT_PFX
        [MonoTODO("Metafiles, both WMF and EMF formats, aren't supported.")]
        public void AddMetafileComment(byte[] data)
        {
            throw new NotImplementedException();
        }
#endif
        #endregion

        #region NOT_PFX: CopyFromScreen
#if NET_2_0
#if NOT_PFX
		[MonoLimitation ("Works on Win32 and on X11 (but not on Cocoa and Quartz)")]
		public void CopyFromScreen (Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize)
		{
			CopyFromScreen (upperLeftSource.X, upperLeftSource.Y, upperLeftDestination.X, upperLeftDestination.Y,
				blockRegionSize, CopyPixelOperation.SourceCopy);				
		}

		[MonoLimitation ("Works on Win32 and (for CopyPixelOperation.SourceCopy only) on X11 but not on Cocoa and Quartz")]
		public void CopyFromScreen (Point upperLeftSource, Point upperLeftDestination, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
		{
			CopyFromScreen (upperLeftSource.X, upperLeftSource.Y, upperLeftDestination.X, upperLeftDestination.Y,
				blockRegionSize, copyPixelOperation);
		}
		
		[MonoLimitation ("Works on Win32 and on X11 (but not on Cocoa and Quartz)")]
		public void CopyFromScreen (int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize)
		{
			CopyFromScreen (sourceX, sourceY, destinationX, destinationY, blockRegionSize,
				CopyPixelOperation.SourceCopy);
		}

		[MonoLimitation ("Works on Win32 and (for CopyPixelOperation.SourceCopy only) on X11 but not on Cocoa and Quartz")]
		public void CopyFromScreen (int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
		{
			if (!Enum.IsDefined (typeof (CopyPixelOperation), copyPixelOperation))
				throw new InvalidEnumArgumentException (Locale.GetText ("Enum argument value '{0}' is not valid for CopyPixelOperation", copyPixelOperation));

			if (GDIPlus.UseX11Drawable) {
				CopyFromScreenX11 (sourceX, sourceY, destinationX, destinationY, blockRegionSize, copyPixelOperation);
			} else if (GDIPlus.UseCarbonDrawable) {
				CopyFromScreenMac (sourceX, sourceY, destinationX, destinationY, blockRegionSize, copyPixelOperation);
			} else {
				CopyFromScreenWin32 (sourceX, sourceY, destinationX, destinationY, blockRegionSize, copyPixelOperation);
			}
		}

		private void CopyFromScreenWin32 (int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
		{
			IntPtr window = GDIPlus.GetDesktopWindow ();
			IntPtr srcDC = GDIPlus.GetDC (window);
			IntPtr dstDC = GetHdc ();
			GDIPlus.BitBlt (dstDC, destinationX, destinationY, blockRegionSize.Width,
				blockRegionSize.Height, srcDC, sourceX, sourceY, (int) copyPixelOperation);

			GDIPlus.ReleaseDC (IntPtr.Zero, srcDC);
			ReleaseHdc (dstDC);			
		}
		
		private void CopyFromScreenMac (int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
		{
			throw new NotImplementedException ();
		}

		private void CopyFromScreenX11 (int sourceX, int sourceY, int destinationX, int destinationY, Size blockRegionSize, CopyPixelOperation copyPixelOperation)
		{
			IntPtr window, image, defvisual, vPtr;
			int AllPlanes = ~0, nitems = 0, pixel;

			if (copyPixelOperation != CopyPixelOperation.SourceCopy)
				throw new NotImplementedException ("Operation not implemented under X11");
		
			if (GDIPlus.Display == IntPtr.Zero) {
				GDIPlus.Display = GDIPlus.XOpenDisplay (IntPtr.Zero);					
			}

			window = GDIPlus.XRootWindow (GDIPlus.Display, 0);
			defvisual = GDIPlus.XDefaultVisual (GDIPlus.Display, 0);				
			XVisualInfo visual = new XVisualInfo ();

			/* Get XVisualInfo for this visual */
			visual.visualid = GDIPlus.XVisualIDFromVisual(defvisual);
			vPtr = GDIPlus.XGetVisualInfo (GDIPlus.Display, 0x1 /* VisualIDMask */, ref visual, ref nitems);
			visual = (XVisualInfo) Marshal.PtrToStructure(vPtr, typeof (XVisualInfo));

			/* Sorry I do not have access to a computer with > deepth. Fell free to add more pixel formats */	
			image = GDIPlus.XGetImage (GDIPlus.Display, window, sourceX, sourceY, blockRegionSize.Width,
				blockRegionSize.Height, AllPlanes, 2 /* ZPixmap*/);
				
			Bitmap bmp = new Bitmap (blockRegionSize.Width, blockRegionSize.Height);
			int red, blue, green;
			for (int y = 0; y < blockRegionSize.Height; y++) {
				for (int x = 0; x < blockRegionSize.Width; x++) {
					pixel = GDIPlus.XGetPixel (image, x, y);

					switch (visual.depth) {
						case 16: /* 16bbp pixel transformation */
							red = (int) ((pixel & visual.red_mask ) >> 8) & 0xff;
							green = (int) (((pixel & visual.green_mask ) >> 3 )) & 0xff;
							blue = (int) ((pixel & visual.blue_mask ) << 3 ) & 0xff;
							break;
						case 24:
						case 32:
							red = (int) ((pixel & visual.red_mask ) >> 16) & 0xff;
							green = (int) (((pixel & visual.green_mask ) >> 8 )) & 0xff;
							blue = (int) ((pixel & visual.blue_mask )) & 0xff;
							break;
						default:
							string text = Locale.GetText ("{0}bbp depth not supported.", visual.depth);
							throw new NotImplementedException (text);
					}
						
					bmp.SetPixel (x, y, Color.FromArgb (255, red, green, blue));							 
				}
			}

			DrawImage (bmp, destinationX, destinationY);
			bmp.Dispose ();
			GDIPlus.XDestroyImage (image);
			GDIPlus.XFree (vPtr);
		}
#endif
#endif
        #endregion
        #endregion
    }
}
