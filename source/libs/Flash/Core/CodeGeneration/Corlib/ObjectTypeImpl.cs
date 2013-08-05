using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Corlib
{
	internal sealed class ObjectTypeImpl
	{
		private readonly AbcGenerator _generator;

		public ObjectTypeImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

		public IType Type
		{
			get { return _generator.SystemTypes.Object; }
		}

		public AbcInstance Instance
		{
			get { return _generator.TypeBuilder.BuildInstance(Type); }
		}

		public AbcMethod this[ObjectMethodId id]
		{
			get { return Methods[(int)id].Value; }
		}

		private LazyValue<AbcMethod>[] Methods
		{
			get
			{
				return _methods ?? (_methods
				                    = new[]
					                    {
						                    _generator.MethodBuilder.LazyMethod(Type, "GetType"),
						                    _generator.MethodBuilder.LazyMethod(Type, "Equals", 1),
						                    _generator.MethodBuilder.LazyMethod(Type, "GetTypeId"),
						                    _generator.MethodBuilder.LazyMethod(Type, "NewHashCode")
					                    });
			}
		}

		private LazyValue<AbcMethod>[] _methods;
	}

	internal enum ObjectMethodId
	{
		GetType,
		Equals,
		GetTypeId,
		NewHashCode
	}
}