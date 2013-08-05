using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Text
{
    [TODO]
    [SwfTag(SwfTagCode.DefineText)]
    public sealed class SwfTagDefineText : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineText; }
        }

        public override void ReadTagData(SwfReader reader)
        {
			throw new NotImplementedException();
        }

        public override void WriteTagData(SwfWriter writer)
        {
			throw new NotImplementedException();
        }
    }
}