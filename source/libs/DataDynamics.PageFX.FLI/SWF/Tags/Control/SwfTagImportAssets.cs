using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// The ImportAssets tag imports characters from another SWF file.
    /// </summary>
    [SwfTag(SwfTagCode.ImportAssets)]
    public class SwfTagImportAssets : SwfTag
    {
        /// <summary>
        /// Gets or sets URL to the exporting SWF file from which characters will be imported.
        /// </summary>
        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }
        private string _url;

        public SwfAssetCollection Assets
        {
            get { return _assets; }
        }
        private readonly SwfAssetCollection _assets = new SwfAssetCollection();

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.ImportAssets; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _url = reader.ReadString();
            _assets.Read(reader, SwfAssetFlags.Imported);
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteString(_url);
            _assets.Write(writer);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("url", _url);
            _assets.DumpXml(writer);
        }
    }
}