using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using DataDynamics.PageFX.Common.Graphics;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Bitmaps
{
    [SwfTag(SwfTagCode.DefineBitsLossless)]
    public class SwfTagDefineBitsLossless : SwfCharacter, ISwfImageCharacter
    {
	    public SwfTagDefineBitsLossless()
        {
        }

        public SwfTagDefineBitsLossless(Image image)
        {
            Image = image;
        }

        public SwfTagDefineBitsLossless(int id, Image image) : base(id)
        {
            Image = image;
        }

	    public Image Image { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineBitsLossless; }
        }

	    protected void ReadBody(SwfReader reader, bool alpha)
        {
            var format = (SwfBitmapFormat)reader.ReadUInt8();
            int width = reader.ReadUInt16();
            int height = reader.ReadUInt16();
            if (format == SwfBitmapFormat.Indexed)
            {
                Image = ReadIndexed(reader, width, height, alpha);
            }
            else
            {
                reader = reader.Unzip();

                if (!alpha && format == SwfBitmapFormat.RGB15)
                {
                    Image = ReadRGB15(reader, width, height);
                }
                else
                {
                    Image = ReadRGB24(reader, width, height, alpha);
                }
            }
        }

        #region ReadIndexed
        private static PixelFormat GetIndexedPixelFormat(int size)
        {
            if (size <= 2)
                return PixelFormat.Format1bppIndexed;
            if (size <= 16)
                return PixelFormat.Format4bppIndexed;
            Debug.Assert(size <= 256);
            return PixelFormat.Format8bppIndexed;
        }

        private static Bitmap ReadIndexed(SwfReader reader, int width, int height, bool alpha)
        {
            //BitmapColorTableSize - this value is one less than the actual number of colors in the
            //color table, allowing for up to 256 colors.
            int palSize = reader.ReadUInt8() + 1;

            reader = reader.Unzip();

            var pf = GetIndexedPixelFormat(palSize);

            var bmp = new Bitmap(width, height, pf);
            var pal = bmp.Palette.Entries;

            for (int i = 0; i < palSize; ++i)
            {
                pal[i] = alpha ? reader.ReadRGBA() : reader.ReadRGB();
            }

            //NOTE: Row widths in the pixel data fields of these structures must be rounded up to the next
            //32-bit word boundary. For example, an 8-bit image that is 253 pixels wide must be
            //padded out to 256 bytes per line. To determine the appropriate padding, make sure to
            //take into account the actual size of the individual pixel structures; 15-bit pixels occupy 2
            //bytes and 24-bit pixels occupy 4 bytes (see PIX15 and PIX24).

            using (var fbmp = new FastBitmap(bmp))
            {
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        int i = reader.ReadUInt8();
                        Debug.Assert(i < palSize);
                        fbmp.SetIndex(x, y, i);
                    }
                    reader.Round32();
                }
            }
            return bmp;
        }
        #endregion

        #region ReadRGB15
        private static Bitmap ReadRGB15(SwfReader reader, int width, int height)
        {
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            using (var fbmp = new FastBitmap(bmp))
            {
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        uint v = reader.ReadUInt24();
                        int r = (int)((v >> 10) & 0x1F);
                        int g = (int)((v >> 5) & 0x1F);
                        int b = (int)(v & 0x1F);
                        fbmp[x, y] = Color.FromArgb(r, g, b);
                    }
                }
            }
            return bmp;
        }
        #endregion

        #region ReadRGB24
        private static Bitmap ReadRGB24(SwfReader reader, int width, int height, bool alpha)
        {
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using (var fbmp = new FastBitmap(bmp))
            {
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        Color c;
                        if (alpha)
                        {
                            //NOTE: The RGB data must already be multiplied by the alpha channel value.
                            c = reader.ReadARGB();
                            c = DemulAlpha(c);

                        }
                        else
                        {
                            reader.ReadUInt8(); //alpha reserved
                            c = reader.ReadRGB();
                        }
                        fbmp[x, y] = c;
                    }
                }
            }
            return bmp;
        }
        #endregion

        protected override void ReadBody(SwfReader reader)
        {
            ReadBody(reader, false);
        }

	    protected void WriteBody(SwfWriter writer, bool alpha)
        {
            if (Image == null)
                throw new InvalidOperationException();

            var bmp = Image.ToBitmap();
            if (IsIndexedImage(bmp))
            {
                WriteIndexed(writer, bmp, alpha);
            }
            else
            {
                WriteRGB24(writer, bmp, alpha);
            }
        }

        #region WriteIndexed
        public bool IsIndexed
        {
            get
            {
                return IsIndexedImage(Image);
            }
        }

        private static bool IsIndexedImage(Image image)
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

        private static void WriteIndexed(SwfWriter writer, Bitmap bmp, bool alpha)
        {
            int w = bmp.Width;
            int h = bmp.Height;

            writer.WriteUInt8((byte)SwfBitmapFormat.Indexed);
            writer.WriteUInt16((ushort)w);
            writer.WriteUInt16((ushort)h);

            var pal = bmp.Palette.Entries;
            int palSize = pal.Length;
            writer.WriteUInt8((byte)(palSize - 1));

            var bmpWriter = new SwfWriter();

            for (int i = 0; i < palSize; ++i)
                bmpWriter.WriteColor(pal[i], alpha);

            //NOTE: Row widths in the pixel data fields of these structures must be rounded up to the next
            //32-bit word boundary. For example, an 8-bit image that is 253 pixels wide must be
            //padded out to 256 bytes per line. To determine the appropriate padding, make sure to
            //take into account the actual size of the individual pixel structures; 15-bit pixels occupy 2
            //bytes and 24-bit pixels occupy 4 bytes (see PIX15 and PIX24).

            int pad = w % 4;
            if (pad != 0)
                pad = 4 - pad;

            using (var fbmp = new FastBitmap(bmp))
            {
                for (int y = 0; y < h; ++y)
                {
                    for (int x = 0; x < w; ++x)
                    {
                        int index = fbmp.GetIndex(x, y);
                        Debug.Assert(index >= 0 && index < palSize);
                        bmpWriter.WriteUInt8((byte)index);
                    }
                    if (pad > 0)
                        bmpWriter.Pad(pad);
                }
            }

            var data = bmpWriter.ToByteArray();
            data = Zip.Compress(data);
            writer.Write(data);
        }
        #endregion

        #region WriteRGB24
        private static void WriteRGB24(SwfWriter writer, Bitmap bmp, bool alpha)
        {
            writer.WriteUInt8((byte)SwfBitmapFormat.RGB24);
            writer.WriteUInt16((ushort)bmp.Width);
            writer.WriteUInt16((ushort)bmp.Height);

            var data = GetRGB24(bmp, alpha);
            data = Zip.Compress(data);
            writer.Write(data);
        }
        #endregion

        #region GetRGB24
        private static Color MulAlpha(Color c)
        {
            int a = c.A;
            if (a == 0)
                return Color.FromArgb(0, 0, 0, 0);
            float k = a / 255f;
            int r = (int)(c.R * k);
            int g = (int)(c.G * k);
            int b = (int)(c.B * k);
            return Color.FromArgb(a, r & 0xff, g & 0xff, b & 0xff);
        }

        private static Color DemulAlpha(Color c)
        {
            int a = c.A;
            if (a == 0)
                return Color.FromArgb(0, 0, 0, 0);
            int r = c.R * 255 / a;
            int g = c.G * 255 / a;
            int b = c.B * 255 / a;
            return Color.FromArgb(a, r & 0xff, g & 0xff, b & 0xff);
        }

        private static byte[] GetRGB24(Bitmap bmp, bool alpha)
        {
            var writer = new SwfWriter();
            //NOTE: The RGB data must already be multiplied by the alpha channel value.
            int w = bmp.Width;
            int h = bmp.Height;
            using (var fbmp = new FastBitmap(bmp))
            {
                for (int y = 0; y < h; ++y)
                {
                    for (int x = 0; x < w; ++x)
                    {
                        var c = fbmp[x, y];
                        if (alpha)
                        {
                            c = MulAlpha(c);
                            writer.WriteARGB(c);
                        }
                        else
                        {
                            writer.WriteUInt8(0);
                            writer.WriteUInt8(c.R);
                            writer.WriteUInt8(c.G);
                            writer.WriteUInt8(c.B);
                        }
                    }
                }
            }
            return writer.ToByteArray();
        }
        #endregion

        protected override void WriteBody(SwfWriter writer)
        {
            WriteBody(writer, false);
        }

	    public override void DumpBody(XmlWriter writer)
        {
            base.DumpBody(writer);
            if (Image != null)
            {
                writer.WriteElementString("width", Image.Width.ToString());
                writer.WriteElementString("height", Image.Height.ToString());
            }
        }
    }

	public enum SwfBitmapFormat : byte
    {
        /// <summary>
        /// Colormapped image
        /// </summary>
        Indexed = 3,

        /// <summary>
        /// 15-bit RGB image
        /// </summary>
        RGB15 = 4,

        /// <summary>
        /// 24-bit RGB image or 32-bit ARGB image
        /// </summary>
        RGB24 = 5,
    }
}