using System.IO;

namespace DataDynamics.PageFX.CLI
{
    internal static class Extensions
    {
        public static string ReadCountedUtf8(this BufferedBinaryReader reader)
        {
            int b0 = reader.ReadUInt8();
            if (b0 == 0xFF) return null;
            if (b0 == 0) return string.Empty;
            int len = reader.ReadPackedInt(b0);
            return reader.ReadUtf8(len);
        }
    }
}