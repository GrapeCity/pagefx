using System.Xml;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.ScriptLimits)]
    public sealed class SwfTagScriptLimits : SwfTag
    {
	    public SwfTagScriptLimits()
        {
	        MaxRecursionDepth = 256;
        }

	    public SwfTagScriptLimits(ushort maxRecusionDepth, ushort timeout)
        {
            MaxRecursionDepth = maxRecusionDepth;
            Timeout = timeout;
        }

	    public ushort MaxRecursionDepth { get; set; }

	    /// <summary>
	    /// Gets or sets the number of seconds before the players opens a dialog box saying that the SWF animation is stuck.
	    /// </summary>
	    public ushort Timeout { get; set; }

	    public override SwfTagCode TagCode
        {
            get { return SwfTagCode.ScriptLimits; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            MaxRecursionDepth = reader.ReadUInt16();
            Timeout = reader.ReadUInt16();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(MaxRecursionDepth);
            writer.WriteUInt16(Timeout);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("max-recursion-depth", MaxRecursionDepth.ToString());
            writer.WriteElementString("timeout", Timeout.ToString());
        }
    }
}