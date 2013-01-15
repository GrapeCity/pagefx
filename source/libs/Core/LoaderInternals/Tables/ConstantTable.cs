using System.Text;
using DataDynamics.PageFX.Common.IO;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal sealed class ConstantTable
	{
		private readonly AssemblyLoader _loader;
		
		public ConstantTable(AssemblyLoader loader)
		{
			_loader = loader;
		}

		public object this[SimpleIndex parent]
		{
			get
			{
				var row = _loader.Metadata.LookupRow(TableId.Constant, Schema.Constant.Parent, parent, false);
				if (row == null) return null;

				var type = (ElementType)row[Schema.Constant.Type].Value;
				var reader = row[Schema.Constant.Value].Blob;

				return ReadValue(type, reader);
			}
		}

		private static object ReadValue(ElementType type, BufferedBinaryReader reader)
		{
			switch (type)
			{
				case ElementType.Boolean:
					return reader.ReadBoolean();

				case ElementType.Char:
					return reader.ReadChar();

				case ElementType.Int8:
					return reader.ReadSByte();

				case ElementType.UInt8:
					return reader.ReadByte();

				case ElementType.Int16:
					return reader.ReadInt16();

				case ElementType.UInt16:
					return reader.ReadUInt16();

				case ElementType.Int32:
					return reader.ReadInt32();

				case ElementType.UInt32:
					return reader.ReadUInt32();

				case ElementType.Int64:
					return reader.ReadInt64();

				case ElementType.UInt64:
					return reader.ReadUInt64();

				case ElementType.Single:
					return reader.ReadSingle();

				case ElementType.Double:
					return reader.ReadDouble();

				case ElementType.String:
					return Encoding.Unicode.GetString(reader.ToArray());

				case ElementType.Class:
					return null;

				default:
					return reader.ToArray();
			}
		}
	}
}
