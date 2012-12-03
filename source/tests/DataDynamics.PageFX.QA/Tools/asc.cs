using System.IO;
using System.Text;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.FLI
{
    /*
        ActionScript 3.0 for AVM+
        version 1.0 build 303358
        Copyright (c) 2003-2004 Macromedia, Inc.
        Copyright (c) 1998-2003 Mountain View Compiler Company
        All rights reserved

        Usage:
          asc {-AS3|-ES|-d|-f|-h|-i|-import <filename>|-in <filename>|-m|-p}* filespec
          -AS3 = use the AS3 class based object model for greater performance and better error reporting
          -ES = use the ECMAScript edition 3 prototype based object model to allow dynamic overriding of prototype properties
          -d = emit debug info into the bytecode
          -f = print the flow graph to standard out
          -h = print this message
          -i = write intermediate code to the .il file
          -import <filename> = make the packages in the
               specified file available for import
          -in <filename> = include the specified filename
               (multiple -in arguments allowed)
          -m = write the avm+ assembly code to the .il file
          -p = write parse tree to the .p file
          -md = emit metadata information into the bytecode
          -warnings = warn on common actionscript mistakes
          -strict = treat undeclared variable and method access as errors
          -sanity = system-independent error/warning output -- appropriate for sanity testing
          -log = redirect all error output to a logfile
          -exe <avmplus path> = emit an EXE file (projector)
          -swf classname,width,height[,fps] = emit a SWF file
          -language = set the language for output strings {EN|FR|DE|IT|ES|JP|KR|CN|TW}
          -optimize = produced an optimized abc file
     */

    public static class Asc
    {
        #region class Options
        public class Options
        {
        	public bool AS3 { get; set; }

        	public bool ES { get; set; }

        	public bool Debug { get; set; }

        	public bool Optimize { get; set; }

        	public bool PrintFlowGraph { get; set; }

        	public bool WriteIntermediateCode { get; set; }

        	public bool WriteAssemblyCode { get; set; }

        	public bool WriteParseTree { get; set; }

        	public bool EmitMetadata { get; set; }

        	public bool Strict { get; set; }

        	public bool Sanity { get; set; }

        	public string LogFile { get; set; }

        	public string Language { get; set; }

        	public string AvmPlusPath { get; set; }

        	public string[] Imports { get; set; }

        	public override string ToString()
            {
                var sb = new StringBuilder();
                if (AS3) sb.Append("-AS3 ");
                if (ES) sb.Append("-ES3 ");
                if (Debug) sb.Append("-d ");
                if (PrintFlowGraph) sb.Append("-f ");
                if (WriteIntermediateCode) sb.Append("-i ");
                if (WriteAssemblyCode) sb.Append("-m ");
                if (EmitMetadata) sb.Append("-md ");
                if (Strict) sb.Append("-strict ");
                if (Sanity) sb.Append("-sanity ");
                if (Optimize) sb.Append("-optimize ");

                if (!string.IsNullOrEmpty(LogFile))
                {
                    sb.AppendFormat("-log {0} ", LogFile);
                }

                if (!string.IsNullOrEmpty(Language))
                {
                    sb.AppendFormat("-language {0} ", Language);
                }

                if (Imports != null)
                {
                    foreach (var import in Imports)
                    {
                        sb.AppendFormat("-import {0} ", import);
                    }
                }

                if (!string.IsNullOrEmpty(AvmPlusPath))
                {
                    sb.AppendFormat("-exe {0} ", AvmPlusPath);
                }

                return sb.ToString();
            }
        }
        #endregion


        public static string GetPath()
        {
            return @"E:\sdk\flex3\lib\asc.jar";
        }

        public static string Run(Options options, params string[] inputs)
        {
            string path = GetPath();
            if (!File.Exists(path))
                return string.Format("Unable to find asc.jar");

            if (options == null)
                options = new Options(); //use default options

            var args = new StringBuilder();
            args.AppendFormat("-jar {0} ", path);
            args.Append(options.ToString());
            for (int i = 0; i < inputs.Length; ++i)
            {
                if (i > 0) args.Append(' ');
                //args.Append("-in ");
                args.Append(inputs[i]);
            }

            int exitCode;
            string cout = CommandPromt.Run("java", args.ToString(), out exitCode);
            if (exitCode != 0)
                return string.Format("Unable to compile ABC file.\n{0}", cout);
            return null;
        }
    }
}