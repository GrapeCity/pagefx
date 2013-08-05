using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Sounds
{
    [TODO]
    [SwfTag(SwfTagCode.DefineSound)]
    public sealed class SwfTagDefineSound : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineSound; }
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