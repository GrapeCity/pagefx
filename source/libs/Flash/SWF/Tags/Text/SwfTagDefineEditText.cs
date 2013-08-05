using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Text
{
    [TODO]
    [SwfTag(SwfTagCode.DefineEditText)]
    public sealed class SwfTagDefineEditText : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineEditText; }
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