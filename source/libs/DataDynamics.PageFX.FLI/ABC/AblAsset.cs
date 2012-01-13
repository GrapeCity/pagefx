using System.Collections.Generic;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.ABC
{
    public class AblAsset
    {
        public string ExportName { get; set; }

        public ISwfCharacter ImportedCharacter { get; set; }

        public int Index { get; set; }

        public void Read(AblFile abl, SwfReader reader)
        {
            ExportName = reader.ReadString();
            Index = (int)reader.ReadUIntEncoded();
        }

        public void Write(SwfWriter writer)
        {
            writer.WriteString(ExportName);
            writer.WriteUIntEncoded(Index);
        }
    }

    public class AblAssets : List<AblAsset>
    {
        public void Read(AblFile abl, SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 0; i < n; ++i)
            {
                var a = new AblAsset();
                a.Read(abl, reader);
                Add(a);
            }
        }

        public void Write(SwfWriter writer)
        {
            int n = Count;
            writer.WriteUIntEncoded(n);
            for (int i = 0; i < n; ++i)
                this[i].Write(writer);
        }
    }
}