using System;

namespace DataDynamics.PageFX.Flash.Swf.Filters
{
    public sealed class SwfConvolutionFilter : SwfFilter
    {
        public override SwfFilterKind Kind
        {
            get { return SwfFilterKind.Convolution; }
        }

        public override void Read(SwfReader reader)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Write(SwfWriter writer)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}