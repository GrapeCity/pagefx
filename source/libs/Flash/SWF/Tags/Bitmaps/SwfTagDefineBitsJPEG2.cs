using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Bitmaps
{
    [SwfTag(SwfTagCode.DefineBitsJPEG2)]
    public sealed class SwfTagDefineBitsJPEG2 : SwfCharacter, ISwfImageCharacter
    {
	    public SwfTagDefineBitsJPEG2()
        {
        }

        public SwfTagDefineBitsJPEG2(int id, Image image) : base(id)
        {
            Image = image;
        }

	    public Image Image { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineBitsJPEG2; }
        }

        protected override void ReadBody(SwfReader reader)
        {
            var data = reader.ReadToEnd();
            var ms = new MemoryStream(data);
            Image = Image.FromStream(ms);
        }

        protected override void WriteBody(SwfWriter writer)
        {
            var ms = new MemoryStream();
            Image.Save(ms, ImageFormat.Jpeg);
            ms.Flush();
            ms.Close();
            var data = ms.ToArray();
            writer.Write(data);
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
}