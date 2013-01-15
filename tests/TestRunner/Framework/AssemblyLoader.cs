using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	internal static class AssemblyLoader
	{
		//TODO: revise error handling, e.g. throw exception

		public static IAssembly Load(string path, VM vm, string tcroot, ref string error)
		{
			SetupCli(vm);

#if DEBUG
			CommonLanguageInfrastructure.Debug = true;
			CommonLanguageInfrastructure.TestCaseDirectory = tcroot;
#endif

			IAssembly asm;
			try
			{
				asm = CommonLanguageInfrastructure.Deserialize(path, null);
			}
			catch (Exception e)
			{
				error = String.Format("Unable to deserialize assembly.\nException:\n{0}", e);
				return null;
			}

			return asm;
		}

		private static void SetupCli(VM vm)
		{
			if (vm == VM.AVM)
			{
				CommonLanguageInfrastructure.ResolveLabels = false;
				CommonLanguageInfrastructure.SubstituteFrameworkAssemblies = true;
			}
			else
			{
				CommonLanguageInfrastructure.ResolveLabels = true;
				CommonLanguageInfrastructure.SubstituteFrameworkAssemblies = false;
			}
		}
	}
}
