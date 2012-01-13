using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualStudio.CommandBars;
using stdole;

namespace DataDynamics.PageFX
{
    static class IconUtils
    {
        public static void SetPicture(CommandBarButton button, string resName)
        {
            if (string.IsNullOrEmpty(resName)) return;

            var asm = typeof(IconUtils).Assembly;
            Stream rs = Utils.GetResourceStream(asm, resName);
            if (rs == null) return;

            try
            {
                var image = Image.FromStream(rs);
                button.Picture = ToPicture(CreateBitmap(image));
                button.Mask = ToPicture(CreateMask(image));
            }
            catch
            {
            }
        }

        static Bitmap CreateBitmap(Image image)
        {
            const int w = 16;
            const int h = 16;
            var bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.White, 0, 0, w, h);
                g.DrawImage(image, 0, 0, w, h);
            }
            return bmp;
        }

        static Bitmap CreateMask(Image image)
        {
            const int w = 16;
            const int h = 16;
            var bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.DrawImage(image, 0, 0, w, h);
            }
            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    var c = bmp.GetPixel(x, y);
                    int a = c.A;
                    int pg = 255 - a;
                    bmp.SetPixel(x, y, Color.FromArgb(pg, pg, pg));
                }
            }
            return bmp;
        }

        public static StdPicture ToPicture(Image image)
        {
            return (StdPicture) ImageConverter.GetIPictureDispFromImage(image);
        }
    }

    class ImageConverter : AxHost
    {
        internal ImageConverter() : base("") { }

        public static IPictureDisp GetIPictureDispFromImage(Image image)
        {
            return (IPictureDisp)GetIPictureDispFromPicture(image);
        }
    }
}
