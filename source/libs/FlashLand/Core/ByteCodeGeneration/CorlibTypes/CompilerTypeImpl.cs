using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.ByteCodeGeneration.CorlibTypes
{
	internal sealed class CompilerTypeImpl
	{
		private readonly AbcGenerator _generator;
		private LazyValue<AbcMethod>[] _methods;

		public CompilerTypeImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

		public AbcMethod this[CompilerMethodId id]
		{
			get { return CompilerMethods[(int)id].Value; }
		}

		private LazyValue<AbcMethod>[] CompilerMethods
		{
			get
			{
				return _methods ?? (_methods =
				                    new[]
					                    {
						                    _generator.LazyMethod(Type, "ToArray", 1)
					                    });
			}
		}

		private IType Type
		{
			get { return _generator.GetType(CorlibTypeId.CompilerUtils); }
		}
	}

	internal enum CompilerMethodId
	{
		ToArray,
	}
}