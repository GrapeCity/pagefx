using System.IO;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.LoaderInternals.Tables
{
	internal sealed class MethodSpecTable
	{
		private readonly AssemblyLoader _loader;
		private IMethod[] _methods;

		public MethodSpecTable(AssemblyLoader loader)
		{
			_loader = loader;
		}

		public IMethod Get(int index, Context context)
		{
			if (_methods == null)
			{
				int n = _loader.Metadata.GetRowCount(TableId.MethodSpec);
				_methods = new IMethod[n];
			}
			return _methods[index] ?? (_methods[index] = Resolve(index, context));
		}

		private IMethod Resolve(int index, Context context)
		{
			var row = _loader.Metadata.GetRow(TableId.MethodSpec, index);
			SimpleIndex idx = row[Schema.MethodSpec.Method].Value;
			var method = _loader.GetMethodDefOrRef(idx, new Context(context, true));

			if (method == null)
				throw new BadTokenException(idx);

			var blob = row[Schema.MethodSpec.Instantiation].Blob;
			var args = ReadMethodSpecArgs(blob, context);

			var spec = GenericType.CreateMethodInstance(method.DeclaringType, method, args);
			spec.MetadataToken = SimpleIndex.MakeToken(TableId.MethodSpec, index + 1);

			return spec;
		}

		private IType[] ReadMethodSpecArgs(BufferedBinaryReader reader, Context context)
		{
			if (reader.ReadByte() != 0x0A)
				throw new BadSignatureException("Invalid MethodSpec signature");

			int n = reader.ReadPackedInt();
			var args = new IType[n];
			for (int i = 0; i < n; ++i)
			{
				var sig = TypeSignature.Decode(reader);
				args[i] = _loader.ResolveType(sig, context);
			}

			return args;
		}
	}
}
