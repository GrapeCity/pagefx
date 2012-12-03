//
// Mono.CSharp CSharpCodeCompiler Class implementation
//
// Authors:
//	Sean Kasun (seank@users.sf.net)
//	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// Copyright (c) Novell, Inc. (http://www.novell.com)
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

namespace Mono.CSharp
{
	using System;
	using System.CodeDom;
	using System.CodeDom.Compiler;
	using System.IO;
	using System.Text;
	using System.Reflection;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Diagnostics;
	using System.Text.RegularExpressions;

#if NET_2_0
	using System.Collections.Generic;
#endif
	
	internal class CSharpCodeCompiler : CSharpCodeGenerator, ICodeCompiler
	{
		static string windowsMcsPath;
		static string windowsMonoPath;

		static CSharpCodeCompiler ()
		{
			if (Path.DirectorySeparatorChar == '\\') {
				PropertyInfo gac = typeof (Environment).GetProperty ("GacPath", BindingFlags.Static|BindingFlags.NonPublic);
				MethodInfo get_gac = gac.GetGetMethod (true);
				string p = Path.GetDirectoryName (
					(string) get_gac.Invoke (null, null));
				windowsMonoPath = Path.Combine (
					Path.GetDirectoryName (
						Path.GetDirectoryName (p)),
					"bin\\mono.bat");
				if (!File.Exists (windowsMonoPath))
					windowsMonoPath = Path.Combine (
						Path.GetDirectoryName (
							Path.GetDirectoryName (p)),
						"bin\\mono.exe");
				if (!File.Exists (windowsMonoPath))
					windowsMonoPath = Path.Combine (
						Path.GetDirectoryName (
							Path.GetDirectoryName (
								Path.GetDirectoryName (p))),
						"mono\\mono\\mini\\mono.exe");
				if (!File.Exists (windowsMonoPath))
					throw new FileNotFoundException ("Windows mono path not found: " + windowsMonoPath);
#if NET_2_0
				windowsMcsPath =
					Path.Combine (p, "2.0\\gmcs.exe");
#else
				windowsMcsPath =
					Path.Combine (p, "1.0\\mcs.exe");
#endif
				if (!File.Exists (windowsMcsPath))
#if NET_2_0
					windowsMcsPath = 
						Path.Combine(
							Path.GetDirectoryName (p),
							"lib\\net_2_0\\gmcs.exe");
#else
					windowsMcsPath = 
						Path.Combine(
							Path.GetDirectoryName (p),
							"lib\\default\\mcs.exe");
#endif
				if (!File.Exists (windowsMcsPath))
					throw new FileNotFoundException ("Windows mcs path not found: " + windowsMcsPath);
			}
		}

		//
		// Constructors
		//
		public CSharpCodeCompiler()
		{
		}

#if NET_2_0
		public CSharpCodeCompiler (Dictionary <string, string> providerOptions) :
			base (providerOptions)
		{
		}
#endif
		
		//
		// Methods
		//
		public CompilerResults CompileAssemblyFromDom (CompilerParameters options, CodeCompileUnit e)
		{
			return CompileAssemblyFromDomBatch (options, new CodeCompileUnit[] { e });
		}

		public CompilerResults CompileAssemblyFromDomBatch (CompilerParameters options, CodeCompileUnit[] ea)
		{
			if (options == null) {
				throw new ArgumentNullException ("options");
			}

			try {
				return CompileFromDomBatch (options, ea);
			} finally {
				options.TempFiles.Delete ();
			}
		}

		public CompilerResults CompileAssemblyFromFile (CompilerParameters options, string fileName)
		{
			return CompileAssemblyFromFileBatch (options, new string[] { fileName });
		}

		public CompilerResults CompileAssemblyFromFileBatch (CompilerParameters options, string[] fileNames)
		{
			if (options == null) {
				throw new ArgumentNullException ("options");
			}

			try {
				return CompileFromFileBatch (options, fileNames);
			} finally {
				options.TempFiles.Delete ();
			}
		}

		public CompilerResults CompileAssemblyFromSource (CompilerParameters options, string source)
		{
			return CompileAssemblyFromSourceBatch (options, new string[] { source });
		}

		public CompilerResults CompileAssemblyFromSourceBatch (CompilerParameters options, string[] sources)
		{
			if (options == null) {
				throw new ArgumentNullException ("options");
			}

			try {
				return CompileFromSourceBatch (options, sources);
			} finally {
				options.TempFiles.Delete ();
			}
		}

