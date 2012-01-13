using System;

namespace DataDynamics.PageFX.FLI.SWF
{
    [TODO]
    [SwfTag(SwfTagCode.SoundStreamHead2)]
    public class SwfTagSoundStreamHead2 : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.SoundStreamHead2; }
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