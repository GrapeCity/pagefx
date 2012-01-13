using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace DataDynamics
{
    public unsafe class FastBitmap : IDisposable
    {
        private readonly Bitmap _bmp;
        private BitmapData _bits;
        private Getter _get;
        private Setter _set;
        private IndexGetter _getIndex;
        private IndexSetter _setIndex;
        private Color[] _pal;

        #region Constructors
        public FastBitmap(Bitmap bmp)
        {
            _bmp = bmp;
            Lock();
        }
        #endregion

        private delegate Color Getter(int x, int y);
        private delegate void Setter(int x, int y, Color c);
        private delegate int IndexGetter(int x, int y);
        private delegate void IndexSetter(int x, int y, int index);

        #region Format16bppArgb1555
        private static Color GetFormat16bppArgb1555(int x, int y)
        {
            //int c = *((ushort*)((byte*)bits.Scan0 + y * bits.Stride + 2 * x));
            //int a = (c & (1 << 15)) != 0 ? 1 : 0;
            //return Color.FromArgb(*(p + 1), *(p + 2), *(p + 3));
            return Color.Empty;
        }

        private static void SetFormat16bppArgb1555(int x, int y, Color c)
        {
            int v = 0;
            if (c.A != 0)
                v |= 1 << 15;
        }
        #endregion

        #region Format1bppIndexed
        private int GetIndex1(int x, int y)
        {
            var p = (byte*)_bits.Scan0 + y * _bits.Stride;
            int offset = x / 8;
            int bit = 7 - x % 8;
            p += offset;
            if ((p[0] & (1 << bit)) != 0)
                return 1;
            return 0;
        }

        private void SetIndex1(int x, int y, int index)
        {
            var p = (byte*)(void*)_bits.Scan0 + y * _bits.Stride;
            int offset = x / 8;
            int bit = 7 - x % 8;
            p += offset;
            if (index != 0)
                p[0] |= (byte)(1 << bit);
            else
                p[0] &= (byte)~(1 << bit);
        }

        private Color GetColor1(int x, int y)
        {
            return _pal[GetIndex1(x, y)];
        }

        private void SetColor1(int x, int y, Color c)
        {
            var c1 = _pal[1];
            if (c.R != c1.R || c.G != c1.G || c.B != c1.B || c.A != c1.A)
                SetIndex1(x, y, 1);
            else
                SetIndex1(x, y, 0);
        }
        #endregion

        #region Format4bppIndexed
        private int GetIndex4(int x, int y)
        {
            var p = (byte*)_bits.Scan0 + y * _bits.Stride + (x >> 1);
            if ((x & 1) == 0) return *p;
            return *(p + 1);
        }

        private void SetIndex4(int x, int y, int index)
        {
            var p = (byte*)_bits.Scan0 + y * _bits.Stride + (x >> 1);
            if ((x & 1) == 0)
                p[0] = (byte)index;
            else
                p[1] = (byte)index;
        }

        private Color GetColor4(int x, int y)
        {
            return _pal[GetIndex4(x, y)];
        }

        private void SetColor4(int x, int y, Color c)
        {
            int index = IndexOf(c);
            SetIndex4(x, y, index);
        }
        #endregion

        #region Format8bppIndexed
        private int GetIndex8(int x, int y)
        {
            return *((byte*)_bits.Scan0 + y * _bits.Stride + x);
        }

        private void SetIndex8(int x, int y, int index)
        {
            var p = (byte*)_bits.Scan0 + y * _bits.Stride + x;
            p[0] = (byte)index;
        }

        private Color GetColor8(int x, int y)
        {
            return _pal[GetIndex8(x, y)];
        }

        private void SetColor8(int x, int y, Color c)
        {
            int index = IndexOf(c);
            SetIndex8(x, y, index);
        }
        #endregion

        #region Format24bppRgb
        private Color GetFormat24bppRgb(int x, int y)
        {
            var p = (byte*)_bits.Scan0 + y * _bits.Stride + 3 * x;
            return Color.FromArgb(p[2], p[1], p[0]);
        }

        private void SetFormat24bppRgb(int x, int y, Color c)
        {
            var p = (byte*)_bits.Scan0 + y * _bits.Stride + 3 * x;
            p[0] = c.B;
            p[1] = c.G;
            p[2] = c.R;
        }
        #endregion

        #region Format32bppArgb
        private Color GetFormat32bppArgb(int x, int y)
        {
            var p = (byte*)_bits.Scan0 + y * _bits.Stride + 4 * x;
            return Color.FromArgb(p[3], p[2], p[1], p[0]);
        }

        private void SetFormat32bppArgb(int x, int y, Color c)
        {
            var p = (byte*)_bits.Scan0 + y * _bits.Stride + 4 * x;
            p[0] = c.B;
            p[1] = c.G;
            p[2] = c.R;
            p[3] = c.A;
        }
        #endregion

        #region Format32bppPArgb
        private Color GetFormat32bppPArgb(int x, int y)
        {
            var p = (byte*)_bits.Scan0 + y * _bits.Stride + 4 * x;
            return Color.FromArgb(p[3], p[2], p[1], p[0]);
        }

        private void SetFormat32bppPArgb(int x, int y, Color c)
        {
            var p = (byte*)_bits.Scan0 + y * _bits.Stride + 4 * x;
            p[0] = c.B;
            p[1] = c.G;
            p[2] = c.R;
            p[3] = c.A;
        }
        #endregion

        #region Format32bppRgb
        private Color GetFormat32bppRgb(int x, int y)
        {
            var p = (byte*)_bits.Scan0 + y * _bits.Stride + 4 * x;
            return Color.FromArgb(p[2], p[1], p[0]);
        }

        private void SetFormat32bppRgb(int x, int y, Color c)
        {
            var p = (byte*)_bits.Scan0 + y * _bits.Stride + 4 * x;
            p[0] = c.B;
            p[1] = c.G;
            p[2] = c.R;
        }
        #endregion

        #region Dispose Pattern
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                    Unlock();
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.
                _bits = null;
            }
            _disposed = true;
        }

        // Use C# destructor syntax for finalization code.
        ~FastBitmap()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }

        private bool _disposed;
        #endregion Dispose Pattern

        #region Public Members
        public void Lock()
        {
            if (_bits == null)
            {
                var pf = _bmp.PixelFormat;
                var r = new Rectangle(0, 0, _bmp.Width, _bmp.Height);
                _bits = _bmp.LockBits(r, ImageLockMode.ReadWrite, pf);
                switch (pf)
                {
                    #region Indexed
                    case PixelFormat.Format1bppIndexed:
                        _get = GetColor1;
                        _set = SetColor1;
                        _getIndex = GetIndex1;
                        _setIndex = SetIndex1;
                        _pal = _bmp.Palette.Entries;
                        break;

                    case PixelFormat.Format4bppIndexed:
                        _get = GetColor4;
                        _set = SetColor4;
                        _getIndex = GetIndex4;
                        _setIndex = SetIndex4;
                        _pal = _bmp.Palette.Entries;
                        break;

                    case PixelFormat.Format8bppIndexed:
                        _get = GetColor8;
                        _set = SetColor8;
                        _getIndex = GetIndex8;
                        _setIndex = SetIndex8;
                        _pal = _bmp.Palette.Entries;
                        break;

                    case PixelFormat.Indexed:
                        throw new NotSupportedException();
                    #endregion

                    case PixelFormat.Format16bppArgb1555:
                        _get = GetFormat16bppArgb1555;
                        _set = SetFormat16bppArgb1555;
                        break;

                    case PixelFormat.Format16bppRgb555:
                        //_get = new GET(GetFormat16bppRgb555);
                        //_set = new SET(SetFormat16bppRgb555);
                        break;

                    case PixelFormat.Format16bppRgb565:
                        //_get = new GET(GetFormat16bppRgb565);
                        //_set = new SET(SetFormat16bppRgb565);
                        break;

                    case PixelFormat.Format16bppGrayScale:
                        //_get = new GET(GetFormat16bppGrayScale);
                        //_set = new SET(SetFormat16bppGrayScale);
                        break;

                    #region 24, 32
                    case PixelFormat.Canonical:
                        _get = GetFormat32bppArgb;
                        _set = SetFormat32bppArgb;
                        break;
                    
                    case PixelFormat.Format24bppRgb:
                        _get = GetFormat24bppRgb;
                        _set = SetFormat24bppRgb;
                        break;
                    case PixelFormat.Format32bppArgb:
                        _get = GetFormat32bppArgb;
                        _set = SetFormat32bppArgb;
                        break;
                    case PixelFormat.Format32bppPArgb:
                        _get = GetFormat32bppPArgb;
                        _set = SetFormat32bppPArgb;
                        break;
                    case PixelFormat.Format32bppRgb:
                        _get = GetFormat32bppRgb;
                        _set = SetFormat32bppRgb;
                        break;
                    #endregion

                    #region NotImplemented
                    case PixelFormat.Format48bppRgb:
                        //_get = GetFormat48bppRgb;
                        //_set = SetFormat48bppRgb;
                        throw new NotImplementedException();
                    
                    case PixelFormat.Format64bppArgb:
                        //_get = new GET(GetFormat64bppArgb);
                        //_set = new SET(SetFormat64bppArgb);
                        throw new NotImplementedException();

                    case PixelFormat.Format64bppPArgb:
                        //_get = new GET(GetFormat64bppPArgb);
                        //_set = new SET(SetFormat64bppPArgb);
                        throw new NotImplementedException();
                    #endregion

                    default:
                        break;
                }
            }
        }

        public void Unlock()
        {
            if (_bits != null)
            {
                _bmp.UnlockBits(_bits);
                _bits = null;
                _pal = null;
            }
        }

        public Color this[int x, int y]
        {
            get
            {
                if (_get != null)
                    return _get(x, y);
                return Color.Empty;
            }
            set
            {
                if (_set != null)
                    _set(x, y, value);
            }
        }

        public int GetIndex(int x, int y)
        {
            if (_getIndex != null)
                return _getIndex(x, y);
            return -1;
        }

        public void SetIndex(int x, int y, int index)
        {
            if (_setIndex != null)
                _setIndex(x, y, index);
        }
        #endregion

        #region IndexOf
        //private Hashtable _palCache;

        private int IndexOf(Color c)
        {
            //TODO: Optimize
            //if (_palCache == null)
            //{

            //}
            int n = _pal.Length;
            for (int i = 0; i < n; ++i)
            {
                var pc = _pal[i];
                if (c.R == pc.R && c.G == pc.G && c.B == pc.B && c.A == pc.A)
                    return i;
            }
            return 0;
        }
        #endregion
    }
}