using System;
using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Control
{
    /// <summary>
    /// The DebugID tag is used to match a debug file (.swd) with a Flash animation (.swf).
    /// This is used by the Flash environment and is not required to create movies otherwise.
    /// </summary>
    [SwfTag(SwfTagCode.DebugID)]
    public sealed class SwfTagDebugID : SwfTag
    {
	    public SwfTagDebugID()
        {
        }

        public SwfTagDebugID(Guid id)
        {
            Id = id;
        }

        public SwfTagDebugID(string id)
        {
            Id = new Guid(id);
        }

	    public Guid Id { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DebugID; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            var uuid = reader.ReadUInt8(16);
            Id = new Guid(uuid);
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.Write(Id.ToByteArray());
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("uuid", Id.ToString());
        }
    }
}