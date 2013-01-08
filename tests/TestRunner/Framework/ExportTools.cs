using System;
using DataDynamics.PageFX.Common.Services;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	internal static class ExportTools
	{
		public static void ToCSharp(IAssembly assembly, string path)
		{
			try
			{
				ExportService.ToFile(assembly, "c#", path);
			}
			catch (Exception e)
			{
				//string.Format("Unable to serialize c# source code.\nException:\n{0}", e);
			}
		}
	}
}
