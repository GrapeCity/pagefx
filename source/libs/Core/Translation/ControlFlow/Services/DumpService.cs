#if DEBUG
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.Core.IL;

namespace DataDynamics.PageFX.Core.Translation.ControlFlow.Services
{
    internal class DumpService
    {
		public static void Dump(IEnumerable<Node> cfg, IClrMethodBody body, string root)
        {
            string dir = body.GetTestDirectory();

            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, "cil.txt");

            using (var writer = new StreamWriter(path))
            {
                Dump(writer, cfg, body);
            }
        }

        static string GetBlockIndent(Block block)
        {
        	var handler = block as HandlerBlock;
            if (handler != null)
                return GetBlockIndent(handler.Owner);

            var tab = "";
            while (block.Parent != null)
            {
                tab += "\t";
                block = block.Parent;
            }
            return tab;
        }

        static void WriteBlockIndent(TextWriter writer, Block block)
        {
            string tab = GetBlockIndent(block);
            writer.Write(tab);
        }

        static string BeginBlock(TextWriter writer, Block block)
        {
            if (block == null) return null;

            if (block.Dumped) return null;

            if (block.Parent != null)
                BeginBlock(writer, block.Parent);

            block.Dumped = true;

            string tab = GetBlockIndent(block);
            writer.Write(tab);
            switch (block.Type)
            {
                case BlockType.Protected:
                    writer.WriteLine("try");
                    break;

                case BlockType.Catch:
                    {
                        var h = (HandlerBlock)block;
                        writer.WriteLine("catch {0}", h.ExceptionType != null ? h.ExceptionType.FullName : "Exception");
                    }
                    break;

                case BlockType.Filter:
                    {
                        var h = (HandlerBlock)block;
                        writer.WriteLine("filter {0}", h.FilterIndex);
                    }
                    break;

                case BlockType.Finally:
                    writer.WriteLine("finally");
                    break;

                case BlockType.Fault:
                    writer.WriteLine("fault");
                    break;
            }
            writer.Write(tab);
            writer.WriteLine("{");
            return tab;
        }

        static void EndBlock(TextWriter writer, Block block)
        {
            if (block != null)
            {
                WriteBlockIndent(writer, block);
                writer.WriteLine("}");
            }
        }

        static readonly string separator = new string('-', 200);

        public static void DumpLocalVariables(TextWriter writer, IClrMethodBody body)
        {
            if (body.LocalVariables != null)
            {
                int n = body.LocalVariables.Count;
                writer.WriteLine("LocalCount: {0}", n);
                int pn = body.Method.Parameters.Count + 1;
                for (int i = 0; i < n; ++i)
                {
                    var v = body.LocalVariables[i];
                    writer.WriteLine("v{0}[{1}]: {2}", pn + i, i, v.Type.FullName);
                }
            }
        }

		public static void Dump(TextWriter writer, IEnumerable<Node> cfg, IClrMethodBody body)
        {
            var method = body.Method;

            writer.WriteLine(separator);
            writer.WriteLine("//{0}: {1}", CommonLanguageInfrastructure.Debug ? "DEBUG" : "RELEASE", method);
            writer.WriteLine(separator);

            DumpLocalVariables(writer, body);
            
            writer.WriteLine(separator);

            if (body.HasProtectedBlocks)
            {
                foreach (var instr in body.Code)
                {
                    if (instr.Dumped) continue;

                    var block = instr.SehBlock;
                    if (block != null)
                    {
                        WriteBlock(writer, block);
                        //if (instruction.Index == block.EntryIndex)
                        //{
                        //    BeginBlock(writer, block);
                        //}
                        //string tab = GetBlockIndent(block);
                        //writer.Write(tab + "\t");
                    }
                    else
                    {
                        instr.Dumped = true;
                        writer.WriteLine(instr.ToString());
                    }
                    //if (block != null)
                    //{
                    //    if (instruction.Index == block.ExitIndex)
                    //        EndBlock(writer, block);
                    //}
                }
            }
            else
            {
                writer.WriteLine(body.Code.ToString());
            }

            writer.WriteLine();

            writer.WriteLine("--------------------------------------------------");
            writer.WriteLine("CFG");
            writer.WriteLine("--------------------------------------------------");
            foreach (var node in cfg)
            {
                writer.WriteLine("{0}: {1} - {2}", node, node.EntryIndex, node.ExitIndex);
            }
            writer.WriteLine();
        }

    	static void WriteBlock(TextWriter writer, Block block)
        {
            string tab = BeginBlock(writer, block);

            foreach (var instr in block.GetInstructions())
            {
                if (instr.SehBlock != block)
                {
                    WriteBlock(writer, instr.SehBlock);
                }
                else
                {
                    instr.Dumped = true;
                    writer.WriteLine(tab + "\t" + instr);
                }
            }

            EndBlock(writer, block);
        }
    }
}
#endif