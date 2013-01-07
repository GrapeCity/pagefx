using System;

namespace DataDynamics.PageFX.FlashLand.Swf.Filters
{
    public sealed class SwfColorMatrixFilter : SwfFilter
    {
        public float[] Matrix
        {
            get { return _matrix; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (value.Length != 20)
                    throw new ArgumentException("Matrix must has 20 elements", "value");
                _matrix = value;
            }
        }
        private float[] _matrix;

        public override SwfFilterKind Kind
        {
            get { return SwfFilterKind.ColorMatrix; }
        }

        public override void Read(SwfReader reader)
        {
            _matrix = reader.ReadSingle(20);
        }

        public override void Write(SwfWriter writer)
        {
            writer.WriteSingle(_matrix);
        }
    }
}