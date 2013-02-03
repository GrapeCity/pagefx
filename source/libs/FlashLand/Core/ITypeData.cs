using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.FlashLand.Abc;

namespace DataDynamics.PageFX.FlashLand.Core
{
	internal interface ITypeData
	{
		IType Type { get; }

		AbcMultiname Name { get; }

		bool IsDefined(AbcFile abc);

		ITypeData Import(AbcFile abc);
	}
}
