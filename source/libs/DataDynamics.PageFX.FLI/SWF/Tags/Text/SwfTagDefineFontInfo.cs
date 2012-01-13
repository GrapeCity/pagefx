using System;

namespace DataDynamics.PageFX.FLI.SWF
{
    [TODO]
    [SwfTag(SwfTagCode.DefineFontInfo)]
    public class SwfTagDefineFontInfo : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineFontInfo; }
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