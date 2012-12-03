using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Control
{
    [SwfTag(SwfTagCode.ScriptLimits)]
    public class SwfTagScriptLimits : SwfTag
    {
        #region Constructors
        public SwfTagScriptLimits()
        {
        }

        public SwfTagScriptLimits(ushort maxRecusionDepth, ushort timeout)
        {
            _maxRecursionDepth = maxRecusionDepth;
            _timeout = timeout;
        }
        #endregion

        public ushort MaxRecursionDepth
        {
            get { return _maxRecursionDepth; }
            set { _maxRecursionDepth = value; }
        }
        private ushort _maxRecursionDepth = 256;

        /// <summary>
        /// Gets or sets the number of seconds before the players opens a dialog box saying that the SWF animation is stuck.
        /// </summary>
        public ushort Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }
        private ushort _timeout;

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.ScriptLimits; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _maxRecursionDepth = reader.ReadUInt16();
            _timeout = reader.ReadUInt16();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(_maxRecursionDepth);
            writer.WriteUInt16(_timeout);
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("max-recursion-depth", _maxRecursionDepth.ToString());
            writer.WriteElementString("timeout", _timeout.ToString());
        }
    }
}