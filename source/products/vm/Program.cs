using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Ecma335.Execution;

namespace DataDynamics.PageFX
{
	internal static class Program
	{
		static void Main(string[] args)
		{
			//ArithmeticGenerator.Generate();

			string path = args[0];

			var vmargs = new List<string>();
			for (int i = 1; i < args.Length; i++)
			{
				vmargs.Add(args[i]);
			}

			var vm = new VirtualMachine();
			vm.Run(path, "", vmargs.ToArray());

			Console.ReadLine();
		}
	}
}