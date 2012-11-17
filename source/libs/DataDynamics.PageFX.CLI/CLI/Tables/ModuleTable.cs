using System;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class ModuleTable : MetadataTable<IModule>
	{
		public ModuleTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.Module; }
		}

		protected override IModule ParseRow(MdbRow row, int index)
		{
			var module = new Module
				{
					Name = row[MDB.Module.Name].String,
					Version = row[MDB.Module.Mvid].Guid,
					IsMain = true,
					RefResolver = Loader,
					MetadataTokenResolver = Loader,
					Resources = Loader.ManifestResources
				};

			var token = MdbIndex.MakeToken(MdbTableId.Module, index + 1);
			module.CustomAttributes = new CustomAttributes(Loader, module, token);

			var file = Loader.Files[module.Name];
			if (file != null)
			{
				throw new NotImplementedException();
			}

			return module;
		}
	}
}
