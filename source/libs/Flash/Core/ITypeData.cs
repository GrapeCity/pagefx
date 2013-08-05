using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Core
{
	internal interface ITypeData
	{
		IType Type { get; }

		AbcMultiname Name { get; }

		bool IsDefined(AbcFile abc);

		ITypeData Import(AbcFile abc);
	}
}
