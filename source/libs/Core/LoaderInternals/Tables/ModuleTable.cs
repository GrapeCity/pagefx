using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.LoaderInternals.Collections;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals.Tables
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
