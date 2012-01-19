using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
	public interface ISwfAssetContainer
	{
		SwfAssetCollection Assets { get; }
	}

    /// <summary>
    /// The ExportAssets tag makes portions of a SWF file available for import by other SWF files.
    /// </summary>
    [SwfTag(SwfTagCode.ExportAssets)]
    public class SwfTagExportAssets : SwfTag, ISwfAssetContainer
    {
        public SwfAssetCollection Assets
        {
            get { return _assets; }
        }
        private readonly SwfAssetCollection _assets = new SwfAssetCollection();

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.ExportAssets; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _assets.Read(reader, SwfAssetFlags.Exported);
        }

        public override void WriteTagData(SwfWriter writer)
        {
            _assets.Write(writer);
        }

        public override void DumpBody(XmlWriter writer)
        {
            _assets.DumpXml(writer);
        }
    }
}