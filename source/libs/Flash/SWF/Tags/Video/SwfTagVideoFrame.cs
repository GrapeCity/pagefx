using System;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Flash.Swf.Tags.Video
{
    /// <summary>
    /// The VideoFrame tag is used to render one frame. It includes the data of the video to be drawn.
    /// </summary>
    [TODO]
    [SwfTag(SwfTagCode.VideoFrame)]
    public sealed class SwfTagVideoFrame : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.VideoFrame; }
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