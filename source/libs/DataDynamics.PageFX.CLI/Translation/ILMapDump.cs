using System;
using System.IO;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Translation.ControlFlow;
using DataDynamics.PageFX.CLI.Translation.ControlFlow.Services;
using DataDynamics.PageFX.Common;

namespace DataDynamics.PageFX.CLI.Translation
{
	internal static class ILMapDump
    {
		private static readonly string Separator = new string('-', 200);

        public static void Dump(IClrMethodBody body, TranslatorResult result, string format, string filename)
        {
			if (!(DebugHooks.EvalFilter(body.Method) || DebugHooks.DumpILMap)) return;

            DebugHooks.LogInfo("DumpILMap started. Format = {0}. FileName = {1}.", format, filename);

            string dir = body.GetTestDirectory();
            Directory.CreateDirectory(dir);
            using (var writer = new StreamWriter(Path.Combine(dir, filename)))
            {
                DumpService.DumpLocalVariables(writer, body);
                writer.WriteLine(Separator);

                if (result.Begin != null && result.Begin.Length > 0)
	            {
					writer.WriteLine("#BEGIN CODE");
					writer.WriteLine(Separator);
		            for (int i = 0; i < result.Begin.Length; ++i)
		            {
			            writer.WriteLine(result.Output[i].ToString(format, null));
		            }
		            writer.WriteLine(Separator);
	            }

	            foreach (var bb in body.ControlFlowGraph.Blocks)
                {
                    DebugHooks.DoCancel();
                    writer.WriteLine("#BASIC BLOCK {0}", bb.Index);
                    DumpStackState(writer, bb);
                    writer.WriteLine(Separator);

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
                	writer.WriteLine(Separator);
                }

                if (result.End != null && result.End.Length > 0)
                {
                    writer.WriteLine("#END CODE");
                    writer.WriteLine(Separator);
                    foreach (var instruction in result.End)
                    	writer.WriteLine(instruction.ToString(format, null));
                }
            }

            DebugHooks.LogInfo("DumpILMap succeded");
        }

        private static void DumpStackState(TextWriter writer, Node bb)
        {
            if (!bb.IsTranslated)
                throw new InvalidOperationException();

            writer.Write("Stack Before: ");
            var arr = bb.StackBefore.ToArray();
            for (int i = 0; i < arr.Length; ++i)
            {
                if (i > 0) writer.Write(", ");
                writer.Write(arr[i].Value.ToString());
            }
            writer.WriteLine();

            if (bb.Stack != null)
            {
                writer.Write("Stack After: ");
                arr = bb.Stack.ToArray();
                for (int i = 0; i < arr.Length; ++i)
                {
                    if (i > 0) writer.Write(", ");
                    writer.Write(arr[i].Value.ToString());
                }
                writer.WriteLine();
            }
        }
    }
}