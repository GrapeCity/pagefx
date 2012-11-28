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

		public override TableId Id
		{
			get { return TableId.Module; }
		}

		protected override IModule ParseRow(MetadataRow row, int index)
		{
			var token = SimpleIndex.MakeToken(TableId.Module, index + 1);
			var module = new Module
				{
					Name = row[Schema.Module.Name].String,
					Version = row[Schema.Module.Mvid].Guid,
					Loader = Loader,
					MetadataToken = token
				};

			module.CustomAttributes = new CustomAttributes(Loader, module);

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
