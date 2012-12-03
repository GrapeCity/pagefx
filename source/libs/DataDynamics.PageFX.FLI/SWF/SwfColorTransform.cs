using System;
using System.Xml;

namespace DataDynamics.PageFX.FlashLand.Swf
{
    //NOTE: The CXFORM record must be byte aligned.
    //The factors are saved as 8.8 fixed values (divide by 256 to obtain a proper floating point value).
    //Note that the values are limited to a signed 16 bits value. This allows for any value between -128.0 and +127.98828.

    public sealed class SwfColorTransform
    {
        public float MulRed = 1;
        public float MulGreen = 1;
        public float MulBlue = 1;
        public float MulAlpha = 1;

        public float AddRed;
        public float AddGreen;
        public float AddBlue;
        public float AddAlpha;

        public bool HasAddTerms(bool alpha)
        {
            return AddRed != 0 || AddGreen != 0 || AddBlue != 0 || (alpha && AddAlpha != 0);
        }

        public bool HasMulTerms(bool alpha)
        {
            return MulRed != 1 || MulGreen != 1 || MulBlue != 1 || (alpha && MulAlpha != 1);
        }

        public SwfColorTransform()
        {
        }

        public SwfColorTransform(SwfReader reader, bool alpha)
        {
            Read(reader, alpha);
        }

        public void Read(SwfReader reader, bool alpha)
        {
            bool hasAddTerms = reader.ReadBit();
            bool hasMulTerms = reader.ReadBit();
            int nbits = (int)reader.ReadUB(4);
            const int q = 8;
            if (hasMulTerms)
            {
                MulRed = reader.ReadFB(nbits, q);
                MulGreen = reader.ReadFB(nbits, q);
                MulBlue = reader.ReadFB(nbits, q);
                if (alpha)
                    MulAlpha = reader.ReadFB(nbits, q);
            }
            if (hasAddTerms)
            {
                AddRed = reader.ReadFB(nbits, q);
                AddGreen = reader.ReadFB(nbits, q);
                AddBlue = reader.ReadFB(nbits, q);
                if (alpha)
                    AddAlpha = reader.ReadFB(nbits, q);
            }
            reader.Align();
        }

		const int Q = 8;

		private int MulBits(bool alpha)
		{
			int bits = Math.Max(Math.Max(MulRed.FixedToInt16(Q).GetMinBits(),
			                             MulGreen.FixedToInt16(Q).GetMinBits()),
			                    MulBlue.FixedToInt16(Q).GetMinBits());
			return alpha ? Math.Max(bits, MulAlpha.FixedToInt16(Q).GetMinBits()) : bits;
		}

		private int AddBits(bool alpha)
		{
			int bits = Math.Max(Math.Max(AddRed.FixedToInt16(Q).GetMinBits(),
										 AddGreen.FixedToInt16(Q).GetMinBits()),
								AddBlue.FixedToInt16(Q).GetMinBits());
			return alpha ? Math.Max(bits, AddAlpha.FixedToInt16(Q).GetMinBits()) : bits;
		}

        public void Write(SwfWriter writer, bool alpha)
        {
            bool hasAddTerms = HasAddTerms(alpha);
            bool hasMulTerms = HasMulTerms(alpha);
            writer.WriteBit(hasAddTerms);
            writer.WriteBit(hasMulTerms);

            int nbits;
            if (hasMulTerms && hasAddTerms)
            {
				nbits = Math.Max(MulBits(alpha), AddBits(alpha));
            }
            else if (hasMulTerms)
            {
				nbits = MulBits(alpha);
            }
            else
            {
            	nbits = AddBits(alpha);
            }
            writer.WriteUB((uint)nbits, 4);

            if (hasMulTerms)
            {
                writer.WriteFB16(MulRed, Q, nbits);
                writer.WriteFB16(MulGreen, Q, nbits);
                writer.WriteFB16(MulBlue, Q, nbits);
                if (alpha)
                    writer.WriteFB16(MulAlpha, Q, nbits);
            }

            if (hasAddTerms)
            {
                writer.WriteFB16(AddRed, Q, nbits);
                writer.WriteFB16(AddGreen, Q, nbits);
                writer.WriteFB16(AddBlue, Q, nbits);
                if (alpha)
                    writer.WriteFB16(AddAlpha, Q, nbits);
            }

            writer.Align();
        }

        public void Dump(XmlWriter writer, bool alpha)
        {
            Dump(writer, "color-transform", alpha);
        }

        public void Dump(XmlWriter writer, string name, bool alpha)
        {
            writer.WriteStartElement(name);

            if (HasAddTerms(alpha))
            {
                writer.WriteElementString("add-red", AddRed.ToString());
                writer.WriteElementString("add-green", AddGreen.ToString());
                writer.WriteElementString("add-blue", AddBlue.ToString());
                if (alpha)
                    writer.WriteElementString("add-alpha", AddAlpha.ToString());
            }

            if (HasMulTerms(alpha))
            {
                writer.WriteElementString("mul-red", MulRed.ToString());
                writer.WriteElementString("mul-green", MulGreen.ToString());
                writer.WriteElementString("mul-blue", MulBlue.ToString());
                if (alpha)
                    writer.WriteElementString("mul-alpha", MulAlpha.ToString());
            }

            writer.WriteEndElement();
        }
    }
}