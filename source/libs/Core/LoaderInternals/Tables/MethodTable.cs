using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal sealed class MethodTable : MetadataTable<IMethod>
	{
		public MethodTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.MethodDef; }
		}

		protected override IMethod ParseRow(MetadataRow row, int index)
		{
			return new InternalMethod(Loader, row, index);
		}

		public string GetFullName(int index)
		{
			var row = Metadata.GetRow(TableId.MethodDef, index);

			var name = row[Schema.MethodDef.Name].String;

			var typeIndex = Loader.Types.ResolveDeclTypeIndex(index, true);
			if (typeIndex < 0)
				throw new InvalidOperationException();

			return Loader.Types.GetFullName(typeIndex) + "." + name;
		}
	}
}
