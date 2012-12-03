//
// System.Drawing.Image.cs
//
// Authors: 	Christian Meyer (Christian.Meyer@cs.tum.edu)
// 		Alexandre Pigolkine (pigolkine@gmx.de)
//		Jordi Mas i Hernandez (jordi@ximian.com)
//		Sanjay Gupta (gsanjay@novell.com)
//		Ravindra (rkumar@novell.com)
//		Sebastien Pouliot  <sebastien@ximian.com>
//
// Copyright (C) 2002 Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2004, 2007 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;

namespace System.Drawing
{
    using FlashBitmapData = flash.display.BitmapData;

    public abstract class Image : MarshalByRefObject, IDisposable, ICloneable
    {
        public delegate bool GetThumbnailImageAbort();

        internal FlashBitmapData Data;

        public EventHandler Loaded;
        
        // constructor
        internal Image()
        {
        }

        // public methods
        #region NOT_PFX: FromFile
#if NOT_PFX
        public static Image FromFile(string filename)
        {
            return FromFile(filename, false);
        }

        public static Image FromFile(string filename, bool useEmbeddedColorManagement)
        {
            //if (!File.Exists(filename))
            //    throw new FileNotFoundException(filename);
            throw new NotImplementedException();
        }
#endif
        #endregion

        public bool IsDataLoaded
        {
            get { return Data != null; }
        }

        // note: FromStream can return either a Bitmap or Metafile instance

        public static Image FromStream(Stream stream)
        {
            return LoadFromStream(stream, false);
        }

        [MonoLimitation("useEmbeddedColorManagement  isn't supported.")]
        public static Image FromStream(Stream stream, bool useEmbeddedColorManagement)
        {
            return LoadFromStream(stream, false);
        }

        // See http://support.microsoft.com/default.aspx?scid=kb;en-us;831419 for performance discussion	
        [MonoLimitation("useEmbeddedColorManagement  and validateImageData aren't supported.")]
        public static Image FromStream(Stream stream, bool useEmbeddedColorManagement, bool validateImageData)
        {
            return LoadFromStream(stream, false);
        }

        internal static Image LoadFromStream(Stream stream, bool keepAlive)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            throw new NotImplementedException();
        }

        #region PixelFormat Utils
        public static int GetPixelFormatSize(PixelFormat pixfmt)
        {
            int result = 0;
            switch (pixfmt)
            {
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    result = 16;
                    break;
                case PixelFormat.Format1bppIndexed:
                    result = 1;
                    break;
                case PixelFormat.Format24bppRgb:
                    result = 24;
                    break;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    result = 32;
                    break;
                case PixelFormat.Format48bppRgb:
                    result = 48;
                    break;
                case PixelFormat.Format4bppIndexed:
                    result = 4;
                    break;
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    result = 64;
                    break;
                case PixelFormat.Format8bppIndexed:
                    result = 8;
                    break;
            }
            return result;
        }

        public static bool IsAlphaPixelFormat(PixelFormat pixfmt)
        {
            bool result = false;
            switch (pixfmt)
            {
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    result = true;
                    break;
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format24bppRgb:
                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format48bppRgb:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                    result = false;
                    break;
            }
            return result;
        }

        public static bool IsCanonicalPixelFormat(PixelFormat pixfmt)
        {
            return ((pixfmt & PixelFormat.Canonical) != 0);
        }

        public static bool IsExtendedPixelFormat(PixelFormat pixfmt)
        {
            return ((pixfmt & PixelFormat.Extended) != 0);
        }
        #endregion

        // non-static	
        public RectangleF GetBounds(ref GraphicsUnit pageUnit)
        {
            throw new NotImplementedException();
        }

#if NOT_PFX
        public EncoderParameters GetEncoderParameterList(Guid encoder)
        {
            throw new NotImplementedException();
        }
#endif

        public int GetFrameCount(FrameDimension dimension)
        {
            throw new NotImplementedException();
        }

        #region NOT_PFX: GetPropertyItem, SetPropertyItem
#if NOT_PFX
        public PropertyItem GetPropertyItem(int propid)
        {
            throw new NotImplementedException();
        }

        public void SetPropertyItem(PropertyItem propitem)
        {
            throw new NotImplementedException();
            /*
                    GdipPropertyItem pi = new GdipPropertyItem ();
                    GdipPropertyItem.MarshalTo (pi, propitem);
                    unsafe {
                        Status status = GDIPlus.GdipSetPropertyItem (nativeObject, &pi);
			
                        GDIPlus.CheckStatus (status);
                    }
            */
        }

        public void RemovePropertyItem(int propid)
        {
            throw new NotImplementedException();
        }
#endif
        #endregion

