using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Sounds
{
    [TODO]
    [SwfTag(SwfTagCode.SoundStreamHead2)]
    public sealed class SwfTagSoundStreamHead2 : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.SoundStreamHead2; }
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