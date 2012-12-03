using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DataDynamics.PageFX.Common.Graphics
{
	public static class ImageExtensions
	{
		public static bool IsIndexed(this Image image)
		{
			if (image == null)
			{
				return false;
			}
			switch (image.PixelFormat)
			{
				case PixelFormat.Format1bppIndexed:
				case PixelFormat.Format4bppIndexed:
				case PixelFormat.Format8bppIndexed:
					return true;
			}
			return false;
		}

		public static void Save(this Image image, string path)
		{
			string ext = Path.GetExtension(path);
			var format = GetImageFormatByExtension(ext);
			using (var stream = File.OpenWrite(path))
			{
				image.Save(stream, format);
			}
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

		public static bool IsEqual(this Bitmap bx, Bitmap by, bool alpha)
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

		public static Bitmap ToBitmap(this Image image)
		{
			if (image.RawFormat.Guid != ImageFormat.MemoryBmp.Guid)
				return new Bitmap(image);
			return (Bitmap)image;
		}
	}
}