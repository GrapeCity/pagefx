using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Buttons
{
    [TODO]
    [SwfTag(SwfTagCode.DefineButtonSound)]
    public sealed class SwfTagDefineButtonSound : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineButtonSound; }
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