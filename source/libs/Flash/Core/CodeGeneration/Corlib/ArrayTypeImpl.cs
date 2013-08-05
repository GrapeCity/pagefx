using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Corlib
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
						                    _generator.MethodBuilder.LazyMethod(Type, "get_Length"),
						                    _generator.MethodBuilder.LazyMethod(Type, "GetElem", 1),
						                    _generator.MethodBuilder.LazyMethod(Type, "SetElem", 2),
						                    _generator.MethodBuilder.LazyMethod(Type, "GetItem", 1),
						                    _generator.MethodBuilder.LazyMethod(Type, "SetItem", 2),
						                    _generator.MethodBuilder.LazyMethod(Type, "ToFlatIndex", t => !t.IsArray),
						                    _generator.MethodBuilder.LazyMethod(Type, "IsCharArray"),
						                    _generator.MethodBuilder.LazyMethod(Type, "CastTo", 1),
						                    _generator.MethodBuilder.LazyMethod(Type, "HasElemType", 2),
						                    _generator.MethodBuilder.LazyMethod(Type, "Clear"),
						                    _generator.MethodBuilder.LazyMethod(Type, "IndexOf", 1),
						                    _generator.MethodBuilder.LazyMethod(Type, "Contains", 1),
						                    _generator.MethodBuilder.LazyMethod(Type, "CopyTo", 2)
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