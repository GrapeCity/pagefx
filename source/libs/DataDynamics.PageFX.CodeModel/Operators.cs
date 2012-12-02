using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
	public enum CastOperator
	{
		Cast,
		Is,
		As
	}

	public enum BinaryOperator
	{
		[CSharp("+")]
		Addition,

		[CSharp("-")]
		Subtraction,

		[CSharp("*")]
		Multiply,

		[CSharp("/")]
		Division,

		[CSharp("%")]
		Modulus,

		[CSharp("<<")]
		LeftShift,

		[CSharp(">>")]
		RightShift,

		[CSharp("==")]
		Equality,

		[CSharp("!=")]
		Inequality,

		[CSharp("|")]
		BitwiseOr,

		[CSharp("&")]
		BitwiseAnd,

		[CSharp("^")]
		ExclusiveOr,

		[CSharp("||")]
		BooleanOr,

		[CSharp("&&")]
		BooleanAnd,

		[CSharp("<")]
		LessThan,

		[CSharp("<=")]
		LessThanOrEqual,

		[CSharp(">")]
		GreaterThan,

		[CSharp(">=")]
		GreaterThanOrEqual,

		[CSharp("=")]
		Assign,
	}

	public enum UnaryOperator
	{
		[CSharp("-")]
		Negate,

		[CSharp("!")]
		BooleanNot,

		[CSharp("~")]
		BitwiseNot,

		[CSharp("++")]
		PreIncrement,

		[CSharp("++")]
		PostIncrement,

		[CSharp("--")]
		PreDecrement,

		[CSharp("--")]
		PostDecrement
	}

	public enum BranchOperator
	{
		None,
		True,
		False,
		Null,
		NotNull,
		Equality,
		Inequality,
		LessThan,
		LessThanOrEqual,
		GreaterThan,
		GreaterThanOrEqual,
	}
}
