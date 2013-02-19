using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class ParameterImpl : MemberBase, IParameter
	{
		private readonly ParamAttributes _flags;

		public ParameterImpl(AssemblyLoader loader, MetadataRow row, int index)
			: base(loader, TableId.Param, index)
		{
			_flags = ((ParamAttributes)row[Schema.Param.Flags].Value);
			Index = ((int)row[Schema.Param.Sequence].Value);
			Name = row[Schema.Param.Name].String;
			Value = Loader.Const[MetadataToken];
		}

		public object Value { get; private set; }

		public string Documentation { get; set; }

		public object Clone()
		{
			return this;
		}

		public int Index { get; private set; }

		public IType Type
		{
			get { return null; }
		}

		public bool IsIn
		{
			get { return (_flags & ParamAttributes.In) != 0; }
		}

		public bool IsOut
		{
			get { return (_flags & ParamAttributes.Out) != 0; }
		}

		public bool IsAddressed { get; set; }

		public IInstruction Instruction { get; set; }
	}
}
