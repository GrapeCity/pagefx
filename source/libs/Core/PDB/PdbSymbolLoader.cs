using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Core.IL;
using DataDynamics.PageFX.Core.Pdb.Internal;

namespace DataDynamics.PageFX.Core.Pdb
{
	internal sealed class PdbSymbolLoader : ISymbolLoader
	{
		private IDictionary<uint, PdbFunction> _functions;

		private PdbSymbolLoader(IDictionary<uint, PdbFunction> functions)
		{
			_functions = functions;
		}

		public static ISymbolLoader Create(string path)
		{
			if (!Path.IsPathRooted(path))
				path = Path.Combine(Environment.CurrentDirectory, path);

			path = Path.ChangeExtension(path, ".pdb");

			try
			{
				if (!File.Exists(path))
					return null;

				using (var stream = File.OpenRead(path))
				{
					return Create(stream);
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
			}
			return null;
		}

		public static ISymbolLoader Create(Stream stream)
		{
			try
			{
				Dictionary<uint, PdbTokenLine> tokenToSourceMapping;
				string sourceServerData;
				int age;
				Guid guid;

				var funcs = PdbFile.LoadFunctions(stream, out tokenToSourceMapping, out sourceServerData, out age, out guid);

				return new PdbSymbolLoader(funcs.ToDictionary(x => x.token, x => x));
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
			}
			return null;
		}

		public void Dispose()
		{
			_functions = null;
		}

		public bool LoadSymbols(IClrMethodBody body)
		{
			var token = body.Method.MetadataToken;

			PdbFunction function;
			if (!_functions.TryGetValue((uint)token, out function))
				return false;

			var points = ReadSequencePoints(function);

			body.SetSequencePoints(points);

			ReadLocals(function.scopes, body);

			return true;
		}

		private static IEnumerable<SequencePoint> ReadSequencePoints(PdbFunction function)
		{
			if (function.lines == null)
				return Enumerable.Empty<SequencePoint>();

			return function.lines.SelectMany(lines => ReadSequencePoints(lines));
		}

		private static IEnumerable<SequencePoint> ReadSequencePoints(PdbLines lines)
		{
			var document = lines.file.name;

			return lines.lines.Select(line => new SequencePoint
				{
					File = document,
					Offset = (int)line.offset,
					StartRow = (int)line.lineBegin,
					StartColumn = line.colBegin,
					EndRow = (int)line.lineEnd,
					EndColumn = line.colEnd
				});
		}

		private static void ReadLocals(IEnumerable<PdbScope> scopes, IClrMethodBody body)
		{
			foreach (var scope in scopes)
				ReadLocals(scope, body);
		}

		private static void ReadLocals(PdbScope scope, IClrMethodBody body)
		{
			if (scope == null)
				return;

			foreach (var slot in scope.slots)
			{
				int index = (int)slot.slot;
				if (index < 0 || index >= body.LocalVariables.Count)
					continue;

				var var = body.LocalVariables[index];
				var.Name = slot.name;
			}

			ReadLocals(scope.scopes, body);
		}
	}
}
