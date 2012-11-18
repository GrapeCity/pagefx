using System;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class AssemblyRefTable : MetadataTable<IAssembly>
	{
		public AssemblyRefTable(AssemblyLoader loader) : base(loader)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.AssemblyRef; }
		}

		protected override IAssembly ParseRow(MdbRow row, int index)
		{
			var asmref = new AssemblyReference
				{
					Version = GetVersion(row, 0),
					Flags = ((AssemblyFlags)row[MDB.AssemblyRef.Flags].Value),
					PublicKeyToken = row[MDB.AssemblyRef.PublicKeyOrToken].Blob,
					Name = row[MDB.AssemblyRef.Name].String,
					Culture = row[MDB.AssemblyRef.Culture].Culture,
					HashValue = row[MDB.AssemblyRef.HashValue].Blob
				};

			var asm = Loader.ResolveAssembly(asmref);
			
			var mod = Loader.Assembly.MainModule as Module;
			if (mod != null)
			{
				mod.Loader = null;
				mod.References.Add(asm);
				mod.Loader = Loader;
			}
			else
			{
				Loader.Assembly.MainModule.References.Add(asm);
			}

			return asm;
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
