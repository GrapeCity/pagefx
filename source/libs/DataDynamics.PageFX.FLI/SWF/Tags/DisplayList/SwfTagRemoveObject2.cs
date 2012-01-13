using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// Removes the character at the specified depth from the display list.
    /// </summary>
    [SwfTag(SwfTagCode.RemoveObject2)]
    public class SwfTagRemoveObject2 : SwfTag
    {
        public SwfTagRemoveObject2()
        {
        }

        public SwfTagRemoveObject2(ushort depth)
        {
            _depth = depth;
        }

        public ushort Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        private ushort _depth;

        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.RemoveObject2; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _depth = reader.ReadUInt16();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(_depth);
        }

        public override void DumpBody(XmlWriter writer)
        {
            if (SwfDumpService.DumpDisplayListTags)
            {
                writer.WriteElementString("depth", _depth.ToString());
            }
        }
    }
}