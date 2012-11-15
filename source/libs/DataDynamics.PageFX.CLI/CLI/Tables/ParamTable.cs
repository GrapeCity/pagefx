using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.CLI.Tables
{
	internal sealed class ParamTable : MetadataTable<IParameter>
	{
		public ParamTable(AssemblyLoader loader)
			: base(loader, MdbTableId.Param)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.Param; }
		}

		protected override IParameter ParseRow(int index)
		{
			var row = Mdb.GetRow(MdbTableId.Param, index);

			var token = MdbIndex.MakeToken(MdbTableId.Param, index + 1);
			var value = Loader.Const[token];

			return new Parameter
				{
					Flags = ((ParamAttributes)row[MDB.Param.Flags].Value),
					Index = ((int)row[MDB.Param.Sequence].Value),
					Name = row[MDB.Param.Name].String,
					Value = value
				};
		}
	}
}
