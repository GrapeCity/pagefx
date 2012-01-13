using System;

namespace DataDynamics.PageFX.FLI.SWF
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