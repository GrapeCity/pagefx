#region ilasm help
/*
        Microsoft (R) .NET Framework IL Assembler.  Version 2.0.50727.42
        Copyright (c) Microsoft Corporation.  All rights reserved.



        Usage: ilasm [Options] <sourcefile> [Options]

        Options:
        /NOLOGO         Don't type the logo
        /QUIET          Don't report assembly progress
        /NOAUTOINHERIT  Disable inheriting from System.Object by default
        /DLL            Compile to .dll
        /EXE            Compile to .exe (default)
        /PDB            Create the PDB file without enabling debug info tracking
        /DEBUG          Disable JIT optimization, create PDB file, use sequence points from PDB
        /DEBUG=IMPL     Disable JIT optimization, create PDB file, use implicit sequence points
        /DEBUG=OPT      Enable JIT optimization, create PDB file, use implicit sequence points
        /OPTIMIZE       Optimize long instructions to short
        /FOLD           Fold the identical method bodies into one
        /CLOCK          Measure and report compilation times
        /RESOURCE=<res_file>    Link the specified resource file (*.res) 
                    into resulting .exe or .dll
        /OUTPUT=<targetfile>    Compile to file with specified name 
                    (user must provide extension, if any)
        /KEY=<keyfile>      Compile with strong signature 
                    (<keyfile> contains private key)
        /KEY=@<keysource>   Compile with strong signature 
                    (<keysource> is the private key source name)
        /INCLUDE=<path>     Set path to search for #include'd files
        /SUBSYSTEM=<int>    Set Subsystem value in the NT Optional header
        /FLAGS=<int>        Set CLR ImageFlags value in the CLR header
        /ALIGNMENT=<int>    Set FileAlignment value in the NT Optional header
        /BASE=<int>     Set ImageBase value in the NT Optional header (max 2GB for 32-bit images)
        /STACK=<int>    Set SizeOfStackReserve value in the NT Optional header
        /MDV=<version_string>   Set Metadata version string
        /MSV=<int>.<int>   Set Metadata stream version (<major>.<minor>)
        /PE64           Create a 64bit image (PE32+)
        /NOCORSTUB      Suppress generation of CORExeMain stub
        /STRIPRELOC     Indicate that no base relocations are needed
        /ITANIUM        Target processor: Intel Itanium
        /X64            Target processor: 64bit AMD processor
        /ENC=<file>     Create Edit-and-Continue deltas from specified source file

        Key may be '-' or '/'
        Options are recognized by first 3 characters
        Default source file extension is .il

        Target defaults:
        /PE64      => /PE64 /ITANIUM
        /ITANIUM   => /PE64 /ITANIUM
        /X64       => /PE64 /X64
*/
#endregion

using System;
using System.IO;
using DataDynamics.PageFX.Common.Extensions;
using DataDynamics.PageFX.Common.Utilities;
using DataDynamics.PageFX.TestRunner.Framework;

namespace DataDynamics.PageFX.TestRunner.Tools
{
	internal static class ILAsm
	{
		public static bool Run(TestCase tc)
		{
			string ilasmPath = Path.Combine(FrameworkInfo.Root_2_0, "ilasm.exe");

			string args = "/nologo /exe ";
			if (tc.Debug) args += "/debug ";
			if (tc.Optimize) args += "/optimize ";
			args += "/output:" + tc.ExePath;

			foreach (var f in tc.SourceFiles)
			{
				args += " ";
				args += f.Name;
			}

			using (tc.Root.ChangeCurrentDirectory())
			{
				try
				{
					int exitCode;
					string cout = CommandPromt.Run(ilasmPath, args, out exitCode);
					if (exitCode != 0)
					{
						tc.Error = "Unable to assembly (ilasm.exe failed).";
						return false;
					}
					//TODO: check ilasm output
				}
				catch (Exception exc)
				{
					tc.Error = String.Format("Unable to assembly test case {0}. Exception:{1}\n", tc.Name, exc);
					return false;
				}
			}
			return true;
		}
	}
}
