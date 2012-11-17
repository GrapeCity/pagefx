using System.Collections.Generic;
using System.IO;
using System.Text;
using DataDynamics.PageFX.CLI.Metadata;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class ConstantTable
	{
		private readonly MdbReader _mdb;
		private readonly Dictionary<MdbIndex, object> _values = new Dictionary<MdbIndex, object>();
		private int _lastIndex; // index of last parsed row

		public ConstantTable(MdbReader mdb)
		{
			_mdb = mdb;
		}

		public object this[MdbIndex parent]
		{
			get
			{
				object value;
				if (_values.TryGetValue(parent, out value))
					return value;

				var n = _mdb.GetRowCount(MdbTableId.Constant);

				for (; _lastIndex < n; _lastIndex++)
				{
					var row = _mdb.GetRow(MdbTableId.Constant, _lastIndex);

					MdbIndex rowParent = row[MDB.Constant.Parent].Value;
					var type = (ElementType)row[MDB.Constant.Type].Value;
					var blob = row[MDB.Constant.Value].Blob;
					value = ReadValue(type, blob);

					_values.Add(rowParent, value);

					if (rowParent == parent)
					{
						_lastIndex++;
						return value;
					}
				}

				return null;
			}
		}

		private static object ReadValue(ElementType type, byte[] blob)
		{
			var reader = new BufferedBinaryReader(blob);
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
					return Encoding.Unicode.GetString(blob);

				case ElementType.Class:
					return null;

				default:
					return blob;
			}
		}
	}
}
