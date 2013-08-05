using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Bitmaps
{
    [TODO]
    [SwfTag(SwfTagCode.DefineBitsJPEG)]
    public sealed class SwfTagDefineBitsJPEG : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineBitsJPEG; }
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