		private CompilerResults CompileFromFileBatch (CompilerParameters options, string[] fileNames)
		{
			if (null == options)
				throw new ArgumentNullException("options");
			if (null == fileNames)
				throw new ArgumentNullException("fileNames");

			CompilerResults results=new CompilerResults(options.TempFiles);
			Process mcs=new Process();

			string mcs_output;
			string mcs_stdout;
			string[] mcs_output_lines;
			// FIXME: these lines had better be platform independent.
			if (Path.DirectorySeparatorChar == '\\') {
				mcs.StartInfo.FileName = windowsMonoPath;
				mcs.StartInfo.Arguments = "\"" + windowsMcsPath + "\" " +
#if NET_2_0
					BuildArgs (options, fileNames, ProviderOptions);
#else
					BuildArgs (options, fileNames);
#endif
			} else {
#if NET_2_0
				// FIXME: This is a temporary hack to make code genaration work in 2.0
				mcs.StartInfo.FileName="gmcs";
				mcs.StartInfo.Arguments=BuildArgs(options, fileNames, ProviderOptions);
#else
				mcs.StartInfo.FileName="mcs";
				mcs.StartInfo.Arguments=BuildArgs(options, fileNames);
#endif
			}
			mcs.StartInfo.CreateNoWindow=true;
			mcs.StartInfo.UseShellExecute=false;
			mcs.StartInfo.RedirectStandardOutput=true;
			mcs.StartInfo.RedirectStandardError=true;
#if NET_2_0
			string mono_inside_mdb = null;
#endif
			
			try {
#if NET_2_0
				mono_inside_mdb = Environment.GetEnvironmentVariable ("MONO_INSIDE_MDB");
				if (mono_inside_mdb != null) {
					if (mono_inside_mdb.Length == 0)
						mono_inside_mdb = "1";
					Environment.SetEnvironmentVariable ("MONO_INSIDE_MDB", null);
				}
#endif
				
				mcs.Start();
				// If there are a few kB in stdout, we might lock
				mcs_output=mcs.StandardError.ReadToEnd();
				mcs_stdout=mcs.StandardOutput.ReadToEnd ();
				mcs.WaitForExit();
				results.NativeCompilerReturnValue = mcs.ExitCode;
			} finally {
				mcs.Close();
#if NET_2_0
				if (mono_inside_mdb != null)
					Environment.SetEnvironmentVariable ("MONO_INSIDE_MDB", mono_inside_mdb);
#endif
			}
			mcs_output_lines=mcs_output.Split(
				System.Environment.NewLine.ToCharArray());
			bool loadIt=true;
			foreach (string error_line in mcs_output_lines)
			{
				CompilerError error=CreateErrorFromString(error_line);
				if (null!=error)
				{
					results.Errors.Add(error);
					if (!error.IsWarning) loadIt=false;
				}
			}
			if (loadIt) {
				if (!File.Exists (options.OutputAssembly)) {
					throw new Exception ("Compiler failed to produce the assembly. Stderr='"
						+mcs_output+"', Stdout='"+mcs_stdout+"'");
				}
				if (options.GenerateInMemory) {
					using (FileStream fs = File.OpenRead(options.OutputAssembly)) {
						byte[] buffer = new byte[fs.Length];
						fs.Read(buffer, 0, buffer.Length);
						results.CompiledAssembly = Assembly.Load(buffer, null, options.Evidence);
						fs.Close();
					}
				} else {
					// Avoid setting CompiledAssembly right now since the output might be a netmodule
					results.PathToAssembly = options.OutputAssembly;
				}
			} else {
				results.CompiledAssembly = null;
			}

			return results;
		}

#if NET_2_0
		private static string BuildArgs(CompilerParameters options,string[] fileNames, Dictionary <string, string> providerOptions)
#else
		private static string BuildArgs(CompilerParameters options,string[] fileNames)
#endif
		{
			StringBuilder args=new StringBuilder();
			if (options.GenerateExecutable)
				args.Append("/target:exe ");
			else
				args.Append("/target:library ");

			if (options.Win32Resource != null)
				args.AppendFormat("/win32res:\"{0}\" ",
					options.Win32Resource);

			if (options.IncludeDebugInformation)
				args.Append("/debug+ /optimize- ");
			else
				args.Append("/debug- /optimize+ ");

			if (options.TreatWarningsAsErrors)
				args.Append("/warnaserror ");

			if (options.WarningLevel >= 0)
				args.AppendFormat ("/warn:{0} ", options.WarningLevel);

			if (options.OutputAssembly == null || options.OutputAssembly.Length == 0) {
				string extension = (options.GenerateExecutable ? "exe" : "dll");
				options.OutputAssembly = GetTempFileNameWithExtension (options.TempFiles, extension,
					!options.GenerateInMemory);
			}
			args.AppendFormat("/out:\"{0}\" ",options.OutputAssembly);

			foreach (string import in options.ReferencedAssemblies) {
				if (import == null || import.Length == 0)
					continue;

				args.AppendFormat("/r:\"{0}\" ",import);
			}

			if (options.CompilerOptions != null) {
				args.Append (options.CompilerOptions);
				args.Append (" ");
			}

#if NET_2_0
			foreach (string embeddedResource in options.EmbeddedResources) {
				args.AppendFormat("/resource:\"{0}\" ", embeddedResource);
			}

			foreach (string linkedResource in options.LinkedResources) {
				args.AppendFormat("/linkresource:\"{0}\" ", linkedResource);
			}
			
			if (providerOptions != null && providerOptions.Count > 0) {
				string langver;

				if (!providerOptions.TryGetValue ("CompilerVersion", out langver))
					langver = "2.0";

				if (langver.Length >= 1 && langver [0] == 'v')
					langver = langver.Substring (1);

				switch (langver) {
					case "2.0":
						args.Append ("/langversion:ISO-2");
						break;

					case "3.5":
						// current default, omit the switch
						break;
				}
			}
#endif

			args.Append (" -- ");
			foreach (string source in fileNames)
				args.AppendFormat("\"{0}\" ",source);
			return args.ToString();
		}
		private static CompilerError CreateErrorFromString(string error_string)
		{
#if NET_2_0
			if (error_string.StartsWith ("BETA"))
				return null;
#endif
			if (error_string == null || error_string == "")
				return null;

			CompilerError error=new CompilerError();
			Regex reg = new Regex (@"^(\s*(?<file>.*)\((?<line>\d*)(,(?<column>\d*))?\)(:)?\s+)*(?<level>\w+)\s*(?<number>.*):\s(?<message>.*)",
				RegexOptions.Compiled | RegexOptions.ExplicitCapture);
			Match match=reg.Match(error_string);
			if (!match.Success) {
				// We had some sort of runtime crash
				error.ErrorText = error_string;
				error.IsWarning = false;
				error.ErrorNumber = "";
				return error;
			}
			if (String.Empty != match.Result("${file}"))
				error.FileName=match.Result("${file}");
			if (String.Empty != match.Result("${line}"))
				error.Line=Int32.Parse(match.Result("${line}"));
			if (String.Empty != match.Result("${column}"))
				error.Column=Int32.Parse(match.Result("${column}"));

			string level = match.Result ("${level}");
			if (level == "warning")
				error.IsWarning = true;
			else if (level != "error")
				return null; // error CS8028 will confuse the regex.

			error.ErrorNumber=match.Result("${number}");
			error.ErrorText=match.Result("${message}");
			return error;
		}

