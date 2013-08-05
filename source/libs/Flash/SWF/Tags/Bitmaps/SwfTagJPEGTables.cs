namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Bitmaps
{
    /// <summary>
    /// This tag defines the JPEG encoding table (the Tables/Misc segment) for all JPEG images
    /// defined using the DefineBits tag. There may only be one JPEGTables tag in a SWF file.
    /// The data in this tag begins with the JPEG SOI marker 0xFF, 0xD8 and ends with the EOI
    /// marker 0xFF, 0xD9. Before SWF 8, SWF files could contain an erroneous header of 0xFF,
    /// 0xD9, 0xFF, 0xD8 before the JPEG SOI marker.
    /// </summary>
    [SwfTag(SwfTagCode.JPEGTables)]
    public sealed class SwfTagJPEGTables : SwfTag
    {
	    public byte[] Data { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.JPEGTables; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            Data = reader.ReadToEnd();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.Write(Data);
        }

        public override byte[] GetData()
        {
            return Data;
        }
    }
}