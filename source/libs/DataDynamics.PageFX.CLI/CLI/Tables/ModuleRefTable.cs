using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class ModuleRefTable : MetadataTable<IModule>
	{
		public ModuleRefTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.ModuleRef; }
		}

		protected override IModule ParseRow(MdbRow row, int index)
		{
			string name = row[MDB.ModuleRef.Name].String;

			//var f = GetFile(name);
			//var res = GetResource(name);

			var token = MdbIndex.MakeToken(MdbTableId.ModuleRef, index + 1);
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
