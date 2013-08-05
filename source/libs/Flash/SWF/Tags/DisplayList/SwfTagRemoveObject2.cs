using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags.DisplayList
{
    /// <summary>
    /// Removes the character at the specified depth from the display list.
    /// </summary>
    [SwfTag(SwfTagCode.RemoveObject2)]
    public sealed class SwfTagRemoveObject2 : SwfTag
    {
        public SwfTagRemoveObject2()
        {
        }

        public SwfTagRemoveObject2(ushort depth)
        {
            Depth = depth;
        }

	    public ushort Depth { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.RemoveObject2; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            Depth = reader.ReadUInt16();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(Depth);
        }

        public override void DumpBody(XmlWriter writer)
        {
            if (SwfDumpService.DumpDisplayListTags)
            {
                writer.WriteElementString("depth", Depth.ToString());
            }
        }
    }
}