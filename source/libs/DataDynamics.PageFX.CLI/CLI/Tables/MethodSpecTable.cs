using System.IO;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
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
				int n = _loader.Mdb.GetRowCount(MdbTableId.MethodSpec);
				_methods = new IMethod[n];
			}
			return _methods[index] ?? (_methods[index] = Resolve(index, context));
		}

		private IMethod Resolve(int index, Context context)
		{
			var row = _loader.Mdb.GetRow(MdbTableId.MethodSpec, index);
			MdbIndex idx = row[MDB.MethodSpec.Method].Value;
			var method = _loader.GetMethodDefOrRef(idx, new Context(context, true));

			if (method == null)
				throw new BadTokenException(idx);

			var blob = row[MDB.MethodSpec.Instantiation].Blob;
			var args = ReadMethodSpecArgs(blob, context);

			var spec = GenericType.CreateMethodInstance(method.DeclaringType, method, args);
			spec.MetadataToken = MdbIndex.MakeToken(MdbTableId.MethodSpec, index + 1);

			return spec;
		}

		private IType[] ReadMethodSpecArgs(byte[] blob, Context context)
		{
			var reader = new BufferedBinaryReader(blob);
			if (reader.ReadByte() != 0x0A)
				throw new BadSignatureException("Invalid MethodSpec signature");

			int n = reader.ReadPackedInt();
			var args = new IType[n];
			for (int i = 0; i < n; ++i)
			{
				var sig = MdbSignature.DecodeTypeSignature(reader);
				args[i] = _loader.ResolveType(sig, context);
			}

			return args;
		}
	}
}
