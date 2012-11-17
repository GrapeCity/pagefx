using System;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class ModuleTable : MetadataTable<IModule>
	{
		public ModuleTable(AssemblyLoader loader)
			: base(loader, MdbTableId.Module)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.Module; }
		}

		protected override IModule ParseRow(int index)
		{
			var row = Mdb.GetRow(MdbTableId.Module, index);

			var module = new Module
				{
					Name = row[MDB.Module.Name].String,
					Version = row[MDB.Module.Mvid].Guid
				};

			var file = Loader.Files[module.Name];
			if (file != null)
			{
				throw new NotImplementedException();
			}

			module.IsMain = true;
			module.RefResolver = Loader;
			module.MetadataTokenResolver = Loader;
			module.Resources = Loader.ManifestResources;

			return module;
		}
	}
}
