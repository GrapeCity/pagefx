using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace System.Drawing
{
    public sealed class TextureBrush : Brush
    {
        readonly Image _image;

        #region Constructors
        public TextureBrush(Image bitmap) :
            this(bitmap, WrapMode.Tile)
        {
        }

        public TextureBrush(Image image, Rectangle dstRect) :
            this(image, WrapMode.Tile, dstRect)
        {
        }

        public TextureBrush(Image image, RectangleF dstRect) :
            this(image, WrapMode.Tile, dstRect)
        {
        }

        public TextureBrush(Image image, WrapMode wrapMode)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            _image = image;
            WrapMode = wrapMode;
            if (image.IsDataLoaded)
                InitMatrix();
            else
                image.Loaded += OnImageLoaded;
        }

        void OnImageLoaded(object sender, EventArgs e)
        {
            InitMatrix();
        }

        [MonoLimitation("ImageAttributes are ignored when using libgdiplus")]
        public TextureBrush(Image image, Rectangle dstRect, ImageAttributes imageAttr)
            : this(image, (RectangleF)dstRect, imageAttr)
        {
        }

        [MonoLimitation("ImageAttributes are ignored when using libgdiplus")]
        public TextureBrush(Image image, RectangleF dstRect, ImageAttributes imageAttr)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            _image = image;
            InitMatrix(dstRect);
        }

        public TextureBrush(Image image, WrapMode wrapMode, Rectangle dstRect)
            : this(image, wrapMode, (RectangleF)dstRect)
        {
        }

        public TextureBrush(Image image, WrapMode wrapMode, RectangleF dstRect)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            _image = image;
            WrapMode = wrapMode;
            InitMatrix(dstRect);
        }

        void InitMatrix()
        {
            InitMatrix(new RectangleF(0, 0, _image.Width, _image.Height));
        }

        void InitMatrix(RectangleF dstRect)
        {
            //float sx = dstRect.Width / _image.Width;
            //float sy = dstRect.Width / _image.Height;
            _transform = new Matrix(1, 0, 0, 1, dstRect.X, dstRect.Y);
        }
        #endregion

        #region Properties
        public Image Image
        {
            get { return _image; }
        }

        public WrapMode WrapMode
        {
            get { return _wrapMode; }
            set
            {
                if ((value < WrapMode.Tile) || (value > WrapMode.Clamp))
                    throw new InvalidEnumArgumentException("WrapMode");
                _wrapMode = value;
            }
        }
        WrapMode _wrapMode;

        internal bool Repeat
        {
            get { return _wrapMode != WrapMode.Clamp; }
        }
        #endregion

        public override object Clone()
        {
            //IntPtr clonePtr;
            //Status status = GDIPlus.GdipCloneBrush (nativeObject, out clonePtr);
            //GDIPlus.CheckStatus (status);

            //return new TextureBrush (clonePtr);
            throw new NotImplementedException();
        }

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
    }
}
