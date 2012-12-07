using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.ByteCodeGeneration.CorlibTypes
{
	internal sealed class EnvironmentTypeImpl
	{
		private readonly AbcGenerator _generator;
		private LazyValue<AbcMethod>[] _methods;

		public EnvironmentTypeImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

		public AbcMethod this[EnvironmentMethodId id]
		{
			get { return EnvironmentMethods[(int)id].Value; }
		}

		private LazyValue<AbcMethod>[] EnvironmentMethods
		{
			get
			{
				return _methods ?? (_methods =
				                    new[]
					                    {
						                    _generator.LazyMethod(Type, "get_StackTrace")
					                    });
			}
		}

		private IType Type
		{
			get { return _generator.CorlibTypes[CorlibTypeId.Environment]; }
		}
	}

	internal enum EnvironmentMethodId
	{
		StackTrace,
	}
}