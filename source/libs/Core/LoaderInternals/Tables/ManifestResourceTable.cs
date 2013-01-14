using System;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.LoaderInternals.Collections;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals.Tables
{
	internal sealed class ManifestResourceTable : MetadataTable<IManifestResource>, IManifestResourceCollection
	{
		public ManifestResourceTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.ManifestResource; }
		}

		public IManifestResource this[string name]
		{
			get { return this.FirstOrDefault(x => x.Name == name); }
		}

		protected override IManifestResource ParseRow(MetadataRow row, int index)
		{
			string name = row[Schema.ManifestResource.Name].String;
			var offset = (int)row[Schema.ManifestResource.Offset].Value;
			var flags = (ManifestResourceAttributes)row[Schema.ManifestResource.Flags].Value;
			var isPublic = (flags & ManifestResourceAttributes.VisibilityMask) == ManifestResourceAttributes.Public;
			var token = SimpleIndex.MakeToken(TableId.ManifestResource, index + 1);

			var resource = new ManifestResource
				{
					Name = name,
					Offset = offset,
					IsPublic = isPublic,
					MetadataToken = token
				};

			resource.CustomAttributes = new CustomAttributes(Loader, resource);

			SimpleIndex impl = row[Schema.ManifestResource.Implementation].Value;
			if (impl == 0)
			{

			}
			else
			{
				switch (impl.Table)
				{
					case TableId.File:
						resource.Data = Metadata.GetResourceStream(offset);
						break;

					case TableId.AssemblyRef:
						throw new NotSupportedException();
				}
			}

			return resource;
		}
	}
}
