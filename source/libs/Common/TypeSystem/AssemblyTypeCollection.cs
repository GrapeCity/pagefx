using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Syntax;

namespace DataDynamics.PageFX.Common.TypeSystem
{
	internal sealed class AssemblyTypeCollection : ITypeCollection
	{
		private readonly IAssembly _assembly;

		public AssemblyTypeCollection(IAssembly assembly)
		{
			_assembly = assembly;
		}

		public IEnumerator<IType> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
			{
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count
		{
			get { return _assembly.Modules.Sum(module => module.Types.Count); }
		}

		public IType this[int index]
		{
			get
			{
				if (index < 0)
					throw new ArgumentOutOfRangeException("index");

				foreach (var module in _assembly.Modules)
				{
					int n = module.Types.Count;
					if (index < n)
						return module.Types[index];
					index -= n;
				}

				throw new ArgumentOutOfRangeException("index");
			}
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Data { get; set; }

		public IType FindType(string fullname)
		{
			return _assembly.Modules.Select(module => module.Types.FindType(fullname)).FirstOrDefault(type => type != null);
		}

		public void Add(IType type)
		{
			_assembly.MainModule.Types.Add(type);
		}

		public bool Contains(IType type)
		{
			return type != null && FindType(type.FullName) != null;
		}
	}
}