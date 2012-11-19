﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CLI.Collections
{
	internal sealed class ParamList : IParameterCollection
	{
		private readonly AssemblyLoader _loader;
		private readonly IMethod _owner;
		private readonly int _from;
		private readonly MdbMethodSignature _signature;
		private IReadOnlyList<IParameter> _list;

		public ParamList(AssemblyLoader loader, IMethod owner, int from, MdbMethodSignature signature)
		{
			_loader = loader;
			_owner = owner;
			_from = from;
			_signature = signature;
		}

		public IEnumerator<IParameter> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count
		{
			get { return List.Count; }
		}

		public IParameter this[int index]
		{
			get { return List[index]; }
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public CodeNodeType NodeType
		{
			get { return CodeNodeType.Parameters; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Tag { get; set; }

		public IParameter this[string name]
		{
			get { return this.FirstOrDefault(x => x.Name == name); }
		}

		public void Add(IParameter parameter)
		{
			throw new NotSupportedException();
		}

		private IReadOnlyList<IParameter> List
		{
			get { return _list ?? (_list = Populate().Memoize()); }
		}

		private IEnumerable<IParameter> Populate()
		{
			var context = ResolveContext();

			for (int i = _from, k = 0; k < _signature.Params.Length; ++i)
			{
				var param = _loader.Parameters[i];
				if (param.Index == 0)
				{
					//0 refers to the owner method's return type;
					continue;
				}

				var ptype = _loader.ResolveType(_signature.Params[k++], context);
				if (ptype == null)
					throw new InvalidOperationException();

				param.Type = ptype;

				//var p = new Parameter(ptype, "arg" + (i + 1), i + 1);
				//method.Parameters.Add(p);

				yield return param;
			}
		}

		private Context ResolveContext()
		{
			var declType = _owner.DeclaringType;
			if (declType == null)
				throw new InvalidOperationException();
			return new Context(declType, _owner);
		}
	}
}