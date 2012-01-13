using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    [SwfTag(SwfTagCode.DefineBitsJPEG2)]
    public class SwfTagDefineBitsJPEG2 : SwfCharacter, ISwfImageCharacter
    {
        #region ctors
        public SwfTagDefineBitsJPEG2()
        {
        }

        public SwfTagDefineBitsJPEG2(int id, Image image) : base(id)
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
            get { return SwfTagCode.DefineBitsJPEG2; }
        }

        protected override void ReadBody(SwfReader reader)
        {
            var data = reader.ReadToEnd();
            var ms = new MemoryStream(data);
            _image = Image.FromStream(ms);
        }

        protected override void WriteBody(SwfWriter writer)
        {
            var ms = new MemoryStream();
            _image.Save(ms, ImageFormat.Jpeg);
            ms.Flush();
            ms.Close();
            var data = ms.ToArray();
            writer.Write(data);
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