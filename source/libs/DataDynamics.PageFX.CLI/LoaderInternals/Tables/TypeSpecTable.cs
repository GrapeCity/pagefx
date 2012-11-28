using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.LoaderInternals.Tables
{
	internal sealed class TypeSpecTable
	{
		private readonly AssemblyLoader _loader;
		private IType[] _types;

		public TypeSpecTable(AssemblyLoader loader)
		{
			_loader = loader;
		}

		public IType Get(int index, Context context)
		{
			if (_types == null)
			{
				int n = _loader.Metadata.GetRowCount(TableId.TypeSpec);
				_types = new IType[n];
			}
			return _types[index] ?? (_types[index] = Resolve(index, context));
		}

		private IType Resolve(int index, Context context)
		{
			var row = _loader.Metadata.GetRow(TableId.TypeSpec, index);
			var blob = row[Schema.TypeSpec.Signature].Blob;
			var sig = TypeSignature.Decode(blob);

			var type = _loader.ResolveType(sig, context);
			if (type == null)
				throw new BadMetadataException(string.Format("Unable to resolve signature {0}", sig));

			return type;
		}
	}
}
