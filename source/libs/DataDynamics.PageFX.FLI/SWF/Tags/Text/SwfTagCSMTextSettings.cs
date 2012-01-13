using System;

namespace DataDynamics.PageFX.FLI.SWF
{
    [TODO]
    [SwfTag(SwfTagCode.CSMTextSettings)]
    public class SwfTagCSMTextSettings : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.CSMTextSettings; }
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