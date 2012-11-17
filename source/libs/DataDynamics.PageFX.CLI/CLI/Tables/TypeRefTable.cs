using System.Diagnostics;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class TypeRefTable : MetadataTable<IType>
	{
		public TypeRefTable(AssemblyLoader loader)
			: base(loader, MdbTableId.TypeRef)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.TypeRef; }
		}

		protected override IType ParseRow(int index)
		{
			var row = Mdb.GetRow(MdbTableId.TypeRef, index);

			MdbIndex scope = row[MDB.TypeRef.ResolutionScope].Value;
			var name = row[MDB.TypeRef.TypeName].String;
			var ns = row[MDB.TypeRef.TypeNamespace].String;
			var fullname = QName(ns, name);

			var type = FindType(scope, fullname);

			if (type == null)
			{
				//TODO: Report error
#if DEBUG
				if (DebugHooks.BreakInvalidTypeReference)
				{
					Debugger.Break();
					FindType(scope, fullname);
				}
#endif
				throw new BadMetadataException(string.Format("Unable to resolve type reference {0}", fullname));
			}

			return type;
		}

		private static string QName(string ns, string name)
		{
			if (string.IsNullOrEmpty(ns)) return name;
			return ns + "." + name;
		}

		private ITypeContainer GetTypeContainer(MdbIndex idx)
		{
			switch (idx.Table)
			{
				case MdbTableId.Module:
					return Loader.Modules[idx.Index - 1];

				case MdbTableId.ModuleRef:
					return Loader.ModuleRefs[idx.Index - 1];

				case MdbTableId.AssemblyRef:
					return Loader.AssemblyRefs[idx.Index - 1];

				case MdbTableId.TypeRef:
					return this[idx.Index - 1];

				default:
					throw new BadMetadataException();
			}
		}

		private IType FindType(MdbIndex rs, string fullname)
		{
			var c = GetTypeContainer(rs);
			if (c != null)
				return c.Types[fullname];
			return null;
		}
	}
}
