using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// Defines a bitmap character with JPEG compression. This tag extends DefineBitsJPEG2,
    /// adding alpha channel (transparency) data. Transparency is not a standard feature in JPEG
    /// images, so the alpha channel information is encoded separately from the JPEG data, and
    /// compressed using the ZLIB standard for compression.
    /// </summary>
    [TODO]
    [SwfTag(SwfTagCode.DefineBitsJPEG3)]
    public class SwfTagDefineBitsJPEG3 : SwfCharacter, ISwfImageCharacter
    {
        #region ctors
        public SwfTagDefineBitsJPEG3()
        {
        }

        public SwfTagDefineBitsJPEG3(ushort id, Image image) : base(id)
        {
            _image = image;
        }
        #endregion

        #region ISwfImageCharacter Members
        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }
        private Image _image;
        #endregion

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineBitsJPEG3; }
        }

        protected override void ReadBody(SwfReader reader)
        {
            int jpegSize = (int)reader.ReadUInt32();
            var jpeg = reader.ReadUInt8(jpegSize);
            var ms = new MemoryStream(jpeg);
            _image = Image.FromStream(ms);

            //TODO: read alpha
        }

        protected override void WriteBody(SwfWriter writer)
        {
            var ms = new MemoryStream();
            _image.Save(ms, ImageFormat.Jpeg);
            ms.Flush();
            ms.Close();
            var jpeg = ms.ToArray();
            writer.WriteUInt32((uint)jpeg.Length);
            writer.Write(jpeg);

            //TODO: write alpha
        }

        public override void DumpBody(XmlWriter writer)
        {
            base.DumpBody(writer);
            if (_image != null)
            {
                writer.WriteElementString("width", _image.Width.ToString());
                writer.WriteElementString("height", _image.Height.ToString());
            }
        }
    }
}