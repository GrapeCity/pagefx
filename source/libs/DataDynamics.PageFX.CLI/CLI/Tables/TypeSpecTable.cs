using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class TypeSpecTable
	{
		private readonly AssemblyLoader _loader;
		private readonly IType[] _types;

		public TypeSpecTable(AssemblyLoader loader)
		{
			_loader = loader;

			int n = loader.Mdb.GetRowCount(MdbTableId.TypeSpec);
			_types = new IType[n];
		}

		public IType Get(int index, Context context)
		{
			var type = _types[index];
			if (type != null) return type;

			var row = _loader.Mdb.GetRow(MdbTableId.TypeSpec, index);
			var blob = row[MDB.TypeSpec.Signature].Blob;
			var sig = MdbSignature.DecodeTypeSignature(blob);

			type = _loader.ResolveType(sig, context);
			if (type == null)
				throw new BadMetadataException(string.Format("Unable to resolve signature {0}", sig));

			_types[index] = type;

			return type;
		}
	}
}
