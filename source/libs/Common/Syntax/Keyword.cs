namespace DataDynamics.PageFX.Common.Syntax
{
	public enum Keyword
	{
		[CSharp("namespace")]
		[VB("Namespace")]
		[ActionScript("package")]
		Namespace,

		[CSharp("const")]
		[VB("Constant")]
		[ActionScript("const")]
		Const,

		[CSharp("extern")]
		[VB("Extern")]
		[ActionScript("native")]
		Extern,

		[CSharp("static")]
		[VB("Shared")]
		[ActionScript("static")]
		Static,

		[CSharp("abstract")]
		[VB("Abstract")]
		[ActionScript("abstract")]
		Abstract,

		[CSharp("virtual")]
		[VB("Virtual")]
		[ActionScript("virtual")]
		Virtual,

		[CSharp("override")]
		[VB("Override")]
		[ActionScript("override")]
		Override,

		[CSharp("new")]
		[VB("New")]
		[ActionScript("new")]
		New,

		[CSharp("readonly")]
		[VB("ReadOnly")]
		[ActionScript("")]
		ReadOnly,

		[CSharp("")]
		[VB("")]
		[ActionScript("var")]
		Field,

		[CSharp("this")]
		[VB("Me")]
		[ActionScript("this")]
		This,

		[CSharp("base")]
		[VB("Base")]
		[ActionScript("super")]
		Base,

		[CSharp("continue")]
		[VB("Continue")]
		[ActionScript("continue")]
		Continue,

		[CSharp("break")]
		[VB("Break")]
		[ActionScript("break")]
		Break,

		[CSharp("null")]
		[VB("Nil")]
		[ActionScript("null")]
		Null,

		[CSharp("true")]
		[VB("True")]
		[ActionScript("true")]
		True,

		[CSharp("false")]
		[VB("False")]
		[ActionScript("false")]
		False,

		[CSharp("return")]
		[VB("return")]
		[ActionScript("return")]
		Return,
	}
}