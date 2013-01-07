using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.DisplayList
{
    /// <summary>
    /// Removes the specified character (at the specified depth) from the display list.
    /// </summary>
    [SwfTag(SwfTagCode.RemoveObject)]
    public sealed class SwfTagRemoveObject : SwfTag
    {
	    public ushort CharId { get; set; }

	    public ushort Depth { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.RemoveObject; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            CharId = reader.ReadUInt16();
            Depth = reader.ReadUInt16();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(CharId);
            writer.WriteUInt16(Depth);
        }

        public override void DumpBody(XmlWriter writer)
        {
            if (SwfDumpService.DumpDisplayListTags)
            {
                writer.WriteElementString("cid", CharId.ToString());
                writer.WriteElementString("depth", Depth.ToString());
            }
        }

	    public override void ImportDependencies(SwfMovie from, SwfMovie to)
	    {
		    var cid = CharId;
            to.ImportCharacter(from, ref cid);
		    CharId = cid;
	    }

        public override void GetRefs(SwfRefList list)
        {
            list.Add(CharId);
        }
    }
}