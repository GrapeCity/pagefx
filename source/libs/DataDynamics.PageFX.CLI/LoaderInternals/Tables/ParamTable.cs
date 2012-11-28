using DataDynamics.PageFX.CLI.LoaderInternals.Collections;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.LoaderInternals.Tables
{
	internal sealed class ParamTable : MetadataTable<IParameter>
	{
		public ParamTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.Param; }
		}

		protected override IParameter ParseRow(MetadataRow row, int index)
		{
			var token = SimpleIndex.MakeToken(TableId.Param, index + 1);
			var value = Loader.Const[token];

			var param = new Parameter
				{
					Flags = ((ParamAttributes)row[Schema.Param.Flags].Value),
					Index = ((int)row[Schema.Param.Sequence].Value),
					Name = row[Schema.Param.Name].String,
					Value = value,
					MetadataToken = token
				};

			param.CustomAttributes = new CustomAttributes(Loader, param);

			return param;
		}
	}
}
