using System;
using System.IO;

namespace DataDynamics.PageFX.CLI.Execution
{
	internal sealed class ConsoleEmulator : NativeClass
	{
		private readonly TextWriter _output;

		public ConsoleEmulator(TextWriter output)
			: base(typeof(Console))
		{
			if (output == null) throw new ArgumentNullException("output");

			_output = output;
		}

		public override object Invoke(string name, object instance, object[] args)
		{
			switch (name)
			{
				case "WriteLine":
					switch (args.Length)
					{
						case 0:
							_output.WriteLine();
							break;
						case 1:
							_output.WriteLine(args[0]);
							break;
						case 2:
							var arr = args[1] as object[];
							if (arr != null)
							{
								_output.WriteLine(args[0] as string, arr);
							}
							else
							{
								_output.WriteLine(args[0] as string, args[1]);
							}
							break;
						case 3:
							_output.WriteLine(args[0] as string, args[1], args[2]);
							break;
						case 4:
							_output.WriteLine(args[0] as string, args[1], args[2], args[3]);
							break;
						default:
							throw new NotImplementedException();
					}
					return null;

				case "Write":
					switch (args.Length)
					{
						case 1:
							_output.Write(args[0]);
							break;
						case 2:
							var arr = args[1] as object[];
							if (arr != null)
							{
								_output.Write(args[0] as string, arr);
							}
							else
							{
								_output.Write(args[0] as string, args[1]);
							}
							break;
						case 3:
							_output.Write(args[0] as string, args[1], args[2]);
							break;
						case 4:
							_output.Write(args[0] as string, args[1], args[2], args[3]);
							break;
						default:
							throw new NotImplementedException();
					}
					return null;

				default:
					throw new NotImplementedException();
			}
		}
	}
}
