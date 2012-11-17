using System.Linq;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class FileTable : MetadataTable<IManifestFile>
	{
		public FileTable(AssemblyLoader loader) : base(loader)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.File; }
		}

		public IManifestFile this[string name]
		{
			get { return this.FirstOrDefault(x => x.Name == name); }
		}

		protected override IManifestFile ParseRow(MdbRow row, int index)
		{
			var flags = (FileFlags)row[MDB.File.Flags].Value;

			return new ManifestFile
				{
					Name = row[MDB.File.Name].String,
					HashValue = row[MDB.File.HashValue].Blob,
					ContainsMetadata = flags == FileFlags.ContainsMetadata
				};
		}
	}
}
