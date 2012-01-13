using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// Removes the specified character (at the specified depth) from the display list.
    /// </summary>
    [SwfTag(SwfTagCode.RemoveObject)]
    public class SwfTagRemoveObject : SwfTag
    {
        #region Properties
        public ushort CharID
        {
            get { return _cid; }
            set { _cid = value; }
        }
        private ushort _cid;

        public ushort Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        private ushort _depth;
        #endregion

        #region IO
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.RemoveObject; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            _cid = reader.ReadUInt16();
            _depth = reader.ReadUInt16();
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.WriteUInt16(_cid);
            writer.WriteUInt16(_depth);
        }

        public override void DumpBody(XmlWriter writer)
        {
            if (SwfDumpService.DumpDisplayListTags)
            {
                writer.WriteElementString("cid", _cid.ToString());
                writer.WriteElementString("depth", _depth.ToString());
            }
        }
        #endregion

        public override void ImportDependencies(SwfMovie from, SwfMovie to)
        {
            to.ImportCharacter(from, ref _cid);
        }

        public override void GetRefs(IIDList list)
        {
            list.Add(_cid);
        }
    }
}