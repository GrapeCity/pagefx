using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Core.CodeGeneration.Corlib
{
	internal sealed class ConsoleTypeImpl
	{
		private readonly AbcGenerator _generator;
		private LazyValue<AbcMethod>[] _methods;

		public ConsoleTypeImpl(AbcGenerator generator)
		{
			_generator = generator;
		}

		public AbcMethod this[ConsoleMethodId id]
		{
			get { return ConsoleMethods[(int)id].Value; }
		}

		private LazyValue<AbcMethod>[] ConsoleMethods
		{
			get
			{
				return _methods ?? (_methods =
				                    new[]
					                    {
						                    _generator.MethodBuilder.LazyMethod(Type, "WriteLine", 0),
						                    _generator.MethodBuilder.LazyMethod(Type, "WriteLine", StringType),
						                    _generator.MethodBuilder.LazyMethod(Type, "OpenSW", 0),
						                    _generator.MethodBuilder.LazyMethod(Type, "CloseSW", 0)
					                    });
			}
		}

		private IType Type
		{
			get { return _generator.Corlib.GetType(CorlibTypeId.Console); }
		}

		private IType StringType
		{
			get { return _generator.SystemTypes.String; }
		}
	}

	internal enum ConsoleMethodId
	{
		WriteLine,
		WriteLine_String,
		OpenSW,
		CloseSW
	}
}