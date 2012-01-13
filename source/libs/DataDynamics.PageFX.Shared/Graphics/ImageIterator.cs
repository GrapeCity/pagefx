using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace DataDynamics
{
    public abstract class ImageIterator : IEnumerator, IEnumerable
    {
        #region Public Properties
        public virtual int X
        {
            get { return _X; }
        }

        public virtual int Y
        {
            get { return _Y; }
        }

        public virtual Image Image
        {
            get { return null; }
        }

        public virtual int Width
        {
            get { return Image.Width; }
        }

        public virtual int Height
        {
            get { return Image.Height; }
        }
        #endregion

        #region IEnumerable Members
        public IEnumerator GetEnumerator()
        {
            return this;
        }
        #endregion

        #region IEnumerator Members
        protected virtual void Init()
        {
        }

        public virtual void Reset()
        {
            _X = -1;
            _Y = -1;
        }

        public abstract object Current { get; }

        public bool MoveNext()
        {
            Init();
            if (_X == -1 && _Y == -1)
            {
                _X = 0;
                _Y = 0;
                return true;
            }
            if (_X < Width - 1)
            {
                ++_X;
                return true;
            }
            _X = 0;
            if (_Y < Height - 1)
            {
                ++_Y;
                return true;
            }
            Reset();
            return false;
        }
        #endregion

        #region Members Variables
        protected int _X = -1;
        protected int _Y = -1;
        #endregion
    }

    public class BitmapIterator : ImageIterator
    {
        #region Constructors
        public BitmapIterator(Image image)
        {
            _Bitmap = ImageHelper.ToBitmap(image);
        }

        public BitmapIterator(Bitmap bmp)
        {
            _Bitmap = bmp;
        }
        #endregion

        #region Public Members
        public override Image Image
        {
            get { return _Bitmap; }
        }

        public Bitmap Bitmap
        {
            get { return _Bitmap; }
        }

        public BitmapData Data
        {
            get
            {
                Lock();
                return _Data;
            }
        }

        public virtual Color GetColor(int x, int y)
        {
            return ImageHelper.GetColor(Data, x, y);
        }
        #endregion

        #region IEnumerator Members
        public override void Reset()
        {
            base.Reset();
            Unlock();
        }

        protected override void Init()
        {
            Lock();
        }

        public override object Current
        {
            get
            {
                if (_X < 0 || _Y < 0
                    || _X >= Width
                    || _Y >= Height)
                    return null;
                return ImageHelper.GetColor(_Data, _X, _Y);
            }
        }

        protected virtual void Lock()
        {
            if (_Data == null)
            {
                // GDI+ still lies to us - the return format is BGR, NOT RGB.
                var r = new Rectangle(0, 0, _Bitmap.Width, _Bitmap.Height);
                _Data = _Bitmap.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            }
        }

        protected virtual void Unlock()
        {
            if (_Data != null)
            {
                _Bitmap.UnlockBits(_Data);
                _Data = null;
            }
        }
        #endregion

        #region Members Variables
        protected Bitmap _Bitmap;
        protected BitmapData _Data;
        #endregion
    }

    public class MaskIterator : BitmapIterator
    {
        #region Constructors
        public MaskIterator(Image image)
            : base(image)
        {
            Debug.Assert(Bitmap.PixelFormat == PixelFormat.Format1bppIndexed);
        }

        public MaskIterator(Bitmap bmp)
            : base(bmp)
        {
            Debug.Assert(Bitmap.PixelFormat == PixelFormat.Format1bppIndexed);
        }
        #endregion

        public override Color GetColor(int x, int y)
        {
            if (ImageHelper.GetBit(Data, x, y))
                return Color.White;
            return Color.Black;
        }

        #region IEnumerator Members

        public override object Current
        {
            get
            {
                if (_X < 0 || _Y < 0 || _X >= Width || _Y >= Height)
                    return null;
                return ImageHelper.GetBit(_Data, _X, _Y);
            }
        }

        protected override void Lock()
        {
            if (_Data == null)
            {
                var r = new Rectangle(0, 0, _Bitmap.Width, _Bitmap.Height);
                _Data = _Bitmap.LockBits(r, ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            }
        }

        #endregion
    }
}