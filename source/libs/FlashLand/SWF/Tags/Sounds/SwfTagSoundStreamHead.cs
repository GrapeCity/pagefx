using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.FlashLand.Swf.Tags.Sounds
{
    [TODO]
    [SwfTag(SwfTagCode.SoundStreamHead)]
    public sealed class SwfTagSoundStreamHead : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.SoundStreamHead; }
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