using System;

namespace DataDynamics.PageFX.FLI.SWF
{
    [TODO]
    [SwfTag(SwfTagCode.DefineVideoStream)]
    public class SwfTagDefineVideoStream : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineVideoStream; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void WriteTagData(SwfWriter writer)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}