        public Image GetThumbnailImage(int thumbWidth, int thumbHeight, GetThumbnailImageAbort callback, IntPtr callbackData)
        {
            if ((thumbWidth <= 0) || (thumbHeight <= 0))
                throw new OutOfMemoryException("Invalid thumbnail size");

            Image ThumbNail = new Bitmap(thumbWidth, thumbHeight);

            //using (Graphics g = Graphics.FromImage(ThumbNail))
            //{
            //    Status status = GDIPlus.GdipDrawImageRectRectI(g.nativeObject, nativeObject,
            //        0, 0, thumbWidth, thumbHeight,
            //        0, 0, this.Width, this.Height,
            //        GraphicsUnit.Pixel, IntPtr.Zero, null, IntPtr.Zero);

            //    GDIPlus.CheckStatus(status);
            //}
            throw new NotImplementedException();

            return ThumbNail;
        }
        
        public void RotateFlip(RotateFlipType rotateFlipType)
        {
            throw new NotImplementedException();
        }

#if NOT_PFX
        internal ImageCodecInfo findEncoderForFormat(ImageFormat format)
        {
            throw new NotImplementedException();
        }
#endif

        #region NOT_PFX: Save
#if NOT_PFX
        public void Save(string filename)
        {
            Save(filename, RawFormat);
        }

        public void Save(string filename, ImageFormat format)
        {
            ImageCodecInfo encoder = findEncoderForFormat(format);
            if (encoder == null)
            {
                // second chance
                encoder = findEncoderForFormat(RawFormat);
                if (encoder == null)
                {
                    string msg = Locale.GetText("No codec available for saving format '{0}'.", format.Guid);
                    throw new ArgumentException(msg, "format");
                }
            }
            Save(filename, encoder, null);
        }

        public void Save(string filename, ImageCodecInfo encoder, EncoderParameters encoderParams)
        {
            throw new NotImplementedException();
        }

        public void Save(Stream stream, ImageFormat format)
        {
            ImageCodecInfo encoder = findEncoderForFormat(format);

            if (encoder == null)
                throw new ArgumentException("No codec available for format:" + format.Guid);

            Save(stream, encoder, null);
        }

        public void Save(Stream stream, ImageCodecInfo encoder, EncoderParameters encoderParams)
        {
            throw new NotImplementedException();
        }

        public void SaveAdd(EncoderParameters encoderParams)
        {
            throw new NotImplementedException();
        }

        public void SaveAdd(Image image, EncoderParameters encoderParams)
        {
            throw new NotImplementedException();
        }

        public int SelectActiveFrame(FrameDimension dimension, int frameIndex)
        {
            throw new NotImplementedException();
        }
#endif
        #endregion

#if NOT_PFX
        // properties	
        [Browsable(false)]
        public int Flags
        {
            get
            {
                //int flags;
                //Status status = GDIPlus.GdipGetImageFlags(nativeObject, out flags);
                //GDIPlus.CheckStatus(status);
                //return flags;
                return 0;
            }
        }
#endif

#if NOT_PFX
        [Browsable(false)]
        public Guid[] FrameDimensionsList
        {
            get
            {
                //uint found;
                //Status status = GDIPlus.GdipImageGetFrameDimensionsCount(nativeObject, out found);
                //GDIPlus.CheckStatus(status);
                //Guid[] guid = new Guid[found];
                //status = GDIPlus.GdipImageGetFrameDimensionsList(nativeObject, guid, found);
                //GDIPlus.CheckStatus(status);
                //return guid;
                throw new NotImplementedException();
            }
        }
#endif

        [DefaultValue(false)]
        [Browsable(false)]
        public int Width
        {
            get 
            {
                if (Data == null) return 0;
                return Data.width;
            }
        }

        [DefaultValue(false)]
        [Browsable(false)]
        public int Height
        {
            get 
            {
                if (Data == null) return 0;
                return Data.height;
            }
        }

//#if NOT_PFX
        //TODO: Conside using screen dpi

        public float HorizontalResolution
        {
            get
            {
                return 96;
            }
        }

        public float VerticalResolution
        {
            get
            {
                return 96;
            }
        }
//#endif

        [Browsable(false)]
        public ColorPalette Palette
        {
            get { return null; }
            set { }
        }

        public SizeF PhysicalDimension
        {
            get
            {
                return new SizeF(Width, Height);
            }
        }

        public PixelFormat PixelFormat
        {
            get { return PixelFormat.Format32bppArgb; }
        }

        public ImageFormat RawFormat
        {
            get { return ImageFormat.MemoryBmp; }
        }

        public Size Size
        {
            get { return new Size(Width, Height); }
        }

#if NET_2_0
        [DefaultValue(null)]
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        object tag;
#endif

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public abstract object Clone();

    }
}
