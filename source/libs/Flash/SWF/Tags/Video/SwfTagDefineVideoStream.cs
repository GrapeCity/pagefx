using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Video
{
    [TODO]
    [SwfTag(SwfTagCode.DefineVideoStream)]
    public sealed class SwfTagDefineVideoStream : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.DefineVideoStream; }
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