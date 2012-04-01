using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DataDynamics.Imaging;
using DataDynamics.PageFX.FLI.SWF;
using NUnit.Framework;

namespace DataDynamics.PageFX.Tests
{
    [TestFixture]
    public class SwfMovieTest
    {
        private static Bitmap GetTestBitmap()
        {
            var bmp = new Bitmap(100, 100, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
                g.FillEllipse(Brushes.Red, 10, 10, 50, 50);
            }
            return bmp;
        }

        [Test]
        public void DefineBitsLoseless()
        {
            var swf = new SwfMovie();
            
            swf.SetDefaultBackgroundColor();
            swf.SetFrameLabel("main");

            var bmp = GetTestBitmap();
            
            ushort cid = swf.DefineBitmap(bmp);
            swf.ShowFrame();

            var ms = new MemoryStream();
            swf.Save(ms);

            ms.Flush();
            ms.Position = 0;
            var swf2 = new SwfMovie(ms);

            var bmp2 = swf2.GetBitmap(cid);

            Assert.IsTrue(bmp.IsEqual(bmp2, true));
        }
    }
}