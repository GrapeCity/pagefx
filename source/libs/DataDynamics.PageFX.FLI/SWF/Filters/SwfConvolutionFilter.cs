using System;

namespace DataDynamics.PageFX.FlashLand.Swf.Filters
{
    public class SwfConvolutionFilter : SwfFilter
    {
        public override SwfFilterID ID
        {
            get { return SwfFilterID.Convolution; }
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