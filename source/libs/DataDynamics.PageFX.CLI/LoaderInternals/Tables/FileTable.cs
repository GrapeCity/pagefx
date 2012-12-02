using System.Linq;
using DataDynamics.PageFX.CLI.LoaderInternals.Collections;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.LoaderInternals.Tables
{
	internal sealed class FileTable : MetadataTable<IManifestFile>
	{
		public FileTable(AssemblyLoader loader) : base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.File; }
		}

		public IManifestFile this[string name]
		{
			get { return this.FirstOrDefault(x => x.Name == name); }
		}

		protected override IManifestFile ParseRow(MetadataRow row, int index)
		{
			var flags = (FileFlags)row[Schema.File.Flags].Value;

			var token = SimpleIndex.MakeToken(TableId.File, index + 1);

			var file = new ManifestFile
				{
					Name = row[Schema.File.Name].String,
					HashValue = row[Schema.File.HashValue].Blob.ToArray(),
					ContainsMetadata = flags == FileFlags.ContainsMetadata,
					MetadataToken = token
				};

			file.CustomAttributes = new CustomAttributes(Loader, file);

			return file;
		}
	}
}
