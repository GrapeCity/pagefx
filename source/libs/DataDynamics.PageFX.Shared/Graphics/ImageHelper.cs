using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace DataDynamics
{
    #region struct BitmapInfoHeader
    [StructLayout(LayoutKind.Sequential, Pack = 2), ComVisible(false)]
    public struct BitmapInfoHeader
    {
        public int Size;
        public int Width;
        public int Height;
        public ushort Planes;
        public ushort BitCount;
        public int Compression;
        public int ImageSize;
        public int XPelsPerMeter;
        public int YPelsPerMeter;
        public int ClrUsed;
        public int ClrImportant;
    }
    #endregion

    public static class ImageHelper
    {
        public static bool IsIndexed(Image image)
        {
            if (image == null) return false;
            switch (image.PixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check if the image is grayscale
        /// </summary>
        /// 
        /// <param name="image">Image to check</param>
        /// 
        /// <returns>Returns <b>true</b> if the image is grayscale or <b>false</b> otherwise.</returns>
        /// 
        /// <remarks>The methods check if the image is a grayscale image of 256 gradients.
        /// The method first examines if the image's pixel format is
        /// <see cref="System.Drawing.Imaging.PixelFormat">Format8bppIndexed</see>
        /// and then it examines its palette to check if the image is grayscale or not.</remarks>
        /// 
        public static bool IsGrayscale(Bitmap image)
        {
            bool ret = false;

            // check pixel format
            if (image.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                ret = true;
                // check palette
                var cp = image.Palette;
                Color c;
                // init palette
                for (int i = 0; i < 256; i++)
                {
                    c = cp.Entries[i];
                    if ((c.R != i) || (c.G != i) || (c.B != i))
                    {
                        ret = false;
                        break;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Create and initialize grayscale image
        /// </summary>
        /// 
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        /// 
        /// <returns>Returns the created grayscale image</returns>
        /// 
        /// <remarks>The methods create new grayscale image and initializes its palette.
        /// Grayscale image is represented as
        /// <see cref="System.Drawing.Imaging.PixelFormat">Format8bppIndexed</see>
        /// image with palette initialized to 256 gradients of gray color</remarks>
        /// 
        public static Bitmap CreateGrayscaleImage(int width, int height)
        {
            // create new image
            var image = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            // set palette to grayscale
            SetGrayscalePalette(image);
            // return new image
            return image;
        }

        /// <summary>
        /// Set pallete of the image to grayscale
        /// </summary>
        /// 
        /// <param name="image">Image to initialize</param>
        /// 
        /// <remarks>The method initializes palette of
        /// <see cref="System.Drawing.Imaging.PixelFormat">Format8bppIndexed</see>
        /// image with 256 gradients of gray color.</remarks>
        /// 
        public static void SetGrayscalePalette(Bitmap image)
        {
            // check pixel format
            if (image.PixelFormat != PixelFormat.Format8bppIndexed)
                throw new ArgumentException();

            // get palette
            var cp = image.Palette;
            // init palette
            for (int i = 0; i < 256; i++)
            {
                cp.Entries[i] = Color.FromArgb(i, i, i);
            }
            // set palette back
            image.Palette = cp;
        }

        /// <summary>
        /// Clone image
        /// </summary>
        /// 
        /// <param name="src">Source image</param>
        /// <param name="format">Pixel format of result image</param>
        /// 
        /// <returns>Returns clone of the source image with specified pixel format</returns>
        ///
        /// <remarks>The original <see cref="System.Drawing.Bitmap.Clone(System.Drawing.Rectangle, System.Drawing.Imaging.PixelFormat)">Bitmap.Clone()</see>
        /// does not produce the desired result - it does not create a clone with specified pixel format.
        /// More of it, the original method does not create an actual clone - it does not create a copy
        /// of the image. That is why this method was implemented to provide the functionality.</remarks> 
        ///
        public static Bitmap Clone(Bitmap src, PixelFormat format)
        {
            // copy image if pixel format is the same
            if (src.PixelFormat == format)
                return Clone(src);

            int width = src.Width;
            int height = src.Height;

            // create new image with desired pixel format
            var bmp = new Bitmap(width, height, format);

            // draw source image on the new one using Graphics
            var g = Graphics.FromImage(bmp);
            g.DrawImage(src, 0, 0, width, height);
            g.Dispose();

            return bmp;
        }

        /// <summary>
        /// Copy a block of memory
        /// </summary>
        /// 
        /// <param name="dst">Destination pointer</param>
        /// <param name="src">Source pointer</param>
        /// <param name="count">Memory block's length to copy</param>
        /// 
        /// <returns>Return's the value of <b>dst</b> - pointer to destination.</returns>
        /// 
        [DllImport("ntdll.dll")]
        private static extern IntPtr memcpy(
            IntPtr dst,
            IntPtr src,
            int count);

        /// <summary>
        /// Clone image
        /// </summary>
        /// 
        /// <param name="src">Source image</param>
        /// 
        /// <returns>Return clone of the source image</returns>
        /// 
        /// <remarks>The original <see cref="System.Drawing.Bitmap.Clone(System.Drawing.Rectangle, System.Drawing.Imaging.PixelFormat)">Bitmap.Clone()</see>
        /// does not produce the desired result - it does not create an actual clone (it does not create a copy
        /// of the image). That is why this method was implemented to provide the functionality.</remarks> 
        /// 
        public static Bitmap Clone(Bitmap src)
        {
            // get source image size
            int width = src.Width;
            int height = src.Height;

            // lock source bitmap data
            var srcData = src.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, src.PixelFormat);

            // create new image
            var dst = new Bitmap(width, height, src.PixelFormat);

            // lock destination bitmap data
            var dstData = dst.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, dst.PixelFormat);

            memcpy(dstData.Scan0, srcData.Scan0, height * srcData.Stride);

            // unlock both images
            dst.UnlockBits(dstData);
            src.UnlockBits(srcData);

            //
            if (
                (src.PixelFormat == PixelFormat.Format1bppIndexed) ||
                (src.PixelFormat == PixelFormat.Format4bppIndexed) ||
                (src.PixelFormat == PixelFormat.Format8bppIndexed) ||
                (src.PixelFormat == PixelFormat.Indexed))
            {
                var srcPalette = src.Palette;
                var dstPalette = dst.Palette;

                int n = srcPalette.Entries.Length;

                // copy pallete
                for (int i = 0; i < n; i++)
                {
                    dstPalette.Entries[i] = srcPalette.Entries[i];
                }

                dst.Palette = dstPalette;
            }

            return dst;
        }

        /// <summary>
        ///  Format an image
        /// </summary>
        /// 
        /// <param name="image">Source image to format</param>
        /// 
        /// <remarks>Formats the image to one of the formats, which are supported
        /// by the <b>AForge.Imaging</b> library. The image is left untouched in the
        /// case if it already of
        /// <see cref="System.Drawing.Imaging.PixelFormat">Format24bppRgb</see>
        /// format or it is grayscale (<see cref="IsGrayscale"/>), otherwise the image converted to
        /// <see cref="System.Drawing.Imaging.PixelFormat">Format24bppRgb</see>
        /// format.</remarks>
        ///
        public static void FormatImage(ref Bitmap image)
        {
            if (
                (image.PixelFormat != PixelFormat.Format24bppRgb) &&
                (IsGrayscale(image) == false)
                )
            {
                var tmp = image;
                // convert to 24 bits per pixel
                image = Clone(tmp, PixelFormat.Format24bppRgb);
                // delete old image
                tmp.Dispose();
            }
        }

        public static void Save(Image image, string path)
        {
            string ext = Path.GetExtension(path);
            var f = GetImageFormatByExtension(ext);
            using (Stream fs = File.OpenWrite(path))
            {
                image.Save(fs, f);
            }
        }

        public static void Invert(Bitmap bmp)
        {
            using (var f = new FastBitmap(bmp))
            {
                int w = bmp.Width;
                int h = bmp.Height;
                Color c;
                for (int y = 0; y < h; ++y)
                {
                    for (int x = 0; x < w; ++x)
                    {
                        c = f[x, y];
                        c = Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
                        f[x, y] = c;
                    }
                }
            }
        }

        public static void Dispose(Bitmap[] frames)
        {
            for (int i = 0; i < frames.Length; ++i)
            {
                if (frames[i] != null)
                {
                    frames[i].Dispose();
                    frames[i] = null;
                }
            }
        }

        public static Bitmap GetAverage(Bitmap[] frames, bool dispose)
        {
            int n = frames.Length;
            if (n <= 1) return frames[0];

            int w = frames[0].Width;
            int h = frames[0].Height;
            var bits = new FastBitmap[n];
            for (int i = 0; i < n; ++i)
            {
                bits[i] = new FastBitmap(frames[i]);
            }

            Color c;
            var res = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            var bits2 = new FastBitmap(res);
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    int r = 0, g = 0, b = 0;
                    for (int i = 0; i < n; ++i)
                    {
                        c = bits[i][x, y];
                        r += c.R;
                        g += c.G;
                        b += c.B;
                    }
                    r /= n;
                    g /= n;
                    b /= n;
                    c = Color.FromArgb(r, g, b);
                    bits2[x, y] = c;
                }
            }

            bits2.Dispose();
            for (int i = 0; i < n; ++i)
                bits[i].Dispose();

            if (dispose)
                Dispose(frames);

            return res;
        }

        public static string GetFormatExtension(Image image)
        {
            var f = image.RawFormat;
            if (f == ImageFormat.Bmp) return "bmp";
            if (f == ImageFormat.Emf) return "emf";
            if (f == ImageFormat.Exif) return "exif";
            if (f == ImageFormat.Gif) return "gif";
            if (f == ImageFormat.Icon) return "ico";
            if (f == ImageFormat.Jpeg) return "jpg";
            if (f == ImageFormat.MemoryBmp) return "bmp";
            if (f == ImageFormat.Png) return "png";
            if (f == ImageFormat.Tiff) return "tif";
            if (f == ImageFormat.Wmf) return "wmf";
            return "bmp";
        }

        public static ImageFormat GetImageFormatByExtension(string ext)
        {
            var cmp = StringComparison.InvariantCultureIgnoreCase;
            if (ext.EndsWith("bmp", cmp))
                return ImageFormat.Bmp;
            if (ext.EndsWith("emf", cmp))
                return ImageFormat.Emf;
            if (ext.EndsWith("exif", cmp))
                return ImageFormat.Exif;
            if (ext.EndsWith("gif", cmp))
                return ImageFormat.Gif;
            if (ext.EndsWith("ico", cmp))
                return ImageFormat.Icon;
            if (ext.EndsWith("jpg", cmp)
                || ext.EndsWith("jpeg", cmp))
                return ImageFormat.Jpeg;
            if (ext.EndsWith("png", cmp))
                return ImageFormat.Png;
            if (ext.EndsWith("tif", cmp)
                || ext.EndsWith("tiff", cmp))
                return ImageFormat.Tiff;
            if (ext.EndsWith("wmf", cmp))
                return ImageFormat.Wmf;
            return ImageFormat.Png;
        }

        #region DIB -> Bitmap
        //[DllImport("GdiPlus.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        //private static extern int GdipCreateBitmapFromGdiDib(IntPtr pBIH, IntPtr pPix, out IntPtr pBitmap);

        //public static Bitmap BitmapFromDIB(IntPtr pDIB, IntPtr pPix)
        //{
        //    MethodInfo mi = typeof(Bitmap).GetMethod("FromGDIplus",
        //                                             BindingFlags.Static | BindingFlags.NonPublic);

        //    if (mi == null) return null; //permission problem

        //    IntPtr pBmp = IntPtr.Zero;
        //    int status = GdipCreateBitmapFromGdiDib(pDIB, pPix, out pBmp);

        //    //if ((status == 0) && (pBmp != IntPtr.Zero))
        //    if (pBmp != IntPtr.Zero)
        //        return (Bitmap)mi.Invoke(null, new object[] {pBmp});

        //    return null;
        //}
        #endregion

        // This method can be used to retrieve an Image from a block 
        // of Base64-encoded text.
        public static Image FromBase64(string text)
        {
            var bytes = Convert.FromBase64String(text);
            var ms = new MemoryStream(bytes);
            return Image.FromStream(ms);
        }

        public static int CorrectBpp(int bpp)
        {
            if (bpp <= 1) return 1;
            if (bpp <= 4) return 4;
            if (bpp <= 8) return 8;
            if (bpp <= 24) return 24;
            return 32;
        }

        public static int GetScanLine(int width, int bpp)
        {
            return (((width * bpp) + 31) >> 5) << 2;
        }

        public static int GetDibSize(int width, int height, int bpp)
        {
            bpp = CorrectBpp(bpp);

            int ps = 0;
            switch (bpp)
            {
                case 1:
                    ps = 2;
                    break;
                case 4:
                    ps = 16;
                    break;
                case 8:
                    ps = 256;
                    break;
            }

            int scanLine = GetScanLine(width, bpp);
            int imageSize = scanLine * height;
            return 40 + ps * 4 + imageSize;
        }

        public static PixelFormat GetPixelFormat(int bpp)
        {
            bpp = CorrectBpp(bpp);
            switch (bpp)
            {
                case 1:
                    return PixelFormat.Format1bppIndexed;
                case 4:
                    return PixelFormat.Format4bppIndexed;
                case 8:
                    return PixelFormat.Format8bppIndexed;
                case 16:
                    return PixelFormat.Format16bppRgb555;
                case 24:
                    return PixelFormat.Format24bppRgb;
            }
            return PixelFormat.Format32bppArgb;
        }

        public static Bitmap CreateFrame(BitmapInfoHeader bi)
        {
            var pf = GetPixelFormat(bi.BitCount);
            var res = new Bitmap(bi.Width, bi.Height);
            return res.Clone(new Rectangle(0, 0, res.Width, res.Height), pf);
        }

        public static bool AreEqual(Bitmap bx, Bitmap by, bool alpha)
        {
            int w = bx.Width;
            if (w != by.Width) return false;
            int h = bx.Height;
            if (h != by.Height) return false;
            using (var fx = new FastBitmap(bx))
            using (var fy = new FastBitmap(by))
            {
                for (int y = 0; y < h; ++y)
                {
                    for (int x = 0; x < w; ++x)
                    {
                        var cx = fx[x, y];
                        var cy = fy[x, y];
                        if (alpha)
                        {
                            if (cx != cy)
                                return false;
                        }
                        else
                        {
                            if (cx.R != cy.R || cx.G != cy.G || cx.B != cy.B)
                                return false;
                        }
                    }
                }
            }
            return true;
        }

        public static bool AreEqual(Image x, Image y, bool alpha)
        {
            var bx = ToBitmap(x);
            var by = ToBitmap(y);
            return AreEqual(bx, by, alpha);
        }

        public static Bitmap ToBitmap(Image image)
        {
            if (image.RawFormat.Guid != ImageFormat.MemoryBmp.Guid)
                return new Bitmap(image);
            return (Bitmap)image;
        }

        public static byte[] ToByteArray(Image image)
        {
            return ToByteArray(ToBitmap(image));
        }

        public static unsafe byte[] ToByteArray(Bitmap bmp)
        {
            var s = new MemoryStream();
            var data = GetBitmapData(bmp);
            var begin = (byte*)(void*)data.Scan0;
            for (int y = 0; y < bmp.Height; ++y)
            {
                var p = begin + (y * data.Stride);
                for (int x = 0; x < data.Stride; ++x)
                    s.WriteByte(p[x]);
            }
            bmp.UnlockBits(data);
            var output = s.ToArray();
            s.Close();
            return output;
        }

        #region BitmapData Utils
        public static BitmapData GetBitmapData(Bitmap bmp)
        {
            return GetBitmapData(bmp, bmp.PixelFormat);
        }

        public static BitmapData GetBitmapData(Bitmap bmp, PixelFormat pf)
        {
            var r = new Rectangle(0, 0, bmp.Width, bmp.Height);
            return bmp.LockBits(r, ImageLockMode.ReadWrite, pf);
        }

        public static unsafe void Clear(BitmapData data, bool color)
        {
            for (int y = 0; y < data.Height; ++y)
            {
                var p = (byte*)(void*)data.Scan0;
                p += y * data.Stride;
                for (int x = 0; x < data.Stride; ++x)
                    p[x] = color ? (byte)0xFF : (byte)0x00;
            }
        }

        public static unsafe void Clear(BitmapData data, Color c)
        {
            var p = (byte*)(void*)data.Scan0;
            for (int y = 0; y < data.Height; ++y)
            {
                var p1 = p + (y * data.Stride);
                for (int x = 0; x < data.Width; ++x)
                {
                    var p2 = p1 + (3 * x);
                    p2[0] = c.B;
                    p2[1] = c.G;
                    p2[2] = c.R;
                }
            }
        }

        public static unsafe void SetBit(BitmapData data, int x, int y, bool value)
        {
            var p = (byte*)(void*)data.Scan0;
            int offset = x / 8;
            int bit = 7 - x % 8;
            p += y * data.Stride;
            p += offset;
            if (value) p[0] |= (byte)(1 << bit);
            else p[0] &= (byte)~(1 << bit);
        }

        public static unsafe bool GetBit(BitmapData data, int x, int y)
        {
            var p = (byte*)(void*)data.Scan0;
            int offset = x / 8;
            int bit = 7 - x % 8;
            p += y * data.Stride;
            p += offset;
            return (p[0] & (1 << bit)) != 0;
        }

        public static unsafe void SetColor(BitmapData data, int x, int y, Color value)
        {
            var p = (byte*)(void*)data.Scan0;
            p += y * data.Stride;
            p += 3 * x;
            p[0] = value.B;
            p[1] = value.G;
            p[2] = value.R;
            //p[0] = value.R;
            //p[1] = value.G;
            //p[2] = value.B;
        }

        public static Color GetPixel(BitmapData bits, int x, int y)
        {
            var pf = bits.PixelFormat;
            if (pf == PixelFormat.Format32bppArgb)
            {
                return Color.Empty;
            }
            if (pf == PixelFormat.Format24bppRgb)
            {
                return GetColor(bits, x, y);
            }
            if (pf == PixelFormat.Format8bppIndexed)
            {
                int l = GetLevel(bits, x, y);
                return Color.FromArgb(l, l, l);
            }
            if (pf == PixelFormat.Format1bppIndexed)
            {
                if (GetBit(bits, x, y)) return Color.White;
                return Color.Black;
            }
            return Color.Empty;
        }

        public static unsafe Color GetColor(BitmapData data, int x, int y)
        {
            var p = (byte*)data.Scan0 + y * data.Stride + 3 * x;
            return Color.FromArgb(*(p + 2), *(p + 1), *p);
        }

        public static unsafe void SetLevel(BitmapData data, int x, int y, int value)
        {
            var p = (byte*)(void*)data.Scan0;
            p += y * data.Stride;
            p += x;
            value = Math.Min(255, Math.Max(0, value));
            p[0] = (byte)value;
        }

        public static unsafe int GetLevel(BitmapData data, int x, int y)
        {
            var p = (byte*)(void*)data.Scan0;
            p += y * data.Stride;
            p += x;
            return p[0];
        }
        #endregion

        #region Mask Utils
        public static bool IsMask(Image mask)
        {
            var pf = mask.PixelFormat;
            if (pf == PixelFormat.Format1bppIndexed
                || pf == PixelFormat.Format8bppIndexed)
                return true;
            return false;
        }

        public static unsafe Bitmap ApplyMask(Image image, Image mask)
        {
            if (image == null)
                throw new ArgumentNullException("image");
            if (mask == null)
                throw new ArgumentNullException("mask");
            if (!IsMask(mask))
                throw new ArgumentException("Mask has invalid pixel format.", "mask");
            int w = image.Width;
            int h = image.Height;
            if (mask.Width != w)
                throw new ArgumentException("Mask width is not equal to source image width", "mask");
            if (mask.Height != h)
                throw new ArgumentException("Mask width is not equal to source image width", "mask");
            var bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            var src = new Bitmap(image);
            var msk = new Bitmap(mask);
            var bits = GetBitmapData(bmp);
            //BitmapData srcBits = GetBitmapData(src);
            var mskBits = GetBitmapData(msk);
            bool mono = mask.PixelFormat == PixelFormat.Format1bppIndexed;
            var ptr = (byte*)bits.Scan0;
            Color c;
            if (mono)
            {
                for (int y = 0; y < h; ++y)
                {
                    for (int x = 0; x < w; ++x)
                    {
                        bool alpha = GetBit(mskBits, x, y);
                        c = src.GetPixel(x, y);
                        int offset = y * bits.Stride + 4 * x;
                        ptr[offset] = (byte)(alpha ? 1 : 0);
                        ptr[offset + 1] = c.R;
                        ptr[offset + 2] = c.G;
                        ptr[offset + 3] = c.B;
                    }
                }
            }
            else
            {
                for (int y = 0; y < h; ++y)
                {
                    for (int x = 0; x < w; ++x)
                    {
                        int alpha = GetLevel(mskBits, x, y);
                        c = src.GetPixel(x, y);
                        int offset = y * bits.Stride + 4 * x;
                        ptr[offset] = (byte)alpha;
                        ptr[offset + 1] = c.R;
                        ptr[offset + 2] = c.G;
                        ptr[offset + 3] = c.B;
                    }
                }
            }
            bmp.UnlockBits(bits);
            //src.UnlockBits(srcBits);
            msk.UnlockBits(mskBits);
            return bmp;
        }

        public static Bitmap CreateMask(Image image)
        {
            var bmp = ToBitmap(image);
            var mask = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format1bppIndexed);
            var maskData = GetBitmapData(mask);
            var iter = new BitmapIterator(bmp);
            var tc = iter.GetColor(0, 0);
            CreateMask(maskData, iter, tc);
            mask.UnlockBits(maskData);
            return mask;
        }

        public static Bitmap CreateMask(Image image, Color tc)
        {
            var bmp = ToBitmap(image);
            var mask = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format1bppIndexed);
            var maskData = GetBitmapData(mask);
            var iter = new BitmapIterator(bmp);
            CreateMask(maskData, iter, tc);
            mask.UnlockBits(maskData);
            return mask;
        }

        public static Bitmap CreateAlphaMask(Image image)
        {
            var bmp = ToBitmap(image);
            int w = bmp.Width;
            int h = bmp.Height;
            var mask = new Bitmap(w, h, PixelFormat.Format8bppIndexed);
            var maskData = GetBitmapData(mask);
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    var c = bmp.GetPixel(x, y);
                    SetLevel(maskData, x, y, c.A);
                }
            }
            mask.UnlockBits(maskData);
            return mask;
        }

        public static void CreateMask(BitmapData maskData, BitmapIterator iter, Color tc)
        {
            foreach (Color c in iter)
            {
                if (c.R == tc.R && c.G == tc.G && c.B == tc.B)
                    SetBit(maskData, iter.X, iter.Y, false);
                else
                    SetBit(maskData, iter.X, iter.Y, true);
            }
        }
        #endregion
    }

    #region class BitmapColors
    public class BitmapColors : IEnumerable
    {
        public BitmapColors(Bitmap bmp)
        {
            _Bitmap = bmp;
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

        public void Lock()
        {
            if (_Data == null)
            {
                // GDI+ still lies to us - the return format is BGR, NOT RGB.
                _Data =
                    _Bitmap.LockBits(new Rectangle(0, 0, _Bitmap.Width, _Bitmap.Height), ImageLockMode.ReadWrite,
                                     PixelFormat.Format24bppRgb);
            }
        }

        public void Unlock()
        {
            if (_Data != null)
            {
                _Bitmap.UnlockBits(_Data);
                _Data = null;
            }
        }

        private Color this[int x, int y]
        {
            get
            {
                Lock();
                return ImageHelper.GetColor(_Data, x, y);
            }
            set
            {
                Lock();
                ImageHelper.SetColor(_Data, x, y, value);
            }
        }

        #region IEnumerable Members
        public IEnumerator GetEnumerator()
        {
            return new BitmapIterator(_Bitmap);
        }
        #endregion

        #region Member Variables
        private readonly Bitmap _Bitmap;
        private BitmapData _Data;
        #endregion
    }
    #endregion
}