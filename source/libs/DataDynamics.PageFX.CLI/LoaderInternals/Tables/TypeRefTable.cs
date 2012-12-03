using System.Diagnostics;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.LoaderInternals.Tables
{
	internal sealed class TypeRefTable : MetadataTable<IType>
	{
		public TypeRefTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.TypeRef; }
		}

		protected override IType ParseRow(MetadataRow row, int index)
		{
			SimpleIndex scope = row[Schema.TypeRef.ResolutionScope].Value;
			var name = row[Schema.TypeRef.TypeName].String;
			var ns = row[Schema.TypeRef.TypeNamespace].String;
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

		private IType FindType(SimpleIndex scope, string fullname)
		{
			var c = GetTypeContainer(scope);
			if (c != null)
				return c.Types[fullname];
			return null;
		}

		private ITypeContainer GetTypeContainer(SimpleIndex idx)
		{
			var index = idx.Index - 1;
			switch (idx.Table)
			{
				case TableId.Module:
					return Loader.Modules[index];

				case TableId.ModuleRef:
					return Loader.ModuleRefs[index];

				case TableId.AssemblyRef:
					return Loader.AssemblyRefs[index];

				case TableId.TypeRef:
					return this[index];

				default:
					throw new BadMetadataException();
			}
		}
	}
}
