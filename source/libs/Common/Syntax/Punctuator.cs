namespace DataDynamics.PageFX.Common.Syntax
{
	public enum Punctuator
	{
		[CSharp(" : ")]
		[VB("Extends ")]
		[ActionScript("extends ")]
		BaseTypeSeparator,

		[CSharp(":")]
		[VB("")]
		[ActionScript("")]
		BeginInterfaces,

		[CSharp("")]
		[VB("Implements ")]
		[ActionScript("implements ")]
		InterfacePrefix,

		[CSharp(", ")]
		[VB("")]
		[ActionScript("")]
		InterfaceSeparator,

		[CSharp(", ")]
		[VB("")]
		[ActionScript("")]
		InterfacesSeparator,

		[CSharp(", ")]
		[VB(", ")]
		[ActionScript(", ")]
		ParameterSeparator,

		[CSharp("{")]
		BeginNamespace,

		[CSharp("}")]
		EndNamespace,

		[CSharp("{")]
		BeginType,

		[CSharp("}")]
		EndType,

		[CSharp("#region")]
		BeginRegion,

		[CSharp("#endregion")]
		EndRegion,

		[CSharp("{")]
		[ActionScript("{")]
		BeginBlock,

		[CSharp("}")]
		[ActionScript("}")]
		EndBlock,

		[CSharp("(")]
		BeginParams,

		[CSharp(")")]
		EndParams,
	}
}