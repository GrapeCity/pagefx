#if DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.CLI.CFG;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    using Code = List<IInstruction>;

    internal partial class ILTranslator
    {
        static readonly string separator = new string('-', 200);
        int _beginSize;

        public void DumpILMap(string format, string filename)
        {
			if (!(DebugHooks.EvalFilter(_method) || DebugHooks.DumpILMap)) return;

            DebugHooks.LogInfo("DumpILMap started. Format = {0}. FileName = {1}.", format, filename);

            string dir = _body.GetTestDirectory();
            Directory.CreateDirectory(dir);
            using (var writer = new StreamWriter(Path.Combine(dir, filename)))
            {
                DumpService.DumpLocalVariables(writer, _body);
                writer.WriteLine(separator);

                writer.WriteLine("#BEGIN CODE");
                writer.WriteLine(separator);
                for (int i = 0; i < _beginSize; ++i)
                {
                    writer.WriteLine(_outcode[i].ToString(format, null));
                }
                writer.WriteLine(separator);

                foreach (var bb in Blocks)
                {
                    DebugHooks.DoCancel();
                    writer.WriteLine("#BASIC BLOCK {0}", bb.Index);
                    DumpStackState(writer, bb);
                    writer.WriteLine(separator);

                    writer.WriteLine("#ORIGINAL CODE");
                    foreach (var instruction in bb.Code)
                    {
                    	writer.WriteLine(instruction.ToString(format, null));
                    }
                	writer.WriteLine();

                    var code = bb.TranslatedCode;
                    writer.WriteLine("#TRANSLATED CODE");
                    foreach (var instruction in code)
                    {
                    	writer.WriteLine(instruction.ToString(format, null));
                    }
                	writer.WriteLine(separator);
                }

                if (_endCode != null && _endCode.Length > 0)
                {
                    writer.WriteLine("#END CODE");
                    writer.WriteLine(separator);
                    foreach (var instruction in _endCode)
                    	writer.WriteLine(instruction.ToString(format, null));
                }
            }

            DebugHooks.LogInfo("DumpILMap succeded");
        }

        static void DumpStackState(TextWriter writer, Node bb)
        {
            if (!bb.IsTranslated)
                throw new InvalidOperationException();

            writer.Write("Stack Before: ");
            var arr = bb.StackBefore.ToArray();
            for (int i = 0; i < arr.Length; ++i)
            {
                if (i > 0) writer.Write(", ");
                writer.Write(arr[i].value.ToString());
            }
            writer.WriteLine();

            if (bb.Stack != null)
            {
                writer.Write("Stack After: ");
                arr = bb.Stack.ToArray();
                for (int i = 0; i < arr.Length; ++i)
                {
                    if (i > 0) writer.Write(", ");
                    writer.Write(arr[i].value.ToString());
                }
                writer.WriteLine();
            }
        }
    }
}
#endif