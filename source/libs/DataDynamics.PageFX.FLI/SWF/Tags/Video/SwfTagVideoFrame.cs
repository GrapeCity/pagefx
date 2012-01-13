using System;

namespace DataDynamics.PageFX.FLI.SWF
{
    /// <summary>
    /// The VideoFrame tag is used to render one frame. It includes the data of the video to be drawn.
    /// </summary>
    [TODO]
    [SwfTag(SwfTagCode.VideoFrame)]
    public class SwfTagVideoFrame : SwfTag
    {
        public override SwfTagCode TagCode
        {
            get { return SwfTagCode.VideoFrame; }
        }

        public override void ReadTagData(SwfReader reader)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void WriteTagData(SwfWriter writer)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}