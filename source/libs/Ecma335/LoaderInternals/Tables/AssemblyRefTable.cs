using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals.Tables
{
	internal sealed class AssemblyRefTable : MetadataTable<IAssembly>
	{
		public AssemblyRefTable(AssemblyLoader loader) : base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.AssemblyRef; }
		}

		protected override IAssembly ParseRow(MetadataRow row, int index)
		{
			var token = SimpleIndex.MakeToken(TableId.AssemblyRef, index + 1);
			var asmref = new AssemblyReference
				{
					Version = GetVersion(row, 0),
					Flags = ((AssemblyFlags)row[Schema.AssemblyRef.Flags].Value),
					PublicKeyToken = row[Schema.AssemblyRef.PublicKeyOrToken].Blob.ToArray(),
					Name = row[Schema.AssemblyRef.Name].String,
					Culture = row[Schema.AssemblyRef.Culture].Culture,
					HashValue = row[Schema.AssemblyRef.HashValue].Blob.ToArray(),
					MetadataToken = token
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

		private static Version GetVersion(MetadataRow row, int i)
		{
			return new Version((int)row[i].Value,
							   (int)row[i + 1].Value,
							   (int)row[i + 2].Value,
							   (int)row[i + 3].Value);
		}
	}
}
