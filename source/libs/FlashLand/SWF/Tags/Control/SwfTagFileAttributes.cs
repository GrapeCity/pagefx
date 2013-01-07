using System;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    /// <summary>
    /// The FileAttributes tag defines characteristics of the SWF file. This tag is required for SWF 8
    /// and later and must be the first tag in the SWF file. Additionally, the FileAttributes tag can
    /// optionally be included in all SWF file versions.
    /// </summary>
    [SwfTag(SwfTagCode.FileAttributes)]
    public class SwfTagFileAttributes : SwfTag
    {
	    public SwfTagFileAttributes()
        {
        }

        public SwfTagFileAttributes(SwfFileAttributes attrs)
        {
            Attributes = attrs;
        }

	    public SwfFileAttributes Attributes { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.FileAttributes; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            Attributes = (SwfFileAttributes)reader.ReadByte();
            reader.ReadUInt24(); //reserved
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt8((byte)Attributes);
            writer.WriteUInt24(0); //reserved
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("file-attributes", string.Format("{0} [0x{1:X4}]", Attributes, (int)Attributes));
        }
    }

    [Flags]
    public enum SwfFileAttributes
    {
        None = 0,

        /// <summary>
        /// 1 - SWF file is given network file access when loaded locally.
        /// 0 - SWF file is given local file access when loaded locally.
        /// </summary>
        UseNetwork = 0x01,

        RelativeUrls = 0x02,

        SuppressCrossDomainCaching = 0x04,

        /// <summary>
        /// If 1, this SWF uses ActionScript 3.0.
        /// If 0, this SWF uses ActionScript 1.0 or 2.0.
        /// Minimum file format version is 9.
        /// </summary>
        [SwfVersion(9)]
        ActionScript3 = 0x08,

        /// <summary>
        /// The HasMetadata flag identifies whether the SWF file contains the Metadata tag.
        /// </summary>
        HasMetadata = 0x10,

        /// <summary>
        /// If 1, the SWF file uses GPU compositing features when drawing graphics, where such acceleration is available.
        /// If 0, the SWF file will not use hardware accelerated graphics facilities.
        /// Minimum file version is 10.
        /// </summary>
        [SwfVersion(10)]
        UseGPU = 0x20,

        /// <summary>
        /// If 1, the SWF file uses hardware acceleration to blit graphics to the screen, where such acceleration is available.
        /// If 0, the SWF file will not use hardware accelerated graphics facilities.
        /// Minimum file version is 10.
        /// </summary>
        [SwfVersion(10)]
        UseDirectBlit = 0x40,

        Default = HasMetadata | UseNetwork,
        Default9 = HasMetadata | ActionScript3 | UseNetwork,
        Default10 = Default9 | UseGPU | UseDirectBlit,
    }
}