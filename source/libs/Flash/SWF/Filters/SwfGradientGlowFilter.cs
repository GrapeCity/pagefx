using System;

namespace DataDynamics.PageFX.Flash.Swf.Filters
{
    public sealed class SwfGradientGlowFilter : SwfFilter
    {
        public override SwfFilterKind Kind
        {
            get { return SwfFilterKind.GradientGlow; }
        }

        public override void Read(SwfReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Write(SwfWriter writer)
        {
			throw new NotImplementedException();
        }
    }
}