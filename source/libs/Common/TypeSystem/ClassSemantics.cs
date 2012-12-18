using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	public enum ClassSemantics
	{
		None,

		[CSharp("static")]
		[ActionScript("abstract final")]
		Static,

		[CSharp("sealed")]
		[ActionScript("final")]
		Sealed,
    
		[CSharp("abstract")]
		[ActionScript("abstract")]
		Abstract,
	}
}