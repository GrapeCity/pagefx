using System.IO;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.LoaderInternals
{
    internal static class Extensions
    {
		public static int RowIndex(this IMetadataElement element)
		{
			return ((SimpleIndex)element.MetadataToken).Index - 1;
		}

        public static string ReadCountedUtf8(this BufferedBinaryReader reader)
        {
            int b0 = reader.ReadUInt8();
            if (b0 == 0xFF) return null;
            if (b0 == 0) return string.Empty;
            int len = reader.ReadPackedInt(b0);
            return reader.ReadUtf8(len);
        }

		public static byte[] ToArray(this BufferedBinaryReader reader)
		{
			reader.Seek(0, SeekOrigin.Begin);
			return reader.ReadBytes((int)reader.Length);
		}
    }
}