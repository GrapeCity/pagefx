using System;
using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// The DebugID tag is used to match a debug file (.swd) with a Flash animation (.swf).
    /// This is used by the Flash environment and is not required to create movies otherwise.
    /// </summary>
    [SwfTag(SwfTagCode.DebugID)]
    public class SwfTagDebugID : SwfTag
    {
        #region ctors
        public SwfTagDebugID()
        {
        }

        public SwfTagDebugID(Guid id)
        {
            _id = id;
        }

        public SwfTagDebugID(string id)
        {
            _id = new Guid(id);
        }
        #endregion

        #region Properties
        public Guid ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private Guid _id;
        #endregion

        #region IO
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DebugID; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            var uuid = reader.ReadUInt8(16);
            _id = new Guid(uuid);
        }

        public override void WriteTagData(SwfWriter writer)
        {
            writer.Write(_id.ToByteArray());
        }

        public override void DumpBody(XmlWriter writer)
        {
            writer.WriteElementString("uuid", _id.ToString());
        }
        #endregion
    }
}