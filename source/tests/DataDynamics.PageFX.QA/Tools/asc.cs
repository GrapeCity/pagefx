using System.IO;
using System.Text;

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
            public bool AS3
            {
                get { return _as3; }
                set { _as3 = value; }
            }
            private bool _as3;

            public bool ES
            {
                get { return _es; }
                set { _es = value; }
            }
            private bool _es;

            public bool Debug
            {
                get { return _debug; }
                set { _debug = value; }
            }
            private bool _debug;

            public bool Optimize
            {
                get { return _optimize; }
                set { _optimize = value; }
            }
            private bool _optimize;

            public bool PrintFlowGraph
            {
                get { return _printFlowGraph; }
                set { _printFlowGraph = value; }
            }
            private bool _printFlowGraph;

            public bool WriteIntermediateCode
            {
                get { return _writeIntermediateCode; }
                set { _writeIntermediateCode = value; }
            }
            private bool _writeIntermediateCode;

            public bool WriteAssemblyCode
            {
                get { return _writeAssemblyCode; }
                set { _writeAssemblyCode = value; }
            }
            private bool _writeAssemblyCode;

            public bool WriteParseTree
            {
                get { return _writeParseTree; }
                set { _writeParseTree = value; }
            }
            private bool _writeParseTree;

            public bool EmitMetadata
            {
                get { return _emitMetadata; }
                set { _emitMetadata = value; }
            }
            private bool _emitMetadata;

            public bool Strict
            {
                get { return _strict; }
                set { _strict = value; }
            }
            private bool _strict;

            public bool Sanity
            {
                get { return _sanity; }
                set { _sanity = value; }
            }
            private bool _sanity;

            public string LogFile
            {
                get { return _log; }
                set { _log = value; }
            }
            private string _log;

            public string Language
            {
                get { return _lang; }
                set { _lang = value; }
            }
            private string _lang;

            public string AvmPlusPath
            {
                get { return _avmPlusPath; }
                set { _avmPlusPath = value; }
            }
            private string _avmPlusPath;

            public string[] Imports
            {
                get { return _imports; }
                set { _imports = value; }
            }
            private string[] _imports;

            public override string ToString()
            {
                var sb = new StringBuilder();
                if (_as3) sb.Append("-AS3 ");
                if (_es) sb.Append("-ES3 ");
                if (_debug) sb.Append("-d ");
                if (_printFlowGraph) sb.Append("-f ");
                if (_writeIntermediateCode) sb.Append("-i ");
                if (_writeAssemblyCode) sb.Append("-m ");
                if (_emitMetadata) sb.Append("-md ");
                if (_strict) sb.Append("-strict ");
                if (_sanity) sb.Append("-sanity ");
                if (_optimize) sb.Append("-optimize ");

                if (!string.IsNullOrEmpty(_log))
                {
                    sb.AppendFormat("-log {0} ", _log);
                }

                if (!string.IsNullOrEmpty(_lang))
                {
                    sb.AppendFormat("-language {0} ", _lang);
                }

                if (_imports != null)
                {
                    foreach (var import in _imports)
                    {
                        sb.AppendFormat("-import {0} ", import);
                    }
                }

                if (!string.IsNullOrEmpty(_avmPlusPath))
                {
                    sb.AppendFormat("-exe {0} ", _avmPlusPath);
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