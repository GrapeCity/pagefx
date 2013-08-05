using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    /// <summary>
    /// The ImportAssets tag imports characters from another SWF file.
    /// </summary>
    [SwfTag(SwfTagCode.ImportAssets)]
    public class SwfTagImportAssets : SwfTag, ISwfAssetContainer
    {
	    /// <summary>
	    /// Gets or sets URL to the exporting SWF file from which characters will be imported.
	    /// </summary>
	    public string Url { get; set; }

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
            Url = reader.ReadString();
            _assets.Read(reader, SwfAssetFlags.Imported);
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteString(Url);
            _assets.Write(writer);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("url", Url);
            _assets.DumpXml(writer);
        }
    }
}