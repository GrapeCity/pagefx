using System;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf.Filters
{
    public enum SwfFilterKind : byte 
    {
        [SwfVersion(8)]
        DropShadow,

        [SwfVersion(8)]
        Blur,

        [SwfVersion(8)]
        Glow,

        [SwfVersion(8)]
        Bevel,

        [SwfVersion(8)]
        GradientGlow,

        [SwfVersion(8)]
        Convolution,

        [SwfVersion(8)]
        ColorMatrix,

        [SwfVersion(8)]
        GradientBevel,
    }

    public abstract class SwfFilter
    {
        public abstract SwfFilterKind Kind { get; }

        public abstract void Read(SwfReader reader);

        public abstract void Write(SwfWriter writer);

        public virtual void Dump(XmlWriter writer)
        {
            writer.WriteStartElement("filter");
            writer.WriteAttributeString("id", Kind.ToString());
            DumpBody(writer);
            writer.WriteEndElement();
        }

        public virtual void DumpBody(XmlWriter writer)
        {
        }

        public static SwfFilter Create(SwfFilterKind kind)
        {
            switch (kind)
            {
                case SwfFilterKind.DropShadow:
                    return new SwfDropShadowFilter();
                case SwfFilterKind.Blur:
                    return new SwfBlurFilter();
                case SwfFilterKind.Bevel:
                    return new SwfBevelFilter();
                case SwfFilterKind.Glow:
                    return new SwfGlowFilter();
                case SwfFilterKind.GradientBevel:
                    return new SwfGradientBevelFilter();
                case SwfFilterKind.GradientGlow:
                    return new SwfGradientGlowFilter();
                case SwfFilterKind.ColorMatrix:
                    return new SwfColorMatrixFilter();
                case SwfFilterKind.Convolution:
                    return new SwfConvolutionFilter();
                default:
                    throw new ArgumentOutOfRangeException("kind");
            }
        }
    }
}