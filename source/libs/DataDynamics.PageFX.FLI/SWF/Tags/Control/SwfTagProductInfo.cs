using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    [SwfTag(SwfTagCode.ProductInfo)]
    public class SwfTagProductInfo : SwfTag
    {
        public uint ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }
        private uint _productID;

        public uint Edition
        {
            get { return _edition; }
            set { _edition = value; }
        }
        private uint _edition;

        public byte MajorVersion
        {
            get { return _majorVersion; }
            set { _majorVersion = value; }
        }
        private byte _majorVersion;

        public byte MinorVersion
        {
            get { return _minorVersion; }
            set { _minorVersion = value; }
        }
        private byte _minorVersion;

        public ulong BuildNumber
        {
            get { return _buildNumber; }
            set { _buildNumber = value; }
        }
        private ulong _buildNumber;

        public ulong BuildDate
        {
            get { return _buildDate; }
            set { _buildDate = value; }
        }
        private ulong _buildDate;

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.ProductInfo; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _productID = reader.ReadUInt32();
            _edition = reader.ReadUInt32();
            _majorVersion = reader.ReadUInt8();
            _minorVersion = reader.ReadUInt8();
            _buildNumber = reader.ReadUInt64();
            _buildDate = reader.ReadUInt64();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt32(_productID);
            writer.WriteUInt32(_edition);
            writer.WriteUInt8(_majorVersion);
            writer.WriteUInt8(_minorVersion);
            writer.WriteUInt64(_buildNumber);
            writer.WriteUInt64(_buildDate);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("product-id", _productID.ToString());
            writer.WriteElementString("edition", _edition.ToString());
            writer.WriteElementString("major-version", _majorVersion.ToString());
            writer.WriteElementString("minor-version", _minorVersion.ToString());
            writer.WriteElementString("build-number", _buildNumber.ToString());
            writer.WriteElementString("build-date", _buildDate.ToString());
        }
    }
}