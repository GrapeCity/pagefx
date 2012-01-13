using System;

namespace DataDynamics.PageFX.FLI.SWF
{
    public class SwfGradientGlowFilter : SwfFilter
    {
        public override SwfFilterID ID
        {
            get { return SwfFilterID.GradientGlow; }
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