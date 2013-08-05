using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Corlib
{
	internal sealed class AssemblyTypeImpl
	{
		private readonly AbcGenerator _generator;
		private LazyValue<AbcMethod>[] _methods;

		public AssemblyTypeImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

		private IType Type
		{
			get { return _generator.Corlib.GetType(CorlibTypeId.Assembly); }
		}

		public AbcInstance Instance
		{
			get { return _generator.Corlib.GetInstance(CorlibTypeId.Assembly); }
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
						                    _generator.MethodBuilder.LazyMethod(Type, "GetType", 1)
					                    });
			}
		}
	}

	internal enum AssemblyMethodId
	{
		GetTypeById
	}
}