using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.SpecialTypes
{
	internal sealed class NativeType : ITypeData
	{
		private readonly QName _name;

		public NativeType(IType type, QName name)
		{
			_name = name;
			Type = type;
		}

		public IType Type { get; private set; }

		public AbcMultiname Name { get; private set; }

		public bool IsDefined(AbcFile abc)
		{
			return abc.IsDefined(Name);
		}

		public ITypeData Import(AbcFile abc)
		{
			var name = Name ?? _name.Define(abc);
			return new NativeType(Type, _name) {Name = name};
		}
	}
}
