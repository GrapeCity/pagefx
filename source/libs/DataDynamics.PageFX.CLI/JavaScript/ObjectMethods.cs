using System.Linq;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal static class ObjectMethods
	{
		public static IMethod FindEquals()
		{
			return SystemTypes.Object.Methods.Find("Equals", SystemTypes.Object);
		}

		public static IMethod FindToString()
		{
			return SystemTypes.Object.Methods.FirstOrDefault(x => x.IsToString());
		}
	}
}