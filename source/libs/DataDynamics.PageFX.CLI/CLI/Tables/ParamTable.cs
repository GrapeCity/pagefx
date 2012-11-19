using DataDynamics.PageFX.CLI.Collections;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class ParamTable : MetadataTable<IParameter>
	{
		public ParamTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.Param; }
		}

		protected override IParameter ParseRow(MdbRow row, int index)
		{
			var token = MdbIndex.MakeToken(MdbTableId.Param, index + 1);
			var value = Loader.Const[token];

			var param = new Parameter
				{
					Flags = ((ParamAttributes)row[MDB.Param.Flags].Value),
					Index = ((int)row[MDB.Param.Sequence].Value),
					Name = row[MDB.Param.Name].String,
					Value = value
				};

			param.CustomAttributes = new CustomAttributes(Loader, param, token);

			return param;
		}
	}
}
