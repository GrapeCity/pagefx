using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Corlib
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

		public IType Type
		{
			get { return _generator.Corlib.GetType(CorlibTypeId.Environment); }
		}

		public AbcInstance Instance
		{
			get { return _generator.Corlib.GetInstance(CorlibTypeId.Environment); }
		}

		private LazyValue<AbcMethod>[] EnvironmentMethods
		{
			get
			{
				return _methods ?? (_methods =
				                    new[]
					                    {
						                    _generator.MethodBuilder.LazyMethod(Type, "get_StackTrace")
					                    });
			}
		}
	}

	internal enum EnvironmentMethodId
	{
		StackTrace,
	}
}