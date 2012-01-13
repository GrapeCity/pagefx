using System.IO;
using System.Drawing.Imaging;
using flash.display;
using flash.net;
using flash.utils;

namespace System.Drawing
{
    using FlashBitmapData = flash.display.BitmapData;
    using FlashBitmap = flash.display.Bitmap;

    public sealed class Bitmap : Image
    {
        #region constructors
        // constructors


#if NET_2_0
        // required for XmlSerializer (#323246)
        private Bitmap()
        {
        }
#endif

        public Bitmap(int width, int height)
            : this(width, height, PixelFormat.Format32bppArgb)
        {
        }

        public Bitmap(int width, int height, Graphics g)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            Data = new FlashBitmapData(width, height);
            Data.draw(g._container);
        }

        public Bitmap(int width, int height, PixelFormat format)
        {
            Data = new FlashBitmapData(width, height);
        }

        public Bitmap(Image original) : this(original, original.Width, original.Height) { }

        public Bitmap(Image original, Size newSize) 
            : this(original, newSize.Width, newSize.Height)
        {
        }

        public Bitmap(Stream stream)
            : this(stream, false)
        {
        }

        public Bitmap(Stream stream, bool useIcm)
        {
            var bytes = new ByteArray();
            while (true)
            {
                int b = stream.ReadByte();
                if (b < 0) break;
                bytes.writeByte(b);
            }
            bytes.position = 0;
            Load(bytes);
        }

        public Bitmap(ByteArray bytes)
        {
            Load(bytes);
        }

        void Load(ByteArray bytes)
        {
            var loader = new Loader();
            HandleLoaderEvents(loader, "<stream>");
            loader.loadBytes(bytes);
        }

        public Bitmap (string url) : this (url, false) {}

        public Bitmap(string url, bool useIcm)
        {
            if (url == null)
                throw new ArgumentNullException("filename");

            var loader = new Loader();
            HandleLoaderEvents(loader, url);
            loader.load(new URLRequest(url));
        }

        void HandleLoaderEvents(Loader loader, string filename)
        {
            loader.contentLoaderInfo.complete +=
                e =>
                    {
                        var content = loader.content;
                        var bmp = content as FlashBitmap;
                        if (bmp == null)
                        {
                            throw new InvalidOperationException(
                                string.Format("Unable to load image {0}. Content is {1}",
                                              filename, content != null ? "not null" : "null"));
                        }

                        Data = bmp.bitmapData.clone();

                        if (Loaded != null)
                        {
                            Loaded(this, EventArgs.Empty);
                            Loaded = null;
                        }
                    };

            loader.contentLoaderInfo.ioError +=
                e =>
                    {
                        throw new IOException(
                            string.Format("Unable to load image {0}", filename));
                    };
        }

        public Bitmap(Image original, int width, int height)
            : this(width, height, PixelFormat.Format32bppArgb)
        {
            var g = Graphics.FromImage(this);
            g.DrawImage(original, 0, 0, width, height);
            g.Dispose();
        }

#if NOT_PFX
        public Bitmap(int width, int height, int stride, PixelFormat format, IntPtr scan0)
        {
            throw new NotImplementedException();
        }
#endif
        #endregion

        // methods
        static uint Premultiply(uint v, uint a)
        {
            if (a == 0) return 0;
            if (a >= 255) return v;
            return (uint)Math.Floor(a * v / 255.0);
        }

        static uint Unmultiply(uint v, uint a)
        {
            if (a == 0) return 0;
            if (a >= 255) return v;
            return (uint)Math.Floor((double)v * 255.0 / (double)a);
        }

        public Color GetPixel(int x, int y)
        {
            uint argb = Data.getPixel32(x, y);
            uint a = (argb >> 24) & 0xff;
            uint r = (argb >> 16) & 0xff;
            uint g = (argb >> 8) & 0xff;
            uint b = argb & 0xff;
            r = Unmultiply(r, a);
            g = Unmultiply(g, a);
            b = Unmultiply(b, a);
            return Color.FromArgb((int)a, (int)r, (int)g, (int)b);
        }

        public void SetPixel(int x, int y, Color color)
        {
            uint a = color.A;
            uint r = Premultiply(color.R, a);
            uint g = Premultiply(color.G, a);
            uint b = Premultiply(color.B, a);
            uint argb = (a << 24) | (r << 16) | (g << 8) | b;
            Data.setPixel32(x, y, argb);
        }

        public Bitmap Clone(Rectangle rect, PixelFormat format)
        {
            throw new NotImplementedException();
        }

        public Bitmap Clone(RectangleF rect, PixelFormat format)
        {
            throw new NotImplementedException();
        }

#if NOT_PFX
		public BitmapData LockBits (Rectangle rect, ImageLockMode flags, PixelFormat format)
		{
			BitmapData result = new BitmapData();
			return LockBits (rect, flags, format, result);
		}

#if NET_2_0
		public
#endif
		BitmapData LockBits (Rectangle rect, ImageLockMode flags, PixelFormat format, BitmapData bitmapData)
		{
			throw new NotImplementedException();
		}

        public void UnlockBits (BitmapData bitmapdata)
		{
            throw new NotImplementedException();
		}
#endif

        #region MakeTransparent
        public void MakeTransparent()
        {
            Color clr = GetPixel(0, 0);
            MakeTransparent(clr);
        }

        public void MakeTransparent(Color transparentColor)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void SetResolution(float xDpi, float yDpi)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            throw new System.NotImplementedException();
        }
    }
}
