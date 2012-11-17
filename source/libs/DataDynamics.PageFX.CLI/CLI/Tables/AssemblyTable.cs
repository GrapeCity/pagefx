using System;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class AssemblyTable : MetadataTable<AssemblyReference>
	{
		public AssemblyTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.Assembly; }
		}

		protected override AssemblyReference ParseRow(MdbRow row, int index)
		{
			var flags = ((AssemblyFlags)row[MDB.Assembly.Flags].Value);
			var hashAlgorithm = (HashAlgorithmId)row[MDB.Assembly.HashAlgId].Value;

			byte[] publicKey = null;
			byte[] publicKeyToken = null;
			if ((flags & AssemblyFlags.PublicKey) != 0)
			{
				publicKey = row[MDB.Assembly.PublicKey].Blob;
				publicKeyToken = publicKey.ComputePublicKeyToken(hashAlgorithm);
			}

			return new AssemblyReference
				{
					Name = row[MDB.Assembly.Name].String,
					Version = GetVersion(row, 1),
					Flags = flags,
					Culture = row[MDB.Assembly.Culture].Culture,
					HashAlgorithm = hashAlgorithm,
					PublicKey = publicKey,
					PublicKeyToken = publicKeyToken
				};
		}

		private static Version GetVersion(MdbRow row, int i)
		{
			return new Version((int)row[i].Value,
							   (int)row[i + 1].Value,
							   (int)row[i + 2].Value,
							   (int)row[i + 3].Value);
		}
	}
}
