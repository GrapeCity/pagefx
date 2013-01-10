using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.CodeGeneration.Corlib
{
	internal sealed class ArrayTypeImpl
	{
		private readonly AbcGenerator _generator;
		private LazyValue<AbcMethod>[] _methods;

		public ArrayTypeImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

		private IType Type
		{
			get { return _generator.SystemTypes.Array; }
		}

		public AbcInstance Instance
		{
			get { return _generator.TypeBuilder.BuildInstance(Type); }
		}

		public AbcMethod this[ArrayMethodId id]
		{
			get { return ArrayMethods[(int)id].Value; }
		}

		private LazyValue<AbcMethod>[] ArrayMethods
		{
			get
			{
				return _methods ?? (_methods =
				                    new[]
					                    {
						                    _generator.LazyMethod(Type, "get_Length"),
						                    _generator.LazyMethod(Type, "GetElem", 1),
						                    _generator.LazyMethod(Type, "SetElem", 2),
						                    _generator.LazyMethod(Type, "GetItem", 1),
						                    _generator.LazyMethod(Type, "SetItem", 2),
						                    _generator.LazyMethod(Type, "ToFlatIndex", t => !t.IsArray),
						                    _generator.LazyMethod(Type, "IsCharArray"),
						                    _generator.LazyMethod(Type, "CastTo", 1),
						                    _generator.LazyMethod(Type, "HasElemType", 2),
						                    _generator.LazyMethod(Type, "Clear"),
						                    _generator.LazyMethod(Type, "IndexOf", 1),
						                    _generator.LazyMethod(Type, "Contains", 1),
						                    _generator.LazyMethod(Type, "CopyTo", 2)
					                    });
			}
		}
	}

	internal enum ArrayMethodId
	{
		GetLength,
		GetElem,
		SetElem,
		GetItem,
		SetItem,
		ToFlatIndex,
		IsCharArray,
		CastTo,
		HasElemType,
		Clear,
		IndexOf,
		Contains,
		CopyTo,
	}
}