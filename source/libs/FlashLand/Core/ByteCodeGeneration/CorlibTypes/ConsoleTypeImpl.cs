using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core.ByteCodeGeneration.CorlibTypes
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
						                    _generator.LazyMethod(Type, "WriteLine", 0),
						                    _generator.LazyMethod(Type, "WriteLine", StringType),
						                    _generator.LazyMethod(Type, "OpenSW", 0),
						                    _generator.LazyMethod(Type, "CloseSW", 0)
					                    });
			}
		}

		private IType Type
		{
			get { return _generator.GetType(CorlibTypeId.Console); }
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