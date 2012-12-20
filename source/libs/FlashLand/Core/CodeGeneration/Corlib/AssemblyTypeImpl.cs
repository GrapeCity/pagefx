using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Corlib
{
	internal sealed class AssemblyTypeImpl
	{
		private readonly AbcGenerator _generator;
		private LazyValue<AbcMethod>[] _methods;

		public AssemblyTypeImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

		public AbcInstance Instance
		{
			get { return _generator.GetInstance(CorlibTypeId.Assembly); }
		}

		public AbcMethod this[AssemblyMethodId id]
		{
			get { return AssemblyMethods[(int)id].Value; }
		}

		private LazyValue<AbcMethod>[] AssemblyMethods
		{
			get
			{
				return _methods ?? (_methods =
				                    new[]
					                    {
						                    _generator.LazyMethod(Type, "GetType", 1)
					                    });
			}
		}

		private IType Type
		{
			get { return _generator.CorlibTypes[CorlibTypeId.Assembly]; }
		}
	}

	internal enum AssemblyMethodId
	{
		GetTypeById
	}
}