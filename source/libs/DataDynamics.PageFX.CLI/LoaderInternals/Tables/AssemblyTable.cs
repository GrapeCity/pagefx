using System;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CLI.Tools;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.Metadata;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.LoaderInternals.Tables
{
	internal sealed class AssemblyTable : MetadataTable<AssemblyReference>
	{
		public AssemblyTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.Assembly; }
		}

		protected override AssemblyReference ParseRow(MetadataRow row, int index)
		{
			var flags = ((AssemblyFlags)row[Schema.Assembly.Flags].Value);
			var hashAlgorithm = (HashAlgorithmId)row[Schema.Assembly.HashAlgId].Value;

			byte[] publicKey = null;
			byte[] publicKeyToken = null;
			if ((flags & AssemblyFlags.PublicKey) != 0)
			{
				publicKey = row[Schema.Assembly.PublicKey].Blob.ToArray();
				publicKeyToken = publicKey.ComputePublicKeyToken(hashAlgorithm);
			}

			var token = SimpleIndex.MakeToken(TableId.Assembly, index + 1);
			return new AssemblyReference
				{
					Name = row[Schema.Assembly.Name].String,
					Version = GetVersion(row, 1),
					Flags = flags,
					Culture = row[Schema.Assembly.Culture].Culture,
					HashAlgorithm = hashAlgorithm,
					PublicKey = publicKey,
					PublicKeyToken = publicKeyToken,
					MetadataToken = token
				};
		}

		private static Version GetVersion(MetadataRow row, int i)
		{
			return new Version((int)row[i].Value,
							   (int)row[i + 1].Value,
							   (int)row[i + 2].Value,
							   (int)row[i + 3].Value);
		}
	}
}
