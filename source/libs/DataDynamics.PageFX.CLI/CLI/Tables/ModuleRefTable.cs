using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class ModuleRefTable : MetadataTable<IModule>
	{
		public ModuleRefTable(AssemblyLoader loader)
			: base(loader, MdbTableId.ModuleRef)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.ModuleRef; }
		}

		protected override IModule ParseRow(int index)
		{
			var row = Mdb.GetRow(MdbTableId.ModuleRef, index);
			string name = row[MDB.ModuleRef.Name].String;

			//var f = GetFile(name);
			//var res = GetResource(name);

			var mod = new Module { Name = name };

			Loader.Assembly.Modules.Add(mod);

			return mod;
		}
	}
}
