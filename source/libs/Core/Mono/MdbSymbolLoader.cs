using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Core.IL;
using Mono.CompilerServices.SymbolWriter;

namespace DataDynamics.PageFX.Core.Mono
{
	internal sealed class MdbSymbolLoader : ISymbolLoader
	{
		public static ISymbolLoader Create(string path)
		{
			if (!Path.IsPathRooted(path))
				path = Path.Combine(Environment.CurrentDirectory, path);

			path = Path.ChangeExtension(path, ".mdb");

			try
			{
				if (!File.Exists(path))
					return null;

				return new MdbSymbolLoader(MonoSymbolFile.ReadSymbolFile(path));
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
			}

			return null;
		}

		private readonly MonoSymbolFile _symbolFile;

		private MdbSymbolLoader(MonoSymbolFile symbolFile)
		{
			_symbolFile = symbolFile;
		}

		public void Dispose()
		{
			_symbolFile.Dispose();
		}

		public bool LoadSymbols(IClrMethodBody body)
		{
			var entry = _symbolFile.GetMethodByToken(body.Method.MetadataToken);
			if (entry == null)
				return false;

			var points = ReadSequencePoints(entry);
			body.SetSequencePoints(points);
			ReadLocals(entry, body);

			return true;
		}

		private static IEnumerable<SequencePoint> ReadSequencePoints(MethodEntry entry)
		{
			var table = entry.GetLineNumberTable();
			return table.LineNumbers.Select(line => new SequencePoint
				{
					File = entry.CompileUnit.SourceFile.FileName,
					Offset = line.Offset,
					StartRow = line.Row,
					EndRow = line.Row
				});
		}

		private static void ReadLocals(MethodEntry entry, IMethodBody body)
		{
			foreach (var local in entry.GetLocals())
			{
				if (local.Index < 0 || local.Index >= body.LocalVariables.Count) // Mono 2.6 emits wrong local infos for iterators
					continue;

				var variable = body.LocalVariables[local.Index];
				variable.Name = local.Name;
			}
		}
	}
}
