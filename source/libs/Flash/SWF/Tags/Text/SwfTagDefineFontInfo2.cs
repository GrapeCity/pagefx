using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Text
{
    [TODO]
    [SwfTag(SwfTagCode.DefineFontInfo2)]
    public sealed class SwfTagDefineFontInfo2 : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineFontInfo2; }
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