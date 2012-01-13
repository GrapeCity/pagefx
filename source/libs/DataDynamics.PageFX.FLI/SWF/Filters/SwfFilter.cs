using System;
using System.Collections.Generic;
using System.Xml;

namespace DataDynamics.PageFX.FLI.SWF
{
    public enum SwfFilterID : byte 
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
        public abstract SwfFilterID ID { get; }

        public abstract void Read(SwfReader reader);

        public abstract void Write(SwfWriter writer);

        public virtual void Dump(XmlWriter writer)
        {
            writer.WriteStartElement("filter");
            writer.WriteAttributeString("id", ID.ToString());
            DumpBody(writer);
            writer.WriteEndElement();
        }

        public virtual void DumpBody(XmlWriter writer)
        {
        }

        public static SwfFilter Create(SwfFilterID id)
        {
            switch (id)
            {
                case SwfFilterID.DropShadow:
                    return new SwfDropShadowFilter();
                case SwfFilterID.Blur:
                    return new SwfBlurFilter();
                case SwfFilterID.Bevel:
                    return new SwfBevelFilter();
                case SwfFilterID.Glow:
                    return new SwfGlowFilter();
                case SwfFilterID.GradientBevel:
                    return new SwfGradientBevelFilter();
                case SwfFilterID.GradientGlow:
                    return new SwfGradientGlowFilter();
                case SwfFilterID.ColorMatrix:
                    return new SwfColorMatrixFilter();
                case SwfFilterID.Convolution:
                    return new SwfConvolutionFilter();
                default:
                    throw new ArgumentOutOfRangeException("id");
            }
        }
    }

    public class SwfFilterList : List<SwfFilter>
    {
        public void Read(SwfReader reader)
        {
            int n = reader.ReadUInt8();
            for (int i = 0; i < n; ++i)
            {
                var id = (SwfFilterID)reader.ReadUInt8();
                var f = SwfFilter.Create(id);
                f.Read(reader);
                Add(f);
            }
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            if (n > byte.MaxValue)
                throw new InvalidOperationException();
            writer.WriteUInt8((byte)n);
            for (int i = 0; i < n; ++i)
            {
                var f = this[i];
                writer.WriteUInt8((byte)f.ID);
                f.Write(writer);
            }
        }

        public void Dump(XmlWriter writer)
        {
            int n = Count;
            writer.WriteStartElement("filters");
            writer.WriteAttributeString("count", n.ToString());
            for (int i = 0; i < n; ++i)
                this[i].Dump(writer);
            writer.WriteEndElement();
        }
    }
}