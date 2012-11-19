using System;
using DataDynamics.PageFX.CLI.Collections;
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
					Loader = Loader
				};

			var token = MdbIndex.MakeToken(MdbTableId.Module, index + 1);
			module.CustomAttributes = new CustomAttributes(Loader, module, token);

			var file = Loader.Files[module.Name];
			if (file != null)
			{
				throw new NotImplementedException();
			}

			module.IsMain = true;
			module.Resources = Loader.ManifestResources;
			module.Types = Loader.Types;

			return module;
		}
	}
}
