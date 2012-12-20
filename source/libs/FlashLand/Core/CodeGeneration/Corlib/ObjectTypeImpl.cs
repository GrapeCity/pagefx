using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Corlib
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
			get { return _generator.DefineAbcInstance(Type); }
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
						                    _generator.LazyMethod(Type, "GetType"),
						                    _generator.LazyMethod(Type, "Equals", 1),
						                    _generator.LazyMethod(Type, "GetTypeId"),
						                    _generator.LazyMethod(Type, "NewHashCode")
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