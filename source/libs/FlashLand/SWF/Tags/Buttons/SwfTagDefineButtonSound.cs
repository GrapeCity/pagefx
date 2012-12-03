using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Buttons
{
    [TODO]
    [SwfTag(SwfTagCode.DefineButtonSound)]
    public class SwfTagDefineButtonSound : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineButtonSound; }
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