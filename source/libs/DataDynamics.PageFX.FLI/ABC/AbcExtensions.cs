namespace DataDynamics.PageFX.FlashLand.Abc
{
	internal static class AbcExtensions
	{
		public static string MakeKey(this AbcConst<string> name, AbcConstKind kind)
		{
			return kind == 0 ? "*" : name.Value + (int)kind;
		}
	}
}