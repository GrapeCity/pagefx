using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Buttons
{
    [TODO]
    [SwfTag(SwfTagCode.DefineButtonCxform)]
    public sealed class SwfTagDefineButtonCxform : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineButtonCxform; }
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