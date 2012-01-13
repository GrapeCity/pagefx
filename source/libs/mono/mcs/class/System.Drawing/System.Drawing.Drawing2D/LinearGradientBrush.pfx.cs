namespace System.Drawing.Drawing2D
{
    using FlashMatrix = flash.geom.Matrix;

    public sealed class LinearGradientBrush : Brush
    {
        RectangleF _rect;

        #region utils
        static float GetAngle(LinearGradientMode mode)
        {
            switch (mode)
            {
                case LinearGradientMode.Vertical:
                    return 90.0f;
                case LinearGradientMode.ForwardDiagonal:
                    return 45.0f;
                case LinearGradientMode.BackwardDiagonal:
                    return 135.0f;
                //case LinearGradientMode.Horizontal:
                default:
                    return 0;
            }
        }
        #endregion

        #region InitFromLine
        void InitFromLine(PointF point1, PointF point2)
        {
            bool xFlipped = false;
            bool yFlipped = false;

            _rect.Width = point2.X - point1.X;
            _rect.Height = point2.Y - point1.Y;
            _rect.X = _rect.Width < 0 ? point2.X : point1.X;
            _rect.Y = _rect.Height < 0 ? point2.Y : point1.Y;

            double angle = 0;

            if (_rect.Width < 0)
            {
                _rect.Width = -_rect.Width;
                xFlipped = true;
            }

            if (_rect.Height < 0)
            {
                _rect.Height = -_rect.Height;
                yFlipped = true;
            }

            if (_rect.Height == 0)
            {
                _rect.Height = _rect.Width;
                _rect.Y = _rect.Y - (_rect.Height / 2.0f);
                angle = xFlipped ? 180 : 0;
            }
            else if (_rect.Width == 0)
            {
                _rect.Width = _rect.Height;
                _rect.X = _rect.X - (_rect.Width / 2.0f);
                angle = yFlipped ? 270 : 90;
            }
            else
            {
                double slope = _rect.Height / _rect.Width;
                double newAngleRad = Math.Atan(slope);
                double newAngle = (newAngleRad / (Const.DEGTORAD));

                if (xFlipped)
                    newAngle = 180 - newAngle;

                if (yFlipped)
                    newAngle = 360 - newAngle;

                angle = newAngle;
            }

            InitMatrix(angle);
        }

        static void PrintMatrix(string ctx, Matrix m)
        {
            float[] e = m.Elements;
            Console.WriteLine("{0}, LGMatrix =[{1}, {2}, {3}, {4}, {5}, {6}]",
                ctx, e[0], e[1], e[2], e[3], e[4], e[5]);
        }

        void InitMatrix(double angle)
        {
            angle = (angle % 360) * Const.DEGTORAD;
            var m = new FlashMatrix();
            m.createGradientBox(_rect.Width, _rect.Height, angle, _rect.X, _rect.Y);
            _transform = new Matrix(m);
            //PrintMatrix("Init", _transform);
        }
        #endregion

        #region Constructors
        private LinearGradientBrush()
        {
        }

        public LinearGradientBrush(Point point1, Point point2, Color color1, Color color2)
            : this((PointF)point1, point2, color1, color2)
        {
        }

        public LinearGradientBrush(PointF point1, PointF point2, Color color1, Color color2)
        {
            _linearColors = new[] {color1, color2};
            InitFromLine(point1, point2);
        }

        public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle)
            : this(rect, color1, color2, angle, false)
        {
        }

        public LinearGradientBrush(Rectangle rect, Color color1, Color color2,
            LinearGradientMode linearGradientMode)
            : this((RectangleF)rect, color1, color2, linearGradientMode)
        {
        }

        public LinearGradientBrush(RectangleF rect, Color color1, Color color2, LinearGradientMode linearGradientMode)
            : this(rect, color1, color2, GetAngle(linearGradientMode), false)
        {
        }

        public LinearGradientBrush(RectangleF rect, Color color1, Color color2, float angle)
            : this(rect, color1, color2, angle, false)
        {
        }

        public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle, bool isAngleScaleable)
            : this((RectangleF)rect, color1, color2, angle, isAngleScaleable)
        {
        }

        public LinearGradientBrush(RectangleF rect, Color color1, Color color2, float angle, bool isAngleScaleable)
        {
            _rect = rect;
            _linearColors = new[] {color1, color2};
            InitMatrix(angle);

            //TODO: isAngleScaleable
        }
        #endregion

        #region Public Properties
        public Blend Blend
        {
            get;
            set;
        }

        [MonoTODO("The GammaCorrection value is ignored when using libgdiplus.")]
        public bool GammaCorrection
        {
            get;
            set;
        }

        public ColorBlend InterpolationColors
        {
            get { return _colorBlend; }
            set { _colorBlend = value; }
        }
        ColorBlend _colorBlend;

        public Color[] LinearColors
        {
            get { return _linearColors; }
            set
            {
                // no null check, MS throws a NullReferenceException here
                _linearColors = value;
            }
        }
        Color[] _linearColors;

        public RectangleF Rectangle
        {
            get
            {
                return _rect;
            }
        }

        public WrapMode WrapMode
        {
            get { return _wrapMode; }
            set
            {
                // note: Clamp isn't valid (context wise) but it is checked in libgdiplus
                if ((value < WrapMode.Tile) || (value > WrapMode.Clamp))
                    throw new ArgumentException("WrapMode");
                _wrapMode = value;
            }
        }
        WrapMode _wrapMode;
        #endregion

        #region Transform

        public Matrix Transform
        {
            get
            {
                return _transform.Clone();
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Transform");
                //PrintMatrix("Transform", value);
                _transform = value.Clone();
            }
        }
        Matrix _transform = new Matrix();

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
        #endregion

        #region TODO: Shapes
        public void SetBlendTriangularShape(float focus)
        {
            SetBlendTriangularShape(focus, 1.0F);
        }

        public void SetBlendTriangularShape(float focus, float scale)
        {
            if (focus < 0 || focus > 1 || scale < 0 || scale > 1)
                throw new ArgumentException("Invalid parameter passed.");

            throw new NotImplementedException();
        }

        public void SetSigmaBellShape(float focus)
        {
            SetSigmaBellShape(focus, 1.0F);
        }

        public void SetSigmaBellShape(float focus, float scale)
        {
            if (focus < 0 || focus > 1 || scale < 0 || scale > 1)
                throw new ArgumentException("Invalid parameter passed.");
            throw new NotImplementedException();
        }
        #endregion

        public override object Clone()
        {
            var lg = new LinearGradientBrush();
            lg.GammaCorrection = GammaCorrection;
            lg._rect = _rect;
            lg._wrapMode = _wrapMode;
            lg._transform = _transform != null ? _transform.Clone() : null;

            if (_linearColors != null)
            {
                lg._linearColors = new[] {_linearColors[0], _linearColors[1]};
            }

            if (_colorBlend != null)
                lg._colorBlend = _colorBlend.Clone();
            if (Blend != null)
                lg.Blend = Blend.Clone();

            return lg;
        }
    }
}