		private static string GetTempFileNameWithExtension (TempFileCollection temp_files, string extension, bool keepFile)
		{
			return temp_files.AddExtension (extension, keepFile);
		}

		private static string GetTempFileNameWithExtension (TempFileCollection temp_files, string extension)
		{
			return temp_files.AddExtension (extension);
		}

		private CompilerResults CompileFromDomBatch (CompilerParameters options, CodeCompileUnit[] ea)
		{
			if (options == null) {
				throw new ArgumentNullException ("options");
			}

			if (ea == null) {
				throw new ArgumentNullException ("ea");
			}

			string[] fileNames = new string[ea.Length];
			StringCollection assemblies = options.ReferencedAssemblies;

			for (int i = 0; i < ea.Length; i++) {
				CodeCompileUnit compileUnit = ea[i];
				fileNames[i] = GetTempFileNameWithExtension (options.TempFiles, i + ".cs");
				FileStream f = new FileStream (fileNames[i], FileMode.OpenOrCreate);
				StreamWriter s = new StreamWriter (f, Encoding.UTF8);
				if (compileUnit.ReferencedAssemblies != null) {
					foreach (string str in compileUnit.ReferencedAssemblies) {
						if (!assemblies.Contains (str))
							assemblies.Add (str);
					}
				}

				((ICodeGenerator) this).GenerateCodeFromCompileUnit (compileUnit, s, new CodeGeneratorOptions ());
				s.Close ();
				f.Close ();
			}
			return CompileAssemblyFromFileBatch (options, fileNames);
		}

		private CompilerResults CompileFromSourceBatch (CompilerParameters options, string[] sources)
		{
			if (options == null) {
				throw new ArgumentNullException ("options");
			}

			if (sources == null) {
				throw new ArgumentNullException ("sources");
			}

			string[] fileNames = new string[sources.Length];

			for (int i = 0; i < sources.Length; i++) {
				fileNames[i] = GetTempFileNameWithExtension (options.TempFiles, i + ".cs");
				FileStream f = new FileStream (fileNames[i], FileMode.OpenOrCreate);
				using (StreamWriter s = new StreamWriter (f, Encoding.UTF8)) {
					s.Write (sources[i]);
					s.Close ();
				}
				f.Close ();
			}
			return CompileFromFileBatch (options, fileNames);
		}
	}
}
