using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Text
{
    [TODO]
    [SwfTag(SwfTagCode.DefineFontInfo2)]
    public class SwfTagDefineFontInfo2 : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineFontInfo2; }
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