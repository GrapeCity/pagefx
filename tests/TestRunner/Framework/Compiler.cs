using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.Common.CompilerServices;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.IO;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.Flash.Core.Tools;
using DataDynamics.PageFX.TestRunner.Tools;

namespace DataDynamics.PageFX.TestRunner.Framework
{
	internal static class Compiler
	{
		public static bool Compile(TestCase tc)
		{
			tc.CopySourceFiles();

			var lang = tc.Language;
			if (lang == CompilerLanguage.CIL)
			{
				return ILAsm.Run(tc);
			}

			using (tc.Root.ChangeCurrentDirectory())
			{
				var options = new CompilerOptions();
				try
				{
					ResolveReferences(tc.Root, tc.References, options.References);
				}
				catch (Exception exc)
				{
					tc.Error = String.Format("Unable to resolve test case {0} references. Exception:\n{1}",
					                         tc.Name, exc);
					return false;
				}

				SetCompilerOptions(tc, lang, options);

				try
				{
					string cout = CompilerConsole.Run(options, true);
					if (CompilerConsole.HasErrors(cout))
					{
						tc.Error = String.Format("Unable to compile test case {0}.\n{1}", tc.Name, cout);
						return false;
					}
				}
				catch (Exception exc)
				{
					tc.Error = String.Format("Unable to compile test case {0}. Exception:\n{1}", tc.Name, exc);
					return false;
				}
			}

			return true;
		}

		private static void SetCompilerOptions(TestCase tc, CompilerLanguage lang, CompilerOptions options)
		{
			options.Language = lang;
			options.Debug = tc.Debug;
			options.Optimize = tc.Optimize;
			options.Unsafe = tc.Unsafe;
			options.Output = tc.ExePath;
			options.Target = CompilerTarget.ConsoleApp;
			options.NoLogo = true;
			options.Checked = false;

			options.Define("TARGET_JVM"); //to remove unsafe
			options.Define("NET_2_0");
			options.Defines.AddRange(tc.Defines);

			if (tc.VM == VM.CLR)
				options.Define("MSCLR");
			else if (tc.VM == VM.AVM)
				options.Define("AVM");

			//common refs
			if (tc.VM == VM.AVM)
			{
				GlobalSettings.AddCommonReferences(options);

				options.AddRef(Path.Combine(GlobalSettings.LibsDirectory, "flash.v10.2.dll"));

				options.NoConfig = true;
				options.NoStdlib = true;

				if (lang == CompilerLanguage.VB)
				{
					//options.NoVBRuntime = true;
					options.VBRuntime = GlobalSettings.Libs.VBRuntime;
					//options.CompactFramework = true;
					options.SDKPath = GlobalSettings.Dirs.Libs;
					options.NoWarn = true;
				}
			}
			else
			{
				options.AddRef("System.Core.dll");
			}

			if (tc.IsNUnit)
			{
				if (tc.VM == VM.CLR)
				{
					string nunit = QA.GetNUnitFrameworkPath(GlobalOptions.UseCommonDirectory ? "" : tc.Root);
					options.AddRef(nunit);
				}
				else
				{
					options.AddRef(GlobalSettings.GetLibPath(QA.NUnitFrameworkDll));
				}
				options.AddRef(tc.GetNUnitTestsPath(tc.Root));
			}

			options.Input.AddRange(tc.SourceFiles.Names);
		}

		private static void ResolveReferences(string dir, IEnumerable<string> refs, ICollection<string> list)
		{
			if (refs == null) return;
			foreach (var name in refs)
			{
				ResolveReference(dir, name, list);
			}
		}

		private static void ResolveReference(string dir, string name, ICollection<string> list)
		{
			const StringComparison cmptype = StringComparison.InvariantCultureIgnoreCase;
			if (name.EndsWith(".abc", cmptype))
			{
				var rs = typeof(QA).GetResourceStream(name);
				if (rs == null)
					throw new InvalidOperationException();

				string path = Path.Combine(dir, name);
				rs.Save(path);

				string dll = Path.ChangeExtension(name, ".dll");
				WrapperGenerator.Generate(path, dll, null);

				list.Add(dll);
			}
			else
			{
				throw new InvalidOperationException(String.Format("Unable to resolve ref: {0}", name));
			}
		}
	}
}
