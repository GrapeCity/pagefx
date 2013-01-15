using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
{
	internal sealed class ModuleRefTable : MetadataTable<IModule>
	{
		public ModuleRefTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.ModuleRef; }
		}

		protected override IModule ParseRow(MetadataRow row, int index)
		{
			string name = row[Schema.ModuleRef.Name].String;

			//var f = GetFile(name);
			//var res = GetResource(name);

			var token = SimpleIndex.MakeToken(TableId.ModuleRef, index + 1);
			var mod = new Module
				{
					Name = name,
					MetadataToken = token
				};

			Loader.Assembly.Modules.Add(mod);

			return mod;
		}
	}
}
