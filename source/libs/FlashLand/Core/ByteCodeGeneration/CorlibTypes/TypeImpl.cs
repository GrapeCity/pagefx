using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.ByteCodeGeneration.CorlibTypes
{
	internal sealed class TypeImpl
	{
		private readonly AbcGenerator _generator;
		private LazyValue<AbcMethod>[] _methods;

		public TypeImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

		private IType Type
		{
			get { return _generator.SysTypes.Type; }
		}

		public AbcInstance Instance
		{
			get { return _generator.DefineAbcInstance(Type); }
		}

		public AbcMethod this[TypeMethodId id]
		{
			get { return Methods[(int)id].Value; }
		}

		private LazyValue<AbcMethod>[] Methods
		{
			get
			{
				return _methods ?? (_methods =
				                    new[]
					                    {
						                    _generator.LazyMethod(Type, "get_ValueTypeKind")
					                    });
			}
		}
	}

	internal enum TypeMethodId
	{
		ValueTypeKind,
	}
}