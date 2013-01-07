using System;

namespace DataDynamics.PageFX.FlashLand.Swf.Filters
{
    public sealed class SwfGradientBevelFilter : SwfFilter
    {
        public override SwfFilterKind Kind
        {
            get { return SwfFilterKind.GradientBevel; }
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